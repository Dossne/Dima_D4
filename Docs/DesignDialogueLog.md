# Orbit Escape Design Dialogue Log

Status: active working vision
Date: 2026-04-08
Purpose: preserve the actual design conversation, agreements, open questions, and future-facing ideas so that humans and AI agents can continue from the same context without flattening everything into a dry spec.

## How To Use This File

- Treat this as a living design log, not a final GDD.
- Prefer this file when you need to understand why a decision exists.
- If a future agent changes direction, they should append a new section rather than silently rewriting the intent.
- Open questions here are intentional. They are not forgotten work. They are postponed decisions.

## Current Product Direction

Orbit Escape should remain a one-tap game at its core, but it should evolve beyond a minimalist prototype into a fast, musical, sometimes tactical arcade game.

The player fantasy is:
- read the space field
- understand the current orbit and visible opportunities
- choose a route
- catch the right moment to release
- feel that success physically and emotionally

The intended experience is not "tap from one planet to the next like stepping stones forever".

The intended experience is:
- fast
- readable
- tense
- skillful
- sometimes strategic
- rewarding when the player takes a smart or greedy risk

## Core Agreements

The following points were explicitly agreed:

- One-tap remains the heart of the game.
- Planets are not decoration. They are the primary gameplay language.
- Risk should be readable before the player commits.
- Success should be felt through feedback, not just understood logically.
- The game should move toward a hybrid of rhythm and tactics:
  - fast and musical in feel
  - but with real route choices and meaningful decisions
- Planet variety should become a central design pillar.
- Future systems should be considered early enough to avoid building a dead-end architecture.

## What "Planets Are The Language" Means

This phrase was clarified in the discussion.

Planets should communicate gameplay meaning through what the player sees:
- size
- color
- orbit size
- orbit behavior
- aura or halo
- nearby hazards
- route consequences

The player should be able to look at a planet and infer something like:
- this is safer
- this is risky but rewarding
- this one changes the rhythm
- this one probably leads to a harder route

If the player needs text to understand planet types, the visual language is failing.

## Phase 1 - Readability Pass

### Intent

Make the game readable and fair enough that the player feels they are making decisions, not guessing.

### Agreed

- The camera should not only follow the player. It should help show the next decision.
- The player should more often see 2-3 meaningful targets in frame, not just one.
- The current orbit, obvious target, and danger should be readable quickly.
- Tap timing should feel like a decision, not "I guess now".
- The game should support:
  - a safe release
  - an optimal release
  - a greedy risky release
- Rhythm matters, but missing the "musical groove" should not itself be punished like a fail state.

### Important clarifications from the dialogue

- The user prefers alternatives to be visible often, not only rarely.
- A dynamic camera is interesting but unresolved.
- Visual hierarchy is important, but overly aggressive highlighting may hurt the visual style.
- Open readability questions should remain open until they become blockers.

### Open Questions

- Exact camera behavior
- Static vs dynamic zoom
- How strongly to highlight "best" targets
- How much visual emphasis secondary targets should get
- How to show good timing windows without ugly UI overlays

## Phase 2 - Planet Language Pass

### Intent

Make planet variety the main gameplay lever rather than only an aesthetic upgrade.

### Agreed Starting Archetypes

Start with 3 archetypes:

- Safe Planet
  - easier
  - more stable
  - lower reward

- Risk Planet
  - smaller and/or tighter
  - harder but still humanly readable
  - higher reward

- Tempo Planet
  - changes the feeling of timing
  - pushes the run into a different rhythm or route pressure

### Agreed differentiation channels

For the first implementation pass, planets should differ through at least:
- core size
- orbit size
- color and aura
- orbit timing behavior

### Clarifications from the dialogue

- "Aura" means a visible halo/field/glow that can be decorative and informative at the same time.
- "Orbit timing behavior" means how the orbit feels to time from, not only its shape.
- Tempo variation may later come from:
  - faster orbit speed
  - unstable timing feel
  - satellites
  - non-circular motion
  - route pressure

### Future-facing ideas explicitly mentioned

- moons
- rings
- asteroid belts
- gravitational fields
- heat / radiation / damage
- black holes
- rare planets
- resource-linked planets

### Open Questions

- Multiplier system design
- Whether classes of planets should have one strict color each or a broader palette family
- How soon to introduce more than 3 planet archetypes

## Phase 3 - Route Choice Pass

### Intent

Give the player real route decisions, not fake ones.

### Clarification from the dialogue

"Real choice" means different visible options produce meaningfully different consequences.

"Fake choice" means multiple visible planets exist, but one is obviously correct and the others are noise.

### Agreed

Route choices can differ by:
- safety
- next timing difficulty
- proximity to danger
- chain potential
- jump length
- scoring potential

The generator should think in scenario roles, not only random distance.

Suggested roles already accepted:
- recovery
- temptation
- tempo spike
- fork choice

### Important note

The user raised a strong concern about too much randomness.

This remains open, but a likely future direction is:
- more controlled opening / safe zone
- more procedural variation later

This has not been fully decided yet.

### Open Questions

- 2 targets, 3 targets, or more in the average frame
- How often route choice should appear
- Whether fail state rules need revision if jump length and route greed become more important
- How procedural vs authored the run should be

## Phase 4 - Reward Structure Pass

### Intent

Give the player a reason to play beautifully and greedily, not only to survive.

### Agreed

Start with run-level motivation, not big meta systems.

For now, the primary reward inside a run is:
- score

Scoring should later support:
- reward by target type
- reward by jump quality
- reward by chain / multiplier

Accepted examples:
- safe planet = lower value
- risk planet = higher value
- tempo planet = special or higher value
- long jump = bonus
- accurate late release = bonus
- risky angle = bonus
- multiple bold decisions in sequence = multiplier

### Important future note

The user explicitly wants the team to remember future possibilities, without implementing them too early:
- cosmetics
- ship skins
- trail effects
- new worlds / star systems / biomes
- resources
- ship archetypes
- progression
- monetization realities

This means:
- do not build heavy progression now
- but do not design current systems in a way that forbids progression later

### Open Questions

- Exact scoring formula
- Multiplier growth and decay
- Future monetization fit
- Best long-term meta layer

## Phase 5 - Feel Pass

### Intent

Every important state transition should feel emotionally satisfying.

### Clarification from the dialogue

When discussing "landing", the intended meaning became:
- successful capture of a new orbit
- not merely touching geometry

### Agreed

Important transitions that should feel strong:
- orbit capture
- launch / release
- danger / near miss
- death

Accepted feel ideas:
- scale punch
- flash / ring pulse
- stronger launch trail
- release sound
- near miss emotion
- fair and readable death

### Important caution

Camera shake should be used carefully.

The user explicitly flagged:
- too much shake can tire or even nauseate some players
- shake may be better reserved for stronger or more negative events

This is intentionally unresolved and should not be forgotten.

### Clarifications added

- "Scale punch" = quick elastic size impulse, not cartoon inflation
- "Directional streak" = short direction-based visual impulse on launch
- "Landing is the drum hit of the game" = orbit capture should act like a rhythmic confirmation beat

### Open Questions

- Which events deserve camera shake
- Which events deserve hit-stop
- Whether all orbit captures should feel equally strong
- Exact launch fantasy:
  - slingshot
  - gravity break
  - escape impulse
- Whether future steering / micro-correction in flight should exist

## Phase 6 - Future-Safe Systems

### Intent

Do not build the current prototype into an architectural dead end.

### Agreed conceptual model

Even before all systems exist, it is useful to think of each planet as having:
- visual identity
- orbit behavior
- hazard profile
- reward profile

Reward profile was clarified as:
- why the player wants to go there
- score value
- bonus potential
- route utility
- future resource or rarity potential

### Agreed future direction

Later systems like HP, heat, radiation, or exposure are valid only if they create more choice.

Good direction:
- safer route = slower / more stable
- risky route = faster progress / more exposure
- player balances timing and survivable risk

Bad direction:
- unavoidable chip damage with no interesting decision

### Future memory items

Keep in mind for later R&D:
- rare planets
- visible rarity from a distance
- resource-linked route choices
- hazard systems that deepen decisions rather than causing attrition

## Important Open Question Backlog

These were intentionally left unresolved. Future agents should not treat them as forgotten.

- Final camera rule set
- Number of planets visible in the average decision frame
- How to visually indicate best / secondary / risky targets
- Exact multiplier logic
- Chain logic
- Procedural vs authored run structure
- Whether current fail state needs revision
- Which feel tools belong to which event
- When to introduce resources, HP, or exposure systems
- How future monetization and progression should fit the core loop without harming pacing

## Practical Guidance For Future Agents

When continuing development:

- Do not jump straight into meta systems.
- Do not add many planet types at once.
- Do not over-randomize the run before readability is solved.
- Do not use feel effects to hide unclear gameplay.
- Do not forget the open question backlog just because it is unresolved.

Recommended order of work:
- readability
- camera
- planet variety
- route choice
- reward structure
- feel
- future systems

## Short Version

Orbit Escape is evolving toward a one-tap cosmic arcade game where:
- planets are readable gameplay symbols
- route choice matters
- timing feels rhythmic but fair
- risky play is more rewarding
- feedback makes each successful orbit capture feel like a beat
- future systems are welcome, but only if they deepen choice rather than add grind

