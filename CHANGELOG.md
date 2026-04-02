# CHANGELOG — Orbit Escape
<!-- AI-READABLE PROJECT HISTORY -->
<!-- Format: newest entry first -->
<!-- Each entry written by the agent that completed the task -->
<!-- Purpose: allows any new agent to understand project state without reading full codebase -->

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
