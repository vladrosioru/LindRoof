# CLAUDE.md

Guidance for working in this repository.

## What this is

LindRoof — a .NET Framework 4.8 **Windows Forms** desktop app (LINDSOFT),
single project (`LindRoof.csproj`). Much of the source appears to be
decompiled, so style is inconsistent and fully-qualified type names are common.

## Build & run

```sh
dotnet build LindRoof.csproj -c Debug      # -> bin/Debug/net48/LindRoof.exe
```

- Must build as **x86 / net48** — do not retarget. The app loads 32-bit
  native DLLs (`gpc.dll`, `msvcr71.dll`).
- It's a GUI app: launching shows a window; there is no console output.

## Gotchas (learned the hard way)

- **COM interop assemblies live in `lib/`.** `Interop.SHDocVw.dll` and
  `AxInterop.SHDocVw.dll` (legacy IE `WebBrowser` ActiveX, used by the Help
  window) are vendored under `lib/` and referenced via `<HintPath>` in the
  csproj. They are not reproducible from source — don't delete them.
- **Embedded resource `LogicalName` must end in `.resources`, not `.resx`.**
  Forms call `new ComponentResourceManager(typeof(SomeForm))`, which looks for
  a manifest resource named `LindRoof.SomeForm.resources`. The `.resx` files
  are compiled to `.resources` and embedded under the `LogicalName` given in
  the csproj. A `.resx` suffix there causes a `MissingManifestResourceException`
  at startup (surfaced by the app as the generic dialog **"Eroare 1"** from a
  catch-all in `MainWindow`).
- **Generic catch blocks swallow exceptions.** `MainWindow.InitComp` is wrapped
  in `try { } catch { MessageBox.Show("Eroare 1") }`. When diagnosing startup
  failures, temporarily include `ex.ToString()` to see the real error.
- **Runtime data files** (`runtime/`: `date.ver`, `lindroof.dbf`, help) and
  `native/` DLLs are copied to the output root; the app reads them from
  `Application.StartupPath`.

## Layout

See `README.md` for the directory map. UI strings are Romanian.
