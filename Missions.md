# Missions

Missions are strategic operations that consume workers and time to achieve specific objectives. 
Unlike daily orders, missions take multiple days to complete and carry significant risk.

Missions are done during the night phase.

## Mission vs Scavenging

**Missions** are predefined operations with fixed outcomes based on probability rolls. They require workers, take several days, and affect the city's strategic situation (siege delay, damage reduction, resources).

**Scavenging** is a separate system where the player chooses the mission destination from a pool of generated locations. Each location has different:
- Danger level (Low, Medium, High)
- Maximum visits before depleted
- Possible rewards
- Casualty chance

Scavenging occurs during the Night phase and lets players pick from 4 randomly generated locations each night.

---

## Mission List

### 1. Forage Beyond Walls
Send workers into the wild lands outside the city to hunt and gather edible plants.

| Property | Value |
|----------|-------|
| Duration | 4 days |
| Worker Cost | 5 |
| Always Available | Yes |

**Outcomes:**
- **50%** (+50% if siege < 4, +40% if siege >= 4): +80 Food. *Narrative: "The foragers returned with a bountiful haul — wild game and edible roots. The granary swells with 80 food."*
- **25%** (+25% if siege < 4, +20% if siege >= 4): +50 Food. *Narrative: "The foragers found some food in the wild lands, though not as much as hoped. 50 food is better than nothing."*
- **25%** / **40%**: Ambush — 5 casualties (2 dead, 3 wounded), +10 Unrest. *Narrative: "Enemy scouts ambushed the foraging party. Only a handful return — 5 casualties in the fields. The city mourns."*

**Modifiers:**
- If Morale >= 60: +5% to high outcome chance

---

### 2. Night Raid
A nighttime assault on enemy positions to disrupt siege preparations.

| Property | Value |
|----------|-------|
| Duration | 2 days |
| Worker Cost | 6 |
| Always Available | Yes |

**Outcomes:**
- **30%** (+5% if Guards >= 10): Siege delay +3 days. *Narrative: "The raid was a masterwork! Supplies were burned, siege engines destroyed. The enemy will not attack for 3 more days."*
- **40%**: Siege delay +2 days. *Narrative: "The raid caused some disruption. Enemy preparations are delayed, though not stopped. 2 days of reprieve bought."*
- **30%**: 6 casualties (2 dead, 4 wounded), +15 Unrest. *Narrative: "The raid failed catastrophically. The enemy was waiting. 6 casualties — the city questions your leadership."*

**Also grants:** +1 Fortification (5 days)

---

### 3. Search Abandoned Homes
Send workers to scavenge the abandoned quarter for supplies.

| Property | Value |
|----------|-------|
| Duration | 2 days |
| Worker Cost | 4 |
| Always Available | Yes |

**Outcomes:**
- **45%** (+5% if Sickness < 20): +40 Materials, +5 Sickness. *Narrative: "The team returns from the abandoned quarter with salvaged materials. They found 40 materials, though the search exposed them to disease."*
- **35%**: +25 Medicine, +5 Sickness. *Narrative: "The team discovered a hidden cache of medicine in an abandoned pharmacy. 25 medicine secured, but the dusty air brought illness."*
- **20%**: +15 Sickness, 2 Deaths. *Narrative: "Disaster! The team found only death. The abandoned homes were plague pits — two volunteers fell to the sickness, and the survivors carry it back."*

---

### 4. Negotiate Black Marketeers
Send diplomats to negotiate with criminal elements for supplies.

| Property | Value |
|----------|-------|
| Duration | 3 days |
| Worker Cost | 2 |
| Always Available | Yes |

**Outcomes:**
- **45%**: +60 Water, +10 Unrest. *Narrative: "The negotiators secured water from the black market. The people drink, but whispers of dealings with criminals spread through the streets."*
- **30%**: +50 Food, +10 Unrest. *Narrative: "The negotiators returned with food. Bellies are full tonight, but the cost of dealing with smugglers breeds contempt."*
- **25%**: Ambush — 2 Deaths, +25 Unrest. *Narrative: "Ambush! The black marketeers turned on your people. Two are killed, the rest barely escape. The deal was a trap."*

---

### 5. Sabotage Enemy Supplies
Send saboteurs to disrupt enemy supply lines. Requires moral compromise.

| Property | Value |
|----------|-------|
| Duration | 3 days |
| Worker Cost | 4 |
| Requires | Tyranny >= 2 |

**Outcomes:**
- **40%**: Siege damage x0.7 for 5 days. *Narrative: "The saboteurs succeeded beyond expectation! Enemy supply lines are disrupted — siege damage reduced by 30% for 5 days."*
- **30%**: Siege damage x0.85 for 3 days. *Narrative: "The saboteurs partially succeeded. Enemy supplies are scarce — siege damage reduced by 15% for 3 days."*
- **30%**: 4 Deaths, +20 Unrest. *Narrative: "Catastrophic failure! The saboteurs were caught and executed publicly. 4 dead, and the enemy taunts you with their heads."*

**Also grants:** +1 Tyranny (5 days)

---

### 6. Scouting Mission
Send scouts to gather intelligence on enemy positions.

| Property | Value |
|----------|-------|
| Duration | 2 days |
| Worker Cost | 2 |
| Requires | Enabled in settings |

**Outcomes:**
- **Varies** (base 60% + mission bonus): Intel buff for 5 days + Siege Warning. May also narrow relief army estimate. *Narrative: "The scouts returned with valuable intelligence. Intel buff active for 5 days. Enemy movements mapped."*
- **Failure**: 3 Deaths, +15 Unrest. *Narrative: "The scouting party was discovered. 3 scouts killed. No intel gathered."*

**Also grants:** +1 Fortification (5 days)

---

### 7. Sortie
A coordinated assault using guards to break the siege.

| Property | Value |
|----------|-------|
| Duration | 1 day |
| Guard Cost | 8 |
| Requires | Enabled in settings, Fortification >= 1 |

**Outcomes:**
- **40%**: Siege intensity -1, siege delay +3 days. *Narrative: "The sortie was a stunning success! Enemy siege engines destroyed. Siege intensity reduced. Escalation delayed by 3 days."*
- **30%**: Siege damage x0.7 for 3 days. *Narrative: "The sortie disrupted enemy positions. Siege damage reduced to 70% for 3 days."*
- **30%**: 4 Guard deaths, +20 Unrest. *Narrative: "The sortie was repulsed with heavy losses. 4 guards fell in the failed assault."*

**Also grants:** +1 Tyranny, +1 Fortification (5 days each)

---

### 8. Raid Civilian Farms
Desperate measure — raid nearby farms for food.

| Property | Value |
|----------|-------|
| Duration | 2 days |
| Worker Cost | 4 |
| Requires | Tyranny >= 3 |

**Outcomes:**
- **60%**: +60 Food. *Narrative: "The raid was a success. Granaries plundered, livestock driven back. +60 food — taken from those who had little to begin with."*
- **40%**: +30 Food, +15 Unrest, 2 Deaths. *Narrative: "The raid met resistance. Farmers fought back. +30 food, but 2 dead and the people whisper of your cruelty."*

**Also grants:** +1 Tyranny

---

### 9. Diplomatic Envoy
Send an envoy to negotiate with enemy commanders or rally allies.

| Property | Value |
|----------|-------|
| Duration | 3 days |
| Worker Cost | 3 |
| Requires | Faith >= 3 |

**Outcomes:**
- **40%**: Siege delay +5 days, may accelerate relief army. *Narrative: "The envoy spoke with eloquence and conviction. The enemy commander hesitates. Siege escalation delayed by 5 days."*
- **30%**: Siege delay +2 days. *Narrative: "Negotiations bore some fruit. A temporary truce is agreed. Siege escalation delayed by 2 days."*
- **30%**: 3 Deaths, +10 Unrest. *Narrative: "The envoy was seized and executed. Their heads were catapulted back over the walls. 3 dead. The people despair."*

**Also grants:** +1 Faith (5 days)

---

### 10. Engineer Tunnels
Dig tunnels to undermine enemy siege positions.

| Property | Value |
|----------|-------|
| Duration | 4 days |
| Worker Cost | 5 |
| Requires | Fortification >= 3 |

**Outcomes:**
- **50%**: Siege damage x0.6 for 5 days. *Narrative: "The tunnels are complete! Engineers have undermined the enemy's siege works. Siege damage reduced to 60% for 5 days."*
- **30%**: Siege damage x0.8 for 3 days. *Narrative: "Partial success. The tunnels diverted some enemy efforts. Siege damage reduced to 80% for 3 days."*
- **20%**: 4 Deaths, +10 Unrest. *Narrative: "Tunnel collapse! The earth swallowed 4 engineers whole. The survivors crawl out trembling. The enemy laughs."*

**Also grants:** +1 Fortification (5 days)

---

## Scavenging Locations

Each night, 4 random locations are generated. Players choose which to scavenge.

### Location Types

| Location | Danger | Max Visits | Casualty Chance | Possible Rewards |
|----------|--------|------------|-----------------|------------------|
| Abandoned Church | Low | 2 | Low | Medicine (4-8) |
| Ruined Granary | Medium | 3 | Medium | Food (12-25) |
| Enemy Supply Camp | High | 1 | High | Materials (15-30), Max 2 casualties |
| Old Well | Low | 3 | Low | Water (10-20) |
| Merchant Caravan Wreck | Medium | 2 | Medium | Food (5-10), Materials (5-10), Medicine (2-5) |
| Collapsed Mine | High | 1 | High | Fuel (10-18), Materials (8-15), Max 2 casualties |
| Field Hospital Ruins | Medium | 2 | Medium | Medicine (4-10), Food (5-12) |
| Watchtower | High | 1 | High | Intel + Materials (3-6) |

**Danger Levels:**
- **Low**: ~10% casualty chance
- **Medium**: ~20% casualty chance  
- **High**: ~35% casualty chance, up to 2 casualties

Scavenging results include resources gained, casualties (deaths/wounded), and narrative text. Locations become depleted after their maximum visits.
