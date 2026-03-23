# Dungeon Wipe
A 3D dungeon crawler game built with **Unity 2022.3.27f1** where players battle waves of enemies across custom-built levels, collecting potions and coins while trying to survive.
---
## Table of Contents
- [About the Game](#about-the-game)
- [Features](#features)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Opening the Project](#opening-the-project)
- [How to Play](#how-to-play)
  - [Controls](#controls)
  - [Weapons](#weapons)
  - [Enemies](#enemies)
  - [Collectibles](#collectibles)
- [Level Editor](#level-editor)
- [Project Structure](#project-structure)
---
## About the Game
Dungeon Wipe is a first/third-person 3D dungeon crawler where a knight fights through dungeons filled with Warriors, Archers, and Mages. Levels are loaded from JSON files created with the built-in Level Editor. The game features a Dynamic Difficulty Adjustment system that adapts health potion spawning based on the player's current health.
---
## Features
- **3D dungeon crawler** with third-person camera and melee/ranged combat
- **Three enemy types** — Warrior, Archer, and Mage — each with unique attack behaviours and AI navigation (Unity NavMesh)
- **Collectible system** — health potions, damage potions, speed potions, and coins
- **Dynamic Difficulty Adjustment** — health potion strength scales with the player's remaining health
- **Custom Level Editor** — build and save dungeon layouts as JSON files, then load and play them
- **Score system** — earn points by defeating enemies
- **Pause menu** and full **main menu** with volume and sensitivity settings
---
## Getting Started
### Prerequisites
- [Unity 2022.3.27f1](https://unity.com/releases/editor/whats-new/2022.3.27) (LTS)
- Git (to clone the repository)
### Opening the Project
1. Clone the repository:
   ```bash
   git clone https://github.com/tiagoffelix/Dungeon-Wipe.git
   ```
2. Open **Unity Hub**, click **Add**, and select the `Dungeon Wipe` folder inside the cloned repository.
3. Unity will import all assets automatically. Once ready, open the **MainMenu** scene (`Assets/Scenes/MainMenu.unity`) to start.
---
## How to Play
From the **Main Menu** you can start a new game (select a saved level) or open the Level Editor.
### Controls
| Action | Input |
|---|---|
| Move | `W` `A` `S` `D` |
| Jump | `Space` |
| Crouch / Sneak | Hold `C` |
| Attack | `Left Mouse Button` |
| Block (shield) | Hold `Right Mouse Button` |
| Draw Sword | `1` |
| Equip Crossbow | `2` |
| Switch Weapon | Mouse Scroll Wheel |
| Look Around | Mouse |
| Pause | `Esc` |
### Weapons
- **Sword** — fast melee attack with a short cooldown.
- **Crossbow** — ranged attack that fires arrows; useful for keeping distance from enemies.
- **Shield** — hold right-click to block incoming melee hits. The shield can break after absorbing too much damage.
### Enemies
| Enemy | Attack Style |
|---|---|
| **Warrior** | Charges and deals melee damage |
| **Archer** | Fires projectiles from range |
| **Mage** | Casts area spells and deals magic damage |
All enemies use Unity's NavMesh for pathfinding and will pursue the player when within detection range on the same floor.
### Collectibles
| Collectible | Effect |
|---|---|
| **Health Potion** | Restores player health (amount scales with how low health is) |
| **Damage Potion** | Temporarily increases attack damage |
| **Speed Potion** | Temporarily increases movement speed |
| **Coin** | Adds to the player's score |
Collectibles spawn periodically on floor tiles near the player.
---
## Level Editor
The built-in Level Editor lets you design your own dungeon layouts:
1. Select **Level Editor** from the Main Menu.
2. Place floor tiles, walls, obstacles, spikes, and spawn points on the grid.
3. Save the level — it is stored as a JSON file.
4. Return to the Main Menu, select your saved level, and play it.
---
## Project Structure
```
Dungeon Wipe/
└── Assets/
    ├── Enemies/          # Enemy prefabs and animators (Warrior, Archer, Mage)
    ├── Player/           # Player prefab, animations, and UI sprites
    ├── Prefabs/          # Shared game object prefabs
    ├── Scenes/           # Unity scenes (MainMenu, Game, Editor)
    ├── Scripts/
    │   ├── Collectibles/ # Coin, potion pickup and spawn logic
    │   ├── Enemies/      # Enemy AI, spawning, and type definitions
    │   ├── Level/        # Level loading, floor script, pause menu
    │   ├── LevelEditor/  # Grid, prefab placement, and save/load tools
    │   ├── Menu/         # Main menu, settings, and volume control
    │   └── Player/       # Player movement, combat, camera, and stats
    ├── Sounds/           # Audio clips
    └── Scriptable Objects/ # ScriptableObject assets (Stats, Enemy types, etc.)
```
