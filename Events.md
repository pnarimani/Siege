# Events Summary

Events are narrative moments that occur during the siege, triggered by specific conditions. 
They represent the unfolding story of the siege — the crises, triumphs, and tragedies that shape the experience of those within the walls.
Only one event can occur per day, and they can only happen during the day phase.

---

## 1. Opening Bombardment

| Field | Value |
|-------|-------|
| **Id** | `opening_bombardment` |
| **Name** | Opening Bombardment |
| **Description** | The siege has begun. Enemy catapults loose their first volley. stones arc through the dawn sky and crash into the outer districts with a sound like the end of the world. |
| **Trigger Condition** | Day 1 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "The first boulders crash into Outer Farms. Smoke rises from burning granaries as the enemy demonstrates their intent."
- **Changes**: Outer Farms integrity -10, Food -10

---

## 2. Enemy Messenger

| Field | Value |
|-------|-------|
| **Id** | `narrative_messenger` |
| **Name** | Enemy Messenger |
| **Description** | A messenger arrives under white flag. "Surrender the city, and your people will be spared." You send him back. |
| **Trigger Condition** | Day 1 |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "A messenger arrives under white flag. "Surrender the city, and your people will be spared." You send him back."

---

## 3. Banners on the Ridge

| Field | Value |
|-------|-------|
| **Id** | `relief_banners` |
| **Name** | Banners on the Ridge |
| **Description** | Banners appear on the eastern ridge — your kingdom's colors. The relief army is here. Hold one more day. |
| **Trigger Condition** | Relief army enabled, 1 day before arrival |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "Banners appear on the eastern ridge — your kingdom's colors. The relief army is here. Hold one more day."

---

## 4. Horns in the Distance

| Field | Value |
|-------|-------|
| **Id** | `relief_horns` |
| **Name** | Horns in the Distance |
| **Description** | The unmistakable sound of war horns echoes from beyond the hills. Someone is coming. Friend or foe, you cannot yet tell. |
| **Trigger Condition** | Relief army enabled, 3 days before arrival |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "The unmistakable sound of war horns echoes from beyond the hills. Someone is coming. Friend or foe, you cannot yet tell."

---

## 5. Dust Clouds on the Horizon

| Field | Value |
|-------|-------|
| **Id** | `relief_dust_clouds` |
| **Name** | Dust Clouds on the Horizon |
| **Description** | Scouts on the watchtower report dust clouds to the east. Could be a caravan. Could be an army. Could be hope. |
| **Trigger Condition** | Relief army enabled, 7 days before arrival |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "Scouts on the watchtower report dust clouds to the east. Could be a caravan. Could be an army. Could be hope."

---

## 6. Supply Carts Intercepted

| Field | Value |
|-------|-------|
| **Id** | `supply_carts_intercepted` |
| **Name** | Supply Carts Intercepted |
| **Description** | Enemy cavalry cut off a supply run at the eastern road. The carts were seized before they reached the gates — their contents lost to the siege. |
| **Trigger Condition** | Days 3-5, 20% chance |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Enemy riders cut down the supply convoy at the eastern road. The food carts are seized before they reach the gates. Hungry mouths will go unfed tonight." OR "Enemy riders intercept the water wagons before they reach the gates. The barrels are smashed and the convoy scattered. The city grows thirstier."
- **Changes**: Either Food -15 or Water -15 (50% chance each)

---

## 7. Smuggler at the Gate

| Field | Value |
|-------|-------|
| **Id** | `smuggler_at_gate` |
| **Name** | Smuggler at the Gate |
| **Description** | A hooded figure taps at the postern door with unusual confidence. A smuggler — offering food for materials. Someone is already making a living from your suffering. |
| **Trigger Condition** | Day 3 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Accept the trade** | 20 food for 15 materials | Food +20, Materials -15 |
| **Demand a better deal** | 30 food for 15 materials | Food +30, Materials -15, Unrest +5 |
| **Turn him away** | Refuse the trade | No changes |

**Narratives**:
- Accept: "You accept the smuggler's offer. Food for materials — a fair trade in desperate times. He slips away before the guards notice."
- Demand: "You press the smuggler for more. He complies, but the transaction is uglier now. Word of your heavy-handedness spreads through the market alleys."
- Refuse: "You turn the smuggler away. He vanishes back into the night."

---

## 8. Well Contamination Scare

| Field | Value |
|-------|-------|
| **Id** | `well_contamination_scare` |
| **Name** | Well Contamination Scare |
| **Description** | Word spreads through the city that the wells may have been poisoned by enemy agents. Whether true or not, the panic is real — and spreading fast. |
| **Trigger Condition** | Day 5 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Treat with medicine** | Use 5 medicine (if available) | Medicine -5 |
| **Boil all water** | Slow production to purify | Sickness +1, TaintedWellDays +1 |
| **Ignore the warnings** | Do nothing | Sickness +5 |

**Narratives**:
- Medicine: "Medicine purifies the wells. The worst is averted... for now."
- Boil: "You order all water boiled. It slows production, but limits the contamination."
- Ignore: "Without action, sickness spreads through the water supply."

---

## 9. A Worker Takes Their Own Life

| Field | Value |
|-------|-------|
| **Id** | `worker_takes_life` |
| **Name** | A Worker Takes Their Own Life |
| **Description** | The weight of despair has become too much. A worker is found dead by their own hand. The city falls silent. |
| **Trigger Condition** | Humanity score enabled, Humanity < 15, Morale < 30 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "A body is found in the quiet hours. No enemy arrow, no illness — just the unbearable weight of everything. The city grieves in silence."
- **Changes**: Deaths +1, Morale -5

---

## 10. Militia Volunteers

| Field | Value |
|-------|-------|
| **Id** | `militia_volunteers` |
| **Name** | Militia Volunteers |
| **Description** | A group of workers arrives at the garrison gate armed with farming tools and grim determination. They are not soldiers. But they want to fight. |
| **Trigger Condition** | Day 6, at least 3 healthy workers |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Accept the volunteers** | Convert 3 workers to guards | 3 workers become guards |
| **Decline, workers are needed** | Send them back | Morale +3 |
| **Conscript even more** | Convert 5 workers to guards | 5 workers become guards, Unrest +5 |

**Narratives**:
- Accept: "You accept their offer. {n} workers take up arms. 'We'd rather fight than starve behind walls,' they say."
- Decline: "You turn them down gently, citing the need for workers. The volunteers appreciate being valued, and return to their posts."
- Conscript: "You conscript even more than they offered. Workers are dragged from their tasks to fill the garrison. Unrest spreads among those left behind."

---

## 11. Siege Towers Spotted

| Field | Value |
|-------|-------|
| **Id** | `narrative_towers` |
| **Name** | Siege Towers Spotted |
| **Description** | Scouts report the enemy has built siege towers. The bombardment will intensify. |
| **Trigger Condition** | Day 7 |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "Scouts report the enemy has built siege towers. The bombardment will intensify."

---

## 12. Siege Bombardment

| Field | Value |
|-------|-------|
| **Id** | `siege_bombardment` |
| **Name** | Siege Bombardment |
| **Description** | The enemy catapults thunder through the night. Stones fall without warning — each impact shaking the city and the resolve of those sheltering within. |
| **Trigger Condition** | Day 8+, 20% chance |

### Consequences
- **Narrative**: "{Zone} sustains damage from bombardment. Buildings crumble under the bombardment."
- **Changes**: Random non-Keep zone loses 8-14 integrity (base + siege level × 2), Food -5 to -11 (base + siege level × 3), Deaths +1, Wounded +2

---

## 13. Hunger Riot

| Field | Value |
|-------|-------|
| **Id** | `hunger_riot` |
| **Name** | Hunger Riot |
| **Description** | Days of food shortage have broken the people's patience. A mob has stormed the granaries, killing guards and taking what little remains. |
| **Trigger Condition** | 2+ consecutive food deficit days, Unrest > 50 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Angry mobs force the granary doors. Guards are beaten back and supplies ransacked. By the time order is restored, lives are lost and stores are depleted."
- **Changes**: Food -80, Unrest +15, Guard deaths +5

---

## 14. Fever Outbreak

| Field | Value |
|-------|-------|
| **Id** | `fever_outbreak` |
| **Name** | Fever Outbreak |
| **Description** | Sickness has reached a breaking point. A sweating fever tears through the overcrowded quarters, filling the clinic past capacity and killing those already weak. |
| **Trigger Condition** | Sickness > 60 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "The fever clinic overflows. Carts carry the dead at dawn. The outbreak claims lives and rattles the nerves of those still standing."
- **Changes**: Deaths +10, Unrest +10

---

## 15. Desertion Wave

| Field | Value |
|-------|-------|
| **Id** | `desertion_wave` |
| **Name** | Desertion Wave |
| **Description** | Morale has shattered. In the dark hours before dawn, soldiers and workers slip through gaps in the walls... choosing the unknown over a siege they no longer believe can be survived. |
| **Trigger Condition** | Morale < 30 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "At dawn, the western gate stands open. Footprints lead into the fog. They chose the enemy over you."
- **Changes**: 10 workers desert

---

## 16. Wall Breach Attempt

| Field | Value |
|-------|-------|
| **Id** | `wall_breach` |
| **Name** | Wall Breach Attempt |
| **Description** | The outer wall is crumbling. Enemy soldiers have gathered at the weakest section with battering rams and axes. The breach is imminent. |
| **Trigger Condition** | Active perimeter zone integrity < 30 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Reinforce with guards** | Use guards to hold the breach | If guards ≥15: hold successfully. If guards <15: zone integrity -8 |
| **Barricade with materials** | Use 10 materials (if available) | Materials -10, zone integrity -5 |
| **Fall back** | Retreat from the wall | Zone integrity -15 |

**Narratives**:
- Reinforce (success): "Guards form a wall of steel at the breach. The enemy crashes against them and falls back. The wall holds!"
- Reinforce (fail): "Too few guards to hold the line. The enemy carves a wound in {zone}. The breach grows."
- Barricade: "Barricades made from scavenged materials slow the enemy tide, but cannot stop them. {zone} sustains damage."
- Fall back: "You order a full retreat. The enemy pours into the gap unchecked. {zone} is torn apart as the garrison falls back."

---

## 17. Fire in the Artisan Quarter

| Field | Value |
|-------|-------|
| **Id** | `fire_artisan_quarter` |
| **Name** | Fire in the Artisan Quarter |
| **Description** | Enemy incendiary projectiles have sparked a blaze in the Artisan Quarter. Workshops burn. Smoke billows over the rooftops as workers scramble to contain the flames. |
| **Trigger Condition** | Siege intensity ≥3, Artisan Quarter not lost, 15% chance |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Flames leap from workshop to workshop. The smell of burning timber fills the city. Materials are consumed and one life is lost before the blaze is controlled."
- **Changes**: Materials -40, Artisan Quarter integrity -12, Deaths +1

---

## 18. Despair

| Field | Value |
|-------|-------|
| **Id** | `despair` |
| **Name** | Wave of Despair |
| **Description** | The weight of the siege has broken something in the city. People move like ghosts. The will to endure is fraying at its edges. |
| **Trigger Condition** | Day 10+, Morale < 45, 15% chance |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "A pall of hopelessness settles over the city. Workers sit idle. Soldiers stare at the walls. The siege has worn through to the soul."
- **Changes**: Morale -10, Unrest +8

---

## 19. Refugees at the Gates

| Field | Value |
|-------|-------|
| **Id** | `refugees_at_gates` |
| **Name** | Refugees at the Gates |
| **Description** | A ragged column of survivors has arrived at your gates — families, the elderly, the wounded. They fled villages the enemy burned and now beg for sanctuary. |
| **Trigger Condition** | Day 12 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Open the gates** | Accept all 8 refugees | 5 healthy workers +3 sick, Unrest +5, Morale +3, Humanity +3 |
| **Admit only the healthy** | Reject sick refugees | 5 healthy workers, Unrest +3 |
| **Turn them away** | Reject all | Morale -10, Unrest +5, Humanity -5 |

**Narratives**:
- Open: "You open the gates. Eight refugees stream in. Five healthy, three already coughing. Humanity triumphs over caution, for better or worse."
- Healthy only: "You admit only those who appear healthy. The sick are turned away. Their pleas fade into the distance as the gates close."
- Turn away: "You turn them all away. The gates remain sealed. The refugees vanish into the night, and the city earns a cruel reputation."

---

## 20. Children's Plea

| Field | Value |
|-------|-------|
| **Id** | `childrens_plea` |
| **Name** | Children's Plea |
| **Description** | A group of orphaned children approaches the keep, begging for shelter. They are gaunt, wide-eyed, and shivering. The crowd watches to see what you will do. |
| **Trigger Condition** | Day 12+, Faith ≥3, 15% chance |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Grant Shelter** | Accept the children | Materials -10, Morale +10, Sickness +3, Faith +1 |
| **Refuse** | Turn children away | Morale -5, Unrest +5, Tyranny +1 |

**Narratives**:
- Grant: "You take the children in. Shelters are hastily built. The people smile through their tears. Sickness spreads in the crowded quarters, but hearts are warmer."
- Refuse: "You turn the children away. They wander back into the ruins. The crowd disperses in silence. Something has broken in their eyes."

---

## 21. Enemy Sappers

| Field | Value |
|-------|-------|
| **Id** | `enemy_sappers` |
| **Name** | Enemy Sappers |
| **Description** | The enemy has been tunneling beneath the walls. Their engineers work in silence, undermining the foundations stone by stone, waiting for the moment to strike. |
| **Trigger Condition** | Day 14 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Enemy miners have been at work beneath the walls. Tunnels collapse in silence, and walls groan as their foundations are undermined."
- **Changes**: All zones lose 5 integrity, Siege intensity +1

---

## 22. Enemy Commander's Letter

| Field | Value |
|-------|-------|
| **Id** | `narrative_letter` |
| **Name** | Enemy Commander's Letter |
| **Description** | A letter from the enemy commander: "Your walls weaken. Your people starve. How many more must die for your pride?" |
| **Trigger Condition** | Day 15 |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "A letter from the enemy commander: "Your walls weaken. Your people starve. How many more must die for your pride?""

---

## 23. Crisis of Faith

| Field | Value |
|-------|-------|
| **Id** | `crisis_of_faith` |
| **Name** | Crisis of Faith |
| **Description** | The faithful gather at the temple, but the prayers ring hollow. Morale is low, and doubt spreads like plague. The priests look to you for guidance — but their eyes are uncertain. |
| **Trigger Condition** | Day 15+, Faith ≥6, Morale < 30 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Hold Vigil** | All-night prayer service | Morale +20, Food -10, Sickness +5, Faith +1 |
| **Abandon Faith** | Close the temple | Morale -5, Unrest +10, Faith -3 |

**Narratives**:
- Vigil: "An all-night vigil is held. Candles burn until dawn. The faithful emerge weary but renewed. Sickness spreads in the crowded temple, but spirits are lifted."
- Abandon: "You declare the temple closed. The faithful scatter. Some weep, others rage. The foundations of belief crumble. What replaces them may be worse."

---

## 24. Siege Engineers Arrive

| Field | Value |
|-------|-------|
| **Id** | `siege_engineers_arrive` |
| **Name** | Siege Engineers Arrive |
| **Description** | A band of wandering engineers appears at the gate, fleeing the enemy's advance. They offer their expertise in exchange for refuge and food. |
| **Trigger Condition** | Day 15+, Fortification ≥5, 25% chance |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Accept** | Welcome the engineers | Workers +3, Materials +20, Food -10, Fortification +1 |
| **Decline** | Turn them away | Morale +5 |

**Narratives**:
- Accept: "The engineers are welcomed. They bring tools, knowledge, and hands hardened by siege work. The walls grow stronger. The food stores grow thinner."
- Decline: "You turn the engineers away. The people appreciate the caution — strangers in times of siege are rarely what they seem."

---

## 25. Dissidents Discovered

| Field | Value |
|-------|-------|
| **Id** | `dissidents_discovered` |
| **Name** | Dissidents Discovered |
| **Description** | A cell of dissidents has been uncovered hiding in the lower district. They have been spreading seditious pamphlets and organizing secret meetings. |
| **Trigger Condition** | Day 10+, Tyranny ≥4, 20% chance |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Execute them** | Public execution | Unrest -15, Deaths +3, Morale -5, Tyranny +1, FearLevel +1 |
| **Imprison them** | Jail the dissidents | Unrest -10, Morale +5 |
| **Release them** | Let them go | Morale +5, Unrest +8, Faith +1 |

**Narratives**:
- Execute: "The dissidents are dragged to the square and executed publicly. The message is clear: dissent means death."
- Imprison: "The dissidents are imprisoned. The people feel safer, and some appreciate the restraint."
- Release: "You release the dissidents with a warning. Some call it mercy, others call it weakness. The faithful see it as grace."

---

## 26. Tainted Well

| Field | Value |
|-------|-------|
| **Id** | `tainted_well` |
| **Name** | Tainted Well |
| **Description** | The city's primary well has been fouled — whether by enemy sabotage or simple rot. The water runs discoloured and smells of death. |
| **Trigger Condition** | Day 18 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "The well-keeper reports a foul smell from the city's main cistern. Testing confirms contamination. Reserves are drained and the water supply is compromised for days to come."
- **Changes**: Water -20, Sickness +10, Water production reduced by 40% for 3 days

---

## 27. Plague Rats

| Field | Value |
|-------|-------|
| **Id** | `plague_rats` |
| **Name** | Plague Rats |
| **Description** | Rats have infested the city in vast numbers, driven inward by the siege. They gnaw through food stores and carry disease wherever they nest. |
| **Trigger Condition** | Day 23 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Organize rat hunts** | Send workers to hunt rats | Sickness +10, Deaths +2, Unrest +5, PlagueRatsActive = false |
| **Burn the infested quarter** | Firebomb the area | Sickness +5, Materials -10, Humanity +1, PlagueRatsActive = false |
| **Do nothing** | Ignore the rats | Sickness +15, Deaths +3, Unrest +10, Humanity -3, PlagueRatsActive = true |

**Narratives**:
- Hunt: "Organized hunts drive the rats from the quarter. But disease has already spread — some fall ill before the rodents are purged."
- Burn: "You order the quarter burned. The flames purge the rats, but also consume precious materials. At least the plague is contained."
- Nothing: "You do nothing. The rats multiply and spread through the city. Disease follows in their wake, spreading faster each day."

---

## 28. Burning Farms

| Field | Value |
|-------|-------|
| **Id** | `narrative_burning_farms` |
| **Name** | Burning Farms |
| **Description** | Smoke rises beyond the walls. The enemy is burning the farms they captured. There will be nothing to reclaim. |
| **Trigger Condition** | Day 25 |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "Smoke rises beyond the walls. The enemy is burning the farms they captured. There will be nothing to reclaim."

---

## 29. Enemy Ultimatum

| Field | Value |
|-------|-------|
| **Id** | `enemy_ultimatum` |
| **Name** | Enemy Ultimatum |
| **Description** | A messenger arrives under flag of truce with a message from the enemy commander: surrender the city or face total annihilation. He gives you until dawn to decide. |
| **Trigger Condition** | Day 30 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Defy them publicly** | Reject with defiance | Morale +10, Unrest +15 |
| **Negotiate for time** | Send envoys | Morale -5, Unrest +5, Workers desert +2 |
| **Ignore the ultimatum** | Refuse to respond | Morale -15, Unrest +20, Workers desert +5 |

**Narratives**:
- Defy: "You stand on the walls and shout defiance. The people rally to your voice, but some hear only war, and grow restless for bloodshed."
- Negotiate: "You send envoys to treat with the enemy. The people see weakness in negotiation. Some desert rather than face a doomed stand."
- Ignore: "You refuse to answer. Silence is interpreted as cowardice. Hope drains from the city like blood from a wound."

---

## 30. Final Assault Begins

| Field | Value |
|-------|-------|
| **Id** | `final_assault` |
| **Name** | Final Assault Begins |
| **Description** | The enemy has marshaled their full strength for a final push. Every siege engine, every soldier, all of it aimed at your gates. |
| **Trigger Condition** | Day 33 |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Battering rams thunder against the gates. Ladders rise against the walls. The enemy no longer waits, they come for you now."
- **Changes**: Unrest +15, FinalAssaultActive = true

---

## 31. Distant Horns

| Field | Value |
|-------|-------|
| **Id** | `narrative_horns` |
| **Name** | Distant Horns |
| **Description** | Horns in the distance. Relief? Or the final assault? You cannot tell. |
| **Trigger Condition** | Day 38 |
| **Available Responses** | None (narrative beat) |

### Consequences
- **Narrative**: "Horns in the distance. Relief? Or the final assault? You cannot tell."

---

## 32. Betrayal From Within

| Field | Value |
|-------|-------|
| **Id** | `betrayal_from_within` |
| **Name** | Betrayal From Within |
| **Description** | A conspiracy among the guards is uncovered. A third of your guards have been plotting to defect. |
| **Trigger Condition** | Day 37 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Offer amnesty** | Forgive the conspirators | Guards convert to workers, Morale +5 |
| **Make an example** | Kill 2 ringleaders | Guards -defectors, workers +max(0, defectors-2), Deaths +2, Unrest +10 |
| **Let them go** | Allow desertion | Guards desert, Unrest +15 if guards <5 |

**Narratives**:
- Amnesty: "You offer amnesty. {n} guards rejoin as workers. Mercy earns grudging respect."
- Example: "You make an example of the ringleaders. {n} are executed, the rest are demoted to workers. Fear keeps order."
- Let go: "The conspirators leave freely... The garrison is weakened, but order is maintained." OR "With so few guards remaining, unrest surges."

---

## 33. Signal Fire

| Field | Value |
|-------|-------|
| **Id** | `signal_fire` |
| **Name** | Light the Signal Fires? |
| **Description** | The watchtower is intact and the keep still stands. If relief is coming, a signal fire could draw them closer — but it will also draw enemy attention. |
| **Trigger Condition** | Day 25+, Relief army enabled, Keep integrity ≥30, Signal fire not lit, enough fuel/materials |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Light the signal fires** | Use fuel and materials | Fuel -5, Materials -15, Relief acceleration +1 day, Unrest +5, SignalFireLit = true |
| **Too risky** | Don't light fires | No changes |

**Narratives**:
- Light: "The signal fires blaze atop the keep. The smoke can be seen for miles. If anyone is coming, they will see it. So will the enemy."
- Too risky: "You decide the risk is too great. The fires remain unlit."

---

## 34. Council Revolt

| Field | Value |
|-------|-------|
| **Id** | `council_revolt` |
| **Name** | Council Revolt |
| **Description** | The city council, long discontented with your rule, has armed their retainers and moved against you. The halls of power are theirs now. |
| **Trigger Condition** | Unrest > revolt threshold |
| **Available Responses** | None (game over) |

### Consequences
- **Narrative**: "Your reign ends in bloodshed. The council has taken over."
- **Changes**: Game Over - Council Revolt

---

## 35. Total Collapse

| Field | Value |
|-------|-------|
| **Id** | `total_collapse` |
| **Name** | Total Collapse |
| **Description** | The city has run out of both food and water. There is nothing left to distribute and no way to sustain life. The end is no longer a threat, it has arrived. |
| **Trigger Condition** | Both food and water at zero for multiple consecutive days |
| **Available Responses** | None (game over) |

### Consequences
- **Narrative**: "The last rations are gone. The last barrels are dry. People collapse in the streets. You led them as far as you could, but this is where the siege ends."
- **Changes**: Game Over - Total Collapse

---

## 36. Black Market Trader

| Field | Value |
|-------|-------|
| **Id** | `black_market_trading` |
| **Name** | Black Market Trader |
| **Description** | A shadowy figure appears at the gate, offering to trade supplies. The goods are real, but the prices are steep — and haggling may stir trouble. |
| **Trigger Condition** | Black market enabled, Day ≥ minimum, cooldown elapsed |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Accept** | Fair trade | Give X of resource A, receive Y of resource B |
| **Haggle** | Demand better terms | Give half, receive full, Unrest +5 |
| **Refuse** | Turn trader away | No changes |

**Narratives**:
- Accept: "The trader accepts. You exchanged {giveAmount} {give} for {receiveAmount} {receive}."
- Haggle: "After tense bargaining, you traded {haggleGive} {give} for {receiveAmount} {receive}. Word spreads of the unsavory deal."
- Refuse: "You turn the trader away. He vanishes into the shadows."

---

## 37. Spy Selling Intel

| Field | Value |
|-------|-------|
| **Id** | `spy_selling_intel` |
| **Name** | Spy Selling Intel |
| **Description** | A hooded figure claims to be a deserter from the enemy camp. He offers military intelligence — troop movements, siege plans — in exchange for supplies. |
| **Trigger Condition** | Spy intel enabled, Day ≥ minimum, random chance |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Buy Intel** | Pay for information | Materials -cost, Food -cost, Intel buff active |
| **Turn him away** | Refuse | No changes |

**Narratives**:
- Buy: "The spy's information is detailed and credible. Intel buff active for {days} days. Missions gain +{bonus}% success."
- Refuse: "You refuse the spy's offer. He disappears without a trace."

---

## 38. Intel: Siege Warning

| Field | Value |
|-------|-------|
| **Id** | `intel_siege_warning` |
| **Name** | Intel: Siege Warning |
| **Description** | Your intelligence reveals an imminent enemy assault. The enemy is massing forces for a major strike. You have time to prepare — but every choice carries risk. |
| **Trigger Condition** | Spy intel enabled, warning pending from prior spy purchase |
| **Available Responses** | OK (default) |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Intercept** | Send guards to ambush | Guards -cost, chance of deaths, siege damage reduced |
| **Brace** | Reinforce walls | Perimeter integrity +bonus |

**Narratives**:
- Intercept: "Guards ambushed the enemy vanguard. Siege damage reduced to {percent}% for {days} days."
- Brace: "Walls reinforced. +{bonus} integrity to {zone}."

---

## 39. Tyrant's Reckoning

| Field | Value |
|-------|-------|
| **Id** | `tyrants_reckoning` |
| **Name** | Tyrant's Reckoning |
| **Description** | The people gather in the square, not in revolt — but in mourning. The weight of your rule has broken something in them. They look to you with hollow eyes. This is the moment of reckoning. |
| **Trigger Condition** | Day 20+, Tyranny ≥8 |

### Responses & Consequences

| Response | Description | Changes |
|----------|-------------|---------|
| **Double Down** | Enforce martial law | Tyranny +1, Morale -30, Martial Law enacted if not active |
| **Show Mercy** | Ask forgiveness | Unrest -20, Morale +15, Faith +2, Tyranny -3 |

**Narratives**:
- Double down: "Martial Law is declared by force. Soldiers flood the streets. The people bow their heads. Hope dies quietly." OR "You double down on tyranny. The garrison tightens its grip. The people have nothing left to give but silence."
- Mercy: "You kneel before the people. For the first time, you ask for forgiveness. Something shifts. The iron grip loosens. Perhaps there is still a path to redemption."

---

## 40. Steady Supplies (Streak Event)

| Field | Value |
|-------|-------|
| **Id** | `streak_no_deficit` |
| **Name** | Steady Supplies |
| **Description** | For days now, the granary and cisterns have held. The people notice — and take heart. |
| **Trigger Condition** | Good day morale enabled, consecutive no-deficit days threshold met |
| ** None (automatic)Available Responses** | |

### Consequences
- **Narrative**: "The steady flow of food and water lifts spirits across the city. Perhaps things are looking up."
- **Changes**: Morale +streak bonus

---

## 41. Health Improving (Streak Event)

| Field | Value |
|-------|-------|
| **Id** | `streak_low_sickness` |
| **Name** | Health Improving |
| **Description** | The clinics report fewer patients each day. The plague's grip is weakening. |
| **Trigger Condition** | Good day morale enabled, consecutive low sickness days threshold met |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Days of low sickness have restored confidence. The healers breathe easier, and the people with them."
- **Changes**: Morale +bonus, Unrest -reduction

---

## 42. Walls Still Stand (Streak Event)

| Field | Value |
|-------|-------|
| **Id** | `streak_zone_held` |
| **Name** | Walls Still Stand |
| **Description** | Against the siege's fury, the perimeter holds. The defenders take pride in their resilience. |
| **Trigger Condition** | Good day morale enabled, consecutive zone held days threshold met |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "The walls hold firm. The garrison's determination inspires the whole city — we can endure this."
- **Changes**: Morale +bonus

---

## 43. Fortune Favors the Bold (Streak Event)

| Field | Value |
|-------|-------|
| **Id** | `streak_mission_success` |
| **Name** | Fortune Favors the Bold |
| **Description** | Back-to-back successes in the field have emboldened the people. Volunteers step forward. |
| **Trigger Condition** | Good day morale enabled, consecutive mission success threshold met |
| **Available Responses** | None (automatic) |

### Consequences
- **Narrative**: "Inspired by recent victories, {n} volunteers join the workforce."
- **Changes**: Workers +bonus

---

# Event Types Summary

| Type | Count | Description |
|------|-------|-------------|
| **IRespondableEvent** | 20 | Events with player choices |
| **ITriggeredEvent** | 23 | Automatic events including narrative beats and streak events |
| **Narrative Beats** | 6 | Story-only events with no mechanical effect |
| **Streak Events** | 4 | Positive momentum bonuses |
| **Game Over Events** | 2 | Council Revolt, Total Collapse |

---

*Last updated: Generated from event source code*
