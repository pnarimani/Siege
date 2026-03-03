# Buildings

Buildings produce resources to sustain the city. Each building has a worker capacity and consumes/produces resources per worker per day.

Buildings can either produce resources or store resources. not both.

## Activation

Buildings can be **activated** or **deactivated** using the `toggle` command. When deactivated:
- Workers are automatically reallocated (freed up for other buildings)
- Production stops
- Input consumption stops

## Building List

### Outer Farms (Lost First)

| Building | Workers | Inputs | Outputs |
|----------|---------|--------|---------|
| Farm | 10 | 1 fuel | 3 food |
| Herb Garden | 6 | — | 1 medicine |

### Outer Residential (Lost Second)

| Building | Workers | Inputs | Outputs |
|----------|---------|--------|---------|
| Well | 10 | 1 fuel | 3 water |
| Fuel Store | 8 | — | 2 fuel |
| Field Kitchen | 6 | 1 fuel | 2 food |

### Artisan Quarter (Lost Third)

| Building | Workers | Inputs | Outputs |
|----------|---------|--------|---------|
| Workshop | 8 | — | 2 materials |
| Smithy | 6 | 2 materials | 1 integrity |
| Cistern | 6 | — | 1 water |

### Inner District (Lost Fourth)

| Building | Workers | Inputs | Outputs |
|----------|---------|--------|---------|
| Clinic | 8 | 1 medicine | 1 care |
| Storehouse | 6 | — | 1 fuel |
| Root Cellar | 4 | — | 1 food |
| Trading Post | 4 | — | — |

### Keep (Never Lost)

| Building | Workers | Inputs | Outputs |
|----------|---------|--------|---------|
| Repair Yard | 8 | 3 materials | 1 integrity |
| Rationing Post | 4 | — | 1 water |

## Specializations

Each building can be specialized to modify its behavior. Use `specialize <building>` to view and select options.

### Farm
- **Grain Silos**: Output 4 food/worker (up from 3)
- **Medicinal Herbs**: Output 2 food + 0.5 medicine/worker

### Herb Garden
- **Apothecary Lab**: Output 1.5 medicine/worker, requires 1 fuel input
- **Healer's Refuge**: No output bonus, -3 sickness/day passive

### Well
- **Deep Boring**: Output 4 water/worker, fuel input 2 (up from 1)
- **Purification Basin**: No output bonus, -2 sickness/day passive

### Fuel Store
- **Coal Pits**: Output 3 fuel/worker, +1 sickness/day
- **Rationed Distribution**: Output 1.5 fuel/worker, -15% fuel consumption globally

### Field Kitchen
- **Soup Line**: Output 3 food/worker, -3 morale/day
- **Fortified Kitchen**: Survives zone loss (rebuilt in next inner zone), +5 morale on spec

### Workshop
- **Arms Foundry**: Output 3 materials/worker, requires 1 fuel input
- **Salvage Yard**: No output bonus, 10% daily chance of +5 random resource

### Smithy
- **War Smith**: Output 2 integrity/worker, input 3 materials (up from 2)
- **Armor Works**: No integrity change, +2 guards on spec, Fortification +1

### Cistern
- **Rain Collection**: Output 1.5 water/worker, doubles on Heavy Rains
- **Emergency Reserve**: +20 water on spec, auto-releases 10 water if water hits 0 (once)

### Clinic
- **Hospital**: +50% recovery slots
- **Quarantine Ward**: -5 sickness/day

### Storehouse
- **Weapon Cache**: No fuel output, instead -5 unrest/day, Tyranny +1
- **Emergency Supplies**: No change, 50% resource salvage on zone loss

### Root Cellar
- **Preserved Stores**: Output 1.5 food/worker, -10% food consumption globally
- **Mushroom Farm**: Output 2 food/worker, +1 sickness/day

### Repair Yard
- **Siege Workshop**: Output 2 integrity/worker, input 4 materials (up from 3)
- **Engineer Corps**: No change, fortification upgrades cost 50% less materials

### Rationing Post
- **Distribution Hub**: Output 1.5 water/worker, -5% food consumption globally
- **Propaganda Post**: No water output, +3 morale/day, -2 unrest/day, Faith +1
