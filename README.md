# Dungeon Wipe

## Overview

Dungeon Wipe is a solo Unity dungeon-survival project featuring melee and ranged combat, three enemy archetypes, a JSON-based level editor, and a health-responsive Dynamic Difficulty Adjustment system. A playable Windows build is available on Itch.io.

## Solo Role

Role: Solo Programmer and Designer

Built as my Bachelor's project, including gameplay, tools, AI, menus, persistence, and the research implementation.

## Research Context

The adaptive potion system was the implementation studied for the published ICEC paper on Dynamic Difficulty Adjustment. Potion eligibility responds to player health, and potion value or type responds to player health. The spawn-check interval remains fixed.

[Read the ICEC 2024 publication](https://doi.org/10.1007/978-3-031-74353-5_27)

## Enemy and Weapon Design

- Warriors rapidly close distance and create melee pressure.
- Archers restrict safe positioning through sustained ranged pressure.
- Mages are high-priority long-range threats with attacks that are intentionally difficult to avoid.
- Mage pressure encourages switching from the sword to the crossbow. Together, the three archetypes discourage reliance on one combat approach.

The player can use a sword, crossbow, and shield.

## Level Editor Workflow

1. Place floors, walls, hazards, obstacles, and spawn points on the editor grid.
2. Save the layout as JSON.
3. Select the saved level from the menu.
4. Reconstruct the authored layout at runtime and play it.

## Main Systems

- JSON level authoring and runtime reconstruction
- Melee, ranged, and defensive combat
- Warrior, Archer, and Mage AI using NavMesh navigation
- Health, damage, and speed potions plus scoring coins
- Health-responsive potion support with fixed spawn checks
- Menus, settings, pause flow, and per-level high scores

## Running the Project

1. Clone this repository.
2. In Unity Hub, add the `Dungeon Wipe` folder inside the repository.
3. Use Unity 2022.3.27f1.
4. Open `Assets/Scenes/MainMenu.unity` and press Play.

## Controls

| Action | Input |
| --- | --- |
| Move | W A S D |
| Jump | Space |
| Crouch | Hold C |
| Attack | Left Mouse Button |
| Block | Hold Right Mouse Button |
| Sword | 1 |
| Crossbow | 2 |
| Switch weapon | Mouse wheel |
| Look | Mouse |
| Pause | Esc |

## Known Limitations

The DDA changes potion eligibility and value or type, not the fixed spawn-check interval. Combat feedback and malformed JSON handling would benefit from further iteration and automated tests.

## Playable Build

[Play Dungeon Wipe on Itch.io](https://speazyy.itch.io/dungeon-wipe)

## Publication

[Dungeon Wipe: Exploring Dynamic Difficulty Adjustment with Power-Up Mechanics](https://doi.org/10.1007/978-3-031-74353-5_27)

## Portfolio Case Study

[View the Dungeon Wipe case study](https://tiagoffelix.com/projects/dungeon-wipe)
