﻿using InnoVault;
using InnoVault.PRT;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Particles;
using ThisTianFaAndWuJingMod.Core;

namespace ThisTianFaAndWuJingMod.Content.Endleses
{
    internal class EndlessChopping : ModProjectile
    {
        public override string Texture => EffectLoader.AssetPath + "placeholder3";
        Vector2 origVer;
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
            if (TFAWMod.Instance.ModHasSetVst) {
                Projectile.DamageType = TFAWMod.Instance.CWRMod.Find<DamageClass>("EndlessDamageClass");
            }
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
                    Vector2 ver = origVer.UnitVector() * (i / 133f * 133 + 0.1f);
                    BasePRT spark = new PRT_RTSpark(Projectile.Center, -ver, false, 19, 2.3f);
                    PRTLoader.AddParticle(spark);
                    BasePRT spark2 = new PRT_RTSpark(Projectile.Center, ver, false, 19, 2.3f);
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

        public override bool PreDraw(ref Color lightColor) {
            return false;
        }
    }
}
