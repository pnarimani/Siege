---
name: unity-localization
description: Unity Localization Guide. Read this when working on UI or Localization.
---

# Unity Localization

## Workflow

2. Classify the request as static localization, dynamic localization, or locale-specific property variants.
3. Prefer Component Localizers when localization is not dynamic.
4. Use Smart Strings for runtime-variable text and grammar logic.
5. Use Localized Property Variants for locale-specific visual/property differences, not high-frequency text updates.

## Decision Rules

- Use Component Localizers when the localized value is not expected to change continuously at runtime.
- Use Smart Strings plus `LocalizedString` local/global variables when output depends on runtime data.
- Use Localized Property Variants for locale-specific layout and styling changes (font, transform, spacing, sprite overrides).
- Use a hybrid when needed: dynamic values through localizers/scripts and static per-locale presentation through variants.
- Use table GUIDs and entry Key IDs for stable long-lived references.

## Unity MCP Authoring

1. Run `unity_compile` before scene/prefab localization edits.
2. Locate target objects with `unity_query_hierarchy`.
3. Inspect components with `unity_query_components` and `includeProperties=true`.
4. Add missing localizer components with `unity_manage_components`.
5. Use `LocalizeStringComponent` for static localization tasks.
6. Set serialized values with `unity_set_property`.
7. Do not guess property paths. Query property paths first, then set values.
8. Verify results with `unity_get_property` or a follow-up `unity_query_components`.

## Smart Strings Rules

- Mark each relevant table entry as Smart.
- Prefer explicit formatter names (`plural`, `choose`, `cond`, `list`, `template`, `time`) over implicit behavior.
- Control source and formatter order in Localization Settings to avoid selector conflicts.
- Use local variables for component-scoped runtime values.
- Add `Persistent Variables Source` for shared global values.
- Preload nested tables when using nested localized strings.
- Add `IsMatch` manually when regex formatting is required.
- Remember `Time` formatter output text is English-only.

## References

- Use `references/manual-map.md` for full feature navigation.
- Use `references/smart-strings-playbook.md` for formatter/source guidance.
