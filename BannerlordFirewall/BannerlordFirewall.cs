using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;

namespace BannerlordFirewall
{
    public class BannerlordFirewall : MBSubModuleBase
    {
        protected HarmonyLib.Harmony HarmonyHandle;
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.HarmonyHandle = new HarmonyLib.Harmony("mentalrob.bannerlordfirewall.bannerlord");
            
            

        }
    }
}
