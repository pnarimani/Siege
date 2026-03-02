# UI Toolkit Quick Reference

## HTML/CSS to UXML/USS Conversion Cookbook

This section provides a quick translation guide for developers familiar with HTML/CSS who need to work with Unity's UXML/USS.

### Document Structure

| HTML | UXML |
|------|------|
| `<!DOCTYPE html>` | `<?xml version="1.0" encoding="utf-8"?>` |
| `<html>` | `<UXML xmlns="UnityEngine.UIElements">` |
| `<head><link rel="stylesheet" href="...">` | `<Style src="..." />` (inside UXML root) |
| `<body>` | Root `<VisualElement>` |
| `<div>` | `<VisualElement>` |
| `<span>` | `<Label>` or `<VisualElement>` |
| `<p>` | `<Label>` |
| `<h1>`..`<h6>` | `<Label class="h1">` (style via USS) |
| `<button>` | `<Button text="Click" />` |
| `<input type="text">` | `<TextField />` |
| `<input type="number">` | `<IntegerField />` or `<FloatField />` |
| `<input type="checkbox">` | `<Toggle />` |
| `<input type="radio">` | `<RadioButton />` |
| `<select>` | `<DropdownField />` |
| `<textarea>` | `<TextField multiline="true" />` |
| `<img>` | `<Image />` |
| `<ul>/<ol>` | `<ListView />` |
| `<progress>` | `<ProgressBar />` |
| `<details>` | `<Foldout>` |
| `<fieldset>` | `<GroupBox>` |
| `<div style="overflow:auto">` | `<ScrollView />` |

### Attributes

| HTML | UXML | Notes |
|------|------|-------|
| `id="foo"` | `name="foo"` | Queried with `#foo` in USS |
| `class="bar baz"` | `class="bar baz"` | Same syntax |
| `style="..."` | `style="..."` | Same syntax (avoid in production) |
| `disabled` | `enabled="false"` | Inverted logic |
| `tabindex="1"` | `tabindex="1"` | Same concept |
| `title="hint"` | `tooltip="hint"` | Editor only |
| `hidden` | `style="display: none;"` | No direct attribute |

### Selectors

| CSS | USS | Notes |
|-----|-----|-------|
| `div {}` | `VisualElement {}` | Type selector uses C# class name |
| `#my-id {}` | `#my-name {}` | Name selector (uses `name` attribute) |
| `.my-class {}` | `.my-class {}` | Identical |
| `* {}` | `* {}` | Identical |
| `A B {}` | `A B {}` | Descendant - identical |
| `A > B {}` | `A > B {}` | Child - identical |
| `A, B {}` | `A, B {}` | List - identical |
| `A.class {}` | `A.class {}` | Compound - identical |
| `:hover` | `:hover` | Identical |
| `:active` | `:active` | Identical |
| `:focus` | `:focus` | Identical |
| `:disabled` | `:disabled` | Identical |
| `:enabled` | `:enabled` | Identical |
| `:checked` | `:checked` | Identical |
| `:root` | `:root` | Identical |
| `:first-child` | N/A | NOT supported |
| `:last-child` | N/A | NOT supported |
| `:nth-child()` | N/A | NOT supported |
| `::before/::after` | N/A | NOT supported |
| `A + B` (adjacent sibling) | N/A | NOT supported |
| `A ~ B` (general sibling) | N/A | NOT supported |
| `[attr="val"]` | N/A | Attribute selectors NOT supported |

### Properties

| CSS | USS | Notes |
|-----|-----|-------|
| `display: flex` | `display: flex` | Only `flex` and `none` supported |
| `display: block/grid/inline` | N/A | NOT supported (everything is flex) |
| `flex-direction` | `flex-direction` | Identical; default is `column` (not `row`!) |
| `flex-grow` | `flex-grow` | Identical |
| `flex-shrink` | `flex-shrink` | Identical |
| `flex-basis` | `flex-basis` | Identical |
| `flex-wrap` | `flex-wrap` | Identical |
| `justify-content` | `justify-content` | Identical |
| `align-items` | `align-items` | Identical |
| `align-self` | `align-self` | Identical |
| `align-content` | `align-content` | Identical |
| `width/height` | `width/height` | Identical |
| `min-width/max-width` | `min-width/max-width` | Identical |
| `min-height/max-height` | `min-height/max-height` | Identical |
| `margin` | `margin` | Identical |
| `padding` | `padding` | Identical |
| `border-width` | `border-width` | Identical |
| `border-color` | `border-color` | Identical |
| `border-radius` | `border-radius` | No elliptical corners |
| `background-color` | `background-color` | Identical |
| `background-image: url()` | `background-image: url()` | Uses project:// or resource:// paths |
| `background-size` | `-unity-background-scale-mode` | `scale-to-fit`, `scale-and-crop`, `stretch-to-fill` |
| `opacity` | `opacity` | Identical |
| `visibility` | `visibility` | Identical |
| `overflow` | `overflow` | `visible` or `hidden` only |
| `position: relative` | `position: relative` | Identical |
| `position: absolute` | `position: absolute` | Identical |
| `position: fixed/sticky` | N/A | NOT supported |
| `top/right/bottom/left` | `top/right/bottom/left` | Identical |
| `color` | `color` | Identical (text color) |
| `font-size` | `font-size` | Identical |
| `font-family` | `-unity-font-definition` | Uses Unity font asset reference |
| `font-weight/font-style` | `-unity-font-style` | `normal`, `bold`, `italic`, `bold-and-italic` |
| `text-align` | `-unity-text-align` | `upper-left`, `middle-center`, `lower-right`, etc. |
| `white-space` | `white-space` | Identical |
| `text-overflow` | `text-overflow` | `clip` or `ellipsis` |
| `letter-spacing` | `letter-spacing` | Identical |
| `word-spacing` | `word-spacing` | Identical |
| `text-shadow` | `text-shadow` | Identical syntax |
| `transform: rotate()` | `rotate` | Separate property |
| `transform: scale()` | `scale` | Separate property |
| `transform: translate()` | `translate` | Separate property |
| `transform-origin` | `transform-origin` | Identical |
| `transition` | `transition` | Identical syntax |
| `transition-property` | `transition-property` | Identical |
| `transition-duration` | `transition-duration` | Identical |
| `transition-timing-function` | `transition-timing-function` | Identical + extra easing functions |
| `transition-delay` | `transition-delay` | Identical |
| `cursor` | `cursor` | Identical |
| `box-sizing` | N/A | Always `border-box` (cannot change) |
| `z-index` | N/A | NOT supported (use hierarchy order) |
| `float` | N/A | NOT supported |
| `text-decoration` | N/A | NOT supported |
| `animation / @keyframes` | N/A | NOT supported (use transitions or C#) |
| `@media` | N/A | NOT supported |
| `calc()` | N/A | NOT supported |
| `var()` | `var()` | Supported but cannot nest in functions |
| `--custom-prop` | `--custom-prop` | Identical declaration syntax |

### Key Differences Summary

| Aspect | HTML/CSS | UXML/USS |
|--------|----------|----------|
| **Box model** | `content-box` by default | `border-box` always |
| **Default flex-direction** | `row` | `column` |
| **Display modes** | `block`, `inline`, `flex`, `grid`, etc. | `flex` or `none` only |
| **ID vs Name** | `id` attribute | `name` attribute |
| **Font reference** | `font-family: "Arial"` | `-unity-font-definition: url("Assets/Fonts/MyFont.asset")` |
| **Text alignment** | `text-align: center` | `-unity-text-align: middle-center` |
| **Image scaling** | `background-size`, `object-fit` | `-unity-background-scale-mode` |
| **z-ordering** | `z-index` | Hierarchy order (last child on top) |
| **Media queries** | `@media` | Not supported |
| **Animations** | `@keyframes` + `animation` | Transitions only (or C# animation) |
| **Pseudo-elements** | `::before`, `::after` | Not supported |
| **Structural pseudos** | `:first-child`, `:nth-child` | Not supported |
| **Sibling selectors** | `+`, `~` | Not supported |

---

## USS Variables Cheat Sheet

```css
:root {
    /* Colors */
    --color-primary: #3498db;
    --color-secondary: #2ecc71;
    --color-danger: #e74c3c;
    --color-bg: #1a1a2e;
    --color-text: #eee;

    /* Spacing */
    --spacing-xs: 4px;
    --spacing-sm: 8px;
    --spacing-md: 16px;
    --spacing-lg: 24px;
    --spacing-xl: 32px;

    /* Typography */
    --font-sm: 12px;
    --font-md: 16px;
    --font-lg: 24px;
    --font-xl: 32px;

    /* Border */
    --radius-sm: 4px;
    --radius-md: 8px;
    --radius-lg: 16px;
    --radius-round: 50%;
}
```

---

## Common Layout Patterns

### Centered Content

```css
.center {
    flex-grow: 1;
    align-items: center;
    justify-content: center;
}
```

### Horizontal Row

```css
.row {
    flex-direction: row;
    align-items: center;
}
```

### Space Between Row

```css
.row-between {
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
}
```

### Full Screen Overlay

```css
.overlay {
    position: absolute;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.5);
    align-items: center;
    justify-content: center;
}
```

### Scrollable Column

```xml
<ScrollView>
    <VisualElement class="scroll-content">
        <!-- items -->
    </VisualElement>
</ScrollView>
```

```css
.scroll-content {
    padding: var(--spacing-md);
}
```

### Fixed Header + Scrollable Body + Fixed Footer

```xml
<VisualElement class="screen">
    <VisualElement class="header">
        <Label text="Title" />
    </VisualElement>
    <ScrollView class="body">
        <!-- content -->
    </ScrollView>
    <VisualElement class="footer">
        <Button text="Action" />
    </VisualElement>
</VisualElement>
```

```css
.screen {
    flex-grow: 1;
}

.header {
    height: 60px;
    flex-direction: row;
    align-items: center;
    padding: 0 var(--spacing-md);
}

.body {
    flex-grow: 1;
}

.footer {
    height: 60px;
    flex-direction: row;
    align-items: center;
    justify-content: center;
    padding: 0 var(--spacing-md);
}
```

### Equal-Width Columns

```css
.columns {
    flex-direction: row;
}

.columns > VisualElement {
    flex-grow: 1;
    flex-basis: 0;
}
```

---

## Event Quick Reference

| Task | Code |
|------|------|
| Button click | `button.clicked += Handler;` |
| Value change | `field.RegisterValueChangedCallback(evt => { ... });` |
| Pointer click | `el.RegisterCallback<ClickEvent>(evt => { ... });` |
| Pointer down | `el.RegisterCallback<PointerDownEvent>(evt => { ... });` |
| Pointer enter/leave | `el.RegisterCallback<PointerEnterEvent/PointerLeaveEvent>(...)` |
| Focus in/out | `el.RegisterCallback<FocusInEvent/FocusOutEvent>(...)` |
| Key down | `el.RegisterCallback<KeyDownEvent>(evt => { ... });` |
| Attached to panel | `el.RegisterCallback<AttachToPanelEvent>(evt => { ... });` |
| Layout changed | `el.RegisterCallback<GeometryChangedEvent>(evt => { ... });` |
| Trickle-down | `el.RegisterCallback<T>(handler, TrickleDown.TrickleDown);` |
| Stop propagation | `evt.StopPropagation();` |
| Unregister | `el.UnregisterCallback<T>(handler);` |

---

## UQuery Quick Reference

```csharp
// Single element by name
var el = root.Q("my-name");

// Single element by type + name
var btn = root.Q<Button>("submit");

// Single element by class
var el = root.Q(className: "active");

// Multiple elements
var list = root.Query<Label>().ToList();

// Combined filters
var el = root.Q<Button>(name: "ok", className: "primary");

// With predicate
var filtered = root.Query<Label>().Where(l => l.text != "").ToList();

// ForEach (no allocation)
root.Query<Button>().ForEach(b => b.SetEnabled(false));
```

---

## Localization Quick Reference

### UXML String Localization

```xml
<Label text="fallback">
    <Bindings>
        <LocalizedString property="text" table="UIStrings" entry="my_key" />
    </Bindings>
</Label>
```

### UXML Sprite Localization

```xml
<Image>
    <Bindings>
        <LocalizedSprite property="style.backgroundImage" table="UISprites" entry="flag" />
    </Bindings>
</Image>
```

### C# String Localization

```csharp
var label = root.Q<Label>("title");
label.SetBinding("text", new LocalizedString("UIStrings", "title_key"));
```

### C# Dynamic Variables

```csharp
var binding = label.GetBinding("text") as LocalizedString;
var score = binding["score"] as IntVariable;
score.Value = newScore;  // Auto-updates UI
```

### Available Binding Types

`LocalizedString`, `LocalizedSprite`, `LocalizedTexture`, `LocalizedAudioClip`, `LocalizedFont`, `LocalizedTmpFont`, `LocalizedGameObject`, `LocalizedMaterial`, `LocalizedMesh`, `LocalizedObject`

---

## USS Pseudo-Classes Reference

| Pseudo-class | Triggered when | Chainable |
|-------------|----------------|-----------|
| `:hover` | Cursor over element | Yes |
| `:active` | User pressing/interacting | Yes |
| `:inactive` | User stopped interacting | Yes |
| `:focus` | Element has keyboard focus | Yes |
| `:checked` | Toggle/RadioButton is on | Yes |
| `:disabled` | `SetEnabled(false)` | Yes |
| `:enabled` | `SetEnabled(true)` | Yes |
| `:root` | Top-level element with stylesheet | No |

Example of chaining: `Button:hover:active { ... }`

---

## Transition Timing Functions

| Function | Description |
|----------|-------------|
| `linear` | Constant speed |
| `ease` | Default smooth curve |
| `ease-in` | Slow start |
| `ease-out` | Slow end |
| `ease-in-out` | Slow start and end |
| `ease-in-sine` | Sinusoidal slow start |
| `ease-out-sine` | Sinusoidal slow end |
| `ease-in-out-sine` | Sinusoidal slow both |
| `ease-in-cubic` | Cubic slow start |
| `ease-out-cubic` | Cubic slow end |
| `ease-in-out-cubic` | Cubic slow both |
| `ease-in-back` | Overshoot start |
| `ease-out-back` | Overshoot end |
| `ease-in-out-back` | Overshoot both |
| `ease-in-bounce` | Bounce at start |
| `ease-out-bounce` | Bounce at end |
| `ease-in-out-bounce` | Bounce at both |
| `ease-in-elastic` | Elastic at start |
| `ease-out-elastic` | Elastic at end |
| `ease-in-out-elastic` | Elastic at both |

---

## Unity-Specific USS Properties

| Property | CSS Equivalent | Values |
|----------|---------------|--------|
| `-unity-font-definition` | `font-family` | `url("path/to/font.asset")` |
| `-unity-font-style` | `font-weight` + `font-style` | `normal`, `bold`, `italic`, `bold-and-italic` |
| `-unity-text-align` | `text-align` + `vertical-align` | `upper-left`, `middle-center`, `lower-right`, etc. |
| `-unity-background-scale-mode` | `background-size` / `object-fit` | `scale-to-fit`, `scale-and-crop`, `stretch-to-fill` |
| `-unity-background-image-tint-color` | N/A | color value |
| `-unity-text-outline-width` | N/A | length |
| `-unity-text-outline-color` | N/A | color |
| `-unity-slice-left/top/right/bottom` | N/A | int (9-slice borders) |
| `-unity-slice-scale` | N/A | float |
| `-unity-overflow-clip-box` | N/A | `padding-box`, `content-box` |
| `-unity-paragraph-spacing` | N/A | length |
| `-unity-material` | N/A | material reference |

---

## File Organization Convention

```
Assets/
  UI/
    Screens/
      MainMenu.uxml
      MainMenu.uss
      Settings.uxml
      Settings.uss
    Components/
      HealthBar.uxml
      HealthBar.uss
      PlayerCard.uxml
      PlayerCard.uss
    Shared/
      Variables.uss        # :root variables
      Typography.uss       # text styles
      Buttons.uss          # button styles
    PanelSettings.asset    # shared panel settings
```

---

## Common Errors and Fixes

| Error | Cause | Fix |
|-------|-------|-----|
| Element not found (Q returns null) | Query runs before UXML is loaded | Move queries to `OnEnable()` or `AttachToPanelEvent` |
| Styles not applying | USS not referenced in UXML | Add `<Style src="..." />` to UXML |
| Layout not updating | Inline styles override USS | Remove inline `style` attributes |
| Transition not animating | Transition defined on wrong state | Define `transition-*` on base selector, not `:hover` |
| Transition skips on first frame | USS transitions don't work frame 0 | Defer style changes using `schedule.Execute` |
| ListView shows nothing | `makeItem`/`bindItem` not set | Set both callbacks + `itemsSource` |
| Click events not firing | `picking-mode="Ignore"` | Set `picking-mode="Position"` or remove attribute |
| Binding not working | Element not `IBindable` | Use bindable controls (TextField, etc.) |
| NullReferenceException | UIDocument not ready | Use `OnEnable`, not `Awake`/`Start` |
