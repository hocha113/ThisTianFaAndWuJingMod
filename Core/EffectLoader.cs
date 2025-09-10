using InnoVault;
using InnoVault.PRT;
using InnoVault.RenderHandles;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Endleses;
using ThisTianFaAndWuJingMod.Content.Nemesies;
using ThisTianFaAndWuJingMod.Content.Particles;

namespace ThisTianFaAndWuJingMod.Core
{
    [VaultLoaden("Asset/")]
    public class EffectLoader : RenderHandle, ITFAWLoader
    {
        internal static Effect StarsShader { get; set; }
        internal static Effect PowerSFShader;
        internal static Effect KnifeRendering;
        internal static Effect StarsTrail;
        internal static float twistStrength = 0f;
        public const string AssetPath = "ThisTianFaAndWuJingMod/Asset/";
        public const string AssetPath2 = "Asset/";
        public override void EndCaptureDraw(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, RenderTarget2D screenSwap) {
            if (HasWarpEffect(out List<IDrawWarp> warpSets, out List<IDrawWarp> warpSetsNoBlueshift)) {
                if (warpSets.Count > 0) {
                    //绘制屏幕
                    graphicsDevice.SetRenderTarget(screenSwap);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Begin(0, BlendState.AlphaBlend);
                    Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                    Main.spriteBatch.End();
                    //绘制需要绘制的内容
                    graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                    graphicsDevice.Clear(Color.Transparent);

                    Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None
                        , RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    foreach (IDrawWarp p in warpSets) { p.Warp(); }
                    Main.spriteBatch.End();

                    //应用扭曲
                    graphicsDevice.SetRenderTarget(Main.screenTarget);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    //如果想热加载，最好这样获取值
                    Effect effect = TFAWMod.Instance.Assets.Request<Effect>(AssetPath2 + "WarpShader").Value;//EffectsRegistry.WarpShader;
                    effect.Parameters["tex0"].SetValue(Main.screenTargetSwap);
                    effect.Parameters["noBlueshift"].SetValue(false);//这个部分的绘制需要使用蓝移效果
                    effect.Parameters["i"].SetValue(0.02f);
                    effect.CurrentTechnique.Passes[0].Apply();
                    Main.spriteBatch.Draw(screenSwap, Vector2.Zero, Color.White);
                    Main.spriteBatch.End();

                    Main.spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState
                        , default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    foreach (IDrawWarp p in warpSets) { if (p.canDraw()) { p.costomDraw(Main.spriteBatch); } }
                    Main.spriteBatch.End();
                }
                if (warpSetsNoBlueshift.Count > 0) {
                    //绘制屏幕
                    graphicsDevice.SetRenderTarget(screenSwap);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Begin(0, BlendState.AlphaBlend);
                    Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                    Main.spriteBatch.End();
                    //绘制需要绘制的内容
                    graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                    graphicsDevice.Clear(Color.Transparent);

                    Main.spriteBatch.Begin(0, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None
                        , RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    foreach (IDrawWarp p in warpSetsNoBlueshift) { p.Warp(); }
                    Main.spriteBatch.End();

                    //应用扭曲
                    graphicsDevice.SetRenderTarget(Main.screenTarget);
                    graphicsDevice.Clear(Color.Transparent);
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                    //如果想热加载，最好这样获取值
                    Effect effect = TFAWMod.Instance.Assets.Request<Effect>(AssetPath2 + "WarpShader").Value;//EffectsRegistry.WarpShader;
                    effect.Parameters["tex0"].SetValue(Main.screenTargetSwap);
                    effect.Parameters["noBlueshift"].SetValue(true);//这个部分的绘制不需要使用蓝移效果
                    effect.Parameters["i"].SetValue(0.02f);
                    effect.CurrentTechnique.Passes[0].Apply();
                    Main.spriteBatch.Draw(screenSwap, Vector2.Zero, Color.White);
                    Main.spriteBatch.End();

                    Main.spriteBatch.Begin(default, BlendState.AlphaBlend, Main.DefaultSamplerState
                        , default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                    foreach (IDrawWarp p in warpSetsNoBlueshift) { if (p.canDraw()) { p.costomDraw(Main.spriteBatch); } }
                    Main.spriteBatch.End();
                }
            }

            if (Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<EndSkillOrbOnSpan>()] > 0) {
                graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                graphicsDevice.Clear(Color.Transparent);//用透明清除
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                graphicsDevice.SetRenderTarget(screenSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointWrap
                    , DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
                DrawPwoerEffect(Main.spriteBatch);
                Main.spriteBatch.End();
                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                PowerSFShader.Parameters["tex0"].SetValue(screenSwap);
                PowerSFShader.Parameters["i"].SetValue(twistStrength);
                PowerSFShader.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            #region RT粒子特效
            if (PRT_RTSpark.HasSet(out List<BasePRT> prts)) {
                graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();

                graphicsDevice.SetRenderTarget(screenSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                PRT_RTSpark.DrawAll(Main.spriteBatch, prts);
                Main.spriteBatch.End();

                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                StarsShader.CurrentTechnique.Passes[0].Apply();
                StarsShader.Parameters["m"].SetValue(0.08f);
                StarsShader.Parameters["n"].SetValue(0.01f);
                StarsShader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly); // 传入游戏时间
                StarsShader.Parameters["worldSize"].SetValue(Main.ScreenSize.ToVector2()); // 传入屏幕分辨率
                Main.spriteBatch.Draw(screenSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            if (EndlessHeld.HasSet(out EndlessHeld endless) && endless.prtGroup != null) {
                graphicsDevice.SetRenderTarget(Main.screenTargetSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTarget, Vector2.Zero, Color.White);
                Main.spriteBatch.End();

                graphicsDevice.SetRenderTarget(screenSwap);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
                endless.prtGroup.Draw(Main.spriteBatch);
                Main.spriteBatch.End();

                graphicsDevice.SetRenderTarget(Main.screenTarget);
                graphicsDevice.Clear(Color.Transparent);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                Main.spriteBatch.Draw(Main.screenTargetSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                StarsShader.CurrentTechnique.Passes[0].Apply();
                StarsShader.Parameters["m"].SetValue(0.08f);
                StarsShader.Parameters["n"].SetValue(0.01f);
                StarsShader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly); // 传入游戏时间
                StarsShader.Parameters["worldSize"].SetValue(Main.ScreenSize.ToVector2()); // 传入屏幕分辨率
                Main.spriteBatch.Draw(screenSwap, Vector2.Zero, Color.White);
                Main.spriteBatch.End();
            }

            #endregion
        }

        private static void DrawPwoerEffect(SpriteBatch sb) {
            int targetProjType = ModContent.ProjectileType<EndSkillOrbOnSpan>();
            foreach (Projectile proj in Main.projectile) {
                Vector2 offsetRotV = proj.rotation.ToRotationVector2() * 1500;
                if (proj.type == targetProjType && proj.active) {
                    Texture2D texture = TFAWUtils.GetT2DValue(AssetPath + "placeholder2");
                    int length = (int)(Math.Sqrt(Main.screenWidth * Main.screenWidth + Main.screenHeight * Main.screenHeight) * 4f);
                    sb.Draw(texture,
                        proj.Center + Vector2.Normalize((proj.Left - proj.Center).RotatedBy(proj.rotation)) * length / 2 - Main.screenPosition + offsetRotV,
                        new(0, 0, 1, 1),
                        new(TFAWUtils.GetCorrectRadian(proj.rotation), proj.ai[0], 0f, 0.2f),
                        proj.rotation,
                        Vector2.Zero,
                        length, SpriteEffects.None, 0);
                    sb.Draw(texture,
                        proj.Center + Vector2.Normalize((proj.Left - proj.Center).RotatedBy(proj.rotation)) * length / 2 - Main.screenPosition + offsetRotV,
                        new(0, 0, 1, 1),
                        new(TFAWUtils.GetCorrectRadian(proj.rotation) + Math.Sign(proj.rotation + 0.001f) * 0.5f, proj.ai[0], 0f, 0.2f),
                        proj.rotation,
                        new(0, 1),
                        length,
                        SpriteEffects.None, 0);
                    twistStrength = 0.055f * proj.localAI[0];
                }
            }
        }

        private static bool HasWarpEffect(out List<IDrawWarp> warpSets, out List<IDrawWarp> warpSetsNoBlueshift) {
            warpSets = [];
            warpSetsNoBlueshift = [];
            foreach (Projectile p in Main.projectile) {
                if (!p.active) {
                    continue;
                }
                if (p.ModProjectile is IDrawWarp drawWarp) {
                    if (drawWarp.noBlueshift()) {
                        warpSetsNoBlueshift.Add(drawWarp);
                    }
                    else {
                        warpSets.Add(drawWarp);
                    }
                }
            }
            return warpSets.Count > 0 || warpSetsNoBlueshift.Count > 0;
        }
    }
}
