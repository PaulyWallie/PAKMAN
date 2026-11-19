# Project Overview
- Game Title: PAKMAN (project name).
- High-Level Concept: Existing 2D platformer with checkpoints, health, lives, required level skulls, and persistent upgrade skulls. This change corrects upgrade feedback, HUD assets/connections, and developer save-reset workflows; it does not redesign gameplay.
- Players: Existing single-player controller; no multiplayer changes.
- Inspiration / Reference Games: Not specified; preserve existing design.
- Tone / Art Direction: Existing sprite-based presentation; reuse player and heart sprites, no generated artwork.
- Target Platform: StandaloneWindows64.
- Screen Orientation / Resolution: Existing responsive landscape-style HUD; current Game view reports 874×888, so validate portrait-like and landscape aspect ratios rather than assume a fixed resolution.
- Render Pipeline: Built-in, Unity 6000.6.6f1.

# Game Mechanics
## Core Gameplay Loop
Preserve all existing mechanics: collect LevelSkulls to enable the exit, collect ExtraLife to increase carried lives, and collect unique UpgradeSkulls to persist upgrades. One upgrade skull currently unlocks double jump. Present an immediate, short confirmation only when an ability changes from locked to unlocked, reinforcing competence without obstructing play. Loading an already-unlocked save must not trigger the message.

## Controls and Input Methods
Existing New Input System remains unchanged. No new gameplay bindings. Reset actions are development-only Unity Editor menu commands with confirmation, not player-facing buttons. Execute them outside Play Mode to avoid partially resetting live gameplay; explain this restriction in the confirmation/menu feedback.

# UI
## Selected design
User selected UI Toolkit for the player-following popup, one player icon × count for lives, and both reset actions.

Screen layout:
```
Coins: N                                  [hearts]
Skulls: N                                 [player sprite] × 3

                 Upgrade unlocked
                 (0.5 world units
                  above head)
                    [PLAYER]
```
- Remove the persistent topbar upgrade label and its saved-state polling.
- Add a separate non-interactive popup layer in the existing HUD document, outside the topbar flex layout.
- Show exact placeholder text `Upgrade unlocked` for 0.5 seconds using unscaled time.
- In LateUpdate, project the player's current SpriteRenderer bounds (top center + Vector3.up * 0.5) through the gameplay camera into panel coordinates. Follow the moving player and camera; position relative to the popup parent, center the label horizontally, and avoid reversed screen Y coordinates or assumptions about PanelSettings scale.
- This visually appears in the scene above the player while remaining a UI Toolkit overlay, as selected; do not add TMP, Canvas, or world-space text systems.
- Hide when time expires, player is inactive, camera/target is unavailable, or anchor is behind/outside camera view. Clear on disable/scene transition; do not replay on reload. Missing references should fail safely.
- A subsequent distinct ability-unlock event may restart the same popup timer; duplicate skull collection does not emit an event.
- Replace `Lives: 3` with the existing player sprite and `× 3`; keep the count accessible/readable and update on death and ExtraLife collection.
- Remove spritesheet/spike imagery from the HUD only, not scene hazards. Assign the actual sliced heart sprite to health icons. Preserve half-heart semantics: current player maxHealth is 6 units for three hearts. Represent each pair as one full heart and odd units as a half/depleted heart treatment using the existing sliced heart sprite and clipping/styling, without displaying the whole sheet.

# Key Asset & Context
## Verified current state
- `Assets/Scripts/Core/PlayerProgress.cs`: JSON data in PlayerPrefs key `PlayerProgress`; caches data in `_data`; exposes `UpgradeSkulls`, `DoubleJumpUnlocked`, `CollectUpgradeSkull(id)`, `ResetForNewGame()`; currently no unlock event.
- `Assets/Scripts/UI/UITKController.cs`: three UIDocument references, HUD polling, persistent upgrade label; anonymous menu callbacks registered on each enable; current heart visibility compares index with half-heart health directly.
- `Assets/Scripts/Interactables/Pickup.cs`: DRY reward/effect/sound pipeline already implemented; UpgradeSkull calls PlayerProgress once per unique ID. Preserve it unless a narrowly necessary event integration change is discovered.
- `Assets/UI/HUD.uxml`: topbar label and three heart VisualElements, text-only lives.
- `Assets/UI/Styles/PAKMAN_Styles.uss`: shared by HUD and other menus; `.heart-icon` points to entire `Assets/Art/Sprites/Items/simple-items-Sheet.png`. Scope new selectors to HUD/popup to avoid menu regressions.
- Player sprite: `Assets/Art/Sprites/Characters/Main Character-Sheet.png`, subasset `Main Character-Sheet_0`, rect (2,68,15,32).
- Heart sprite: `Assets/Art/Sprites/Items/simple-items-Sheet.png`, subasset `simple-items-Sheet_12`, rect (52,100,9,8).
- `Assets/Scenes/Test.unity`: HUD, PauseMenu, GameOver connected on controller; all document sorting orders currently 0, shared PanelSettings. One enabled orthographic MainCamera.
- `Assets/Prefabs/UITK_UI.prefab`: PauseMenu and GameOver references exist; HUD reference missing. Repair the shared reusable prefab and reconcile Test overrides without creating duplicate documents.
- `Assets/Scripts/UI/MainMenu.cs`: NewGame resets lives/upgrades, zeros `levelNames` PlayerPrefs keys and CoinCount before scene load.
- `Assets/Scripts/UI/LevelSelect.cs`: progression reads/writes scene-name-like keys and literal `Level1`.
- `Assets/Scripts/Core/LevelManager.cs`: PlayerLives defaults to 3; carried through scenes. Do not regress level objectives, life deduction, or restart behavior.
- Asset dependency inspection used AssetDatabase.GetDependencies because a dedicated dependency tool is not available in this session. Recheck dependencies/dependents before implementation edits, including shared UI prefab and shared USS impacts.

## Planned files
Modify:
- `Assets/Scripts/Core/PlayerProgress.cs`
- `Assets/Scripts/UI/UITKController.cs`
- `Assets/UI/HUD.uxml`
- `Assets/UI/Styles/PAKMAN_Styles.uss`
- `Assets/Prefabs/UITK_UI.prefab`
- `Assets/Scenes/Test.unity`
- `Assets/Scripts/UI/MainMenu.cs` (route new-game reset through shared reset service only).
Create:
- `Assets/Scripts/UI/PlayerUpgradePopup.cs`: small HUD-popup presenter for positioning and timer, separate from general HUD/menu controller.
- `Assets/Scripts/Core/GameSaveReset.cs`: shared targeted reset operations/key definitions.
- `Assets/Scripts/Editor/GameSaveResetMenu.cs`: confirmed editor-only menu actions.
- Lightweight saved progression-key configuration asset only if inspection shows existing MainMenu.levelNames cannot be reused safely without opening scenes or hardcoding keys. Prefer a shared list established from actual serialized MainMenu configuration and the literal Level1 key; do not delete arbitrary PlayerPrefs keys.
- Temporary tests as needed, removed afterward; no package additions unless separately approved.

# Implementation Steps
1. **Recheck context and impact.** Dependency: approval.
   - Inspect actual serialized MainMenu.levelNames in its existing scene/prefab assets read-only to obtain complete owned progression keys, including any not matching scene names.
   - Inspect sliced sprite import/subasset IDs, UI prefab structure, Test overrides, PanelSettings reference, document sorting order, and menu names.
   - Verify installed UI Toolkit positioning approach against Unity 6000.6.6f1 and package/API availability; no guessed obsolete APIs.
   - Confirm no existing reset service/popup duplicates. Establish a clean console baseline.
2. **Publish ability-unlock transition and cache-safe reset operations.** Dependency: Step 1.
   - In PlayerProgress, determine locked state before collection; save reward then emit an ability-unlocked event only when the threshold is newly crossed. Use a small event payload or named current-ability event, not a generalized upgrade framework.
   - Do not emit on load, reset, duplicate ID, or second skull when double jump is already unlocked.
   - Clear event subscribers at subsystem registration where necessary for domain-reload-disabled sessions; presenter subscribes/unsubscribes symmetrically.
   - Expose narrowly scoped cache invalidation/reset operation needed by developer menus; avoid reflection-based reset.
3. **Replace persistent upgrade HUD with timed following popup.** Dependency: Step 2.
   - Implement PlayerUpgradePopup with cached HUD label, PlayerController target/SpriteRenderer, MainCamera, 0.5-second duration and 0.5-world-unit head clearance, configurable serialized defaults.
   - Hook into ability-unlock event; update positioning and timer in LateUpdate with proper world-to-panel conversion and parent-relative positioning.
   - Use USS for styling/visibility; runtime code controls only necessary dynamic geometry and class toggles.
   - Remove persistent label bindings and saved-state visibility polling from UITKController. Add popup label/layer in HUD.uxml without shifting topbar.
4. **Repair HUD iconography and reusable UI references.** Dependency: Steps 1 and 3.
   - Replace lives text prefix with a sliced static player icon and numeric count.
   - Assign actual sprite subassets via serialized controller references and/or valid subasset UI resource references; never assign a spritesheet texture as a single icon. Reuse existing assets without changing their import settings or slicing.
   - Replace erroneous heart sheet background with sliced hearts and correct half-heart display for 0–6 health units; null-safe element binding.
   - Add/restore exactly one HUD UIDocument on the shared UI prefab, wire all controller/popup references, preserve existing PauseMenu and GameOver documents and PanelSettings.
   - Set deterministic ordering: HUD below PauseMenu/GameOver; popup stays in HUD layer, menus overlay it.
   - Reconcile Test with prefab changes, avoiding duplicate HUD children and preserving unrelated scene overrides.
   - Ensure menu callbacks do not accumulate on repeated enable/disable: use cached named handlers with symmetric registration/unregistration if connections are changed.
5. **Add both safe reset commands.** Dependency: Steps 1 and 2.
   - Provide `PAKMAN/Save Data/Reset Upgrades Only` and `PAKMAN/Save Data/Reset Game Save` (or project-equivalent menu placement).
   - Require destructive confirmation; cancellation leaves everything unchanged. Disable during Play Mode and transitions, explaining to exit Play Mode first.
   - Upgrade reset clears this game's PlayerProgress key and in-memory cache only; leaves lives, coins, and level unlocks untouched. Collected upgrade skulls reappear on next scene load/Play session.
   - Game-save reset clears owned PlayerProgress, PlayerLives, CoinCount, and verified level progress keys; next gameplay entry defaults to three lives and locked upgrades. Preserve all unrelated preferences and never call PlayerPrefs.DeleteAll.
   - Centralize save-key definitions and reset logic; reuse appropriate non-editor reset operation in NewGame with current level-key list, preserving its existing initialization semantics and scene-load order.
   - Save once after reset; no automatic deletion of real save data during implementation.
6. **Validate and demonstrate.** Dependency: Steps 3–5.
   - Validate UXML/USS, compile scripts, confirm all assets/references survive save/reload.
   - Run timing, positioning, health, menu and reset tests with backup/restore of every affected save key; remove temporary tests.
   - Visually inspect Test in Game view and report actual outcomes, including any limitations.

# Verification & Testing
- **Unlock feedback:** fresh/reset upgrade state, collect first UpgradeSkull → text immediately above head; hide after 0.5 seconds within one frame tolerance. Move/jump during display and move camera; label remains 0.5 world units above current top bounds. No persistent topbar upgrade label.
- **No false feedback:** already unlocked save, second skull, duplicate ID, scene reload, death/respawn, enabling UI again: no replay of unlock message.
- **Geometry:** landscape 1920×1080, current 874×888 view, altered PanelSettings scale, screen edge and off-camera target; no Y inversion, head overlap, full-sheet images or stale target errors. No popup blocks gameplay input.
- **Pause/scene lifecycle:** 0.5-second unscaled expiry, missing camera/player handling, menus layer above HUD, subscriber cleanup across scenes and domain reload enabled/disabled.
- **Lives:** icon is exactly sliced static player sprite, count 3→2 on death and increments on ExtraLife; large counts remain readable. No `Lives:` prefix. Persistence remains unchanged.
- **Health:** full 6 units shows three full hearts; 5/3/1 units show appropriate half-heart representation; zero shows depleted health; no spike/spritesheet imagery; scene spike hazards unchanged.
- **Connections:** Test and shared prefab HUD/PauseMenu/GameOver references populated; only one HUD exists; resume/restart/quit handlers execute once across repeated enable cycles; no missing font/panel/sprite warnings.
- **Resets:** cancellation has no effect; upgrades-only preserves lives/coins/level keys; full-game reset targets only verified owned keys and preserves a sentinel unrelated preference. Clear cache so double jump relocks and collected skulls can be retested next load. Starting lives = 3 afterward. Do not reset actual user saves except in reversible tests or explicit user action.
- **Regression:** LevelSkull exit requirement and skull-free bypass, unique upgrade persistence, ExtraLife duplicate reward guard, game over/restart and NewGame transitions remain intact.
- **Final checks:** no compile errors, UXML/USS validation passed, no unapproved package/project-setting changes, temporary tests removed, editor returned out of Play Mode.
