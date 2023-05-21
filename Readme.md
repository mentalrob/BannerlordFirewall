# Bannerlord Firewall

Bannerlord Firewall is a mod that adds automated firewall rule to the Windows Firewall in order to defend server to DDoS attackers.

### How it works ?

When a player wants to connect to your server, player's client sends a request to Master Server and then Master Server sends a request to your server containing player's ip address. The mod intercepts this request to get player's ip address before the actual connection and whitelists it in automated firewall rule. Based on that the mod filters out any other junk packet from communication.

### Any PoC ?

This system currently used by official Persistent Empires EU and NA server. **With this mod and hosting provided firewall %100 of the attacks have been mitigated.**

### How to setup ?

Download the latest release from releases page (Right side of this page), drag and drop the content of the zip file in your "Modules" folder inside of your server installation folder.

Add `*BannerlordFirewall*` to your start.bat file

It should look like this

```
start DedicatedCustomServer.Starter.exe /dedicatedcustomserverconfigfile ds_config_sample_team_deathmatch.txt /port 7233 /DisableErrorReporting /multiplayer _MODULES_*Native*Multiplayer*BannerlordFirewall*_MODULES_
```

After that locate DedicatedCustomServer.Starter.exe in your `bin/Win64_Shipping_Server` folder and then check "Run this program as an administrator" ( Right click -> Properties -> Compatability )

! You need to put 0Harmony.dll file inside of the zip/bin/Win64_Shipping_Server to your root bin/Win64_Shipping_Server folder also

### Some Notes

This worked for Persistent Empires quite good. **But it might not be the case for you since this mitigates application layer attacks**

### Donation

If you find this mod useful you can donate some $$$ using https://www.patreon.com/avrasiacommunity
