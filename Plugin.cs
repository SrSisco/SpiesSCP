using Exiled.API.Features;
using System;
namespace Spies
{
    public class Plugin : Plugin<Config>
    {
        private EventHandlers EventHandler;
        public override string Name => "Spies";
        public override string Author => "srsisco";
        public override Version Version => new Version(1, 2, 0);

        public override void OnEnabled()
        {
            EventHandler = new EventHandlers(this);
            Exiled.Events.Handlers.Player.Joined += EventHandler.OnPlayerJoined;
            Exiled.Events.Handlers.Player.Died += EventHandler.PlayerOnDied;
            Exiled.Events.Handlers.Player.Hurting += EventHandler.PlayerOnHurting;
            Exiled.Events.Handlers.Player.Escaping += EventHandler.OnPlayerEscaping;
            Exiled.Events.Handlers.Server.RespawningTeam += EventHandler.ServerOnRespawningTeam;
           
            Log.Info("Spies has been enabled.");
            base.OnEnabled();

        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Joined -= EventHandler.OnPlayerJoined;
            Exiled.Events.Handlers.Player.Died -= EventHandler.PlayerOnDied;
            Exiled.Events.Handlers.Player.Hurting -= EventHandler.PlayerOnHurting;
            Exiled.Events.Handlers.Player.Escaping -= EventHandler.OnPlayerEscaping;
            Exiled.Events.Handlers.Server.RespawningTeam -= EventHandler.ServerOnRespawningTeam;
            EventHandler = null;
            Log.Info("Spies has been disabled.");
            base.OnDisabled();
        }
    }
}