using System;
using CommandSystem;
using Exiled.API.Features;

namespace Spies
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class RevealCommand : ICommand
    {
        public string Command => "reveal";
        public string[] Aliases { get; } = { };
        public string Description => "Reveal that you are a spy";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Round.InProgress)
            {
                response = "The round must be in progress.";
                return false;
            }
            Player player = Player.Get(sender);
            if (player.SessionVariables["IsASpyx"].Equals(false))
            {
                response = "You are not a spy.";
                return false;
            }
            if (player.SessionVariables["IsASpyx"].Equals(true))
            {
                EventHandlers.RevealPlayer(player);
                response = "You have been revealed.";
                return true;
            }
            response = "lol? how did you get here xdddd";
            return false;
        }
    }
}