# Expanded Skills

ExpandedSkills extends The Long Dark's 9 vanilla survival skills from 5 to 10 levels, spreading their progression over a longer curve and adding a unique final buff to each skill. 

The original five skill tiers are redistributed across ten levels. Existing vanilla buffs are progressively spread through the new progression, with the original level 5 buffs reached at level 9.

Level 10 acts as a final mastery level and adds a new unique buff to each skill.

Existing vanilla progression is migrated automatically when loading a save.


## Level 10 buffs

| Skill | Buff |
| --- | --- |
| Carcass Harvesting | 10% more meat recovered from *new* carcasses |
| Cooking | Starvation condition loss reduced by 50% |
| Fire Starting | Torches pulled from fires have 30% additional condition |
| Ice Fishing | Fishing lines never break on a catch |
| Rifle Firearm | Rifle reload time reduced by 25% |
| Archery | Bow nock and draw time reduced by 30% |
| Mending | Ruined clothing can be restored to 10% condition |
| Revolver Firearm | Can move while aiming the revolver |
| Gunsmithing | Ammunition crafting time reduced by 50% |


## Progression

Expanded Skills keeps the vanilla skill system but the 5 original level buffs are stretched across 10 levels:

| Expanded Level | Vanilla Reward Tier |
| --- | --- |
| 1–2 | Level 1 |
| 3–4 | Level 2 |
| 5–6 | Level 3 |
| 7–8 | Level 4 |
| 9–10 | Level 5 |

The bonuses vary by level, for instance, the bonuses granted at skill level 5 differ from those at level 6, even though they are based on the bonuses provided by the vanilla level 3, while level 10 adds the buffs listed above.

Skill point requirements are derived from the skill's existing vanilla progression and extended beyond level 5.

## Compatibility notes

Expanded Skills is designed to remain compatible with mods that use the vanilla skill system normally **BUT** Considering this mod touch **a lot** of stuff, it can cause several incompatibilities or weird behaviors. 

Known compatibility:

- **Better Skill XP** -> Compatible.
- **Survivor Knowledge** -> Compatible.
- **Filigrani's Skills (or any mods using it to make custom skills)** -> Compatible. Custom skills will just be ignored by ES.
- **Recipe Requirements** -> Compatible.
- **SkillManager** -> Generally compatible, **but** level-up notifications may not always reflect the real ES level correctly.
- **Universal Tweaks** -> Compatible, **but** `Revolver Handling Improvements` should be disabled because it overlaps with the Revolver level 10 buff.
- **Adaptive Arsenal** -> Generally compatible, **but** its revolver movement behavior overlaps with the Revolver level 10 benefit.
- **Target Practice & Master Hunter** -> Partially compatible. Master Hunter may treat ES level 9 as the vanilla maximum and interfere with progression to level 10.
- **Skill Adjustment** -> Incompatible. ES overrides many vanilla skill benefits directly, causing a large part of Skill Adjustment's perk settings to be ignored.
- **Ruined Mending** -> Incompatible. It will overlap the Mending level 10 buff.

Basically, mods that directly replace vanilla skill progression, skill caps, tier handling and/or skill reward calculations may cause issues.

Not everything got tested ofc so if you spot any weird behaviors or true incompatibilities, **PLEASE** report those to me with a issue on GitHub or directly to me on the TLDModding discord server.  


## Installation

1. Install MelonLoader 0.7.2 *non-nightly*.
2. Install [ModData](https://github.com/dommrogers/ModData).
3. Place `ExpandedSkills.dll` in your `Mods` folder.

---

## AI Notice

This project was developed with the assistance of AI tools.
