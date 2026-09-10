# YunaAUbot Atlas fork

Based on yokkenUA/Atlas **0.5.5**, upstream commit `55a4bd6`.
This fork targets **GameHelper2-LinuxFork with the shared price-provider API**.

Our additions:

- **Ritual Atlas Line price weights:** uses the registered NinjaPricer provider's
  Exalted-equivalent prices. Known stack sizes multiply the price. Unknown stack
  sizes use one item's price, marked `*`; this is a ranking score, not guaranteed profit.
- **Manual fallback:** when a reward has no quote, the existing manual weight applies.
  Automatic weights can be disabled. Upstream's saved/default manual weights remain
  available; in automatic mode their values count as Exalted-equivalent scores.
- **Bounded refresh work:** one quote per distinct item name, at most once per five
  seconds while the planner/settings are used. Routes re-sort only when weights change.
  No additional price-network client is introduced.
- **Atlas performance:** skip inventory/player/language work when the Atlas is closed,
  and reuse route/graph coordinate dictionaries. Draw-list channels are only split
  after the early-return checks.

Upstream's current routing, reward predictions, maps, assets and other features
remain in place. The plugin assembly is **Atlas.dll**, installed under **Plugins/Atlas**.
It is a separate plugin from the host's bundled Atlas2; enable only the intended one
when comparing them. Creating this fork does not migrate Atlas2's saved settings.

## Build and verification

Build the complete GameHelper2-LinuxFork host solution first. This plugin supports
`GAMEHELPER2_HOST_ROOT` or `-p:GameHelperHostRoot=...` for a standalone checkout,
and resolves the host automatically when placed under its `Plugins/Atlas` directory.

```bash
export GAMEHELPER2_HOST_ROOT=/path/to/GameHelper2-LinuxFork
dotnet build Atlas.csproj -c Release -p:EnableWindowsTargeting=true -p:BuildProjectReferences=false
dotnet run --project test/AtlasPricing.Tests.csproj -c Release
```

The pricing probe covers mapping, quantities, fractional prices, refresh cadence,
provider unload/failure and manual fallback. The fork was built as part of the full
host/plugin solution. Rendering and predictions still require live in-game validation.
Tests are confined to `test/`, excluded from production compilation and Git-plugin
discovery. Original upstream documentation follows.

---

# Atlas

A [GH](https://github.com/Gordin/GameHelper2) plugin that overlays
the endgame Atlas: it labels each map node with its name, content badges and biome border, can hide
completed / not-accessible maps, draws routing lines to citadels / towers / searched maps, and
colour-codes maps via custom groups.

All per-node data (map id, biome, completed / accessible state) is read straight from game memory
using offsets verified live for build **0.5.x**.

## Requirements

- A current [GH](https://github.com/Gordin/GameHelper2) checkout (this is a plugin, not a
  standalone app).
- .NET 10 SDK (targets `net10.0-windows`, x64).

## Build & install

Drop this repo into the GameHelper2 `Plugins` directory so the layout is:

```
<GameHelper2>/
  GameHelper/GameHelper.csproj
  Plugins/
    Atlas/               ← contents of this repo
      Atlas.csproj
      json/biome.json
      ...
```

The `.csproj` expects `..\..\GameHelper\GameHelper.csproj` and, on build, copies `Atlas.dll` plus
the `json/` data into `GameHelper/<OutDir>/Plugins/Atlas/`. Build, then enable **Atlas** in
GameHelper's plugin list and open the in-game Atlas (World) screen.

> The `json/` folder (biome / content definitions) is **source data and must ship with the repo** —
> without it biome borders and content badges are empty.

## Settings (highlights)

- **Search Maps** — highlight and draw lines to matching maps (comma-separated).
- **Draw Lines Settings** — route lines through nodes (A\*), and to citadels / towers / search hits.
- **Hide Completed Maps** / **Hide Not Accessible Maps** — filter nodes by state.
- **Show Biome Border** — colour each node's border by biome.
- **Layout Settings** — nudge / scale the labels.
- **Map Groups** — colour-code custom lists of maps (Citadels, Towers, …).

## Credits

- Built as a plugin for [GH](https://github.com/Gordin/GameHelper2).

## Disclaimer

This is a read-only overlay tool for personal use. Use at your own risk and in accordance with the game's terms of service.
