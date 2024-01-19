using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs;
using PlayerRoles;
using SpiesSCP.ConfigObjects;
using SpiesSCP.Configs;
using System.Collections.Generic;
using System.ComponentModel;

namespace SpiesSCP
{
    public class Config : IConfig
    {
        [Description("Enable or disable the plugin.")]
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Probability of spy spawning ON A CHAOS WAVE")]
        public float NTFSpySpawnProbabilityInChaosWave { get; set; } = 60;

        [Description("Probability of spy spawning ON A NTF WAVE")]
        public float ChaosSpySpawnProbabilityInNTFWave { get; set; } = 60;

        [Description("Probability of spy spawning WHEN  CLASS-D ESCAPES")]
        public float SpySpawnProbabilityAfterClassDEscape { get; set; } = 60;

        [Description("Probability of spy spawning WHEN SCIENTIST ESCAPES")]
        public float SpySpawnProbabilityAfterScientistEscape { get; set; } = 60;

        [Description("Damage multiplier from a spy to REAL its team (1 = normal, <1 = less, >1 more)")]
        public float SpyTKFactor { get; set; } = 0.2f;

        [Description("Damage multiplier to a spy from its REAL team (1 = normal, <1 = less, >1 more)")]
        public float ToSpyTKFactor { get; set; } = 0.2f;

        [Description("Message shown when a spy shoots its REAL team")]
        public string SpyTKMessage { get; set; } = "You are a spy!";

        [Description("Message shown to a spy REAL team member when he shoots the spy, %player% is replaced with the spy name")]
        public string TeamShootsSpyMessage { get; set; } = "You are shooting %player%, a spy from your team!";

        [Description("Message shown when a spy shoots another spy from its REAL team")]
        public string SpyTKSpyMessage { get; set; } = "You are shooting another spy!";

        [Description("The spy cannot take damage from the team they are spying while being a spy")]
        public bool SpyReceiveDamageFromSpied { get; set; } = true;

        [Description("Should the spy get discovered whe he damages the team he is spying?")]
        public bool SpyRevealWhenDamaging { get; set; } = true;

        [Description("Message of the Reveal hint")]
        public string RevealMessage { get; set; } = "Your disguise has been revealed!";

        [Description("Duration of the Reveal hint")]
        public ushort RevealMessageDuration { get; set; } = 10;

        [Description("Message of the Revealing hint")]
        public string RevealingMessage { get; set; } = "You will lose your disguise in 60 seconds!";

        [Description("Duration of the Revealing hint")]
        public ushort RevealingMessageDuration { get; set; } = 10;

        [Description("Amount of time before the disguise of a spy is revealed, in seconds. Set to anything below 60 to disable")]
        public ushort DisguiseDuration { get; set; } = 300;

        [Description("What should be displayed to a Spy when they spawn in. This broadcast will always end with the TK multiplier factor (e.g. 0.2)")]
        public string StartMessage { get; set; } = "<b>You are a spy!</b>\nYou will be revealed once you use the .reveal command\nDamage against the team you spy for is multiplied by ";

        [Description("What should be displayed to a Spy when they escape and become a spy. This broadcast will always end with the TK multiplier factor (e.g. 0.2)")]
        public string EscapeMessage { get; set; } = "<b>You are a spy!</b>\nYes, this is intended, so act normal\nDamage against the team you spy for is multiplied by ";

        [Description("Whether or not probabilities should be additive (50 + 50 = 100) or not (50 + 50 = 2 seperate 50% chances)")]
        public bool AdditiveProbabilities { get; set; } = false;

        [Description(
            "The list of starting items for Spies. IMPORTANT: LEAVE SLOT 1 & 2 EMPTY! The spy's gun & keycard is assigned here automatically based on their rank, as well as Ammo. ItemName is the item to give them, and Chance is the percent chance of them spawning with it. You can specify the same item multiple times.")]
        public Dictionary<string, RoleInventory> StartingInventories { get; set; } = new()
{
            { 
                "Spy", new RoleInventory
                {
                    Slot3 = new List<ItemChance>
                    {
                        new()
                        {
                            ItemName = ItemType.Medkit.ToString(),
                            Chance = 100,
                        },
                    },
                    Slot4 = new List<ItemChance>
                    {
                        new()
                        {
                            ItemName = ItemType.Radio.ToString(),
                            Chance = 100,
                        },
                    },
                    Slot5 = new List<ItemChance>
                    {
                        new()
                        {
                            ItemName = ItemType.SCP268.ToString(),
                            Chance = 100,
                        },
                    },
                    Slot6 = new List<ItemChance>
                    {
                        new()
                        {
                            ItemName = ItemType.ArmorCombat.ToString(),
                            Chance = 100,
                        },
                    },
                }
            },
        };
    }
}
