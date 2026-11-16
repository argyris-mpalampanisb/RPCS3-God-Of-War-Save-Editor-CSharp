<p align="center">
  <img src="https://img.shields.io/badge/.NET-11-purple?logo=dotnet&logoColor=white" alt=".NET 11" />
  <img src="https://img.shields.io/badge/C%23-15-blue?logo=csharp&logoColor=white" alt="C# 15" />
  <img src="https://img.shields.io/badge/Avalonia-12-5C2D91?logo=avalonia&logoColor=white" alt="Avalonia 12" />
  <img src="https://img.shields.io/badge/Platform-Windows%20%7C%20macOS%20%7C%20Linux-brightgreen" alt="Platforms" />
  <img src="https://img.shields.io/badge/RPCS3-Save%20Editor-orange" alt="RPCS3" />
  <img src="https://img.shields.io/badge/License-MIT-yellow" alt="License" />
</p>

<h1 align="center">⚔️ God of War Save Editor (RPCS3)</h1>

<p align="center">
  <strong>Cross-platform save editor for God of War 1, 2, 3, and Ascension</strong>
</p>

<p align="center">
  A modern .NET 11 / C# 15 rewrite of <a href="https://github.com/Wulf2k/GoWSaveEdit">Wulf2k's GoWSaveEdit</a>,
  rebuilt with <a href="https://avaloniaui.net">Avalonia UI</a> for native performance on Windows, macOS, and Linux.
</p>

---

## 🎮 Supported Games

| Game | Save Files | Slots | Notable Features |
|:-----|:-----------|:------|:-----------------|
| **God of War 1** | `MASTER.BIN` + `DATA00.BIN` … `DATA03.BIN` | 4 | Checkpoint warping (~90 locations), movie skip, costume/difficulty, weapon levels |
| **God of War 2** | `MASTER.BIN` + `DATA00.BIN` … `DATA09.BIN` | 10 | Stats, weapons, checkpoints, movie skip |
| **God of War 3** | `SAVEDATA` (+ `PARAM.SFO`) per save folder; `-USERDATA` folder for global settings | 1 per folder | Stats, weapons, globals |
| **God of War: Ascension** | `DAT-1` … `DAT-31` | 31 | Dynamic inventory (151 items), sub-weapons, costumes, NG+, frozen/swim states |

---

## ✨ Features

- **Cross-platform**: one codebase, native feel on Windows, macOS and Linux
- **RPCS3-focused**: works directly on unencrypted save files (no PS3 crypto)
- **Big-endian aware**: correctly handles PS3 byte ordering via `System.Buffers.Binary.BinaryPrimitives`
- **Dynamic offset discovery**: Ascension uses byte-pattern search (`kptr` / `kptr2`) for variable-offset saves
- **Checksum handling**: GoW1/GoW2 save checksums are recomputed and written on save
- **Movie skip**: mark all cinematics as "seen" with one click (GoW1 / GoW2)
- **Checkpoint warping**: predefined WAD/camera/position presets for GoW1 (90+ locations across 19 categories)
- **Inventory editor**: full item checklist for Ascension, kept in sync with the weapon / sub-weapon / costume controls
- **Smart slot finding**: if a slot file isn't next to `MASTER.BIN`, neighbouring save folders of the same game are searched; Ascension jumps to the first valid `DAT-n` if `DAT-1` is empty
- **Status line**: GoW1, GoW2 and Ascension report what was loaded or saved, and why a load failed
- **MVVM architecture**: clean model / view-model / view separation with CommunityToolkit.Mvvm

---

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────┐
│ Views (Avalonia XAML, compiled bindings)                │
│ ├── MainWindow.axaml   (launcher)                       │
│ ├── GoW1View.axaml     (GoW1 editor)                    │
│ ├── GoW2View.axaml     (GoW2 editor)                    │
│ ├── GoW3View.axaml     (GoW3 editor)                    │
│ └── GoWAView.axaml     (Ascension editor)               │
├─────────────────────────────────────────────────────────┤
│ ViewModels (CommunityToolkit.Mvvm)                      │
│ ├── MainWindowViewModel.cs (opens editors, lifetime)    │
│ ├── GoW1ViewModel.cs                                    │
│ ├── GoW2ViewModel.cs                                    │
│ ├── GoW3ViewModel.cs                                    │
│ ├── GoWAViewModel.cs                                    │
│ └── InventoryItem.cs                                    │
├─────────────────────────────────────────────────────────┤
│ Models (pure C#, no UI dependencies)                    │
│ ├── SaveFile.cs        (endian helpers + checksums)     │
│ ├── GoW1Save.cs        (load / save / checksum)         │
│ ├── GoW2Save.cs                                         │
│ ├── GoW3Save.cs                                         │
│ ├── GoWASave.cs        (kptr discovery + inventory)     │
│ ├── GoW1Checkpoints.cs (warp presets)                   │
│ └── Checkpoint.cs      (record for warp data)           │
├─────────────────────────────────────────────────────────┤
│ Converters                                              │
│ ├── IntToRadioConverter.cs                              │
│ └── InverseBoolConverter.cs                             │
└─────────────────────────────────────────────────────────┘
```

### Tech Stack

| Layer | Technology |
|:------|:-----------|
| Runtime | .NET 11 |
| Language | C# 15 |
| UI Framework | Avalonia 12 (cross-platform XAML, Fluent theme) |
| MVVM | CommunityToolkit.Mvvm (source-generated properties & commands) |
| Byte I/O | `BinaryPrimitives` (big-endian) |
| Build | .NET SDK / MSBuild |
| IDE | Visual Studio 2026 Community / VS Code |

---

## 📁 Save File Formats

### GoW 1 / GoW 2: `MASTER.BIN`

```
MASTER.BIN (68+ bytes)
├── [0x04 + 16×slot] Slot marker (GoW1 0xCAFEBAD1, GoW2 0xCAFEBAD4)
├── [0x08 + 16×slot] Playtime (float32 BE)
├── [0x0D + 16×slot] Active flag
├── [0x0E + 16×slot] Difficulty (byte)
└── [0x43]           Global flags (beat game / VH / CotG / statues)
```

### GoW 1: `DATA0x.BIN`

GoW2 uses the same idea with its own offsets (see `Models/GoW2Save.cs`).

```
DATA0x.BIN (~1064 bytes)
├── [0x00]          0xCA header
├── [0x08–0x0F]     WAD1 name (8×ASCII)
├── [0x12]          WAD1 active
├── [0x13–0x1A]     WAD2 name (8×ASCII)
├── [0x1D]          WAD2 active
├── [0x4E–0x59]     X/Y/Z position (float32 BE × 3)
├── [0x60]          Swim state
├── [0x6A]          Seconds played (float32)
├── [0x76–0x85]     Health / Magic / Rage / Regen (float32 × 4)
├── [0x86]          Red orbs (uint16 BE)
├── [0x94–0x9B]     Extensions + weapon levels
├── [0x9C]          Selected weapon
├── [0x9D–0x9F]     Gorgon eyes / Phoenix feathers / Muse keys
├── [0xA0]          Bit 0: Poseidon's Trident
├── [0xA1]          Bits 2–6: weapon unlocked flags
├── [0xAE–0xC5]     Current camera (24×ASCII)
├── [0xC6–0xDD]     Camera WAD (24×ASCII)
├── [0x28A]         Movies seen (int16)
├── [0x28E+]        Movie checksums (uint32 × N)
├── [0x41E–0x428]   World idle / volumes / flags / costume / difficulty
└── [end−4]         Checksum (uint32)
```

### God of War: Ascension: `DAT-n`

```
DAT-{1–31} (variable length, big-endian)
├── [0x00]  0xC0 header
├── [kptr]  Discovered via pattern {00 00 03 24}
│   ├── [+0x0C]  Health (float32)
│   ├── [+0x10]  Health Regen (float32)
│   ├── [+0x14]  Magic (float32)
│   ├── [+0x18]  Magic Regen (float32)
│   ├── [+0x24]  Rage (float32)
│   ├── [+0x28]  Rage Regen (float32)
│   ├── [+0x30]  Red Orbs (float32)
│   ├── [+0x38]  X position (float32)
│   ├── [+0x3C]  Height (float32)
│   ├── [+0x40]  Y position (float32)
│   ├── [+0x5E]  Swim state (0x50 = swimming, 0x08 = not)
│   ├── [+0xCC]  Frozen for cutscene (1 / 0)
│   ├── [+0xD7]  Inventory start (16-byte entries × N)
│   ├── [+0x2D7] Inventory count
│   ├── [+0x303] Gorgon Eyes
│   ├── [+0x307] Phoenix Feathers
│   └── [+0x317] Sub-weapon Ammo
└── [kptr2] Discovered via pattern {04 FF 00 00} − 4
    ├── [+0x17]  Difficulty (0–3)
    └── [+0x1B]  New Game+ (1 / 0)
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download)
- Git
- RPCS3 save data (unencrypted, in RPCS3's `dev_hdd0/home/<user-id>/savedata/` folder)

### Build & Run

```bash
# Clone
git clone https://github.com/argyris-mpalampanisb/RPCS3-God-Of-War-Save-Editor-CSharp.git
cd RPCS3-God-Of-War-Save-Editor-CSharp/GodOfWarCSharpSE

# Build
dotnet build -c Release

# Run
dotnet run
```

### Publish (self-contained, no runtime required)

```bash
# Windows x64
dotnet publish -c Release -r win-x64 --self-contained -o publish/win

# macOS (Intel)
dotnet publish -c Release -r osx-x64 --self-contained -o publish/mac

# macOS (Apple Silicon)
dotnet publish -c Release -r osx-arm64 --self-contained -o publish/mac-arm

# Linux x64
dotnet publish -c Release -r linux-x64 --self-contained -o publish/linux

# Linux ARM64
dotnet publish -c Release -r linux-arm64 --self-contained -o publish/linux-arm
```

Each output folder contains the app and everything it needs to run; copy the whole folder to the target machine.
For a single-file build, add `-p:PublishSingleFile=true`.

> The `publish/` folder is git-ignored. Ready-made builds are attached to the
> [Releases](https://github.com/argyris-mpalampanisb/RPCS3-God-Of-War-Save-Editor-CSharp/releases) page.

---

## 📂 Project Structure

```
GodOfWarCSharpSE/
├── GodOfWarCSharpSE.slnx
└── GodOfWarCSharpSE/
    ├── GodOfWarCSharpSE.csproj
    ├── Program.cs
    ├── App.axaml / App.axaml.cs
    ├── ViewLocator.cs
    ├── Views/
    │   ├── MainWindow.axaml(.cs)
    │   ├── GoW1View.axaml(.cs)
    │   ├── GoW2View.axaml(.cs)
    │   ├── GoW3View.axaml(.cs)
    │   └── GoWAView.axaml(.cs)
    ├── ViewModels/
    │   ├── ViewModelBase.cs
    │   ├── MainViewModel.cs
    │   ├── MainWindowViewModel.cs
    │   ├── GoW1ViewModel.cs
    │   ├── GoW2ViewModel.cs
    │   ├── GoW3ViewModel.cs
    │   ├── GoWAViewModel.cs
    │   └── InventoryItem.cs
    ├── Models/
    │   ├── SaveFile.cs
    │   ├── GoW1Save.cs
    │   ├── GoW1Checkpoints.cs
    │   ├── GoW2Save.cs
    │   ├── GoW3Save.cs
    │   ├── GoWASave.cs
    │   └── Checkpoint.cs
    ├── Converters/
    │   ├── IntToRadioConverter.cs
    │   └── InverseBoolConverter.cs
    └── Data/
        ├── G1BlankSave.BIN
        ├── G2BlankSave.BIN
        ├── Gow1Movies.txt
        └── Gow2Movies.txt
```

---

## 🔧 Usage

1. **Launch the app**: you'll see four game buttons.
2. **Select a game**: the editor opens and the launcher hides. Closing the editor brings the launcher back; closing the launcher exits the app.
3. **Browse** to your RPCS3 save folder:
   - GoW1 / GoW2: the folder containing `MASTER.BIN`
   - GoW3: the folder containing `SAVEDATA`
   - Ascension: the folder containing `DAT-1` … `DAT-31`
4. **Open**: GoW1/GoW2 load `MASTER.BIN` first; pick **Slot 1–4** (GoW2: **1–10**) to edit a save slot. Ascension loads slot 1, or the first valid slot if slot 1 is empty.
5. **Edit**: change values, toggle checkboxes, select checkpoints.
6. **Save**: writes back to disk, recomputing checksums where the game needs them.

In GoW1, GoW2 and Ascension, the status line at the bottom of the editor tells you what was loaded or saved, and why something failed.

> ⚠️ **Always back up your saves before editing.** Incorrect values can corrupt your file.
>
> Saves copied from a real PS3 are encrypted (they come with a `PARAM.PFD`). Decrypt them first,
> e.g. with Apollo Save Tool, before opening them here.

---

## 📷 Screenshots

_Coming soon._

---

## 📜 Changelog

| Version | Date | Changes |
|:--------|:-----|:--------|
| 1.0.1 | 2026-11-17 | App now exits when the launcher is closed (editors no longer leave a hidden window running) · GoW1/GoW2 slot buttons and editor tabs fixed (only the Master page was reachable) · Ascension editor wired up, ported from ReactiveUI to CommunityToolkit.Mvvm (fixes crash on open) · Ascension load/save aligned with the original editor: empty `DAT-1` no longer blocks other slots, correct flag values written, Amulet values saved, sub-weapon / unlock / element-selection inventory sync fixed, culture-safe number parsing · Status line with load/save errors in GoW1, GoW2 and Ascension · Slot files are also searched in neighbouring save folders · Smaller font in all editors for readability · Removed incompatible `Avalonia.ReactiveUI` package |
| 1.0.0 | 2026-11-16 | Initial release: full port of GoWSaveEdit to .NET 11 / Avalonia |

---

## 🙏 Credits

- **Original author:** [Wulf2k](https://github.com/Wulf2k): save format research, offset tables, and original editor logic
- **This rewrite:** argyris-mpalampanisb
- **UI Framework:** [Avalonia UI](https://avaloniaui.net)
- **MVVM:** [CommunityToolkit.Mvvm](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/)

---

## 📄 License

MIT: do whatever you want, just keep the license header.
