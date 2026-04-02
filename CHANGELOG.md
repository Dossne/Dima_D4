# CHANGELOG — Orbit Escape
<!-- AI-READABLE PROJECT HISTORY -->
<!-- Format: newest entry first -->
<!-- Each entry written by the agent that completed the task -->
<!-- Purpose: allows any new agent to understand project state without reading full codebase -->

## [OE-SETUP-001] project-config/android-build-settings | COMPLETED | 2026-04-02

### context
project: orbit-escape | engine: unity-6 (6000.4.0f1) | pipeline: urp-2d | platform: android

### what_was_implemented
- ProjectSettings (ProjectSettings/ProjectSettings.asset):
    pattern: ProjectBootstrap | VersionedConfig
    subscribes_to: []
    fires: []
    responsibility: "stores versioned Android and input defaults required before gameplay scripts are added"
- Script folder hierarchy (Assets/Scripts/...):
    pattern: RepositoryScaffold | ModuleBoundaryPrep
    subscribes_to: []
    fires: []
    responsibility: "creates the module layout for Core, Player, Level, Managers, and UI code"

### files
| action   | path                                 | notes                                           |
|----------|--------------------------------------|-------------------------------------------------|
| MODIFIED | ProjectSettings/ProjectSettings.asset | portrait orientation, API levels, arches, input |
| CREATED  | Assets/Scripts/.gitkeep              | tracks root Scripts folder in git               |
| CREATED  | Assets/Scripts/Core/.gitkeep         | tracks Core module folder                       |
| CREATED  | Assets/Scripts/Player/.gitkeep       | tracks Player module folder                     |
| CREATED  | Assets/Scripts/Level/.gitkeep        | tracks Level module folder                      |
| CREATED  | Assets/Scripts/Managers/.gitkeep     | tracks Managers module folder                   |
| CREATED  | Assets/Scripts/UI/.gitkeep           | tracks UI module folder                         |

### architecture_decisions
- use `.gitkeep` placeholders so the planned Unity script layout exists in git before the first `.cs` files are added
- keep setup changes minimal and versioned: edit only repository-backed PlayerSettings values, not editor-local state

### known_limitations
- active Android build platform switch is not reliably represented by a normal git-tracked file; confirm in Unity Editor that Android is the selected target platform
- this task was not verified inside Unity Editor yet, so `.meta` files for the new folders will be generated on the next asset refresh
- `bd ready --json` returned an empty list immediately after closing this task, even though `bd dep tree` reports `OE-CORE-001` as READY

### task_ref
beads_id: OE-SETUP-001
commit_title: chore(setup): configure android defaults and script folders
current_branch: feature/OE-SETUP-001-android-config

### project_state
completed_tasks: [OE-SETUP-001]
next_ready: OE-CORE-001
blocked: [OE-PLAYER-001 (waiting for OE-CORE-001 and OE-CORE-002), OE-LEVEL-002 (waiting for OE-LEVEL-001 and OE-PLAYER-003), OE-BUILD-001 (waiting for upstream systems)]
remaining: 21 of 22 tasks
current_branch: feature/OE-SETUP-001-android-config

## [OE-BOOTSTRAP] project-initialization — 2026-04-02

### Status
COMPLETED

### What was implemented
- Beads task database: 22 tasks created with OE-<MODULE>-<NNN> naming convention
- Task descriptions: machine-readable, include pattern names, dependencies, file paths
- Dependency graph: linear chain with parallel branches where modules are independent
- CHANGELOG.md initialized for AI-readable project history
- CLAUDE.md / AGENTS.md contain full agent instructions

### Task summary
| Task ID | Module | Description | Status |
|---------|--------|-------------|--------|
| OE-SETUP-001 | SETUP | Android build config + folder structure | READY |
| OE-CORE-001 | CORE | GameEvents static event bus (incl. OnPlanetReached) | BLOCKED by SETUP |
| OE-CORE-002 | CORE | PlayerRegistry — no FindGameObjectWithTag | BLOCKED by SETUP |
| OE-INPUT-001 | INPUT | InputManager with _isGameActive guard | BLOCKED |
| OE-PLAYER-001 | PLAYER | PlayerOrbitController + PlayerRegistry.Register | BLOCKED |
| OE-PLAYER-002 | PLAYER | PlayerMovement linear flight | BLOCKED |
| OE-PLAYER-003 | PLAYER | PlayerCollider fires OnPlanetReached (no score state) | BLOCKED |
| OE-PLAYER-004 | PLAYER | AsteroidMover soft homing via PlayerRegistry | BLOCKED |
| OE-LEVEL-001 | LEVEL | Planet data + orbit ring visual | BLOCKED |
| OE-LEVEL-002 | LEVEL | LevelGenerator viewport-safe via PlayerRegistry | BLOCKED |
| OE-LEVEL-003 | LEVEL | CameraFollower with ShakeOffset API | BLOCKED |
| OE-LEVEL-004 | LEVEL | AsteroidSpawner difficulty curve | BLOCKED |
| OE-STATE-001 | STATE | GameStateManager slow-mo death + _isDead guard | BLOCKED |
| OE-STATE-002 | STATE | ScoreManager — SINGLE score owner, listens OnPlanetReached | BLOCKED |
| OE-UI-001 | UI | UIMainMenu canvas panel | BLOCKED |
| OE-UI-002 | UI | UITapHint one-time pulse hint | BLOCKED |
| OE-UI-003 | UI | UIHUD score display | BLOCKED |
| OE-UI-004 | UI | UIGameOver score + restart | BLOCKED |
| OE-AUDIO-001 | AUDIO | AudioManager named-delegate pattern | BLOCKED |
| OE-FEEL-001 | FEEL | GameFeelManager punch+shake via ShakeOffset | BLOCKED |
| OE-BUILD-001 | BUILD | NeonGlow URP material + APK | BLOCKED |

### Architecture notes (for next agent)
- event_bus: GameEvents.cs static class — ALL inter-script communication goes through it
- player_registry: PlayerRegistry.cs static class — use PlayerRegistry.Player instead of FindGameObjectWithTag
- score_ownership: ONLY ScoreManager owns score — PlayerCollider fires OnPlanetReached, ScoreManager increments
- camera_shake: write to CameraFollower.ShakeOffset — NOT to transform.localPosition (LateUpdate conflict)
- audio_delegates: AudioManager MUST use named delegate fields — anonymous lambdas cannot be unsubscribed (lambda leak on Restart)
- death_guard: GameStateManager._isDead prevents multiple DeathSequence coroutines (PlayerMovement fires GameOver every frame while off-screen)
- input_guard: InputManager._isGameActive prevents taps on Game Over screen
- unity_version: 6000.4.0f1 | pipeline: Universal 2D URP | input: Legacy (Both mode in Player Settings)
- no_find: FindObjectOfType and FindGameObjectWithTag are FORBIDDEN everywhere

### Beads task ID
OE-BOOTSTRAP (manual)

### Next task
OE-SETUP-001
