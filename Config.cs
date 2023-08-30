using Exiled.API.Features;
using Exiled.API.Interfaces;
using Exiled.Events.EventArgs;
using System.ComponentModel;

public class Config : IConfig
{
    [Description("Enable or disable the plugin.")]
    public bool IsEnabled { get; set; } = true;
    public bool Debug { get; set; } = true;

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
}