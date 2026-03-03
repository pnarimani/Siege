# Laws

Laws are edicts passed by the player to manage the city during the siege. Each law has specific enact conditions, effects, and narrative flavor.
Laws are permanently active once enacted and cannot be revoked. 
They represent major policy decisions that shape the city's trajectory through the siege. 
Some laws may have prerequisites or be mutually exclusive with others, forcing the player to make tough choices about how to govern under pressure.
Some consequences of laws are immediate, while others unfold over time, reflecting the long-term impact of these decisions on the city's survival and morale.
Some consequences can also fade out over time.

Laws can be enacted during day phase.

---

## Abandon Outer Ring

- **Id:** `abandon_outer_ring`
- **Name:** Abandon Outer Ring

**Description/Tooltip:** Immediately lose Outer Farms, x0.8 daily siege damage (-20%), +15 unrest. Requires Outer Farms integrity < 40.

**Enact Conditions:**
- Outer Farms must not be lost
- Outer Farms integrity < 40

**Narrative Text:** You order evacuation of the outer farms. Families are dragged from their homes, their livestock left behind to the enemy.
The perimeter contracts. Fewer walls to defend, but the lost land feeds fewer mouths.
Siege pressure eases as the enemy must cover more ground to reach us.

---

## Burn the Dead

- **Id:** `burn_the_dead`
- **Name:** Burn the Dead

**Description/Tooltip:** -15 sickness, -2 fuel, -10 morale

**Enact Conditions:**
- Faith Risen flag must NOT be set
- Sickness > 35

**Narrative Text:** Pyres burn day and night. The stench of cremation fills the air. The dead find no proper burial, but at least they no longer spread plague.

---

## Cannibalism Law

- **Id:** `cannibalism`
- **Name:** Cannibalism Law

**Description/Tooltip:** Permanent. Food from deaths (1-10/day), -5 morale/day, +3 sickness/day, -3 unrest/day. On enact: Tyranny +3, Fear +2, +20 unrest, 5 worker desertions. 15% daily guard desertion chance, 10% daily worker desertion chance. Requires food <= 40 and food deficit.

**Enact Conditions:**
- Cannibalism law enabled in game balance
- Not already enacted
- Food <= 40
- At least 1 consecutive food deficit day

**Narrative Text:** A terrible law is passed. The dead shall feed the living. No one speaks of it openly, but the smell of the cookfires has changed.

---

## Collective Farms

- **Id:** `collective_farms`
- **Name:** Collective Farms

**Description/Tooltip:** +30% food production, +5 morale on enact, +3 unrest/day (arguments over sharing). Requires Faith >= 2.

**Enact Conditions:**
- NOT under Martial State
- Faith >= 2

**Narrative Text:** The farms are collectivized. All harvest is shared equally among the people. Arguments erupt over fair portions, but bellies are fuller.

---

## Conscript the Elderly

- **Id:** `conscript_elderly`
- **Name:** Conscript the Elderly

**Description/Tooltip:** Convert all elderly (X) to workers. -20 morale, +10 unrest. 1 death/day from exhaustion. Day 8+.

**Enact Conditions:**
- People First flag must NOT be set
- Tyranny >= 1
- Day >= 8
- Elderly population > 0

**Narrative Text:** The elderly are sent to the forges and walls. X former elders take up tools, their aged hands ill-suited for labor.

---

## Curfew

- **Id:** `curfew`
- **Name:** Curfew

**Description/Tooltip:** -5 unrest/day, -15% production. Requires unrest > 50. Incompatible with Martial Law.

**Enact Conditions:**
- NOT currently under Martial Law
- Unrest > 50

**Narrative Text:** No one may walk the streets after sunset. The city becomes a prison at dusk.

---

## Emergency Shelters

- **Id:** `emergency_shelters`
- **Name:** Emergency Shelters

**Description/Tooltip:** +4 capacity to all non-lost zones, +2 sickness/day, +2 unrest/day, +8 unrest on enact. Requires first zone loss.

**Enact Conditions:**
- NOT under Martial State
- A zone loss has occurred

**Narrative Text:** Emergency shelters erected in [zone name]. Cramped but shelter nonetheless.
The displaced huddle together. Desperation breeds disease.

---

## Extended Shifts

- **Id:** `extended_shifts`
- **Name:** Extended Shifts

**Description/Tooltip:** +25% production, +2 sickness/day, 30% chance of 1 death/day, -15 morale on enact. Day 5+.

**Enact Conditions:**
- Faith < 4
- Day >= 5

**Narrative Text:** The factories and forges roar through the night. Sleep is a weakness the siege cannot afford.

---

## Faith Processions

- **Id:** `faith_processions`
- **Name:** Faith Processions

**Description/Tooltip:** +15 morale on enact, -10 materials, +5 unrest. Daily: +2 morale, +1 sickness from gatherings. Requires morale < 40.

**Enact Conditions:**
- Iron Fist flag must NOT be set
- Morale < 40

**Narrative Text:** Priests lead processions through the streets, chanting prayers for salvation. The faithful find comfort; others see superstition.

---

## Food Confiscation

- **Id:** `food_confiscation`
- **Name:** Food Confiscation

**Description/Tooltip:** +35 food, +25 unrest, -15 morale, 3 deaths, +2 unrest/day. Requires food < 60.

**Enact Conditions:**
- Faith < 3
- Food < 60

**Narrative Text:** Soldiers empty pantries and storerooms. Those who resist are made examples of. The food will keep the many alive — at the cost of the few.

---

## Garrison Mandate

- **Id:** `garrison_mandate`
- **Name:** Garrison Mandate

**Description/Tooltip:** Convert 3 workers to guards on enact. +1 guard every 3 days. -5 food/day, -5 morale on enact. Requires Fortification >= 4.

**Enact Conditions:**
- People First flag must NOT be set
- Fortification >= 4

**Narrative Text:** The garrison mandate is declared. X workers take up arms. Every able body will serve the walls.

---

## Mandatory Guard Service

- **Id:** `mandatory_guard_service`
- **Name:** Mandatory Guard Service

**Description/Tooltip:** Convert 5 workers to guards, -4 food/day, -10 morale. Requires unrest > 40.

**Enact Conditions:**
- Unrest > 40

**Narrative Text:** Workers are conscripted into the garrison. X take up arms, leaving their trades behind.

---

## Martial Law

- **Id:** `martial_law`
- **Name:** Martial Law

**Description/Tooltip:** Unrest cannot exceed 60, morale capped at 35. 2 executions/day, -10 food/day. Requires unrest > 75. Incompatible with Curfew.

**Enact Conditions:**
- Faith < 5
- NOT currently under Curfew
- Tyranny >= 2
- Unrest > 75

**Narrative Text:** The garrison takes control. Soldiers patrol every street. Dissent will be answered with steel.

---

## Medical Triage

- **Id:** `medical_triage`
- **Name:** Medical Triage

**Description/Tooltip:** -50% medicine usage, 3 sick deaths/day, -2 morale/day. Requires medicine < 20.

**Enact Conditions:**
- People First flag must NOT be set
- Tyranny >= 2
- Medicine < 20

**Narrative Text:** The sick ward becomes a sorting ground. Doctors must choose who receives medicine and who is left to die.

---

## Oath of Mercy

- **Id:** `oath_of_mercy`
- **Name:** Oath of Mercy

**Description/Tooltip:** +5 morale/day, -2 sickness/day, -10% production. Requires Faith >= 4 and Tyranny <= 2.

**Enact Conditions:**
- Iron Fist flag must NOT be set
- Faith >= 4
- Tyranny <= 2

**Narrative Text:** An oath is sworn before the people: no life shall be taken by decree. The sick shall be tended, the hungry fed. Production slows, but hope endures.

---

## Public Executions

- **Id:** `public_executions`
- **Name:** Public Executions

**Description/Tooltip:** -25 unrest instantly, -20 morale, 5 deaths. Requires unrest > 60.

**Enact Conditions:**
- Faith < 4
- Unrest > 60

**Narrative Text:** Five bodies hang from the gallows. The crowd watches in silence. Fear keeps the streets calm — for now.

---

## Purge the Disloyal

- **Id:** `purge_disloyal`
- **Name:** Purge the Disloyal

**Description/Tooltip:** -30 unrest, -15 morale, 8 deaths. One-time. Requires Tyranny >= 7 and Martial State.

**Enact Conditions:**
- People First flag must NOT be set
- Tyranny >= 7
- Under Martial State

**Narrative Text:** The purge begins at dawn. Lists are read aloud. Doors are broken down. By nightfall, the streets run red. No one speaks against the regime now.

---

## Scorched Earth Doctrine

- **Id:** "scorched_earth"
- **Name:** Scorched Earth Doctrine

**Description/Tooltip:** Siege damage -30% permanently. -20 materials on enact. +5 unrest/day. Outer zones cannot be evacuated. Requires Fortification >= 6 and Garrison State.

**Enact Conditions:**
- Faith < 4
- Fortification >= 6
- Under Garrison State

**Narrative Text:** Everything beyond the inner walls is set ablaze. The enemy will find nothing but ash and stone. The walls will hold — or we all perish within them.

---

## Shadow Council

- **Id:** `shadow_council`
- **Name:** Shadow Council

**Description/Tooltip:** -3 unrest/day, +5% production, 1 death/day (dissenters vanish). Morale capped at 30. Requires Tyranny >= 5 and Iron Fist.

**Enact Conditions:**
- Faith < 3
- Tyranny >= 5
- Iron Fist flag is set

**Narrative Text:** A council of masked figures now governs from the shadows. Dissenters vanish in the night. Order is absolute.

---

## Strict Rations

- **Id:** `strict_rations`
- **Name:** Strict Rations

**Description/Tooltip:** -25% food consumption, -10 morale on enact, +3 unrest/day, +1 sickness/day.

**Enact Conditions:**
- None (always available)

**Narrative Text:** You order portions halved. The bowls are empty, but they will stretch our stores.

---

## Water Rationing

- **Id:** `water_rationing`
- **Name:** Water Rationing

**Description/Tooltip:** -25% water consumption, +1 sickness/day, +2 unrest/day, -10 morale on enact.

**Enact Conditions:**
- None (always available)

**Narrative Text:** You order water rations cut. The people drink less, but what remains is stretched with river water — it carries disease.

---
