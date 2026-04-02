# Orbit Escape

Mobile arcade prototype in Unity 6.

## Concept

The player controls a ball orbiting around planets.

Core loop:

1. The ball rotates around the current planet
2. Tap to launch along the tangent
3. Reach the next planet and enter orbit
4. Gain score for each successful transfer
5. Avoid asteroids and do not fly off-screen

The prototype goal is to validate:

- timing and release decision-making
- satisfaction of accurate transfers
- rising tension from asteroid pressure

## Current Status

This repository is in bootstrap / early prototype setup.

Implemented project groundwork:

- Unity 6 URP 2D project initialized
- `bd` (beads) issue tracking initialized
- agent workflow documented in `AGENTS.md` and `CLAUDE.md`
- `CHANGELOG.md` initialized
- 22 gameplay / build tasks created in beads with dependency graph

Next ready task:

- `OE-SETUP-001` - project config and folder structure

## Tech Stack

- Unity `6000.4.0f1`
- Universal Render Pipeline 2D
- C#
- Android target prototype
- `bd` (beads) for issue tracking

## Repository Workflow

This project uses `bd` for all task tracking.

Useful commands:

```bash
bd ready --json
bd show <id>
bd update <id> --claim --json
bd close <id> --reason "done"
bd dolt push
```

Branch strategy:

- `main` stays as the stable base
- work happens in feature branches
- usually one branch per task

Branch examples:

- `feature/OE-BOOTSTRAP-agent-setup`
- `feature/OE-SETUP-001-android-config`
- `feature/OE-CORE-001-game-events`

## Project Structure

Current Unity structure:

```text
Assets/
  Scenes/
  Settings/
Packages/
ProjectSettings/
```

Planned script structure:

```text
Assets/Scripts/
  Core/
  Player/
  Level/
  Managers/
  UI/
```

## Design Constraints

Architecture rules from the master project document:

- all script communication goes through `GameEvents.cs`
- no `FindObjectOfType`
- no `FindGameObjectWithTag`
- no direct gameplay logic <-> UI coupling
- small single-responsibility scripts

## Key Documents

- [AGENTS.md](AGENTS.md) - Codex workflow and project rules
- [CLAUDE.md](CLAUDE.md) - Claude workflow and project rules
- [CHANGELOG.md](CHANGELOG.md) - AI-readable project history

## Roadmap

1. `OE-SETUP-001` - Android config and script folders
2. Core systems: events, registry, input
3. Player movement and orbit flow
4. Level generation and camera
5. UI, audio, and feel
6. Android build and first APK

## Goal

Build a clean first playable that can answer one question:

Is the orbit-to-tangent movement loop fun enough to keep developing?
