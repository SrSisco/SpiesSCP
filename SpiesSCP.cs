using Exiled.API.Features;
using SpiesSCP.Configs;
using System;
using System.Collections;

namespace SpiesSCP
{
    public class SpiesSCP : Plugin<Config>
    {
        public override string Name => "Spies";
        public override string Author => "srsisco | noahxo";
        public override Version Version => new Version(1, 3, 0);

        public static SpiesSCP Instance;

        public EventHandlers _handlers;

        public Random Rng = new();

        public override void OnEnabled()
        {
            Instance = this;

            _handlers = new EventHandlers();

            Exiled.Events.Handlers.Player.Joined += _handlers.OnPlayerJoined;
            Exiled.Events.Handlers.Player.Died += _handlers.PlayerOnDied;
            Exiled.Events.Handlers.Player.Hurting += _handlers.PlayerOnHurting;
            Exiled.Events.Handlers.Player.Escaping += _handlers.OnPlayerEscaping;
            Exiled.Events.Handlers.Player.ChangingRole += _handlers.OnPlayerChangingRole;
            Exiled.Events.Handlers.Server.RespawningTeam += _handlers.ServerOnRespawningTeam;
            Exiled.Events.Handlers.Player.SpawningRagdoll += _handlers.OnPlayerSpawningRagdoll;

            Log.Info("Spies has been enabled.");
            base.OnEnabled();

        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Joined -= _handlers.OnPlayerJoined;
            Exiled.Events.Handlers.Player.Died -= _handlers.PlayerOnDied;
            Exiled.Events.Handlers.Player.Hurting -= _handlers.PlayerOnHurting;
            Exiled.Events.Handlers.Player.Escaping -= _handlers.OnPlayerEscaping;
            Exiled.Events.Handlers.Player.ChangingRole -= _handlers.OnPlayerChangingRole;
            Exiled.Events.Handlers.Server.RespawningTeam -= _handlers.ServerOnRespawningTeam;
            Exiled.Events.Handlers.Player.SpawningRagdoll -= _handlers.OnPlayerSpawningRagdoll;

            _handlers = null;

            Instance = null;

            Log.Info("Spies has been disabled.");
            base.OnDisabled();
        }
    }
}