using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using WindowsFirewallHelper;

namespace BannerlordFirewall.MissionBehaviors
{
    public class RemoveIpBehavior : MissionNetwork
    {

        public override void OnBehaviorInitialize()
        {
            base.OnBehaviorInitialize();
            Debug.Print("[BannerlordFirewall] RemoveIpBehavior has been initialized.", 0, Debug.DebugColor.Purple);
        }

        public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
        {
            if (BannerlordFirewall.Instance.GetFirewallRule() == null) return;
            if(BannerlordFirewall.Instance.WhitelistedIps.ContainsKey(networkPeer.PlayerConnectionInfo.PlayerID))
            {
                BannerlordFirewall.Instance.WhitelistedIps.Remove(networkPeer.PlayerConnectionInfo.PlayerID);
                IAddress[] addresses = BannerlordFirewall.Instance.WhitelistedIps.Values.ToArray();
                Debug.Print("[BannerlordFirewall] " + networkPeer.UserName + " was removed from the firewall whitelist, whitelisted ip count: " + addresses.Length.ToString(), 0, Debug.DebugColor.Red);
                BannerlordFirewall.Instance.GetFirewallRule().RemoteAddresses = addresses;
            }
        }
    }
}
