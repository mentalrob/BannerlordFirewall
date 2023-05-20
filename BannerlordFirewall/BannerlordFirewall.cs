using BannerlordFirewall.MissionBehaviors;
using BannerlordFirewall.PatchedCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.PlayerServices;
using WindowsFirewallHelper;

namespace BannerlordFirewall
{
    public class BannerlordFirewall : MBSubModuleBase
    {
        public static BannerlordFirewall Instance;
        public Dictionary<PlayerId, IAddress> WhitelistedIps;

        private IFirewallRule _cachedFirewallRule;
        public string GetFirewallRuleName() {
            int port = Module.CurrentModule.StartupInfo.ServerPort;
            return "Bannerlord Firewall " + port.ToString();
        }
        public IFirewallRule GetFirewallRule() {
            if(this._cachedFirewallRule == null)
            {
                this._cachedFirewallRule = FirewallManager.Instance.Rules.SingleOrDefault(r => r.Name == this.GetFirewallRuleName());
            }
            return this._cachedFirewallRule;
        }

        public void CreateFirewallRule() {
            this._cachedFirewallRule = FirewallManager.Instance.CreatePortRule(
                FirewallProfiles.Domain | FirewallProfiles.Private | FirewallProfiles.Public,
                this.GetFirewallRuleName(), 
                FirewallAction.Allow, 
                Convert.ToUInt16(Module.CurrentModule.StartupInfo.ServerPort), FirewallProtocol.UDP
            );
            this._cachedFirewallRule.IsEnable = true;
            this._cachedFirewallRule.Direction = FirewallDirection.Inbound;
            FirewallManager.Instance.Rules.Add(this._cachedFirewallRule);
            Debug.Print("[BannerlordFirewall] FirewallRule " + this.GetFirewallRuleName() + " is created for your bannerlord server.", 0, Debug.DebugColor.Green);
        }

        private void PrintBanner() {
            string banner = @"
                                         
 _____                     _           _ 
| __  |___ ___ ___ ___ ___| |___ ___ _| |
| __ -| .'|   |   | -_|  _| | . |  _| . |
|_____|__,|_|_|_|_|___|_| |_|___|_| |___|
                                         
                                         
 _____ _                   _ _           
|   __|_|___ ___ _ _ _ ___| | |          
|   __| |  _| -_| | | | .'| | |          
|__|  |_|_| |___|_____|__,|_|_|          

- Made by mentalrob and haliliceylan with love                                         
";
            Debug.Print(banner, 0, Debug.DebugColor.Cyan);
        }

        protected HarmonyLib.Harmony HarmonyHandle;
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            this.PrintBanner();
            BannerlordFirewall.Instance = this;
            this.WhitelistedIps = new Dictionary<PlayerId, IAddress>();
            // Initialize harmony lib.
            this.HarmonyHandle = new HarmonyLib.Harmony("mentalrob.bannerlordfirewall.bannerlord");
            // Patch OnClientWantsToConnectCustomGameMessage to get ip address of connecting players. This information provided by TaleWorlds master server.
            var original = typeof(CustomBattleServer).GetMethod("OnClientWantsToConnectCustomGameMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var prefix = typeof(PatchCustomBattleServer).GetMethod("PrefixOnClientWantsToConnectCustomGameMessage", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            this.HarmonyHandle.Patch(original, prefix: new HarmonyLib.HarmonyMethod(prefix));

            // Check if firewall rule is present, if not create one.
            if (this.GetFirewallRule() == null)
            {
                Debug.Print("[BannerlordFirewall] FirewallRule " + this.GetFirewallRuleName() + " not found on your server. Creating...", 0, Debug.DebugColor.Red);
                this.CreateFirewallRule();
            }
        }

        public override void OnBeforeMissionBehaviorInitialize(Mission mission)
        {
            Debug.Print("[BannerlordFirewall] Trying to add RemoveIpBehavior...", 0, Debug.DebugColor.DarkYellow);
            mission.AddMissionBehavior(new RemoveIpBehavior());
        }
    }
}
