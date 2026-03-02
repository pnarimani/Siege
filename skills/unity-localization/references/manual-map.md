# Unity Localization Manual Map (1.5)

## Core

- `index.md`: package scope and capabilities.
- `Installation.md`: package install constraints.
- `whats-new.md`: 1.5 additions.
- `upgrade-guide.md`: migration notes and breaking changes.
- `Package-Samples.md`: sample import path and usage.

## Project Setup

- `LocalizationSettings.md`: locales, selectors, databases, preload, async, Smart Format config.
- `Locale.md`: locale metadata and fallback chains.
- `LocaleSelector.md`: startup locale selection order and selector types.
- `Addressables.md`: group rules, labels, analyzers, safety constraints.
- `EditModeSupport.md`: edit-mode preview and driven properties.

## Tables and Keys

- `LocalizationTablesWindow.md`: create and manage table collections.
- `StringTables.md`: string tables, Smart flag, metadata, character set export.
- `AssetTables.md`: asset table collections and preload behavior.
- `TableEntryKeys.md`: key IDs, distributed generator, custom generators.

## Authoring Paths

- `LocalizedPropertyVariants.md`: locale-specific property overrides and tracking.
- `Scripting.md`: `LocalizedString`, `LocalizedAsset`, async loading, providers, patchers.
- `SearchFilters.md`: localization search query filters and scripted search.
- `UIToolkit.md`: Unity 6+ UI Toolkit bindings.

## Smart Strings

- `Smart/SmartStrings.md`: syntax model, source/formatter pipeline, scope, nesting.
- Sources:
  - `Smart/Default-Source.md`
  - `Smart/Dictionary-Source.md`
  - `Smart/Persistent-Variables-Source.md`
  - `Smart/List-Source.md`
  - `Smart/Reflection-Source.md`
  - `Smart/Value-Tuple-Source.md`
  - `Smart/Xml-Source.md`
  - `Smart/Creating-a-Custom-Source.md`
- Formatters:
  - `Smart/Choose-Formatter.md`
  - `Smart/Conditional-Formatter.md`
  - `Smart/Default-Formatter.md`
  - `Smart/IsMatch-Formatter.md`
  - `Smart/List-Formatter.md`
  - `Smart/Plural-Formatter.md`
  - `Smart/SubString-Formatter.md`
  - `Smart/Template-Formatter.md`
  - `Smart/Time-Formatter.md`
  - `Smart/XElement-Formatter.md`
  - `Smart/Creating-a-Custom-Formatter.md`

## Metadata and Overrides

- `Metadata.md`: metadata model and extension points.
- `Metadata-Platform-Overrides.md`: platform entry redirection.

## Pipeline Integrations

- `CSV.md`: import/export and CSV extension mapping.
- `XLIFF.md`: 1.2/2.0 export/import workflow and mapping.
- `Google-Sheets.md`: overview.
- `Google-Sheets-Sheets-Service-Provider.md`: provider asset and auth mode.
- `Google-Sheets-Configuring-Authentication.md`: OAuth/API key setup.
- `Google-Sheets-Syncing-StringTableCollections.md`: mapped columns and sync operations.

## Platform and Test Tooling

- `Android-App-Localization.md`: app name/icon localization metadata.
- `iOS-App-Localization.md`: Apple Info.plist localization metadata.
- `Pseudo-Localization.md`: pseudo-locale setup and runtime usage.
- `Pseudo-Localization-Methods.md`: method-by-method behavior.
