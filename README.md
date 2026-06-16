# LindRoof

Roof panel layout and optimization tool (LINDSOFT). Windows Forms desktop
application for laying out and optimizing sheet-metal roof panels.

## Prerequisites

- Windows
- .NET Framework 4.8 (developer pack / targeting pack)
- .NET SDK (for `dotnet build`) **or** Visual Studio 2022
- Builds as **x86** (`PlatformTarget` is `x86`) because it depends on
  32-bit native libraries (`gpc.dll`, `msvcr71.dll`).

## Build

```sh
dotnet build LindRoof.sln -c Debug
```

or build the project directly:

```sh
dotnet build LindRoof.csproj -c Debug
```

The output is `bin/Debug/net48/LindRoof.exe`.

## Run

```sh
dotnet build LindRoof.csproj -c Debug
./bin/Debug/net48/LindRoof.exe
```

Run from the output directory so the app finds its companion runtime files.

## Project layout

| Path | Contents |
|------|----------|
| `LindRoof/` | Main application code (forms, document model, drawing). |
| `LindRoof.FormsEnvironment/`, `LindRoof.GraphicEnvironment/`, `LindRoof.TextualEnvironment/`, `LindRoof.toolbar/` | Supporting source folders (all compiled into the single project). |
| `GpcWrapper/` | Managed wrapper around the GPC polygon-clipping native library. |
| `Resources/` | Embedded resources: form `.resx`, bitmaps, cursors, icons, font. |
| `lib/` | Vendored COM interop assemblies (see below). |
| `native/` | 32-bit native DLLs copied next to the executable at build time. |
| `runtime/` | Companion data files the app reads at runtime (`date.ver`, `lindroof.dbf`, help). |
| `Properties/` | Assembly metadata. |

## Vendored binary dependencies

These are committed because they cannot be reproduced from source in this repo:

- **`lib/Interop.SHDocVw.dll`**, **`lib/AxInterop.SHDocVw.dll`** — COM interop
  wrappers for the legacy Internet Explorer `WebBrowser` ActiveX control, used
  by the Help window. Originally recovered from an installed copy of the app.
  Referenced from `LindRoof.csproj` via `<HintPath>lib\...</HintPath>`.
- **`native/gpc.dll`** — General Polygon Clipper (used via `GpcWrapper`).
- **`native/msvcr71.dll`** — Visual C++ 7.1 runtime required by `gpc.dll`.

`native/` DLLs are copied to the output root on build so `DllImport` can resolve them.
