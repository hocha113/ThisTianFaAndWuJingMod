using InnoVault;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod.Core
{
    internal class SwingSystem : ITFAWLoader
    {
        internal static List<BaseSwing> Swings;
        internal static Dictionary<string, int> SwingFullNameToType;
        internal static Dictionary<int, Asset<Texture2D>> trailTextures;
        internal static Dictionary<int, Asset<Texture2D>> gradientTextures;
        internal static Dictionary<int, Asset<Texture2D>> glowTextures;
        void ITFAWLoader.LoadData() {
            Swings = [];
            SwingFullNameToType = [];
            trailTextures = [];
            gradientTextures = [];
            glowTextures = [];
        }
        void ITFAWLoader.SetupData() {
            Swings = VaultUtils.GetSubclassInstances<BaseSwing>();
            foreach (var swing in Swings) {
                string pathValue = swing.GetType().Name;
                int type = TFAWMod.Instance.Find<ModProjectile>(pathValue).Type;
                SwingFullNameToType.Add(pathValue, type);
            }
        }
        void ITFAWLoader.LoadAsset() {
            foreach (var swing in Swings) {
                string path1 = swing.trailTexturePath;
                string path2 = swing.gradientTexturePath;
                string path3 = swing.GlowTexturePath;

                if (path1 == "") {
                    path1 = EffectLoader.AssetPath + "MotionTrail3";
                }
                if (path2 == "") {
                    path2 = EffectLoader.AssetPath + "NullEffectColorBar";
                }

                int type = SwingFullNameToType[swing.GetType().Name];

                trailTextures.TryAdd(type, TFAWUtils.GetT2DAsset(path1));
                gradientTextures.TryAdd(type, TFAWUtils.GetT2DAsset(path2));

                if (path3 != "") {
                    glowTextures.TryAdd(type, TFAWUtils.GetT2DAsset(path3));
                }
            }
        }
        void ITFAWLoader.UnLoadData() {
            Swings = null;
            SwingFullNameToType = null;
            trailTextures = null;
            gradientTextures = null;
            glowTextures = null;
        }
    }
}
