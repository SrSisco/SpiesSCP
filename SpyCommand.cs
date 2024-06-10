using System;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using PlayerRoles;

namespace SpiesSCP
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpyCommand : ICommand
    {
        public string Command => "spy";
        public string[] Aliases { get; } = { };
        public string Description => "Forces a person to be a chaos/ntf spy";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.InProgress)
            {
                response = "The round must be in progress.";
                return false;
            }
            if (arguments.Count != 2)
            {
                response = "Usage: spy ((player id) (ntf/chaos)";
                return false;
            }
            Player player = Player.Get(arguments.At(0));
            if (player == null)
            {
                response = "Player not found: " + arguments.At(0);
                return false;
            }
            string a = arguments.At(1);
            if (a == "ntf")
            {
                player.SessionVariables["IsASpyx"] = true;
                player.Role.Set(RoleTypeId.NtfSergeant, SpawnReason.ForceClass, RoleSpawnFlags.All);
                Timing.RunCoroutine(EventHandlers.RevealTimer(player));
                player.Broadcast(10, SpiesSCP.Instance.Config.StartMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);
                response = "Player revived as NTF spy";
                return true;
            }
            if (a != "chaos")
            {
                response = "Please, select between chaos or ntf";
                return false;
            }
            player.SessionVariables["IsASpyx"] = true;
            player.Role.Set(RoleTypeId.ChaosRifleman, SpawnReason.ForceClass, RoleSpawnFlags.All);
            Timing.RunCoroutine(EventHandlers.RevealTimer(player));
            player.Broadcast(10, SpiesSCP.Instance.Config.StartMessage + SpiesSCP.Instance.Config.SpyTKFactor, Broadcast.BroadcastFlags.Normal, true);
            response = "Player revived as Chaos spy";
            return true;
        }

    }
}