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
    }
}
