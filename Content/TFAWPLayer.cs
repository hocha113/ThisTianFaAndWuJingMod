using InnoVault;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod.Content
{
    public class TFAWPLayer : ModPlayer
    {
        public int SwingIndex;
        public int DontUseItemTime;
        public override void PostUpdate() {
            if (DontUseItemTime > 0) {
                DontUseItemTime--;
            }
        }

        public override void OnEnterWorld() {
            string modName = "[Ultimate Infinite Star]: ";
            if (TFAWMod.Instance.CWRMod == null) {
                string text1 = "检测到您并未开启灾厄大修进行游玩，本模组的武器的合成配方无法正常加载，作为替换备案，您将可以使用泥土制作这些武器";
                string text2 = "It has been detected that you are not playing with the Calamity Overhaul enabled, " +
                    "\nso the crafting recipes for this mod's weapons cannot be properly loaded. As an alternative, " +
                    "\nyou will be able to craft these weapons using dirt.";
                SpwanTextProj.New(Player, () => VaultUtils.Text(modName + VaultUtils.Translation(text1, text2), Color.Red), 360);
                return;
            }

            if (TFAWMod.Instance.ModHasSetVst) {
                string text1 = "配方已经装载进欧米茄物质聚合仪，灾厄大修作者在此祝您游戏愉快:)";
                string text2 = "The recipe has been loaded into the Omega Matter Synthesizer. " +
                    "\nThe Calamity Overhaul author wishes you an enjoyable game :)";
                SpwanTextProj.New(Player, () => VaultUtils.Text(modName + VaultUtils.Translation(text1, text2), Color.GreenYellow), 360);
            }
            else {
                string text1 = "检测到您安装了灾厄大修，但灾厄大修版本过低，无法向加载欧米茄终焉合成台加载合成内容，请确保灾厄大修版本高于或者等于0.4046";
                string text2 = "It has been detected that you have installed Calamity Overhaul, " +
                    "\nbut the version is too low to load crafting content onto the Omega End Synthesizer. " +
                    "\nPlease ensure that your Calamity Overhaul version is 0.4046 or higher.";
                SpwanTextProj.New(Player, () => VaultUtils.Text(modName + VaultUtils.Translation(text1, text2), Color.Red), 360);
            }
        }
    }
}
