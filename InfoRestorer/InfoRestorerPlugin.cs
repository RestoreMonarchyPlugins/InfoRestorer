using Adam.InfoRestorer.Models;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;

namespace Adam.InfoRestorer
{
    public class InfoRestorerPlugin : RocketPlugin<InfoRestorerConfiguration>
    {
        public static InfoRestorerPlugin Instance { get; private set; }

        public UnityEngine.Color MessageColor { get; set; }
        public Dictionary<CSteamID, List<InventoryModel>> Players { get; set; }

        protected override void Load()
        {
            Instance = this;
            MessageColor = UnturnedChat.GetColorFromName(Configuration.Instance.MessageColor, UnityEngine.Color.green);
            Players = new Dictionary<CSteamID, List<InventoryModel>>();

            PlayerLife.OnPreDeath += OnPreDeath;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            U.Events.OnPlayerConnected += OnPlayerConnected;

            Rocket.Core.Logging.Logger.Log($"Made by AdamAdam, maintained by Restore Monarchy Plugins", ConsoleColor.Yellow);
            Rocket.Core.Logging.Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!", ConsoleColor.Yellow);
        }

        protected override void Unload()
        {
            PlayerLife.OnPreDeath -= OnPreDeath;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            U.Events.OnPlayerConnected -= OnPlayerConnected;

            Rocket.Core.Logging.Logger.Log($"{Name} has been unloaded!", ConsoleColor.Yellow);
        }

        private void OnPreDeath(PlayerLife life)
        {
            Players[life.player.channel.owner.playerID.steamID].Add(InventoryModel.FromPlayer(life.player));
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            if (!Players.ContainsKey(player.CSteamID))
                Players.Add(player.CSteamID, new List<InventoryModel>());
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            if (Configuration.Instance.ShouldRemoveSavesOnLeave)
                Players.Remove(player.CSteamID);
        }

        public override TranslationList DefaultTranslations => new TranslationList()
        {
            {
                "invalid_syntax",
                "Invalid Syntax! Usage: /restore <player> <times ago>"
            },
            {
                "player_not_found",
                "Player not found!"
            },
            {
                "not_number",
                "{0} is not a number higer then 0!"
            },
            {
                "too_much",
                "{0} hasn't died that much!"
            },
            {
                "restored",
                "You've succesfully restored {0}'s inventory!"
            }
        };
    }
}
