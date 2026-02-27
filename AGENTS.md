# AGENTS.md

## Project Overview

The player must manage a city under siege while resources, morale, and zone integrity decline under siege pressure. 

The enemy cannot be defeated; the city cannot be stabilized. The core experience is **managing decline under dual pressure**:

* **External siege pressure** causes **zone contraction** (outer districts fall/evacuate, perimeter shrinks).
* **Internal collapse** (morale/unrest/sickness) escalates, worsened by **overcrowding** when zones contract.

## EMOTIONAL TARGET

The player must feel:

* Constant pressure
* Moral compromise
* Shrinking space
* No clean solution
* Survival equals sacrifice

Success must feel like endurance, not triumph.

## Code Style Guidelines

- Write simple code. Avoid heavy abstractions.
- Favor readability and maintainability over cleverness.
- Prefer composition to inheritance.
- Always ask: "Is there a more elegant way to solve this problem?"
- Create folders by feature (Laws, Missions, Simulation, Events, etc.). Avoid folder names like "Models".

### Minimizing Serialized Fields

For inner-prefab and inner-scene dependencies, avoid serialized fields.
Instead, name the target GameObject with a name starting with `#` and find it at runtime with `FindRecursive<T>`.
```csharp
        Button _start;
        
        void Awake()
        {
            _start = this.FindRecursive<Button>("#Start");
        }
```