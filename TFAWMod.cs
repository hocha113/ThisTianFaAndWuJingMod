global using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod
{
    public class TFAWMod : Mod
	{
        public static TFAWMod Instance => (TFAWMod)ModLoader.GetMod("ThisTianFaAndWuJingMod");
        public Mod CWRMod;
        internal static List<ILoader> ILoaders { get; private set; }
        public override void Load() {
            ModLoader.TryGetMod("CalamityOverhaul", out CWRMod);
            ILoaders = TFAWUtils.GetSubInterface<ILoader>();
            foreach (ILoader setup in ILoaders) {
                setup.LoadData();
                setup.DompLoadText();
            }
        }

        public override void PostSetupContent() {
            foreach (ILoader setup in ILoaders) {
                setup.SetupData();
                if (!Main.dedServ) {
                    setup.LoadAsset();
                }
            }
        }

        public override void Unload() {
            foreach (ILoader setup in ILoaders) {
                setup.UnLoadData();
                setup.DompUnLoadText();
            }
        }
    }
}
