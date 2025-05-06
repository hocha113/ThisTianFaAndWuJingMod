using InnoVault;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using ThisTianFaAndWuJingMod.Content.Item1;
using ThisTianFaAndWuJingMod.Core;

namespace ThisTianFaAndWuJingMod.Content.Item2
{
    internal class Endless : ModItem, ILoader
    {
        static string[] fullItems = ["0", "0", "0", "0", "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot",
            "0", "0", "0", "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/NeutronGlaive", "CalamityOverhaul/InfiniteIngot",
            "0", "0", "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "ThisTianFaAndWuJingMod/Nemesis", "CalamityOverhaul/InfiniteIngot", "0",
            "0", "0", "0", "0", "CalamityOverhaul/InfiniteIngot", "CalamityMod/ArkoftheCosmos", "CalamityOverhaul/InfiniteIngot", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "CalamityOverhaul/InfiniteIngot", "4956", "CalamityOverhaul/InfiniteIngot", "0", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityMod/Murasama", "CalamityOverhaul/InfiniteIngot", "CalamityMod/IridescentExcalibur", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0",
            "0", "CalamityOverhaul/InfiniteIngot", "CalamityMod/Ataraxia", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0", "0",
            "CalamityOverhaul/InfiniteIngot", "CalamityMod/GaelsGreatsword", "CalamityOverhaul/InfiniteIngot", "CalamityMod/Exoblade", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0",
            "CalamityOverhaul/DawnshatterAzure", "CalamityOverhaul/InfiniteIngot", "0", "CalamityOverhaul/InfiniteIngot", "CalamityOverhaul/InfiniteIngot", "0", "0", "0", "0",
            "ThisTianFaAndWuJingMod/Endless"
        ];
        public static Color[] rainbowColors = [Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet];
        private int fireIndex;
        private float Charge;
        private const float MaxCharge = 100;
        void ILoader.LoadData() {
            if (TFAWMod.Instance.ModHasSetVst) {
                TFAWMod.Instance.CWRMod.Call(0, fullItems);
            }
        }
        public override void AddRecipes() {
            if (TFAWMod.Instance.ModHasSetVst) {
                return;//如果能成功加载，就不要添加下面的配方
            }
            CreateRecipe().
                AddIngredient(ItemID.DirtBlock, 1).
                Register();
        }
        public override void SetDefaults() {
            Item.height = 154;
            Item.width = 154;
            Item.damage = 9999;
            Item.DamageType = DamageClass.Melee;
            Item.useAnimation = Item.useTime = 20;
            Item.scale = 1;
            Item.useTurn = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = true;
            Item.knockBack = 5.5f;
            Item.UseSound = null;
            Item.autoReuse = true;
            Item.value = Item.buyPrice(155, 33, 15, 0);
            Item.rare = ItemRarityID.Red;
            Item.shoot = ModContent.ProjectileType<NemesisProj>();
            Item.shootSpeed = 18f;
            Item.SetKnifeHeld<EndlessHeld>();
            Charge = fireIndex = 0;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            if (TFAWMod.Instance.ModHasSetVst) {
                TFAWMod.Instance.CWRMod.Call(1, Item, fullItems);
                Item.DamageType = TFAWMod.Instance.CWRMod.Find<DamageClass>("EndlessDamageClass");
            }
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) {
            crit = 9999;
        }

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale) {
            if (!(Charge <= 0f)) {//这是一个通用的进度条绘制，用于判断充能进度
                Texture2D barBG = ModContent.Request<Texture2D>(EffectLoader.AssetPath + "GenericBarBack", (AssetRequestMode)2).Value;
                Texture2D barFG = ModContent.Request<Texture2D>(EffectLoader.AssetPath + "GenericBarFront", (AssetRequestMode)2).Value;
                float barScale = 5f;
                Vector2 barOrigin = barBG.Size() * 0.5f;
                float yOffset = 50f;
                Vector2 drawPos = position + Vector2.UnitY * scale * (frame.Height - yOffset);
                Rectangle frameCrop = new Rectangle(0, 0, (int)(Charge / MaxCharge * barFG.Width), barFG.Height);
                Color color = Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.6f % 1f, 1f, 0.75f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f) * 0.1f);
                spriteBatch.Draw(barBG, drawPos, null, color, 0f, barOrigin, scale * barScale, 0, 0f);
                spriteBatch.Draw(barFG, drawPos, frameCrop, color * 0.8f, 0f, barOrigin, scale * barScale, 0, 0f);
            }
        }

        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset) {
            if (line.Name == "ItemName" && line.Mod == "Terraria") {
                _ = Main.DiscoColor;
                Vector2 basePosition = new Vector2(line.X, line.Y);
                string text = Language.GetTextValue("Mods.ThisTianFaAndWuJingMod.Items.Endless.DisplayName");
                drawColorText(Main.spriteBatch, line, text, basePosition);
                return false;
            }
            return true;
        }

        public static void drawColorText(SpriteBatch sb, DrawableTooltipLine line, string text, Vector2 basePosition) {
            ChatManager.DrawColorCodedStringWithShadow(sb, line.Font, line.Text, basePosition
                , VaultUtils.MultiStepColorLerp(Main.GameUpdateCount % 120 / 120f, rainbowColors)
                , line.Rotation, line.Origin, line.BaseScale * 1.05f, line.MaxWidth, line.Spread);

        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position
            , Vector2 velocity, int type, int damage, float knockback) {
            Charge += 5;
            if (Charge > MaxCharge) {
                Charge = MaxCharge;
            }
            int newLevel = 0;
            if (++fireIndex > 6) {
                newLevel = 1;
                fireIndex = 0;
            }
            if (player.altFunctionUse == 2) {
                newLevel = 3;
            }

            KeyboardState state = Keyboard.GetState();

            if (Charge >= MaxCharge) {
                if (state.IsKeyDown(Keys.LeftShift)) {
                    SoundEngine.PlaySound(new SoundStyle(EffectLoader.AssetPath + "Pecharge"), player.Center);
                    Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<EndlessHeldAlt>(), damage, knockback, player.whoAmI, newLevel);
                    Charge = 0;
                    return false;
                }
                else if (state.IsKeyDown(Keys.Q)) {
                    SoundEngine.PlaySound(new SoundStyle(EffectLoader.AssetPath + "Pecharge"), player.Center);
                    newLevel = 2;
                    Charge = 0;
                }

            }

            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, newLevel);
            return false;
        }
    }
}
