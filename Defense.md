# Defense System

The player can take defensive actions to defend the city against siege damage. 
These defenses can be built in the perimeter zones and provide various benefits to mitigate incoming damage. 
However, they require resources and strategic planning to use effectively.
Some defenses will require guard assignment, which creates trade-offs between offense and defense.


## Defense Types

### Barricades

A defensive buffer that absorbs siege damage before it reaches zone integrity.

- **Cost**: 15 Materials
- **Effect**: Adds 12 damage buffer to the zone
- **Usage**: Buffer absorbs incoming siege damage each day. The buffer decreases by the amount of damage absorbed. Can be rebuilt multiple times to replenish.

### Oil Cauldrons

A one-time defensive measure that completely negates siege damage for a single day.

- **Cost**: 10 Fuel + 10 Materials
- **Effect**: Negates ALL siege damage for one day
- **Usage**: Single-use defense. After being triggered during siege resolution, the cauldron is consumed. Only one cauldron can be prepared per zone at a time.

### Archer Posts

Permanent defensive structures that reduce siege damage when staffed with guards.

- **Cost**: 20 Materials
- **Effect**: Reduces siege damage by 12%
- **Requirements**: Must assign exactly 2 guards to activate
- **Usage**: Build the post first, then use `assign-archer` command to assign guards. The post only provides damage reduction when fully staffed. Guards assigned to archer posts cannot be committed to defensive posture.

## Commands

- `build defense <type> <zone>` - Build a defense structure in a zone
- `assign-archer <zone> <count>` - Assign guards to an archer post (0-2)

## Strategic Notes

- Barricades are best for early-game survival when siege damage is lower
- Oil cauldrons are valuable for negating high-intensity siege days
- Archer posts provide permanent damage reduction but require guard allocation
- Defenses only protect the perimeter zone they are built in
- If a zone is lost, all defenses in that zone are lost with it

---

## Proposed Additional Defenses

These are potential defensive mechanics that could be added to expand strategic options.

### Watchtower

An early warning system that detects incoming attacks.

- **Concept**: Provides small siege delay or reduces night raid casualties
- **Strategic purpose**: Early warning allows better preparation

### Spike Traps

Cheap, limited-use defensive traps embedded in the zone perimeter.

- **Concept**: Activates at high siege intensity, absorbs damage above a threshold
- **Strategic purpose**: Budget option for emergency situations

### Repair Crews / Maintenance Hall

Passive integrity regeneration for zones.

- **Concept**: Produces integrity instead of resources. Zones can recover lost integrity over time.
- **Strategic purpose**: Addresses the core gap that zones can only lose integrity, never recover. Creates resource allocation decisions between production and recovery.

### Signal Beacon

Global defense system providing coordination bonuses.

- **Concept**: When built across multiple zones, provides a multiplier to all archer posts or a coordination bonus
- **Strategic purpose**: Encourages balanced zone development

### Emergency Patch

One-time integrity restoration for critical zones.

- **Concept**: Uses excess materials to instantly restore zone integrity
- **Strategic purpose**: Emergency option when a zone is about to fall

### Counterweight Catapult / Ballista

Active defense that fires back at attackers.

- **Concept**: Provides a small chance to reduce siege intensity or deal damage to attacker morale
- **Strategic purpose**: Offensive countermeasure
