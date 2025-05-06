using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod.Content.Item1
{
    internal class Nemesis : ModItem, ILoader
    {
        private int fireIndex;
        static string[] fullItems = ["0", "0", "0", "0", "0", "0", "0", "3458", "3458",
            "0", "0", "0", "0", "0", "0", "3458", "CalamityMod/TheBurningSky", "3458",
            "0", "0", "0", "0", "0", "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0",
            "0", "0", "0", "0", "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0", "0",
            "0", "0", "0", "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0", "0", "0",
            "0", "0", "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0", "0", "0", "0",
            "3458", "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0", "0", "0", "0", "0",
            "3458", "CalamityOverhaul/NeutronStarIngot", "3458", "0", "0", "0", "0", "0", "0",
            "CalamityMod/Earth", "3458", "3458", "0", "0", "0", "0", "0", "0",
            "ThisTianFaAndWuJingMod/Nemesis"
        ];
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
            Item.damage = 420;
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
            Item.SetKnifeHeld<NemesisHeld>();
            fireIndex = 0;
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            if (TFAWMod.Instance.ModHasSetVst) {
                TFAWMod.Instance.CWRMod.Call(1, Item, fullItems);
            }
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
