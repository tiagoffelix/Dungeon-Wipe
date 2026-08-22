# Dungeon Wipe

## Overview

Dungeon Wipe is a solo Unity/C# dungeon-survival project built around combat and an in-game level-authoring pipeline. The editor saves custom multi-floor layouts to JSON and reconstructs them as playable, NavMesh-enabled levels at runtime. A Windows build is available on itch.io.

## Role

Role: Solo Programmer and Designer

Built as my Bachelor's project, including gameplay, level-authoring tools, NavMesh enemy systems, menus, persistence, and the research implementation.

## Research Context

The adaptive potion system was the implementation studied in **Dungeon Wipe: Exploring Dynamic Difficulty Adjustment with Power-Up Mechanics**, published at ICEC 2024 in Springer LNCS. Potion eligibility responds to player health, and potion value or type responds to player health. The spawn-check interval remains fixed.

[Read the ICEC 2024 publication](https://doi.org/10.1007/978-3-031-74353-5_27)

## Enemy and Weapon Design

- Warriors rapidly close distance and create melee pressure.
- Archers restrict safe positioning through sustained ranged pressure.
- Mages are high-priority long-range threats with attacks that are intentionally difficult to avoid.
- Mage pressure encourages switching from the sword to the crossbow. Together, the three archetypes discourage reliance on one combat approach.

The player can use a sword, crossbow, and shield.

## Level-Authoring Pipeline

1. Build layouts on an in-game, multi-floor grid.
2. Validate grid occupancy and placement before adding a prefab.
3. Save each placed object's prefab name, position, and rotation as JSON.
4. Discover and select saved level files from the menu.
5. Resolve each saved prefab name through the runtime prefab registry.
6. Reconstruct the level incrementally from the saved transforms.
7. Build the NavMesh after reconstruction so enemies can navigate the loaded layout.

```text
Editor -> Validation -> JSON -> Level selection -> Runtime prefab registry
       -> Reconstruction -> NavMesh generation -> Play
```

## Main Systems

- Multi-floor grid authoring, placement validation, JSON persistence, and runtime reconstruction
- Melee, ranged, and defensive combat
- Warrior, Archer, and Mage roles using NavMesh movement and role-specific attack logic
- Health, damage, and speed potions plus scoring coins
- Health-responsive potion support with fixed spawn checks
- Menus, settings, pause flow, and per-level high scores

## Platforms and Testing

| Platform | Status | Verified |
| --- | --- | --- |
| Windows | Released on itch.io | Played end to end: menus, all three enemy types, sword, crossbow and shield, level loading, scoring, potions, restart and return to menu. |
| Browser (WebGL) | Builds, not yet published | The project compiles and builds for WebGL. No published browser build has been played through yet. |

Gameplay, the level editor, runtime reconstruction, NavMesh generation and the
potion behaviour are shared by both platforms. The only platform-specific work
is where levels are stored and two browser input details.

## Level Storage

The editor, the menu, the high-score list and the runtime loader all read and
write levels through `LevelStore`, which owns the only file access in the
project.

- Windows and the Editor keep the original location, `<dataPath>/Resources/Levels`.
  Existing builds and existing saved levels are unaffected.
- Browser and Android builds cannot write into the installed game data, so they
  use `Application.persistentDataPath/Levels`, seeded on first run from the JSON
  files compiled into `Resources/Levels`. In a browser that path is backed by
  IndexedDB, so a level authored in the editor survives a page reload.

## Running the Project

1. Clone this repository.
2. In Unity Hub, add the `Dungeon Wipe` folder inside the repository.
3. Use Unity 2022.3.49f1.
4. Open `Assets/Scenes/MainMenu.unity` and press Play.

### Building

Both builds run through `Assets/Editor/BuildScript.cs`, so a build from a clean
checkout matches the published one. From the Editor use the `Build` menu:

- `Build > Dungeon Wipe Windows` writes to `Builds/Windows`.
- `Build > Dungeon Wipe WebGL` writes to `Builds/WebGL/DungeonWipe`.

From the command line:

```bat
"<editor>\Unity.exe" -quit -batchmode -nographics -logFile - ^
  -projectPath "<repo>\Dungeon Wipe" -buildTarget WebGL ^
  -executeMethod BuildScript.BuildWebGL
```

The WebGL build needs the WebGL Build Support module for the editor version in
use. The build script selects Brotli compression with the JavaScript
decompression fallback disabled, which is what itch.io expects: it serves `.br`
files with the matching `Content-Encoding` header. Zip the contents of the
output folder so `index.html` sits at the root of the archive, and serve the
build over HTTP rather than opening `index.html` from disk.

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

Enemy behaviour is handled through centralised conditional logic. Level files identify prefabs by name, so renamed assets can break older layouts. Malformed JSON is not validated, and save/load round-trip tests are currently manual.

## Credits

Character and environment assets by Kay Lousberg. Some 2D art was produced with
AI assistance, as disclosed on the itch.io page. All gameplay code, tools,
systems and level design are my own.

## Links

- [Play on itch.io](https://speazyy.itch.io/dungeon-wipe)
- [Read Paper](https://doi.org/10.1007/978-3-031-74353-5_27)
- [Case Study](https://tiagoffelix.com/projects/dungeon-wipe/)
