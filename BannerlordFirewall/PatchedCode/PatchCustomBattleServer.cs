using Messages.FromCustomBattleServerManager.ToCustomBattleServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using WindowsFirewallHelper.Addresses;

namespace BannerlordFirewall.PatchedCode
{
    public class PatchCustomBattleServer
    {
        public static bool PrefixOnClientWantsToConnectCustomGameMessage(ClientWantsToConnectCustomGameMessage message)
        {
            if (BannerlordFirewall.Instance.GetFirewallRule() == null) return true;
            /**
             * First iterate the connecting players data, get their ip addresses.
             * Check if the ip address is not 0.0.0.0 (If we don't check this and add it to firewall, firewall basically allows anyone)
             * Add the ip addresses to whitelisted ips
             * Apply it to firewall rule
             * */
            foreach(PlayerJoinGameData playerData in message.PlayerJoinGameData)
            {
                if (playerData.IpAddress == "0.0.0.0") continue;
                SingleIP firewallIp = SingleIP.Parse(playerData.IpAddress);
                BannerlordFirewall.Instance.WhitelistedIps[playerData.PlayerId] = firewallIp;
                Debug.Print("[BannerlordFirewall] " + playerData.IpAddress + " added to whitelisted ip address", 0, Debug.DebugColor.Green);
            }

            BannerlordFirewall.Instance.GetFirewallRule().RemoteAddresses = BannerlordFirewall.Instance.WhitelistedIps.Values.ToArray();
            return true;
        }
    }
}
