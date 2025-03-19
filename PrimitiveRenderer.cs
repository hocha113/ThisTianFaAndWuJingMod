using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace ThisTianFaAndWuJingMod
{
    public static class PrimitiveRenderer
    {
        #region Static Members
        private static DynamicVertexBuffer VertexBuffer;

        private static DynamicIndexBuffer IndexBuffer;

        private static PrimitiveSettings MainSettings;

        private static Vector2[] MainPositions;

        private static short[] MainIndices;

        private const short MaxPositions = 1000;

        private const short MaxVertices = 3072;

        private const short MaxIndices = 8192;

        private static short PositionsIndex;

        private static short VerticesIndex;

        private static short IndicesIndex;

        internal static void Initialize() {
            Main.QueueMainThreadAction(() => {
                MainPositions = new Vector2[MaxPositions];
                MainIndices = new short[MaxIndices];
                IndexBuffer ??= new DynamicIndexBuffer(Main.instance.GraphicsDevice, IndexElementSize.SixteenBits, MaxIndices, BufferUsage.WriteOnly);
            });
        }

        /// <summary>
        /// Renders a primitive trail.
        /// </summary>
        /// <param name="positions">The list of positions to use. Keep in mind that these are expected to be in <b>world position</b>, and <see cref="Main.screenPosition"/> is automatically subtracted from them all.<br/>At least 4 points are required to use smoothing.</param>
        /// <param name="settings">The primitive draw settings to use.</param>
        /// <param name="pointsToCreate">The amount of points to use. More is higher detailed, but less performant. By default, is the number of positions provided. <b>Going above 100 is NOT recommended.</b></param>
        public static void RenderTrail(List<Vector2> positions, PrimitiveSettings settings, int? pointsToCreate = null) => RenderTrail(positions.ToArray(), settings, pointsToCreate);

        /// <summary>
        /// Renders a primitive trail.
        /// </summary>
        /// <param name="positions">The list of positions to use. Keep in mind that these are expected to be in <b>world position</b>, and <see cref="Main.screenPosition"/> is automatically subtracted from them all.<br/>At least 4 points are required to use smoothing.</param>
        /// <param name="settings">The primitive draw settings to use.</param>
        /// <param name="pointsToCreate">The amount of points to use. More is higher detailed, but less performant. By default, is the number of positions provided. <b>Going above 100 is NOT recommended.</b></param>
        public static void RenderTrail(Vector2[] positions, PrimitiveSettings settings, int? pointsToCreate = null) {
            // Return if not enough to draw anything.
            if (positions.Length <= 2)
                return;

            // Return if too many to draw anything,
            if (positions.Length >= MaxPositions)
                return;

            // IF this is false, a correct position trail could not be made and rendering should not continue.
            if (!AssignPointsRectangleTrail(positions, settings, pointsToCreate ?? positions.Length))
                return;

            // A trail with only one point or less has nothing to connect to, and therefore, can't make a trail.
            if (MainPositions.Length <= 2)
                return;

            AssignIndicesRectangleTrail();

            // Else render without wasting resources creating a set.
            // PrivateRender();
            return;
        }

        #endregion

        #region Set Preperation
        private static bool AssignPointsRectangleTrail(Vector2[] positions, PrimitiveSettings settings, int pointsToCreate) {
            // Don't smoothen the points unless explicitly told do so.
            if (!settings.Smoothen) {
                PositionsIndex = 0;

                // Would like to remove this, but unsure how else to properly ensure that none are zero.
                positions = positions.Where(originalPosition => originalPosition != Vector2.Zero).ToArray();

                if (positions.Length <= 2)
                    return false;

                // Remap the original positions across a certain length.
                for (int i = 0; i < pointsToCreate; i++) {
                    float completionRatio = i / (float)(pointsToCreate - 1f);
                    int currentIndex = (int)(completionRatio * (positions.Length - 1));
                    Vector2 currentPoint = positions[currentIndex];
                    Vector2 nextPoint = positions[(currentIndex + 1) % positions.Length];

                    // 29FEB2024: Ozzatron: offset function needs to apply even in cases where smoothing is off.
                    Vector2 finalPos = Vector2.Lerp(currentPoint, nextPoint, completionRatio * (positions.Length - 1) % 0.99999f) - Main.screenPosition;
                    if (settings.OffsetFunction != null)
                        finalPos += settings.OffsetFunction(completionRatio);

                    MainPositions[PositionsIndex] = finalPos;
                    PositionsIndex++;
                }
                return true;
            }

            // Due to the first point being manually added, points should be added starting at the second position instead of the first.
            PositionsIndex = 1;

            // Create the control points for the spline.
            List<Vector2> controlPoints = new();
            for (int i = 0; i < positions.Length; i++) {
                // Don't incorporate points that are zeroed out.
                // They are almost certainly a result of incomplete oldPos arrays.
                if (positions[i] == Vector2.Zero)
                    continue;

                float completionRatio = i / (float)positions.Length;
                Vector2 offset = -Main.screenPosition;
                if (settings.OffsetFunction != null)
                    offset += settings.OffsetFunction(completionRatio);
                controlPoints.Add(positions[i] + offset);
            }

            // Avoid stupid index errors.
            if (controlPoints.Count <= 4)
                return false;

            for (int j = 0; j < pointsToCreate; j++) {
                float splineInterpolant = j / (float)pointsToCreate;
                float localSplineInterpolant = splineInterpolant * (controlPoints.Count - 1f) % 1f;
                int localSplineIndex = (int)(splineInterpolant * (controlPoints.Count - 1f));

                Vector2 farLeft;
                Vector2 left = controlPoints[localSplineIndex];
                Vector2 right = controlPoints[localSplineIndex + 1];
                Vector2 farRight;

                // Special case: If the spline attempts to access the previous/next index but the index is already at the very beginning/end, simply
                // cheat a little bit by creating a phantom point that's mirrored from the previous one.
                if (localSplineIndex <= 0) {
                    Vector2 mirrored = left * 2f - right;
                    farLeft = mirrored;
                }
                else
                    farLeft = controlPoints[localSplineIndex - 1];

                if (localSplineIndex >= controlPoints.Count - 2) {
                    Vector2 mirrored = right * 2f - left;
                    farRight = mirrored;
                }
                else
                    farRight = controlPoints[localSplineIndex + 2];

                MainPositions[PositionsIndex] = Vector2.CatmullRom(farLeft, left, right, farRight, localSplineInterpolant);
                PositionsIndex++;
            }

            // Manually insert the front and end points.
            MainPositions[0] = controlPoints.First();
            MainPositions[PositionsIndex] = controlPoints.Last();
            PositionsIndex++;
            return true;
        }

        private static void AssignIndicesRectangleTrail() {
            // What this is doing is basically representing each point on the vertices list as
            // indices. These indices should come together to create a tiny rectangle that acts
            // as a segment on the trail. This is achieved here by splitting the indices (or rather, points)
            // into 2 triangles, which requires 6 points.
            // The logic here basically determines which indices are connected together.
            IndicesIndex = 0;
            for (short i = 0; i < PositionsIndex - 2; i++) {
                short connectToIndex = (short)(i * 2);
                MainIndices[IndicesIndex] = connectToIndex;
                IndicesIndex++;

                MainIndices[IndicesIndex] = (short)(connectToIndex + 1);
                IndicesIndex++;

                MainIndices[IndicesIndex] = (short)(connectToIndex + 2);
                IndicesIndex++;

                MainIndices[IndicesIndex] = (short)(connectToIndex + 2);
                IndicesIndex++;

                MainIndices[IndicesIndex] = (short)(connectToIndex + 1);
                IndicesIndex++;

                MainIndices[IndicesIndex] = (short)(connectToIndex + 3);
                IndicesIndex++;
            }
        }

        private static void CalcuatePixelatedPerspectiveMatrices(out Matrix viewMatrix, out Matrix projectionMatrix) {
            // Due to the scaling, the normal transformation calcuations do not work with pixelated primitives.
            projectionMatrix = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);
            viewMatrix = Matrix.Identity;
        }
        #endregion
    }
}
