using InnoVault;
using InnoVault.PRT;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Particles;

namespace ThisTianFaAndWuJingMod.Content.Endleses
{
    internal class EndlessHeldAlt : ModProjectile
    {
        public override string Texture => "ThisTianFaAndWuJingMod/Content/Endleses/Endless";
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 254;
            Projectile.timeLeft = 180;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 4;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 2;
            Projectile.tileCollide = false;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
            if (TFAWMod.Instance.ModHasSetVst) {
                Projectile.DamageType = TFAWMod.Instance.CWRMod.Find<DamageClass>("EndlessDamageClass");
            }
        }

        public override void AI() {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
            if (++Projectile.ai[0] > 60) {
                Projectile.scale += 0.004f;
            }

            if (++Projectile.ai[1] > 10 && Projectile.ai[2] < 3 && Projectile.IsOwnedByLocalPlayer()) {
                Projectile.NewProjectile(Projectile.GetSource_FromAI()
                    , Projectile.Center + new Vector2(0, -600), new Vector2(Projectile.velocity.X * 0.1f, 16)
                    , ModContent.ProjectileType<EXEndlessProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.ai[1] = 0;
                Projectile.ai[2]++;
            }

            for (int i = 0; i < 19; i++) {
                BasePRT particle = new PRT_Light(
                    Projectile.Center + Projectile.velocity.UnitVector() * i * 2, Vector2.Zero
                        , 0.2f, Color.BlueViolet, 26, 0.2f);
                PRTLoader.AddParticle(particle);
            }
        }

        public override bool PreDraw(ref Color lightColor) {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() / 2;
            for (int k = 0; k < Projectile.oldPos.Length; k++) {
                Vector2 offsetPos = Projectile.oldPos[k].To(Projectile.position);
                Vector2 drawPos = Projectile.Center - Main.screenPosition - offsetPos;
                Color color = Main.DiscoColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            VaultUtils.DrawRotatingMarginEffect(Main.spriteBatch, texture, Projectile.timeLeft, Projectile.Center - Main.screenPosition
                , null, Main.DiscoColor, Projectile.rotation, drawOrigin, Projectile.scale * 1.05f, 0);

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Color.White
                , Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}
