---
name: ui-toolkit
description: Comprehensive guide for AI agents to work with Unity UI Toolkit - covering UXML, USS, visual tree, controls, events, data binding, ListView/TreeView, custom controls, transitions, and localization
---

# Unity UI Toolkit

UI Toolkit is Unity's modern UI framework for building both editor and runtime user interfaces. It uses a retained-mode system based on a visual tree of elements, styled with USS (Unity Style Sheets) and structured with UXML (a markup language inspired by HTML/XAML).

## Core Capabilities

1. **UXML Markup** - Declarative UI structure using XML-based markup
2. **USS Styling** - CSS-inspired styling with flexbox layout
3. **Visual Tree** - Hierarchical element tree with query support (UQuery)
4. **Event System** - Event dispatching with trickle-down and bubble-up propagation
5. **Data Binding** - Bind UI to SerializedObject properties or runtime data
6. **Built-in Controls** - 70+ controls: Button, Label, TextField, ListView, TreeView, etc.
7. **Custom Controls** - Create reusable elements with `[UxmlElement]` attribute
8. **Transitions** - CSS-like animated property transitions
9. **Localization** - Integration with Unity Localization package via data bindings

---

## Quick Start

### Runtime UI Setup

1. Create a UXML file (`Assets/UI/MyScreen.uxml`)
2. Create a USS file (`Assets/UI/MyScreen.uss`)
3. Create a `PanelSettings` asset (right-click in Project > Create > UI Toolkit > Panel Settings Asset)
4. Add a `UIDocument` component to a GameObject in your scene
5. Assign the PanelSettings and UXML to the UIDocument

### Minimal UXML

```xml
<UXML xmlns="UnityEngine.UIElements">
    <Style src="MyScreen.uss" />
    <VisualElement name="root" class="container">
        <Label text="Hello UI Toolkit" />
        <Button name="my-button" text="Click Me" />
    </VisualElement>
</UXML>
```

### Minimal USS

```css
.container {
    flex-grow: 1;
    align-items: center;
    justify-content: center;
}

Label {
    font-size: 24px;
    color: white;
}

Button {
    width: 200px;
    height: 50px;
    font-size: 18px;
}
```

### Minimal C# Controller

```csharp
using UnityEngine;
using UnityEngine.UIElements;

public class MyScreenController : MonoBehaviour
{
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>("my-button");
        button.clicked += OnButtonClicked;
    }

    void OnDisable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var button = root.Q<Button>("my-button");
        button.clicked -= OnButtonClicked;
    }

    void OnButtonClicked()
    {
        Debug.Log("Button clicked!");
    }
}
```

> **IMPORTANT**: Place UI initialization in `OnEnable()` and cleanup in `OnDisable()`. When the UI reloads, UIDocument disables and re-enables associated MonoBehaviours. Do NOT use `Awake()` or `Start()` for UI setup.

---

## UXML (Unity XML Markup)

### Document Structure

```xml
<?xml version="1.0" encoding="utf-8"?>
<UXML xmlns="UnityEngine.UIElements"
      xmlns:editor="UnityEditor.UIElements">
    <Style src="project://database/Assets/UI/Styles.uss" />
    <!-- UI elements go here -->
</UXML>
```

### Namespaces

| Prefix | Namespace | Use |
|--------|-----------|-----|
| (default) | `UnityEngine.UIElements` | Runtime elements |
| `editor:` | `UnityEditor.UIElements` | Editor-only elements |

Set a default namespace with `xmlns="UnityEngine.UIElements"` to avoid prefixing every element.

### Common Attributes (inherited from VisualElement)

| Attribute | Type | Description |
|-----------|------|-------------|
| `name` | string | Unique identifier for queries and USS `#name` selectors |
| `class` | string | Space-separated USS class names |
| `style` | string | Inline styles (avoid in production; use USS) |
| `picking-mode` | Position/Ignore | Whether element receives pointer events |
| `focusable` | bool | Whether element can receive focus |
| `tabindex` | int | Tab order (>= 0 to participate) |
| `tooltip` | string | Hover text (Editor UI only) |
| `enabled` | bool | Local enabled state |
| `view-data-key` | string | Key for persisting view state |
| `data-source` | Object | Data binding source |
| `data-source-path` | string | Path within data source |
| `language-direction` | LTR/RTL | Text directionality |
| `usage-hints` | UsageHints | Rendering optimization hints |

### Nesting and Hierarchy

```xml
<VisualElement class="panel">
    <Label text="Title" />
    <VisualElement class="row">
        <Button text="OK" />
        <Button text="Cancel" />
    </VisualElement>
</VisualElement>
```

### Including Templates

Reference other UXML files:

```xml
<Template name="ListItem" src="project://database/Assets/UI/ListItem.uxml" />
<Instance template="ListItem" />
```

### Style References

```xml
<!-- Relative path -->
<Style src="./MyStyles.uss" />

<!-- Project path -->
<Style src="project://database/Assets/UI/MyStyles.uss" />
```

### Data Binding in UXML

```xml
<TextField binding-path="m_PlayerName" label="Name" />
<FloatField binding-path="m_Health" label="Health" />
```

### Inline Styles (use sparingly)

```xml
<VisualElement style="flex-direction: row; padding: 10px;">
    <Label text="Inline styled" style="color: red;" />
</VisualElement>
```

---

## USS (Unity Style Sheets)

USS is modeled after CSS but has key differences. It uses the **border-box** model by default.

### Selector Types

| Selector | Syntax | CSS Equivalent | Example |
|----------|--------|----------------|---------|
| Type | `ElementType {}` | Element selector | `Button {}` |
| Name | `#element-name {}` | ID selector | `#my-button {}` |
| Class | `.class-name {}` | Class selector | `.header {}` |
| Universal | `* {}` | Universal selector | `* {}` |
| Descendant | `A B {}` | Descendant combinator | `.panel Label {}` |
| Child | `A > B {}` | Child combinator | `.panel > Button {}` |
| Multiple | `A.class {}` | Compound selector | `Button.primary {}` |
| Selector list | `A, B {}` | Grouping | `Button, Label {}` |

### Pseudo-Classes

| Pseudo-class | Description |
|-------------|-------------|
| `:hover` | Cursor over element |
| `:active` | User interacting with element |
| `:inactive` | User stopped interacting |
| `:focus` | Element has focus |
| `:checked` | Toggle/RadioButton is selected |
| `:disabled` | Element is disabled |
| `:enabled` | Element is enabled |
| `:root` | Highest-level element with this stylesheet |

**Chain pseudo-classes** for compound states:

```css
Toggle:checked:hover {
    background-color: yellow;
}
```

> **Note**: USS does NOT support `:selected`. Use `:checked` instead.

### USS Custom Properties (Variables)

```css
:root {
    --primary-color: #3498db;
    --spacing: 10px;
    --font-large: 24px;
}

.header {
    color: var(--primary-color);
    margin: var(--spacing);
    font-size: var(--font-large);
}

/* With fallback value */
.text {
    color: var(--accent-color, #333);
}
```

**Limitations vs CSS**:
- `var()` cannot nest inside other functions (`rgb(var(--r), 0, 0)` is NOT supported)
- Mathematical operations on variables are NOT supported
- No `calc()` function

### Layout System (Flexbox via Yoga)

USS uses a Yoga-based Flexbox layout engine.

**Default behavior**: Elements stack vertically (`flex-direction: column`).

| Property | Values | Default | Description |
|----------|--------|---------|-------------|
| `flex-direction` | row, row-reverse, column, column-reverse | column | Main axis direction |
| `flex-grow` | number | 0 | How much element grows |
| `flex-shrink` | number | 1 | How much element shrinks |
| `flex-basis` | length/auto | auto | Initial size along main axis |
| `flex-wrap` | nowrap, wrap, wrap-reverse | nowrap | Whether items wrap |
| `justify-content` | flex-start, flex-end, center, space-between, space-around | flex-start | Main axis alignment |
| `align-items` | flex-start, flex-end, center, stretch, auto | stretch | Cross axis alignment |
| `align-self` | auto, flex-start, flex-end, center, stretch | auto | Override parent align-items |
| `align-content` | flex-start, flex-end, center, stretch | flex-start | Multi-line cross axis |

**Positioning**:

| Property | Values | Description |
|----------|--------|-------------|
| `position` | relative, absolute | Relative = normal flow; Absolute = removed from flow |
| `left`, `top`, `right`, `bottom` | length | Offset from parent edges |

> **Absolute positioning** makes an element invisible to the flexbox layout engine. Use for overlays and popups.

### Box Model Properties

```css
.element {
    /* Sizing */
    width: 200px;
    height: 100px;
    min-width: 50px;
    max-width: 500px;
    min-height: 30px;
    max-height: 300px;
    aspect-ratio: 16 / 9;

    /* Margins (outside border) */
    margin: 10px;                        /* all sides */
    margin: 10px 20px;                   /* vertical horizontal */
    margin: 10px 20px 10px 20px;         /* top right bottom left */
    margin-top: 10px;
    margin-right: 20px;
    margin-bottom: 10px;
    margin-left: 20px;

    /* Padding (inside border) */
    padding: 10px;
    padding-top: 5px;
    padding-right: 15px;
    padding-bottom: 5px;
    padding-left: 15px;

    /* Border */
    border-width: 2px;
    border-color: black;
    border-radius: 5px;
    border-top-left-radius: 10px;
    border-top-right-radius: 10px;
    border-bottom-left-radius: 5px;
    border-bottom-right-radius: 5px;
}
```

### Visual Properties

```css
.element {
    /* Background */
    background-color: #2c3e50;
    background-image: url("project://database/Assets/UI/icon.png");
    -unity-background-scale-mode: scale-to-fit;  /* scale-to-fit, scale-and-crop, stretch-to-fill */
    -unity-background-image-tint-color: white;

    /* Appearance */
    opacity: 0.8;
    visibility: visible;          /* visible, hidden */
    display: flex;                /* flex, none */
    overflow: hidden;             /* visible, hidden */

    /* 9-slice for sprites */
    -unity-slice-left: 10;
    -unity-slice-top: 10;
    -unity-slice-right: 10;
    -unity-slice-bottom: 10;
    -unity-slice-scale: 1;
}
```

### Text Properties

```css
.text {
    color: white;
    font-size: 16px;
    -unity-font-definition: url("project://database/Assets/Fonts/MyFont.asset");
    -unity-font-style: bold;            /* normal, bold, italic, bold-and-italic */
    -unity-text-align: middle-center;   /* upper-left, middle-center, lower-right, etc. */
    white-space: normal;                /* normal, nowrap */
    text-overflow: ellipsis;            /* clip, ellipsis */
    letter-spacing: 2px;
    word-spacing: 5px;
    -unity-paragraph-spacing: 10px;

    /* Text effects */
    text-shadow: 2px 2px 4px black;
    -unity-text-outline-width: 1px;
    -unity-text-outline-color: black;
}
```

### Transform Properties

```css
.element {
    rotate: 45deg;
    scale: 1.2 1.2;
    translate: 10px 20px;
    transform-origin: center center;
}
```

---

## Transitions

USS transitions animate property changes smoothly, similar to CSS transitions.

### Syntax

```css
.button {
    background-color: #333;
    transition-property: background-color, scale;
    transition-duration: 0.3s, 0.2s;
    transition-timing-function: ease-out, ease-in-out;
    transition-delay: 0s, 0.1s;
}

/* Shorthand */
.button {
    transition: background-color 0.3s ease-out, scale 0.2s ease-in-out 0.1s;
}

.button:hover {
    background-color: #666;
    scale: 1.05 1.05;
}
```

> **IMPORTANT**: Define `transition-*` properties on the base state, NOT on the pseudo-class.

### Timing Functions

`linear`, `ease`, `ease-in`, `ease-out`, `ease-in-out`, `ease-in-sine`, `ease-out-sine`, `ease-in-out-sine`, `ease-in-cubic`, `ease-out-cubic`, `ease-in-out-cubic`, `ease-in-back`, `ease-out-back`, `ease-in-out-back`, `ease-in-bounce`, `ease-out-bounce`, `ease-in-out-bounce`, `ease-in-elastic`, `ease-out-elastic`, `ease-in-out-elastic`

### Transition Constraints

- Transitions do NOT work on the first frame. Initiate after the scene has loaded.
- Start and end values must use matching units (e.g., `0px` to `100px`, not `auto` to `100px`).
- Transform properties (scale, rotate, translate) and color properties have optimized animation paths.

---

## Event System

UI Toolkit events propagate through the visual tree in phases: **TrickleDown** (from root to target) and **BubbleUp** (from target to root).

### Registering Callbacks

```csharp
// Default: BubbleUp phase
element.RegisterCallback<ClickEvent>(OnClick);

// TrickleDown phase
element.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);

// Unregister
element.UnregisterCallback<ClickEvent>(OnClick);
```

### Common Event Types

| Event | Description |
|-------|-------------|
| `ClickEvent` | Element clicked |
| `PointerDownEvent` | Pointer pressed |
| `PointerUpEvent` | Pointer released |
| `PointerMoveEvent` | Pointer moved |
| `PointerEnterEvent` | Pointer entered element |
| `PointerLeaveEvent` | Pointer left element |
| `FocusInEvent` | Element gained focus |
| `FocusOutEvent` | Element lost focus |
| `KeyDownEvent` | Key pressed |
| `KeyUpEvent` | Key released |
| `ChangeEvent<T>` | Value changed on a control |
| `AttachToPanelEvent` | Element added to visual tree |
| `DetachFromPanelEvent` | Element removed from visual tree |
| `GeometryChangedEvent` | Element layout changed |
| `NavigationMoveEvent` | Navigation input (gamepad/keyboard) |

### Value Change Events

```csharp
var toggle = root.Q<Toggle>("my-toggle");
toggle.RegisterValueChangedCallback(evt =>
{
    Debug.Log($"Toggle changed: {evt.previousValue} -> {evt.newValue}");
});
```

### Button Click Shorthand

```csharp
// Preferred shorthand for buttons
button.clicked += () => Debug.Log("Clicked");

// Or with full event
button.RegisterCallback<ClickEvent>(evt => Debug.Log("Clicked"));
```

### Stopping Propagation

```csharp
element.RegisterCallback<PointerDownEvent>(evt =>
{
    evt.StopPropagation();  // Prevents further propagation
});
```

---

## Manipulators

Manipulators are state machines that encapsulate event handling logic. They register/unregister callbacks automatically when attached to elements.

### Built-in Manipulators

| Manipulator | Purpose |
|------------|---------|
| `Clickable` | Detects press-and-release click patterns |
| `PointerManipulator` | Base for pointer-based interactions |
| `MouseManipulator` | Base for mouse-specific interactions |
| `KeyboardNavigationManipulator` | Keyboard navigation |
| `ContextualMenuManipulator` | Right-click context menus |

### Custom Manipulator Example

```csharp
public class DragManipulator : PointerManipulator
{
    Vector3 startPosition;
    bool active;

    public DragManipulator(VisualElement target)
    {
        this.target = target;
        activators.Add(new ManipulatorActivationFilter { button = MouseButton.LeftMouse });
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnPointerDown);
        target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
        target.RegisterCallback<PointerUpEvent>(OnPointerUp);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
        target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
        target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
    }

    void OnPointerDown(PointerDownEvent evt)
    {
        if (!CanStartManipulation(evt)) return;
        startPosition = evt.localPosition;
        active = true;
        target.CapturePointer(evt.pointerId);
        evt.StopPropagation();
    }

    void OnPointerMove(PointerMoveEvent evt)
    {
        if (!active || !target.HasPointerCapture(evt.pointerId)) return;
        var diff = evt.localPosition - startPosition;
        target.style.left = target.layout.x + diff.x;
        target.style.top = target.layout.y + diff.y;
        evt.StopPropagation();
    }

    void OnPointerUp(PointerUpEvent evt)
    {
        if (!active || !target.HasPointerCapture(evt.pointerId)) return;
        active = false;
        target.ReleasePointer(evt.pointerId);
        evt.StopPropagation();
    }
}

// Usage
element.AddManipulator(new DragManipulator(element));
```

---

## UQuery (Querying the Visual Tree)

UQuery finds elements in the visual tree efficiently. It is inspired by jQuery and designed to minimize dynamic memory allocation.

### Query Methods

```csharp
var root = GetComponent<UIDocument>().rootVisualElement;

// Q<T>() - returns first match (shorthand for Query<T>.First())
Button button = root.Q<Button>("my-button");
Label label = root.Q<Label>(className: "title");

// Query<T>() - returns queryable collection
var allButtons = root.Query<Button>().ToList();
var yellowLabels = root.Query<Label>(className: "yellow").ToList();

// By name only
VisualElement el = root.Q("container");

// Combined filters
var specific = root.Q<Button>(name: "ok", className: "primary");

// With predicate
var filtered = root.Query<Label>(className: "item")
    .Where(e => e.text.Contains("Score"))
    .ToList();

// ForEach (avoids list allocation)
root.Query<Button>().ForEach(b => b.SetEnabled(false));
```

> **Performance tip**: Cache query results during initialization. Do NOT re-query every frame.

---

## Data Binding

### SerializedObject Binding (Editor/Inspector)

```csharp
// In UXML
// <FloatField binding-path="m_Health" label="Health" />

// In C# - Bind entire hierarchy
var root = GetComponent<UIDocument>().rootVisualElement;
var serializedObj = new SerializedObject(target);
root.Bind(serializedObj);

// Or bind a specific property
var healthField = root.Q<FloatField>("health");
var prop = serializedObj.FindProperty("m_Health");
healthField.BindProperty(prop);
```

### Nested Binding Paths

Use `BindableElement`, `TemplateContainer`, or `GroupBox` to combine parent and child paths:

```xml
<BindableElement binding-path="m_Stats">
    <FloatField binding-path="health" label="Health" />
    <FloatField binding-path="stamina" label="Stamina" />
</BindableElement>
```

This binds to `m_Stats.health` and `m_Stats.stamina`.

### Tracking Value Changes

```csharp
var serializedObj = new SerializedObject(target);
var prop = serializedObj.FindProperty("m_Health");

// Track a specific property
root.TrackPropertyValue(prop, p =>
{
    Debug.Log($"Health changed to: {p.floatValue}");
});

// Track the entire object
root.TrackSerializedObjectValue(serializedObj, obj =>
{
    Debug.Log("Object was modified");
});
```

---

## ListView and TreeView

### ListView

```xml
<ListView name="my-list" fixed-item-height="30" />
```

```csharp
var listView = root.Q<ListView>("my-list");
var items = new List<string> { "Apple", "Banana", "Cherry" };

listView.itemsSource = items;
listView.makeItem = () => new Label();
listView.bindItem = (element, index) =>
{
    (element as Label).text = items[index];
};
listView.selectionType = SelectionType.Single;
listView.selectionChanged += objects =>
{
    foreach (var obj in objects)
        Debug.Log($"Selected: {obj}");
};
```

### ListView with Custom Template

```xml
<!-- ListItem.uxml -->
<UXML xmlns="UnityEngine.UIElements">
    <VisualElement class="list-item">
        <Image name="icon" />
        <Label name="item-name" />
        <Label name="item-value" />
    </VisualElement>
</UXML>
```

```csharp
var itemTemplate = Resources.Load<VisualTreeAsset>("ListItem");

listView.makeItem = () => itemTemplate.Instantiate();
listView.bindItem = (element, index) =>
{
    var data = items[index];
    element.Q<Label>("item-name").text = data.Name;
    element.Q<Label>("item-value").text = data.Value.ToString();
    element.Q<Image>("icon").image = data.Icon;
};
```

### TreeView

```csharp
var treeView = root.Q<TreeView>("my-tree");

var roots = new List<TreeViewItemData<string>>
{
    new TreeViewItemData<string>(0, "Root", new List<TreeViewItemData<string>>
    {
        new TreeViewItemData<string>(1, "Child A"),
        new TreeViewItemData<string>(2, "Child B", new List<TreeViewItemData<string>>
        {
            new TreeViewItemData<string>(3, "Grandchild")
        })
    })
};

treeView.SetRootItems(roots);
treeView.makeItem = () => new Label();
treeView.bindItem = (element, index) =>
{
    var item = treeView.GetItemDataForIndex<string>(index);
    (element as Label).text = item;
};
treeView.Rebuild();
```

### MultiColumnListView

```xml
<MultiColumnListView name="multi-list" fixed-item-height="30">
    <Columns>
        <Column name="name" title="Name" width="200" />
        <Column name="score" title="Score" width="100" />
    </Columns>
</MultiColumnListView>
```

```csharp
var listView = root.Q<MultiColumnListView>("multi-list");
listView.itemsSource = data;

listView.columns["name"].makeCell = () => new Label();
listView.columns["name"].bindCell = (element, index) =>
    (element as Label).text = data[index].Name;

listView.columns["score"].makeCell = () => new Label();
listView.columns["score"].bindCell = (element, index) =>
    (element as Label).text = data[index].Score.ToString();
```

> **Performance**: Set `fixed-item-height` for virtualized lists. Only visible items are rendered.

---

## Custom Controls

### Basic Custom Element

```csharp
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class HealthBar : VisualElement
{
    [UxmlAttribute]
    public float MaxHealth { get; set; } = 100f;

    [UxmlAttribute]
    public float CurrentHealth { get; set; } = 100f;

    VisualElement fillBar;

    public HealthBar()
    {
        // Add USS class for styling
        AddToClassList("health-bar");

        // Build internal structure
        fillBar = new VisualElement();
        fillBar.AddToClassList("health-bar__fill");
        Add(fillBar);

        // Deferred init if panel context needed
        RegisterCallback<AttachToPanelEvent>(OnAttach);
        RegisterCallback<DetachFromPanelEvent>(OnDetach);
    }

    void OnAttach(AttachToPanelEvent evt)
    {
        UpdateFill();
    }

    void OnDetach(DetachFromPanelEvent evt) { }

    void UpdateFill()
    {
        float ratio = Mathf.Clamp01(CurrentHealth / MaxHealth);
        fillBar.style.width = new Length(ratio * 100, LengthUnit.Percent);
    }
}
```

```css
/* health-bar.uss */
.health-bar {
    width: 200px;
    height: 20px;
    background-color: #333;
    border-radius: 4px;
    overflow: hidden;
}

.health-bar__fill {
    height: 100%;
    background-color: #2ecc71;
    transition: width 0.3s ease-out;
}
```

```xml
<!-- Usage in UXML -->
<HealthBar max-health="100" current-health="75" />
```

### Custom Control Best Practices

- Mark classes `[UxmlElement]` and `partial`
- Use `[UxmlAttribute]` for properties exposed in UXML/UI Builder
- Expose functional aspects as UXML attributes; visual aspects as USS properties
- Use BEM naming for USS classes (`block__element--modifier`)
- Use static callbacks to minimize memory allocations
- Use unique, concise namespaces to prevent naming conflicts
- Initialize in constructor for immediate setup, or use `AttachToPanelEvent` for deferred init

---

## Localization (Unity Localization Package)

UI Toolkit integrates with Unity's Localization package via data bindings (Unity 6+).

### UXML Binding

```xml
<Label text="default">
    <Bindings>
        <LocalizedString property="text" table="UI" entry="welcome_message" />
    </Bindings>
</Label>
```

Reference tables by name or GUID:
```xml
<LocalizedString property="text" table="GUID:27153e20147c06c4c8d1304d28104a87" entry="welcome_message" />
```

Reference entries by name or ID:
```xml
<LocalizedString property="text" table="UI" entry="Id(1234)" />
```

### Supported Binding Types

| Binding Type | Asset Type |
|-------------|------------|
| `LocalizedString` | string (text) |
| `LocalizedSprite` | Sprite |
| `LocalizedTexture` | Texture |
| `LocalizedAudioClip` | Audio Clip |
| `LocalizedFont` | Font |
| `LocalizedGameObject` | GameObject/Prefab |
| `LocalizedMaterial` | Material |
| `LocalizedObject` | Object (generic) |
| `LocalizedTmpFont` | TextMeshPro Font |
| `LocalizedMesh` | Mesh |

### C# Binding

```csharp
var label = new Label();
var localizedString = new LocalizedString("UI", "welcome_message");
label.SetBinding("text", localizedString);
```

### Smart Strings with Variables

```csharp
// Retrieve binding and update variables
var binding = label.GetBinding("text") as LocalizedString;
var score = binding["score"] as IntVariable;
score.Value = 42;  // Triggers UI update automatically
```

### UI Builder Approach

1. Right-click any element field in the Inspector
2. Select "Add binding"
3. Choose binding type (e.g., LocalizedString)
4. Specify table and entry
5. Optionally add local variables for string formatting

### Custom Element Localization

Apply localization bindings to custom `[UxmlElement]` attributes decorated with `[CreateProperty]`.

> Bindings update automatically when: the UIDocument is created, the locale changes, or persistent variables are modified.

---

## Best Practices

### UXML Best Practices

1. **One UXML per screen/panel** - Keep files focused and modular
2. **Use templates** for reusable components (`<Template>` + `<Instance>`)
3. **Avoid inline styles** - Use USS files for all styling
4. **Use meaningful names** - `name="player-health-bar"` not `name="bar1"`
5. **Use classes for styling** - Reserve `name` for C# queries
6. **Keep hierarchy shallow** - Deep nesting hurts performance and readability

### USS Best Practices

1. **Use BEM naming**: `.panel__header--active` (Block, Element, Modifier)
2. **Prefer class selectors** over name or type selectors for performance
3. **Use USS variables** for consistent theming (`--primary-color`, `--spacing-md`)
4. **Define variables in `:root`** for global scope
5. **One USS file per component/screen** for maintainability
6. **Avoid `*` selector** in production (performance impact)
7. **Use shorthand properties** where possible (`margin: 10px` vs four separate properties)
8. **Set transitions on base state**, not on pseudo-class

### C# Best Practices

1. **Initialize in OnEnable, cleanup in OnDisable** for MonoBehaviour controllers
2. **Cache element references** from queries
3. **Use Q<T>() for single elements, Query<T>() for collections**
4. **Unregister callbacks** to prevent memory leaks
5. **Use manipulators** for complex interaction patterns
6. **Avoid repeated Bind() calls** on the same element (performance)

### Performance Best Practices

1. Use `fixed-item-height` on ListView/TreeView for virtualization
2. Set `usage-hints="DynamicTransform"` on frequently-moving elements
3. Cache query results, do not query every frame
4. Use `ForEach` on queries instead of `ToList()` when possible
5. Minimize absolute positioning (breaks layout optimization)
6. Use USS transitions over C# animation for simple property changes

---

## Built-in Controls Quick Reference

### Display

| Element | UXML | Description |
|---------|------|-------------|
| `Label` | `<Label text="Hello" />` | Text display |
| `Image` | `<Image />` | Image display |
| `ProgressBar` | `<ProgressBar value="0.5" title="Loading" />` | Progress indicator |
| `HelpBox` | `<HelpBox text="Info" message-type="Info" />` | Info/warning/error box |

### Input

| Element | UXML | Description |
|---------|------|-------------|
| `Button` | `<Button text="Click" />` | Clickable button |
| `RepeatButton` | `<RepeatButton text="Hold" />` | Repeating click button |
| `Toggle` | `<Toggle label="Enable" />` | Checkbox |
| `RadioButton` | `<RadioButton label="Option" />` | Radio option |
| `RadioButtonGroup` | `<RadioButtonGroup label="Pick one" />` | Radio group |
| `TextField` | `<TextField label="Name" />` | Text input |
| `IntegerField` | `<IntegerField label="Count" />` | Integer input |
| `FloatField` | `<FloatField label="Speed" />` | Float input |
| `Slider` | `<Slider low-value="0" high-value="100" />` | Range slider |
| `SliderInt` | `<SliderInt low-value="0" high-value="10" />` | Integer slider |
| `MinMaxSlider` | `<MinMaxSlider low-limit="0" high-limit="100" />` | Range selector |
| `DropdownField` | `<DropdownField label="Choice" />` | Dropdown select |
| `EnumField` | `<EnumField label="Direction" />` | Enum dropdown |
| `EnumFlagsField` | `<EnumFlagsField label="Flags" />` | Multi-enum flags |

### Layout / Container

| Element | UXML | Description |
|---------|------|-------------|
| `VisualElement` | `<VisualElement />` | Base container |
| `Box` | `<Box />` | Container with border |
| `GroupBox` | `<GroupBox text="Group" />` | Labeled group |
| `ScrollView` | `<ScrollView />` | Scrollable container |
| `TwoPaneSplitView` | `<TwoPaneSplitView />` | Resizable split |
| `TabView` | `<TabView />` | Tab container |
| `Tab` | `<Tab label="Tab 1" />` | Individual tab |
| `Foldout` | `<Foldout text="Details" />` | Collapsible section |
| `TemplateContainer` | `<Instance template="..." />` | UXML template instance |

### Data Display

| Element | UXML | Description |
|---------|------|-------------|
| `ListView` | `<ListView />` | Virtualized list |
| `TreeView` | `<TreeView />` | Hierarchical tree |
| `MultiColumnListView` | `<MultiColumnListView />` | Multi-column list |
| `MultiColumnTreeView` | `<MultiColumnTreeView />` | Multi-column tree |

### Editor Only

| Element | Description |
|---------|-------------|
| `ObjectField` | Asset/object reference picker |
| `ColorField` | Color picker |
| `GradientField` | Gradient editor |
| `CurveField` | Animation curve editor |
| `PropertyField` | Auto-generates UI for a SerializedProperty |
| `InspectorElement` | Embeds an inspector |
| `Toolbar`, `ToolbarButton`, `ToolbarMenu` | Editor toolbar elements |
| `IMGUIContainer` | Hosts legacy IMGUI rendering |

---

## Common Patterns

### Show/Hide Elements

```csharp
// Hide
element.style.display = DisplayStyle.None;
// Show
element.style.display = DisplayStyle.Flex;

// Alternative: visibility (keeps layout space)
element.style.visibility = Visibility.Hidden;
element.style.visibility = Visibility.Visible;
```

### Enable/Disable

```csharp
element.SetEnabled(false);  // Grays out and disables interaction
element.SetEnabled(true);
```

### Dynamic Class Toggling

```csharp
element.AddToClassList("active");
element.RemoveFromClassList("active");
element.ToggleInClassList("active");
element.EnableInClassList("active", isActive);
```

### Creating Elements from Code

```csharp
var container = new VisualElement();
container.AddToClassList("container");

var label = new Label("Player Name");
var input = new TextField { value = "Default" };
var submit = new Button(() => Debug.Log(input.value)) { text = "Submit" };

container.Add(label);
container.Add(input);
container.Add(submit);

root.Add(container);
```

### Loading UXML Templates at Runtime

```csharp
var template = Resources.Load<VisualTreeAsset>("UI/MyPanel");
var instance = template.Instantiate();
root.Add(instance);
```

### Adding Stylesheets from Code

```csharp
var stylesheet = Resources.Load<StyleSheet>("UI/MyStyles");
root.styleSheets.Add(stylesheet);
```
