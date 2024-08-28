using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Item1;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;

namespace ThisTianFaAndWuJingMod.Content.Item2
{
    internal class Endless : ModItem
    {
        public static Color[] rainbowColors = [Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet];
        private int fireIndex;
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
            fireIndex = 0;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
        }

        public override void ModifyWeaponCrit(Player player, ref float crit) {
            crit = 9999;
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
                , TFAWUtils.MultiStepColorLerp(Main.GameUpdateCount % 120 / 120f, rainbowColors)
                , line.Rotation, line.Origin, line.BaseScale * 1.05f, line.MaxWidth, line.Spread);
           
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position
            , Vector2 velocity, int type, int damage, float knockback) {
            int newLevel = 0;
            if (++fireIndex > 6) {
                newLevel = 1;
                fireIndex = 0;
            }
            if (player.altFunctionUse == 2) {
                newLevel = 2;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, newLevel);
            return false;
        }
    }
}
