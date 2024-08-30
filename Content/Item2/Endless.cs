using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ThisTianFaAndWuJingMod.Content.Item1;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework.Input;

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
        public override void AddRecipes() {
            if (TFAWMod.Instance.ModHasSetVst) {
                Recipe recipe = CreateRecipe().
                    AddIngredient(TFAWMod.Instance.CWRMod.Find<ModItem>("DawnshatterAzure"), 1).
                    AddIngredient(TFAWMod.Instance.CWRMod.Find<ModItem>("NeutronGlaive"), 1).
                    AddIngredient(ItemID.Zenith, 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("ArkoftheCosmos"), 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("Murasama"), 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("IridescentExcalibur"), 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("Ataraxia"), 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("GaelsGreatsword"), 1).
                    AddIngredient(ModLoader.GetMod("CalamityMod").Find<ModItem>("Exoblade"), 1).
                    AddIngredient(TFAWMod.Instance.CWRMod.Find<ModItem>("InfiniteIngot"), 23).
                    AddTile(TFAWMod.Instance.CWRMod.Find<ModTile>("TransmutationOfMatter").Type);
                ((Recipe)TFAWMod.Instance.CWRMod.Call(2, recipe)).Register();
                return;
            }
            CreateRecipe().
                AddIngredient(ItemID.DirtBlock, 1).
                Register();
        }
        public static Color[] rainbowColors = [Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Violet];
        private int fireIndex;
        void ILoader.LoadData() {
            if (TFAWMod.Instance.ModHasSetVst) {
                TFAWMod.Instance.CWRMod.Call(0, fullItems);
            }
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
            fireIndex = 0;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            if (TFAWMod.Instance.ModHasSetVst) {
                TFAWMod.Instance.CWRMod.Call(1, Item, fullItems);
            }
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
                newLevel = 3;
            }
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W)) {
                newLevel = 2;
            }
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, newLevel);
            return false;
        }
    }
}
