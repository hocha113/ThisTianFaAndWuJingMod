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
            if (TFAWMod.Instance.ModHasSetVst) {
                string text1 = "配方已经装载进欧米茄物质聚合仪，灾厄大修作者在此祝您游戏愉快:)";
                string text2 = "The recipe has been loaded into the Omega Matter Synthesizer. " +
                    "\nThe Calamity Overhaul author wishes you an enjoyable game :)";
                SpwanTextProj.New(Player, () => TFAWUtils.Text(TFAWUtils.Translation(text1, text2), Color.GreenYellow), 360);
            }
            else {
                string text1 = "检测到您安装了灾厄大修，但灾厄大修版本过低，无法加载向欧米茄终焉合成台加载合成内容，请确保灾厄大修版本高于或者等于0.4044";
                string text2 = "It has been detected that you have installed Calamity Overhaul, " +
                    "\nbut the version is too low to load crafting content onto the Omega End Synthesizer. " +
                    "\nPlease ensure that your Calamity Overhaul version is 0.4044 or higher.";
                SpwanTextProj.New(Player, () => TFAWUtils.Text(TFAWUtils.Translation(text1, text2), Color.Red), 360);
            }
        }
    }
}
