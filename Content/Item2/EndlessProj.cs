using InnoVault;
using InnoVault.PRT;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Particles;

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
            if (TFAWMod.Instance.ModHasSetVst) {
                Projectile.DamageType = TFAWMod.Instance.CWRMod.Find<DamageClass>("EndlessDamageClass");
            }
        }

        public override void AI() {
            Lighting.AddLight(Projectile.Center, Color.White.ToVector3());
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Projectile.timeLeft < 220) {
                NPC targetNPC = Projectile.Center.FindClosestNPC(1600);
                if (targetNPC != null) {
                    Projectile.SmoothHomingBehavior(targetNPC.Center, 1.001f, 0.2f);
                }
            }

            for (int i = 0; i < 6; i++) {
                BasePRT spark = new PRT_RTSpark(Projectile.Center, Projectile.velocity, false, 19, 1.3f);
                PRTLoader.AddParticle(spark);
            }
        }
    }
}
