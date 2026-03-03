### Mission

Implement a complete playable MVP of a siege survival city manager where the player must endure between 35 to 45 days.
The enemy cannot be defeated; the city cannot be stabilized. The core experience is **managing decline under dual pressure**:

* **External siege pressure** causes **zone contraction** (outer districts fall/evacuate, perimeter shrinks).
* **Internal collapse** (morale/unrest/sickness) escalates, worsened by **overcrowding** when zones contract.

Most runs should fail before **Day 25**. Survival to Day 40 should be rare and feel costly.

### Development notes
* Make variables easy to tune
* We are making the game in Unity
* Use UI Toolkit for UI
* The game is 3D with a top-down camera
* Architecture the code in such a way that it's easy to explain to the player WHY something is happening.
* Use maintainable and extendable architecture.
* Make sure the player knows all the important game state, all the available actions and the cost and consequences of those actions.
* Prefer composition to inheritance.
* Avoid unnecessary abstractions
* If you have a solution to a problem, ALWAYS ASK YOURSELF: Is there a more elegant way to solve this problem?
* Use constants in each class for configuration numbers and variables. DO NOT read from a json or external config files.
* Folder structure should be on feature to feature basis. Avoid folder names like "Models". Create folders like "Laws", "Missions", "Simuation", etc.


### EMOTIONAL TARGET

The player must feel:

* Constant pressure
* Moral compromise
* Shrinking space
* No clean solution
* Survival equals sacrifice

Success must feel like endurance, not triumph.

### Ask Questions

If something is not clear, or something feels contradictory, ASK QUESTIONS.

Do not move forward until you are confident that you understand both INTENT and REQUIREMENTS of every feature.

---

# GLOBAL STARTING STATE (Day 1)

* Population: `starting_population`
    * `healthy_workers` Healthy Workers
    * `sick_workers` Sick Workers
    * `guards` Guards
    * `wounded_guards` starting 0
    * `elderly` Elderly (consume, don’t work)
* Food: `starting_food`
* Water: `starting_water`
* Fuel: `starting_fuel`
* Medicine: `starting_meds`
* Materials: `starting_materials`
* Morale: `starting_morale`
* Unrest: `starting_unrest`
* Sickness: `starting_sickness`
* Siege Intensity: 1

City is already unstable. If the player does nothing smart, collapse begins by Day 6–8.

Food and Water must be critical within first 6–8 days without intervention.

**Important**: In this document, when we mention "resources", we mean any of the variables above.

---

# Core Loop & Simulation

This game is a real time game similar to FrostPunk. The player can pause the game by hitting "Space" key.
Introduce a variable for day length with the default value of 60 seconds.

All the resources are produced and consumed in real time. 

---

# City Model

### Zones (5, ordered)

1. Outer Farms
2. Outer Residential
3. Artisan Quarter
4. Inner District
5. Keep

Each zone has:

* Integrity (0–100)
* Capacity
* Population currently housed
* Production modifiers / on-loss effects
* “Active perimeter” = the outermost non-lost zone
* Buildings

Define a variable for each zone's starting parameter.

**Zone Integrity is the defensive line.** There is no separate wall HP. Siege always targets the **active perimeter zone**.

Each zone is a physical area in the world. Each zone has buildings (See Buildings.md)

### Overcrowding rule (stacking)

For every `overcrowding_threshold` over capacity in a zone, there can be consequences to other resources.

For example (DO NOT TAKE THE CONCRETE NUMBERS, DEFINE VARIABLES):
* +2 Unrest/day
* +2 Sickness/day
* +5% Food consumption

Apply after deficits, before sickness/unrest progression.


### Zone loss

If a zone reaches Integrity ≤ 0 OR is voluntarily evacuated:

* It becomes Lost permanently
* All its population is forced inward to next surviving zone(s)
* Apply loss shock (see below)
* Active perimeter moves inward
* Production modifiers update

**Loss shock (natural fall)**
On natural fall (Integrity ≤ 0), there will be penalities to player's resources (unrest, sicness, morale, etc).

**Controlled evacuation shock**
On voluntary evacuation, there will be separate penalities to player's resources.

---


# Evacuation Rules (Player Agency)

Evacuation exists and is irreversible, intended as “trade land for time.”

### Eligibility (no free out-of-order contraction)

A zone can be evacuated only if:

* All outer zones are already Lost
  OR
* That zone Integrity < `evac_integrity_threshold`
  OR
* Siege Intensity ≥ `evac_siege_threshold`

Keep cannot be evacuated.


### Zone-specific additional penalties

When evacuating a zone, a percentage of `salvage_zone_percentage` of resources in the buildings in that zone are transferred to StorageBuildings in other, safer zones.

Daily Siege Damage = (`perimiter_scaling` + Siege Intensity) × `PerimeterFactor`
PerimeterFactor based on active perimeter:

* Outer Farms: 1.0
* Outer Residential: 0.9
* Artisan Quarter: 0.8
* Inner District: 0.7
* Keep: 0.6

This is the primary mechanical benefit of early evacuation.

---

## Buildings

Buildings are preplaced in the zones.

See Buildings.md

## Resources

Resources are physically stored in the world inside StorageBuildings.

Losing a zone to an offense (without evacuating earlier) would cut the player's access to the resources stored in that zone. 
Evacuating a zone would transfer a percentage of those resources to the remaining StorageBuildings.

We will later add a mission where the player can send a couple of workers during the night phase to scavenge resources from a lost zone, but the player will never be able to access 100% of the resources in a lost zone.

## Guards

Guards are always on duty. They cannot be reassigned. Workers can only be turned into guards using player actions but guards will never be turned into workers.

# Laws

Implement each law as their own class, with clear effects and prerequisites.

Players are NOT forced to enact a law.

See Laws.md

# Emergency Orders

See Orders.md

---

# Missions

See Missions.md

# Events (Trigger Rules)

See Events.md

---

# Siege System

* Siege Intensity starts at 1 (may be adjusted by profile)
* Intensity can increase dynamically based on the condition of the player. (Make this part easy to adjust)
* Missions can affect the siege intensity.
* Caps at 6

# Loss Conditions (only these)

1. Keep Integrity ≤ 0 → Breach (Game Over)
2. Unrest > `revolt_threshold` → Revolt (Game Over)
3. Food and Water both 0 for `food_water_loss_threshold` consecutive days → Total Collapse (Game Over)

No other hidden fail states.