﻿global using Microsoft.Xna.Framework;
using InnoVault;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ThisTianFaAndWuJingMod
{
    public class TFAWMod : Mod
    {
        public static TFAWMod Instance => (TFAWMod)ModLoader.GetMod("ThisTianFaAndWuJingMod");
        public Mod CWRMod;
        public bool ModHasAddE {
            get {
                if (!ModHasSetVst) {
                    return false;
                }
                return (bool)CWRMod.Call(4);
            }
        }
        public bool ModHasSetVst {
            get {
                if (CWRMod == null) {
                    return false;
                }
                return CWRMod.Version >= new Version(0, 4046);
            }
        }
        internal static List<ITFAWLoader> ILoaders { get; private set; }
        public override void Load() {
            CWRMod = null;
            ModLoader.TryGetMod("CalamityOverhaul", out CWRMod);
            ILoaders = VaultUtils.GetSubInterface<ITFAWLoader>();
            foreach (ITFAWLoader setup in ILoaders) {
                setup.LoadData();
            }
        }

        public override void PostSetupContent() {
            foreach (ITFAWLoader setup in ILoaders) {
                setup.SetupData();
                if (!Main.dedServ) {
                    setup.LoadAsset();
                }
            }
        }

        public override void Unload() {
            foreach (ITFAWLoader setup in ILoaders) {
                setup.UnLoadData();
            }
            ILoaders = null;
        }
    }
}
