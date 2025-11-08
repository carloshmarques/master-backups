## Quick orientation for AI coding agents

This repo is a single Windows desktop application (WinForms) named "LifeCicles" under the `LifeCicles/` folder. Primary facts you must know to be productive:

- Entry point: `LifeCicles/Program.cs` — it launches `new SplashScreen()` (file: `LifeCicles/Boot System/SplashScreen.cs`).
- Solution / project: `LifeCicles/HydraLife.sln` and `LifeCicles/HydraLife.csproj` target .NET Framework 4.8 (non-SDK csproj). Prefer Visual Studio 2022 for builds and debugging.
- Runtime / UI: Windows Forms (Designer files like `*.Designer.cs` are generated; do not edit them directly unless necessary).

Key directories and responsibilities
- `LifeCicles/Modules/` — contains the app services and subsystems (Theme manager, Voice, Media, Terminal, Lexicon). Example: `Modules/HydraThemeManager.cs`, `Modules/Voice/HydraVoice.cs`.
- `LifeCicles/Boot System/` — splash/boot UI (contains `SplashScreen.cs` and its designer/resx). Program.Main runs the splash flow.
- `LifeCicles/LoginSystem/` — login and virtual desktop forms (WinForms partial classes + resx).
- `LifeCicles/Assets/` — media, themes and theme manifests. `Assets/Themes/Colorful-Plasma-Themes` includes `update-themes.sh` and `source.txt` (scripts rely on bash/yt-dlp).
- `packages/` and `packages.config` — NuGet packages are vendored; `Newtonsoft.Json` used widely.

Build & run (concrete, reproducible)
- Recommended: open `LifeCicles/HydraLife.sln` in Visual Studio 2022 and press F5.
- CLI alternative (Windows developer environment):
  - Restore packages: `nuget restore LifeCicles\HydraLife.sln` (or use VS to restore).
  - Build: open Developer Command Prompt or use MSBuild: `msbuild LifeCicles\HydraLife.sln /p:Configuration=Debug`
  - Run: the built EXE will be at `LifeCicles\bin\Debug\LifeCicles.exe`.

Important repo-specific patterns
- Many folders contain spaces (e.g., `Boot System`) — always quote or escape these paths in scripts and commands.
- UI classes use WinForms partial class pattern: pair `.cs` + `.Designer.cs` + `.resx`. Prefer modifying the `.cs` file and designer via Visual Studio designer when possible.
- Theme/media ingestion: themes are declared in `Assets/Themes/*` and often use `list.txt` or `source.txt` files read by runtime code (see `Modules/HydraLauncher.cs` and calls to `update-themes.sh`). Example: `Modules/HydraLauncher.cs` may call `Process.Start("bash", "LifeCicles/Assets/.../update-themes.sh")`.
- Logging/lexicon: `Modules/Lexicon/HydraLexiconReporter.cs` collects events — follow this file to understand how runtime events are recorded.

Integration & external dependencies
- Uses System.Speech (TTS) and `Newtonsoft.Json` (packages under `packages/`). Ensure NuGet packages are present/restored before build.
- Some runtime behaviors call external tooling (e.g., `yt-dlp` via scripts). Those are optional for core builds but required to reproduce theme/media downloads.

Search patterns quickly useful when coding
- Find app entry and boot flow: search for `SplashScreen` or `Application.Run(new SplashScreen())`.
- Find theme/media flows: `update-themes.sh`, `source.txt`, `list.txt`, `ThemeManifest`.
- Find voiced/TTS logic: search `HydraVoice` and `System.Speech`.

Do / Do not (project specific)
- Do: prefer small, reversible edits; run the solution in Visual Studio to validate UI changes.
- Do not: manually rewrite `.Designer.cs` files outside the designer. Avoid changing resource names without updating `.resx` and `Properties/Resources.Designer.cs`.

Examples to reference in prompts
- "Change the splash sequence" — look at `LifeCicles/Boot System/SplashScreen.cs` and `LifeCicles/Modules/Ui/SplashScreenManager.cs`.
- "Add a new theme entry" — update `Assets/Themes/ThemeManifest.json` and add media under `Assets/Themes/...`, then document any `list.txt` entries.

If you're uncertain about build failures
- Check `LifeCicles/HydraLife.csproj` target framework (v4.8). Prefer Visual Studio's build output; if CLI builds fail, copy the exact msbuild/nuget output into the issue for diagnostics.

Where to read next (high value files)
- `LifeCicles/Program.cs` (entry)
- `LifeCicles/HydraLife.csproj` (target framework, references)
- `LifeCicles/Modules/` (core logic boundaries and helpers)
- `LifeCicles/Boot System/SplashScreen.cs` (UI startup flow)
- `LifeCicles/Assets/Themes/` + `Assets/Themes/Colorful-Plasma-Themes/update-themes.sh` (media ingestion)

If a user asks for tests or CI changes: there are no unit tests or test projects. Add tests only after discussing conventions with the maintainer.

Questions for the maintainer
- Do you want Copilot agents to modify designer files directly, or always prefer Visual Studio designer changes?
- Are the theme/media download scripts intended to be run on Windows via WSL (bash) in CI, or only locally by the maintainer?

End of instructions — please review and tell me any missing areas you want expanded.
