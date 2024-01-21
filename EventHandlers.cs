using System;
using System.Collections;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using Respawning;
using MEC;
using UnityEngine;
using Exiled.CustomItems.API.Features;
using SpiesSCP.ConfigObjects;
using System.Linq;

namespace SpiesSCP
{

    public class EventHandlers
    {
        public bool spyattacked;
        public bool spyattacks;

        public void ServerOnRespawningTeam(RespawningTeamEventArgs ev)
        {
            if (ev.NextKnownTeam == SpawnableTeamType.ChaosInsurgency && UnityEngine.Random.Range(0f, 100f) <= SpiesSCP.Instance.Config.NTFSpySpawnProbabilityInChaosWave)
            {
                Player player = ev.Players[UnityEngine.Random.Range(0, ev.Players.Count)];
                player.SessionVariables["IsASpyx"] = true;
                Log.Debug(player.Nickname + " respawned as a spy");

                player.Broadcast(10, SpiesSCP.Instance.Config.StartMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);

                Timing.RunCoroutine(RevealTimer(player));
            }
            if (ev.NextKnownTeam == SpawnableTeamType.NineTailedFox && UnityEngine.Random.Range(0f, 100f) <= SpiesSCP.Instance.Config.ChaosSpySpawnProbabilityInNTFWave)
            {
                Player player2 = ev.Players[UnityEngine.Random.Range(0, ev.Players.Count)];
                player2.SessionVariables["IsASpyx"] = true;
                Log.Debug(player2.Nickname + " respawned as a spy");

                player2.Broadcast(10, SpiesSCP.Instance.Config.StartMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);

                Timing.RunCoroutine(RevealTimer(player2));
            }
        }

        public void OnPlayerSpawningRagdoll(SpawningRagdollEventArgs ev)
        {
            if (ev.Player is null)
            {
                return;
            }

            if (ev.Player.SessionVariables["IsASpyx"].Equals(false) || ev.Player.SessionVariables["IsASpyx"] is null)
            {
                return;
            }

            ev.IsAllowed = false;

            // reverse ragdoll roles on spy death
            if (ev.Player.IsCHI) { Ragdoll.CreateAndSpawn(RoleTypeId.NtfPrivate, ev.Player.Nickname, "You killed the spy!", new Vector3(ev.Player.Position.x, ev.Player.Position.y - 1, ev.Player.Position.z), default, ev.Player); }
            else { Ragdoll.CreateAndSpawn(RoleTypeId.ChaosRifleman, ev.Player.Nickname, "You killed the spy!", new Vector3(ev.Player.Position.x, ev.Player.Position.y - 1, ev.Player.Position.z), default, ev.Player); }
        }

        public void OnPlayerChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player == null)
            {
                Log.Warn($"{nameof(OnPlayerChangingRole)}: Triggering player is null.");
                return;
            }

            if (ev.Player.SessionVariables["IsASpyx"].Equals(false)  || ev.Player.SessionVariables["IsASpyx"] is null) // leaves custom inventory if player isnt a spy
            {
                return;
            }

            // fixes some forceclass issues but not all, they won't even be found in normal gameplay
            if (RoleExtensions.GetTeam(ev.NewRole) != Team.ChaosInsurgency && RoleExtensions.GetTeam(ev.NewRole) != Team.FoundationForces)
            {
                ev.Player.SessionVariables["IsASpyx"] = false;
            }

            if (SpiesSCP.Instance.Config.StartingInventories.ContainsKey("Spy"))
            {
                if (ev.Items == null)
                {
                    Log.Warn("items is null");
                    return;
                }

                ev.Items.Clear();

                // adds gun/items based on rank 
                switch (ev.NewRole)
                {
                    case RoleTypeId.ChaosConscript:
                    case RoleTypeId.ChaosRifleman:
                        ev.Items.Add(ItemType.GunAK);
                        ev.Items.Add(ItemType.KeycardChaosInsurgency);
                        break;
                    case RoleTypeId.ChaosMarauder:
                        ev.Items.Add(ItemType.GunShotgun);
                        ev.Items.Add(ItemType.GunRevolver);
                        ev.Items.Add(ItemType.KeycardChaosInsurgency);
                        break;
                    case RoleTypeId.ChaosRepressor:
                        ev.Items.Add(ItemType.GunLogicer);
                        ev.Items.Add(ItemType.KeycardChaosInsurgency);
                        break;
                    case RoleTypeId.NtfPrivate:
                        ev.Items.Add(ItemType.GunCrossvec);
                        ev.Items.Add(ItemType.KeycardMTFOperative);
                        break;
                    case RoleTypeId.NtfSergeant:
                    case RoleTypeId.NtfSpecialist:
                        ev.Items.Add(ItemType.GunE11SR);
                        ev.Items.Add(ItemType.KeycardMTFOperative);
                        break;
                    case RoleTypeId.NtfCaptain:
                        ev.Items.Add(ItemType.GunFRMG0);
                        ev.Items.Add(ItemType.KeycardMTFOperative);
                        break;
                }

                // adds rest of custom inventory
                ev.Items.AddRange(StartItems(ev.Player));
            }
        }

        public void PlayerOnDied(DiedEventArgs ev)
        {
            ev.Player.SessionVariables["IsASpyx"] = false;
        }


        public void PlayerOnHurting(HurtingEventArgs ev)
        {

            if (ev.Attacker == null || ev.Player == null)
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
                if (SpiesSCP.Instance.Config.SpyReceiveDamageFromSpied == false && ev.Attacker.LeadingTeam == ev.Player.LeadingTeam)
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
                if (SpiesSCP.Instance.Config.SpyRevealWhenDamaging == true && ev.Player.LeadingTeam == ev.Attacker.LeadingTeam)
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
                ev.Attacker.ShowHint(SpiesSCP.Instance.Config.TeamShootsSpyMessage.Replace("%player%", ev.Player.Nickname), 5f);
                ev.Amount *= SpiesSCP.Instance.Config.ToSpyTKFactor;
            }
            else if (ev.Attacker.LeadingTeam != ev.Player.LeadingTeam && spyattacks && ev.Player.IsHuman)
            {
                if (!ev.Player.IsCHI && !ev.Player.IsNTF) return;
                ev.Attacker.ShowHint(SpiesSCP.Instance.Config.SpyTKMessage, 5f);

                ev.Amount *= SpiesSCP.Instance.Config.SpyTKFactor;
            }
            else if (ev.Attacker.LeadingTeam == ev.Player.LeadingTeam && spyattacks && spyattacked)
            {
                ev.Attacker.ShowHint(SpiesSCP.Instance.Config.SpyTKSpyMessage, 5f);
            }
            else if (spyattacks || spyattacked)
            {
                ev.IsAllowed = true;
            }


        }


        public void OnPlayerJoined(JoinedEventArgs ev)
        {
            ev.Player.SessionVariables["IsASpyx"] = false;
        }


        public void OnPlayerEscaping(EscapingEventArgs ev)
        {
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedScientist || ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.CuffedClassD || !ev.IsAllowed)
            {
                return;
            }
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.ClassD && UnityEngine.Random.Range(0f, 100f) <= SpiesSCP.Instance.Config.SpySpawnProbabilityAfterClassDEscape)
            {
                ev.Player.SessionVariables["IsASpyx"] = true;
                Timing.RunCoroutine(RevealTimer(ev.Player));
                Log.Debug(ev.Player.Nickname + " escaped as a spy");
                ev.Player.Broadcast(10, SpiesSCP.Instance.Config.EscapeMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);
            }
            if (ev.EscapeScenario == Exiled.API.Enums.EscapeScenario.Scientist && UnityEngine.Random.Range(0f, 100f) <= SpiesSCP.Instance.Config.SpySpawnProbabilityAfterScientistEscape)
            {
                ev.Player.SessionVariables["IsASpyx"] = true;
                Timing.RunCoroutine(RevealTimer(ev.Player));
                Log.Debug(ev.Player.Nickname + " escaped as a spy");
                ev.Player.Broadcast(10, SpiesSCP.Instance.Config.EscapeMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);
            }
        }


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
            player.Broadcast(SpiesSCP.Instance.Config.RevealMessageDuration, SpiesSCP.Instance.Config.RevealMessage, Broadcast.BroadcastFlags.Normal, true);
        }

        public static IEnumerator<float> RevealTimer(Player player)
        {
            if (player.SessionVariables["IsASpyx"] is true && SpiesSCP.Instance.Config.DisguiseDuration > 60)
            {
                yield return Timing.WaitForSeconds(SpiesSCP.Instance.Config.DisguiseDuration - 60);

                if (player.SessionVariables["IsASpyx"] is true)
                {
                    player.Broadcast(SpiesSCP.Instance.Config.RevealingMessageDuration, SpiesSCP.Instance.Config.RevealingMessage, Broadcast.BroadcastFlags.Normal, true);

                    yield return Timing.WaitForSeconds(60f);
                }

                RevealPlayer(player);
            }
        }

        public static List<ItemType> StartItems(Player player = null)
        {
            List<ItemType> items = new();

            for (int i = 0; i < SpiesSCP.Instance.Config.StartingInventories["Spy"].UsedSlots; i++)
            {
                IEnumerable<ItemChance> itemChances = SpiesSCP.Instance.Config.StartingInventories["Spy"][i];
                double r;
                if (SpiesSCP.Instance.Config.AdditiveProbabilities)
                    r = SpiesSCP.Instance.Rng.NextDouble() * itemChances.Sum(val => val.Chance);
                else
                    r = SpiesSCP.Instance.Rng.NextDouble() * 100;
                Log.Debug($"[StartItems] ActualChance ({r})/{itemChances.Sum(val => val.Chance)}");
                foreach ((string item, double chance) in itemChances)
                {
                    Log.Debug($"[StartItems] Probability ({r})/{chance}");
                    if (r <= chance)
                    {
                        if (Enum.TryParse(item, true, out ItemType type))
                        {
                            items.Add(type);
                            break;
                        }
                        else if (CustomItem.TryGet(item, out CustomItem customItem))
                        {
                            if (player != null)
                                customItem!.Give(player);
                            else
                                Log.Warn($"{nameof(StartItems)}: Tried to give {customItem!.Name} to a null player.");

                            break;
                        }
                        else
                            Log.Warn($"{nameof(StartItems)}: {item} is not a valid ItemType or it is a CustomItem that is not installed! It is being skipper in inventory decisions.");
                    }

                    r -= chance;
                }
            }

            return items;
        }
    }
}