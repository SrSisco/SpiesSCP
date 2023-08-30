using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Respawning;
using UnityEngine;

namespace Spies
{
    // Token: 0x02000005 RID: 5
    public class EventHandlers
    {
        public bool spyattacked;
        public bool spyattacks;
        // Token: 0x06000022 RID: 34 RVA: 0x00002342 File Offset: 0x00000542
        public EventHandlers(Plugin plugin)
        {
            this.plugin = plugin;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002354 File Offset: 0x00000554
        public void ServerOnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency && UnityEngine.Random.Range(0f, 100f) <= this.plugin.Config.NTFSpySpawnProbabilityInChaosWave)
            {
                Player player = ev.Players[UnityEngine.Random.Range(0, ev.Players.Count)];
                player.SessionVariables["IsASpyx"] = true;
                Log.Debug(player.Nickname + " respawned as a spy");
                player.ShowHint("You are a spy! \nYou will be revealed once you use de .reveal command \n" + string.Format("Damage against the team you spy for is multiplied by {0}", this.plugin.Config.SpyTKFactor), 3f);
            }
            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox && UnityEngine.Random.Range(0f, 100f) <= this.plugin.Config.ChaosSpySpawnProbabilityInNTFWave)
            {
                Player player2 = ev.Players[UnityEngine.Random.Range(0, ev.Players.Count)];
                player2.SessionVariables["IsASpyx"] = true;
                Log.Debug(player2.Nickname + " respawned as a spy");
                player2.ShowHint("You are a spy! \nYou will be revealed once you use de .reveal command \n" + string.Format("Damage against the team you spy for is multiplied by {0}", this.plugin.Config.SpyTKFactor), 3f);
            }
        }

        // Token: 0x06000024 RID: 36 RVA: 0x000024AF File Offset: 0x000006AF
        public void PlayerOnDied(DiedEventArgs ev)
        {
            ev.Player.SessionVariables["IsASpyx"] = false;
        }

        // Token: 0x06000025 RID: 37 RVA: 0x000024CC File Offset: 0x000006CC
        public void PlayerOnHurting(HurtingEventArgs ev)
        {

            if (ev.Attacker == null)
            {
                return;
            }
            if (ev.Attacker == ev.Player)
            {
                return;
            }
            if (ev.Player.SessionVariables["IsASpyx"].Equals(false) || ev.Player.SessionVariables["IsASpyx"] is null)
            {
                spyattacked = false;
            }

            if (ev.Player.SessionVariables["IsASpyx"].Equals(true))
            {
                if (plugin.Config.SpyReceiveDamageFromSpied == false && ev.Attacker.LeadingTeam == ev.Player.LeadingTeam)
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    spyattacked = true;
                }
                
            }
            if (!ev.Attacker.SessionVariables["IsASpyx"].Equals(true) || ev.Attacker.SessionVariables["IsASpyx"] is null)
            {
                spyattacks = false;
            }
            if (ev.Attacker.SessionVariables["IsASpyx"].Equals(true))
            {
                if(plugin.Config.SpyRevealWhenDamaging == true && ev.Player.LeadingTeam == ev.Attacker.LeadingTeam)
                {
                    RevealPlayer(ev.Attacker);
                    ev.Attacker.SessionVariables["IsASpyx"].Equals(false);
                }
                else
                {
                    spyattacks = true;
                }
                

            }

            if (ev.Attacker.LeadingTeam != ev.Player.LeadingTeam && spyattacked && ev.Attacker.IsHuman)
            {
                if (!ev.Attacker.IsCHI && !ev.Attacker.IsNTF) return;
                ev.Attacker.ShowHint(this.plugin.Config.TeamShootsSpyMessage.Replace("%player", ev.Player.Nickname), 5f);
                ev.Amount *= this.plugin.Config.ToSpyTKFactor;
            }
            else if (ev.Attacker.LeadingTeam != ev.Player.LeadingTeam && spyattacks && ev.Player.IsHuman)
            {
                if (!ev.Player.IsCHI && !ev.Player.IsNTF) return;
                ev.Attacker.ShowHint(this.plugin.Config.SpyTKMessage, 5f);

                ev.Amount *= this.plugin.Config.SpyTKFactor;
            }
            else if (ev.Attacker.LeadingTeam == ev.Player.LeadingTeam && spyattacks && spyattacked)
            {
                ev.Attacker.ShowHint(this.plugin.Config.SpyTKSpyMessage, 5f);
            }
            else if(spyattacks||spyattacked)
            {
                ev.IsAllowed = true;
            }


        }

        // Token: 0x06000026 RID: 38 RVA: 0x000026A0 File Offset: 0x000008A0
        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            ev.Player.SessionVariables["IsASpyx"] = false;
        }

        // Token: 0x06000027 RID: 39 RVA: 0x000026C0 File Offset: 0x000008C0
        public void OnPlayerEscaping(EscapingEventArgs ev)
        {
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedScientist || ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedClassD || !ev.IsAllowed)
            {
                return;
            }
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.ClassD && UnityEngine.Random.Range(0f, 100f) <= this.plugin.Config.SpySpawnProbabilityAfterClassDEscape)
            {
                ev.Player.SessionVariables["IsASpyx"] = true;
                ev.Player.Role.Set(RoleTypeId.NtfSergeant, SpawnReason.ForceClass, RoleSpawnFlags.All);
                Log.Debug(ev.Player.Nickname + " escaped as a spy");
                ev.Player.ShowHint("You are a spy!\nYes, this is intended, so act normal\n" + string.Format("Damage against the team you spy for is multiplied by {0}", this.plugin.Config.SpyTKFactor), 10f);
            }
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.Scientist && UnityEngine.Random.Range(0f, 100f) <= this.plugin.Config.SpySpawnProbabilityAfterScientistEscape)
            {
                ev.Player.SessionVariables["IsASpyx"] = true;
                ev.Player.Role.Set(RoleTypeId.ChaosRifleman, SpawnReason.ForceClass, RoleSpawnFlags.All);
                Log.Debug(ev.Player.Nickname + " escaped as a spy");
                ev.Player.ShowHint("You are a spy!\nYes, this is intended, so act normal\n" + string.Format("Damage against the team you spy for is multiplied by {0}", this.plugin.Config.SpyTKFactor), 10f);
            }
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00002820 File Offset: 0x00000A20
        public static void RevealPlayer(Player player)
        {
            if (!Round.InProgress)
            {
                return;
            }
            if (player.SessionVariables["IsASpyx"].Equals(false))
            {
                return;
            }
            player.SessionVariables["IsASpyx"] = false;
            if (player.Role.Type == RoleTypeId.NtfPrivate || player.Role.Type == RoleTypeId.NtfCaptain || player.Role.Type == RoleTypeId.NtfSergeant || player.Role.Type == RoleTypeId.NtfSergeant)
            {
                player.Role.Set(RoleTypeId.ChaosRifleman, SpawnReason.ForceClass, RoleSpawnFlags.None);
            }
            else if (player.Role.Team == Team.ChaosInsurgency)
            {
                player.Role.Set(RoleTypeId.NtfSergeant, SpawnReason.ForceClass, RoleSpawnFlags.None);
            }
            player.ShowHint("You have been revealed!", 3f);
        }

        // Token: 0x0400000E RID: 14
        private readonly Plugin plugin;
    }
}