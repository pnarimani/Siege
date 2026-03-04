# AGENTS.md

## Code Style Guidelines

- **ALWAYS**, **ALWAYS** find the root cause of bugs. Don't apply Band-Aids. Don't fix symptoms. Don't apply "hacks". Don't add "workarounds". Don't add "temporary fixes". 
- Write maintainable and extendable code. Don't write code that is only good enough for the current feature. Don't write code that will be difficult to extend or modify in the future.
- Prefer composition to inheritance.
- Write high performance code. Don't allocate memory in hot paths. Don't do unnecessary work.
- **ALWAYS** ask: "Is there a more elegant way to solve this problem?"
- Create folders by feature (Laws, Missions, Simulation, Events, etc.). Avoid folder names like "Models".
- Use UI Toolkit best practices. Read `ui-toolkit` skill whenever working with UI.
- This game is localized. Read `unity-localization` skill whenever working with text.


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