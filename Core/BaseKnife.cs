﻿using InnoVault;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod.Core
{
    internal class BaseKnife : BaseSwing
    {
        public override string Texture => EffectLoader.AssetPath + "placeholder3";
        public override Texture2D TextureValue => TargetID == ItemID.None ? TFAWUtils.GetT2DValue(Texture) : TextureAssets.Item[TargetID].Value;
        public SwingDataStruct SwingData = new SwingDataStruct();
        public SwingAITypeEnum SwingAIType;
        protected float inWormBodysDamageFaul = 0.85f;
        public enum SwingAITypeEnum
        {
            None = 0,
            UpAndDown,
            Down,
            Sceptre,
        }
        public sealed override void SetSwingProperty() {
            Projectile.extraUpdates = 4;
            ownerOrientationLock = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10 * updateCount;
            Projectile.DamageType = DamageClass.Melee;
            SetKnifeProperty();
            VaultUtils.SafeLoadItem(TargetID);
        }

        protected void updaTrailTexture() => SwingSystem.trailTextures[Type] = TFAWUtils.GetT2DAsset(trailTexturePath);
        protected void updaGradientTexture() => SwingSystem.gradientTextures[Type] = TFAWUtils.GetT2DAsset(gradientTexturePath);

        public virtual void SetKnifeProperty() { }

        public sealed override void Initialize() {
            maxSwingTime = Item.useTime;
            SwingData.maxSwingTime = maxSwingTime;
            toProjCoreMode = (IgnoreImpactBoxSize ? 22 : Projectile.width) / 2f;
            if (++SwingIndex > 1) {
                SwingIndex = 0;
            }
            KnifeInitialize();
        }

        public virtual void KnifeInitialize() {

        }

        public virtual void WaveUADBehavior() {

        }

        public virtual void SceptreBehavior() {

        }

        public virtual void MeleeEffect() {

        }

        public sealed override void SwingAI() {
            switch (SwingAIType) {
                case SwingAITypeEnum.None:
                    SwingBehavior(SwingData);
                    break;
                case SwingAITypeEnum.UpAndDown:
                    SwingDataStruct swingData = SwingData;
                    if (SwingIndex == 1) {
                        inDrawFlipdiagonally = true;
                        swingData.starArg += 120;
                        swingData.baseSwingSpeed *= -1;
                    }
                    WaveUADBehavior();
                    SwingBehavior(swingData);
                    break;
                case SwingAITypeEnum.Down:
                    inDrawFlipdiagonally = true;
                    SwingData.starArg += 120;
                    SwingData.baseSwingSpeed *= -1;
                    SwingBehavior(SwingData);
                    break;
                case SwingAITypeEnum.Sceptre:
                    shootSengs = 0.95f;
                    maxSwingTime = 70;
                    canDrawSlashTrail = false;
                    SwingData.starArg = 13;
                    SwingData.baseSwingSpeed = 2;
                    SwingData.ler1_UpLengthSengs = 0.1f;
                    SwingData.ler1_UpSpeedSengs = 0.1f;
                    SwingData.ler1_UpSizeSengs = 0.062f;
                    SwingData.ler2_DownLengthSengs = 0.01f;
                    SwingData.ler2_DownSpeedSengs = 0.14f;
                    SwingData.ler2_DownSizeSengs = 0;
                    SwingData.minClampLength = 160;
                    SwingData.maxClampLength = 200;
                    SwingData.ler1Time = 8;
                    SwingData.maxSwingTime = 60;
                    SceptreBehavior();
                    SwingBehavior(SwingData);
                    break;
            }
        }

        public sealed override void NoServUpdate() {
            if (Time % updateCount == 0) {
                MeleeEffect();
            }
        }
    }
}
