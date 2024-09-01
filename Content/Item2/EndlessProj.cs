using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Particles;
using ThisTianFaAndWuJingMod.Core;

namespace ThisTianFaAndWuJingMod.Content.Item2
{
    internal class EndlessProj : ModProjectile
    {
        public override void SetDefaults() {
            Projectile.width = Projectile.height = 22;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.timeLeft = 300;
            Projectile.extraUpdates = 2;
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.timeLeft < 220) {
                NPC targetNPC = Projectile.Center.FindClosestNPC(1600);
                if (targetNPC != null) {
                    Projectile.ChasingBehavior2(targetNPC.Center, 1.001f, 0.2f);
                }
            }
            
            for (int i = 0; i < 9; i++) {
                BaseParticle particle = new PRT_Light_SpecialColoring(
                    Projectile.Center + Projectile.velocity.UnitVector() * i * 2, Vector2.Zero
                        , 0.1f, Color.BlueViolet, 26, 0.2f);
                PRTLoader.AddParticle(particle);
            }
        }
    }
}
