# Orbit Escape Implementation Roadmap v2

Status: working implementation roadmap
Date: 2026-04-08
Source: derived from `Docs/DesignDialogueLog.md`
Purpose: translate agreed design direction into a practical build order without overcommitting to unresolved systems.

## How To Use This File

- This is not a full production plan. It is the next sensible development order.
- If a task conflicts with the dialogue log, prefer the dialogue log.
- If a decision depends on an open question, avoid locking it prematurely.

## Development Principle

Build the next version of the game in this order:

1. make decisions readable
2. make planets meaningfully different
3. make route choice real
4. reward bold play
5. make state transitions feel great
6. keep room for future systems without implementing them yet

## Milestone 1 - Planet Variety Vertical Slice

Goal:
- the player can see meaningful differences between planets
- the camera shows more than one potential path
- the run starts feeling like route choice, not only linear timing

### Deliverables

- Camera pass v1
  - support 2-3 visible planets more often
  - bias framing toward the next decision, not only the player
  - keep tension by avoiding overly zoomed-out framing

- Planet archetypes v1
  - Safe Planet
  - Risk Planet
  - Tempo Planet

- Planet visuals v1
  - distinct size
  - distinct orbit size
  - distinct color / aura
  - readable identity without text

- Generator pass v1
  - no longer think only in distance
  - begin supporting scenario roles:
    - recovery
    - temptation
    - tempo spike
    - fork choice

### Success Criteria

- a playtester can usually identify at least 2 meaningful options in motion
- planet types feel different before the player learns scoring details
- runs no longer feel like repeating the same planet over and over

## Milestone 2 - Timing And Choice

Goal:
- release timing feels skill-based instead of fuzzy
- route choice has gameplay consequences

### Deliverables

- Timing readability pass
  - improve relation between orbit position and visible targets
  - keep safe / optimal / greedy release paths possible

- Route choice pass v1
  - choices differ by safety, next timing difficulty, and reward potential
  - avoid fake choices

- Trajectory readability pass
  - help prediction without adding noisy UI
  - ensure trajectory visuals assist the player rather than compete with orbit rings

### Success Criteria

- players can describe some jumps as "safe" and others as "greedy"
- players feel responsible for good or bad timing, rather than confused by it

## Milestone 3 - Reward And Pressure

Goal:
- bold play becomes meaningfully attractive
- strategy emerges inside a run

### Deliverables

- Score system expansion
  - reward by target type
  - reward by jump quality
  - reward by chain / multiplier

- Pressure tuning pass
  - align planet risk and asteroid pressure
  - make danger shape route choice instead of acting as random punishment

### Success Criteria

- safe play is viable
- risky play is clearly more profitable
- players can explain why they chose a harder route

## Milestone 4 - Feel Pass

Goal:
- the run feels satisfying, not only functional

### Deliverables

- Orbit capture feedback v1
  - flash / pulse
  - scale punch or equivalent impulse
  - sound cue

- Launch feedback v1
  - stronger release feel
  - better trail / directional streak
  - clearer sound identity

- Danger feedback v1
  - near miss emotion
  - readable asteroid pressure

- Death feedback v1
  - fair
  - readable
  - emotionally coherent

### Success Criteria

- successful orbit capture feels like a strong beat
- launch feels intentional and energetic
- death causes "again" more often than frustration

## Milestone 5 - Future-Safe Refactor Pass

Goal:
- ensure the game can later support richer systems without architectural dead ends

### Deliverables

- Data/model thinking around planets
  - visual identity
  - orbit behavior
  - hazard profile
  - reward profile

- Code structure pass only where needed
  - enough flexibility for more planet types later
  - enough flexibility for hazard systems later
  - no giant framework

### Success Criteria

- adding a new planet archetype later does not require rewriting the loop
- future systems can be layered in without breaking readability-first design

## Explicitly Deferred

These are intentionally not first-wave implementation targets:

- full progression systems
- full economy / currencies
- resource harvesting
- permanent ship stat upgrades
- content grind loops
- many planet families at once
- black holes, moons, rings, belts, radiation, heat, HP systems in full form

They are valid future directions, but not the next implementation priority.

## Open Questions To Revisit Later

- exact camera behavior
- ideal number of visible planets
- multiplier rules
- chain rules
- authored vs procedural run structure
- fail state revision
- event-specific feedback mapping
- when to introduce resources / HP / exposure

Do not force these early unless one becomes a blocking issue.

## Recommended Next Build Order

If implementation starts now, the next practical sequence should be:

1. camera framing pass
2. planet archetype data/behavior pass
3. generator pass for route choice
4. trajectory and timing readability pass
5. score and multiplier prototype
6. orbit capture / launch feel pass

## Definition Of A Good Next Version

The next strong version of Orbit Escape should make a player say:

- "I can see my options."
- "Those planets are not all the same."
- "I chose the risky one on purpose."
- "That release felt good."
- "I died because I got greedy, not because the game was unclear."

