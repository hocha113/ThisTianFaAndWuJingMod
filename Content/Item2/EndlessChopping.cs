using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Core;
using ThisTianFaAndWuJingMod.Content.Particles;

namespace ThisTianFaAndWuJingMod.Content.Item2
{
    internal class EndlessChopping : ModProjectile
    {
        public override string Texture => EffectLoader.AssetPath + "placeholder3";
        Vector2 origVer;
        int drawTrailCount = 100;
        bool set;
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.ignoreWater = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override bool? CanHitNPC(NPC target) {
            if (Projectile.ai[0] < Projectile.ai[1] || Projectile.ai[0] > Projectile.ai[1] + 30) {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void AI() {
            if (!set && Projectile.ai[0] > Projectile.ai[1]) {
                origVer = Projectile.velocity;
                Projectile.velocity = Vector2.Zero;
                for (int i = 0; i < 133; i++) {
                    Vector2 ver = origVer.UnitVector() * ((i / 133f) * 133 + 0.1f);
                    Color color = TFAWUtils.MultiStepColorLerp(i / 133f, Endless.rainbowColors);
                    BaseParticle spark = new PRT_Spark(Projectile.Center, -ver, false, 19, 2.3f, color);
                    PRTLoader.AddParticle(spark);
                    BaseParticle spark2 = new PRT_Spark(Projectile.Center, ver, false, 19, 2.3f, color);
                    PRTLoader.AddParticle(spark2);
                }
                set = true;
            }
            Projectile.ai[0]++;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            float point = 0f;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size()
                , Projectile.Center - origVer.UnitVector() * 1200
                , Projectile.Center + origVer.UnitVector() * 1200, 38, ref point);
        }

        public void DrawTrail(List<VertexPositionColorTexture> bars) {
            Effect effect = TFAWMod.Instance.Assets.Request<Effect>(EffectLoader.AssetPath2 + "StarsTrail").Value;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(TFAWUtils.GetT2DValue(EffectLoader.AssetPath + "MotionTrail4"));
            effect.Parameters["gradientTexture"].SetValue(TFAWUtils.GetT2DValue(EffectLoader.AssetPath + "ShaduraGradient"));
            effect.Parameters["worldSize"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight));
            effect.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly / 5);
            effect.Parameters["uExchange"].SetValue(0.87f + 0.05f * MathF.Sin(Main.GlobalTimeWrappedHourly));

            //应用shader，并绘制顶点
            foreach (EffectPass pass in effect.CurrentTechnique.Passes) {
                pass.Apply();
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                Main.graphics.GraphicsDevice.BlendState = BlendState.Additive;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            return false;
        }
    }
}
