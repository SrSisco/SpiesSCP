# SpiesSCP

![downloads](https://img.shields.io/github/downloads/SrSisco/SpiesSCP/total?style=for-the-badge)

## About

A plugin that adds spies to the game, both NTF and CHI.

**On a spawn wave, there is a configurable probability that a random person will spawn as a Spy.** 

The spy will act against the team that they spawn with. They can shoot their teammates, but once they do, their disguise is revealed.  

You can also set configurable probabilities for people to become a spy when they escape, and how long their disguise duration should last before they are revealed automatically (you can also disable this).

When the spy dies, their ragdoll will change teams to show that it was the spy that you killed.

You are able to customise the inventory that a Spy spawns with. Please leave Slot 1 and Slot 2 as empty, as these are for the gun & keycard that are automatically assigned based on the rank they spawn as. The rest of the slots can be configured as normal.

## Commands

**Client Console**  
.reveal : reveals your true team  

**Remote Admin Panel**  
spy <player id/username> <ntf/chaos> : spawns player as a spy.

## Default Config
```yaml
spies:
# Enable or disable the plugin.
  is_enabled: true
  debug: false
  # Probability of spy spawning ON A CHAOS WAVE
  n_t_f_spy_spawn_probability_in_chaos_wave: 60
  # Probability of spy spawning ON A NTF WAVE
  chaos_spy_spawn_probability_in_n_t_f_wave: 60
  # Probability of spy spawning WHEN  CLASS-D ESCAPES
  spy_spawn_probability_after_class_d_escape: 60
  # Probability of spy spawning WHEN SCIENTIST ESCAPES
  spy_spawn_probability_after_scientist_escape: 60
  # Damage multiplier from a spy to REAL its team (1 = normal, <1 = less, >1 more)
  spy_t_k_factor: 0.200000003
  # Damage multiplier to a spy from its REAL team (1 = normal, <1 = less, >1 more)
  to_spy_t_k_factor: 0.200000003
  # Message shown when a spy shoots its REAL team
  spy_t_k_message: 'You are a spy!'
  # Message shown to a spy REAL team member when he shoots the spy, %player% is replaced with the spy name
  team_shoots_spy_message: 'You are shooting %player%, a spy from your team!'
  # Message shown when a spy shoots another spy from its REAL team
  spy_t_k_spy_message: 'You are shooting another spy!'
  # The spy cannot take damage from the team they are spying while being a spy
  spy_receive_damage_from_spied: true
  # Should the spy get discovered whe he damages the team he is spying?
  spy_reveal_when_damaging: true
  # Message of the Reveal hint
  reveal_message: 'Your disguise has been revealed!'
  # Duration of the Reveal hint
  reveal_message_duration: 10
  # Message of the Revealing hint
  revealing_message: 'You will lose your disguise in 60 seconds!'
  # Duration of the Revealing hint
  revealing_message_duration: 10
  # Amount of time before the disguise of a spy is revealed, in seconds. Set to anything below 60 to disable
  disguise_duration: 300
  # What should be displayed to a Spy when they spawn in. This broadcast will always end with the TK multiplier factor (e.g. 0.2)
  start_message: "<b>You are a spy!</b>\nYou will be revealed once you use the .reveal command\nDamage against the team you spy for is multiplied by "
  # What should be displayed to a Spy when they escape and become a spy. This broadcast will always end with the TK multiplier factor (e.g. 0.2)
  escape_message: "<b>You are a spy!</b>\nYes, this is intended, so act normal\nDamage against the team you spy for is multiplied by "
  # Whether or not probabilities should be additive (50 + 50 = 100) or not (50 + 50 = 2 seperate 50% chances)
  additive_probabilities: false
  # The list of starting items for Spies. IMPORTANT: LEAVE SLOT 1 & 2 EMPTY! The spy's gun & keycard is assigned here automatically based on their rank, as well as Ammo. ItemName is the item to give them, and Chance is the percent chance of them spawning with it. You can specify the same item multiple times.
  starting_inventories:
    Spy:
      slot3:
      - item_name: 'Medkit'
        chance: 100
      slot4:
      - item_name: 'Radio'
        chance: 100
      slot5:
      - item_name: 'SCP268'
        chance: 100
      slot6:
      - item_name: 'ArmorCombat'
        chance: 100
      slot7: []
      slot8: []
```