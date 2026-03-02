# Smart Strings Playbook

## Use Cases

- Use Smart Strings for text that depends on runtime values.
- Use Smart Strings for grammar-sensitive localization (plural, gender, conditional phrasing).
- Keep static UI labels as normal strings unless smart behavior is needed.

## Authoring Rules

- Enable Smart per table entry before using placeholders.
- Prefer explicit formatter names to reduce ambiguity:
  - `plural`, `choose`, `cond`, `list`, `template`, `time`.
- Use named selectors only when source order is controlled and conflicts are unlikely.
- Use indexed selectors (`{0}`, `{1}`) when selector collisions are possible.
- Use `{}` for current scope value inside nested expressions.

## Source and Formatter Ordering

- Remember Smart processing order: source resolution first, then formatter.
- Configure source order in Localization Settings deliberately.
- Avoid selector names that collide across sources (for example `Count` with reflection/dictionary).

## High-Value Patterns

- Pluralization:
  - `I have {count:plural:an apple|{} apples}`
- Choice by value:
  - `{gender:choose(Male|Female):El|La} {item}`
- Conditional output:
  - `{lives:cond:zero|one|many}`
- Nested scope:
  - `{User.Address:{City}, {State}}`
- Lists:
  - `{items:list:{}|, |, and }`

## Variables

- Use Local Variables on `LocalizedString` for component-level dynamic data.
- Add `Persistent Variables Source` for global shared values.
- Use nested localized strings for reusable phrase composition.
- Preload nested tables when root and nested strings are in different tables.

## Advanced

- Add custom sources via `ISource` and register in Smart Format Sources.
- Add custom formatters via `FormatterBase` and register in Smart Format Formatters.
- Add `IsMatch` explicitly; it is not enabled by default.
- Note that `time` formatter unit labels are English-only.

## Debug and Safety

- Keep Smart Format error actions strict during implementation (`ThrowError`).
- Validate parse/format failures through Smart format failure events when debugging complex templates.
- Test Smart Strings with representative locale data and runtime variable ranges.
