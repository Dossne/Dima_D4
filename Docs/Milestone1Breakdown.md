# Orbit Escape Milestone 1 Breakdown

Status: implementation planning
Date: 2026-04-08
Depends on:
- `Docs/DesignDialogueLog.md`
- `Docs/ImplementationRoadmapV2.md`

Purpose: define the first focused implementation milestone without drifting into later systems too early.

## Milestone 1 Goal

Transform the current prototype from:
- a mostly linear orbit-to-next-planet loop

into:
- a more readable, varied, route-choice-driven vertical slice

without yet adding heavy meta, resource systems, or many advanced hazards.

## Milestone 1 Summary

This milestone should deliver 4 things:

1. better camera framing
2. basic planet variety
3. more meaningful route choice
4. stronger gameplay readability

If these 4 are not improved, later reward systems and feel polish will sit on a weak core.

## Scope In

Included in this milestone:

- Camera behavior pass v1
- 3 planet archetypes v1
- Planet visual language pass v1
- Generator pass v1 for multi-option situations
- Timing readability pass v1
- Light HUD/readability adjustments if needed to support the above

## Scope Out

Explicitly not in this milestone:

- full multiplier system
- full chain system
- resources / HP / heat / radiation
- progression / cosmetics / unlocks
- moons / rings / black holes / asteroid belts as full systems
- large audio / haptic production pass
- deep death redesign

These are future milestones unless one becomes a direct blocker.

## Deliverable 1 - Camera Pass v1

### Goal

Make the camera show decisions, not only the player.

### Desired Outcome

- player usually sees current orbit plus 2-3 meaningful targets
- frame feels readable, not zoomed out into noise
- scene communicates "what can I do next?"

### Implementation Intent

- review current orthographic framing
- adjust base framing so current orbit and future options coexist more often
- test mild look-ahead toward route direction if needed
- avoid aggressive motion that harms readability or comfort

### Acceptance Signals

- player is less often surprised by off-screen next steps
- targets feel intentionally presented, not randomly discovered

### Open Questions To Respect

- static vs dynamic zoom
- how often to show 3 vs more targets
- how far camera should anticipate future decisions

Do not over-solve these in Milestone 1.

## Deliverable 2 - Planet Archetypes v1

### Goal

Introduce 3 clearly different planet types that affect gameplay meaningfully.

### Archetypes

#### Safe Planet

- larger / easier to read
- wider-feeling orbit timing
- lower reward potential
- used for recovery and stability

#### Risk Planet

- smaller / tighter / more demanding
- still humanly readable and fair
- higher reward potential
- used to tempt greed and skill expression

#### Tempo Planet

- changes orbit feel or route pacing
- not necessarily hardest, but rhythm-altering
- used to break repetition

### Minimum Differentiation

Each archetype should differ through:
- body size
- orbit size
- color / aura
- orbit behavior feel

### Acceptance Signals

- a player can visually tell that planets are not all "the same but recolored"
- different planets imply different route value before scoring is explained

## Deliverable 3 - Generator Pass v1

### Goal

Stop generating only "next planet" situations and begin creating route-choice situations.

### Desired Outcome

- more frequent multi-option frames
- choices with different consequences
- reduced feeling of hopping linearly from one identical node to another

### Scenario Roles To Support

- recovery
- temptation
- tempo spike
- fork choice

### Example Shape

Current orbit presents:
- one safer obvious target
- one more rewarding or more difficult target
- sometimes one extra follow-up route option

### Acceptance Signals

- player can describe why one option feels safer and another feels greedier
- runs start producing small tactical stories

### Important Constraint

Do not chase fully random generation quality yet.

If needed, prefer:
- controlled early-game setups
- then more variation later

over:
- uncontrolled randomness everywhere

## Deliverable 4 - Timing Readability Pass v1

### Goal

Make release timing feel readable and expressive.

### Desired Outcome

- safe release exists
- optimal release exists
- greedy release exists
- players begin to feel "I chose that timing" rather than "I guessed"

### Implementation Intent

- inspect how current target placement interacts with orbit angle
- tune situations so target positioning creates meaningful timing windows
- ensure trajectory visuals help rather than clutter

### Acceptance Signals

- different releases create noticeably different outcomes
- players can intentionally choose safe vs greedy behavior

## Suggested Work Order

Recommended order inside the milestone:

1. Camera pass first
2. Planet archetype definitions second
3. Generator pass third
4. Timing readability pass fourth
5. Small HUD/readability cleanup last, only if necessary

Reason:
- camera and framing define what route choice can even be seen
- planet differences must exist before route choice has meaning
- generator must know what kinds of planets and situations it is placing
- timing only becomes worth tuning once the field itself is readable

## Risks In This Milestone

### Risk 1 - Too much too soon

Adding too many planet types or too many rules too early will destroy readability.

Response:
- stay with 3 archetypes

### Risk 2 - Over-zoomed camera

Showing too much can remove tension and make the game feel visually weak.

Response:
- optimize for readable choice, not maximum information

### Risk 3 - Fake route choice

Visible branches that are not meaningfully different will create noise, not depth.

Response:
- only count a branch as valid if its consequences differ

### Risk 4 - Premature scoring complexity

If score logic becomes too complex before route choice is fun, the game will feel gamified rather than exciting.

Response:
- keep reward systems light in Milestone 1

## Definition Of Done For Milestone 1

Milestone 1 is successful when:

- camera usually presents decisions clearly
- planets are visibly and functionally different
- route choice exists and is understandable
- release timing feels more intentional
- the game is more varied without becoming harder to read

## What Should Come Next

After Milestone 1 succeeds, the natural next step is:

- reward structure pass
- then feel pass

Not before.

