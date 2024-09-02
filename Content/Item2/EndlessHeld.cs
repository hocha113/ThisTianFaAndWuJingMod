using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Item1;
using ThisTianFaAndWuJingMod.Content.Particles;
using ThisTianFaAndWuJingMod.Core;

namespace ThisTianFaAndWuJingMod.Content.Item2
{
    internal class EndlessHeld : BaseKnife
    {
        public override int TargetID => ModContent.ItemType<Endless>();
        public override string trailTexturePath => EffectLoader.AssetPath + "MotionTrail4";
        public override string gradientTexturePath => EffectLoader.AssetPath + "ShaduraGradient";
        Vector2 TargetPos;
        public override void SetKnifeProperty() {
            Projectile.width = Projectile.height = 282;
            overOffsetCachesRoting = MathHelper.ToRadians(8);
            IgnoreImpactBoxSize = true;
            drawTrailHighlight = false;
            canDrawSlashTrail = true;
            Incandescence = true;
            drawTrailBtommWidth = 90;
            drawTrailTopWidth = 130;
            distanceToOwner = 120;
            OtherMeleeSize = 1f;
            unitOffsetDrawZkMode = -8;
            SwingData.starArg = 60;
            SwingData.baseSwingSpeed = 9;
            drawTrailCount = 30;
            ShootSpeed = 20;
            Length = 104;
        }

        public override void Shoot() {
            if (Projectile.ai[0] == 2 || Projectile.ai[0] == 3) {
                return;
            }
            if (Projectile.ai[0] == 1) {
                for (int i = 0; i < 33; i++) {
                    Projectile.NewProjectile(Source, Main.MouseWorld, (MathHelper.TwoPi / 33f * i).ToRotationVector2() * 3
                    , ModContent.ProjectileType<EndlessChopping>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, i * 2);
                }
                return;
            }
            for (int i = 0; i < 3; i++) {
                Projectile.NewProjectile(Source, Main.MouseWorld, TFAWUtils.randVr(3, 6)
                , ModContent.ProjectileType<EndlessChopping>(), Projectile.damage, Projectile.knockBack, Projectile.owner, 0, Main.rand.Next(12));
            }
        }

        public override bool PreInOwnerUpdate() {
            if (Time == 0) {
                TargetPos = Projectile.Center;
                SoundEngine.PlaySound(SoundID.Item71, Owner.position);
            }

            if (Projectile.ai[0] == 4) {
                Owner.Center = Vector2.Lerp(Owner.Center, Projectile.Center, 0.1f);
                
                if (Owner.Distance(Projectile.Center) < 160) {
                    Owner.TFAW().DontUseItemTime = 160;

                    for (int i = 0; i < 333; i++) {
                        BaseParticle particle = new PRT_Light(Owner.Center, TFAWUtils.randVr(36, 116)
                        , Main.rand.NextFloat(0.3f, 0.7f), Main.DiscoColor, 12, 0.2f, _entity: Owner);
                        PRTLoader.AddParticle(particle);
                    }
                    
                    SoundEngine.PlaySound(SoundID.Item69, Owner.position);

                    int maxSpanNum = 23;
                    for (int i = 0; i < maxSpanNum; i++) {
                        Vector2 spanPos = Projectile.Center + TFAWUtils.randVr(1380, 2200);
                        Vector2 vr = spanPos.To(Projectile.Center + TFAWUtils.randVr(180, 320 + 3 * 12)).UnitVector() * 12;
                        Projectile.NewProjectile(Owner.parent(), spanPos, vr, ModContent.ProjectileType<EndSkillOrbOnSpan>()
                            , (int)(Projectile.damage * 3.7f), 0, Owner.whoAmI);
                    }
                    Projectile.Kill();
                }
                return true;
            }

            if (Projectile.ai[0] == 3) {
                canSetOwnerArmBver = false;
                SwingData.baseSwingSpeed = 2;
                SwingData.maxSwingTime = maxSwingTime = 300;
                SwingData.ler1_UpLengthSengs = 0;
                SwingData.ler1_UpSizeSengs = 0;
                SwingData.ler2_DownSpeedSengs = 0;
                if (DownLeft && Time > 160) {
                    Projectile.ai[0] = 4;
                }
                if (Time > 0 && Time % 30 == 0 && Projectile.IsOwnedByLocalPlayer()) {
                    Projectile.NewProjectile(Source, Projectile.Center, TFAWUtils.randVr(13, 16)
                        , ModContent.ProjectileType<EndlessProj>()
                            , (int)(Projectile.damage * 0.7f), 0, Owner.whoAmI);
                }
                return true;
            }

            if (Projectile.ai[0] == 0) {
                SwingData.baseSwingSpeed = 4;
            }

            if (Projectile.ai[0] == 2) {
                canSetOwnerArmBver = false;
                OtherMeleeSize = 6;
                SwingData.maxSwingTime = maxSwingTime = 300;
                SwingData.ler1_UpLengthSengs = 0;
                SwingData.ler1_UpSizeSengs = 0;
                SwingData.ler2_DownSpeedSengs = 0;
                drawTrailBtommWidth += 0.01f;
                BaseParticle particle = new PRT_Light(Owner.Center + new Vector2(0, -Length / 2), TFAWUtils.randVr(6, 116)
                        , Main.rand.NextFloat(0.3f, 0.7f), Main.DiscoColor, 22, 0.2f, _entity: Owner);
                PRTLoader.AddParticle(particle); 
                if (Time % 130 * updateCount == 0 && Time > 180) {
                    foreach (var npc in Main.ActiveNPCs) {
                        if (npc.friendly) {
                            continue;
                        }
                        if (npc.Center.DistanceSQ(Owner.Center) > 3000 * 3000) {
                            continue;
                        }
                        killAction(npc);
                    }
                }
            }
            return base.PreInOwnerUpdate();
        }

        public override Vector2 GetDrawTrailOrig() {
            if (Projectile.ai[0] == 3 || Projectile.ai[0] == 4) {
                return Projectile.Center;
            }
            return base.GetDrawTrailOrig();
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
            if (Projectile.ai[0] == 3) {
                return TFAWUtils.CircularHitboxCollision(Projectile.Center, 280, targetHitbox);
            }
            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void PostInOwnerUpdate() {
            if (Projectile.ai[0] == 3 || Projectile.ai[0] == 4) {
                if (Projectile.ai[0] != 4) {
                    TargetPos = Vector2.Lerp(TargetPos, InMousePos, 0.04f);
                }
                Projectile.Center = TargetPos;
            }  
        }

        static void killAction(NPC npc) {
            npc.dontTakeDamage = false;
            _ = npc.SimpleStrikeNPC(npc.lifeMax, 0);
            npc.life = 0;
            npc.checkDead();
            npc.HitEffect();
            npc.NPCLoot();
            npc.netUpdate = true;
            npc.netUpdate2 = true;
            npc.active = false;
        }

        public override void DrawSwing(SpriteBatch spriteBatch, Color lightColor) {
            if (Projectile.ai[0] == 2) {
                Vector2 drawPos = Owner.Center - Main.screenPosition + new Vector2(0, -Length / 2);
                float dort = -MathHelper.PiOver4;
                Vector2 orig = TextureValue.Size() / 2;
                TFAWUtils.DrawMarginEffect(Main.spriteBatch, TextureValue, Projectile.timeLeft, drawPos
                        , null, Main.DiscoColor, dort, orig, Projectile.scale, SpriteEffects.None);
                Main.EntitySpriteDraw(TextureValue, drawPos, null, Color.White, dort
                    , orig, Projectile.scale, SpriteEffects.None, 0);
                return;
            }
            base.DrawSwing(spriteBatch, lightColor);
        }

        public override void DrawTrail(List<VertexPositionColorTexture> bars) {
            Effect effect = TFAWMod.Instance.Assets.Request<Effect>(EffectLoader.AssetPath2 + "StarsTrail").Value;

            Matrix world = Matrix.CreateTranslation(-Main.screenPosition.Vec3());
            Matrix view = Main.GameViewMatrix.TransformationMatrix;
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, -1, 1);

            effect.Parameters["transformMatrix"].SetValue(world * view * projection);
            effect.Parameters["sampleTexture"].SetValue(TrailTexture);
            effect.Parameters["gradientTexture"].SetValue(GradientTexture);
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
    }
}
