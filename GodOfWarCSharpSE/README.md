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
| **God of War 1** | `MASTER.BIN` + `DATA0{0–3}.BIN` | 4 | Checkpoint warping (~90 locations), movie skip, costume/difficulty, weapon levels |
| **God of War 2** | `MASTER.BIN` + `DATA0{0–3}.BIN` | 4 | Stats, weapons, checkpoints |
| **God of War 3** | `MASTER.BIN` + `DATA0{0–3}.BIN` | 4 | Stats, weapons, globals |
| **God of War: Ascension** | `DAT-1` … `DAT-31` | 31 | Dynamic inventory (153 items), sub-weapons, costumes, NG+, frozen/swim states |

---

## ✨ Features

- **Cross-platform** — Single codebase, native feel on Windows / macOS / Linux
- **RPCS3-only** — Works directly on decrypted save files (no PS3 crypto)
- **Big-endian aware** — Correctly handles PS3 byte ordering via `BitConverter` + `Array.Reverse`
- **Dynamic offset discovery** — Ascension uses byte-pattern search (`kptr` / `kptr2`) for variable-offset saves
- **Checksum validation** — GoW1 save integrity checksum computed and written on save
- **Movie skip** — Set all cinematics to "seen" with one click (GoW1)
- **Checkpoint warping** — Predefined WAD/camera/position presets for GoW1 (90+ locations across 19 categories)
- **Inventory editor** — Full 153-item checklist for Ascension with weapon/sub-weapon/costume sync
- **Range validation** — UI highlights out-of-range values in red
- **Dirty-state tracking** — Warns before switching slots with unsaved changes
- **MVVM architecture** — Clean separation of model / view-model / view via ReactiveUI

---

## 🏗️ Architecture
 

┌─────────────────────────────────────────────────────────┐
│ Views (Avalonia XAML) │
│ ├── MainWindow.axaml (launcher) │
│ ├── GoW1View.axaml (GoW1 editor) │
│ ├── GoW2View.axaml (GoW2 editor) │
│ ├── GoW3View.axaml (GoW3 editor) │
│ └── GoWAView.axaml (Ascension editor) │
├─────────────────────────────────────────────────────────┤
│ ViewModels (ReactiveUI) │
│ ├── MainViewModel.cs │
│ ├── GoW1ViewModel.cs │
│ ├── GoW2ViewModel.cs │
│ ├── GoW3ViewModel.cs │
│ └── GoWAViewModel.cs │
├─────────────────────────────────────────────────────────┤
│ Models (pure C#, no UI dependencies) │
│ ├── GoW1Save.cs — Load/Save/Checksum │
│ ├── GoW2Save.cs │
│ ├── GoW3Save.cs │
│ ├── GoWASave.cs — kptr discovery + inventory │
│ └── Checkpoint.cs — record struct for warp data │
├─────────────────────────────────────────────────────────┤
│ Converters / Utilities │
│ ├── IntToRadioConverter.cs │
│ ├── InverseBoolConverter.cs │
│ └── (byte-level read/write helpers) │
└─────────────────────────────────────────────────────────┘


### Tech Stack

| Layer | Technology |
|:------|:-----------|
| Runtime | .NET 11 (LTS) |
| Language | C# 15 |
| UI Framework | Avalonia 12 (cross-platform XAML) |
| MVVM | ReactiveUI (compiled bindings) |
| Byte I/O | `BitConverter` + `Array.Reverse` (big-endian) |
| Build | .NET SDK / MSBuild |
| IDE | Visual Studio 2026 Community / VS Code |

---

## 📁 Save File Formats

### GoW 1 / 2 / 3
 

MASTER.BIN (68+ bytes)
├── [0x04 + 16×slot] 0xCAFEBAD1 marker
├── [0x08 + 16×slot] Playtime (float32 BE)
├── [0x0D + 16×slot] Active flag
├── [0x0E + 16×slot] Difficulty (byte)
└── [0x43] Global flags (beat game / VH / CotG / statues)

DATA0x.BIN (~1064 bytes)
├── [0x00] 0xCA header
├── [0x08–0x0F] WAD1 name (8×ASCII)
├── [0x12] WAD1 active
├── [0x13–0x1A] WAD2 name (8×ASCII)
├── [0x1D] WAD2 active
├── [0x4E–0x59] X/Y/Z position (float32 BE × 3)
├── [0x60] Swim state
├── [0x6A] Seconds played (float32)
├── [0x76–0x85] Health / Magic / Rage / Regen (float32 × 4)
├── [0x86] Red orbs (uint16 BE)
├── [0x94–0x9B] Extensions + weapon levels
├── [0x9C] Selected weapon
├── [0x9D–0x9F] Gorgon eyes / Phoenix feathers / Muse keys
├── [0xA0] Bit 0: PT
├── [0xA1] Bits 2–6: weapon unlocked flags
├── [0xAE–0xC5] Current camera (24×ASCII)
├── [0xC6–0xD5] Camera WAD (24×ASCII)
├── [0x28A] Movies seen (int16)
├── [0x28E+] Movie checksums (uint32 × N)
├── [0x41E–0x428] World idle / volumes / flags / costume / difficulty
└── [end−4] Checksum (uint32)


### God of War: Ascension
 

DAT-{1–31} (variable length, big-endian)
├── [0x00] 0xC0 header
├── [kptr] Discovered via pattern {00 00 03 24}
│ ├── [+0x0C] Health (float32)
│ ├── [+0x10] Health Regen (float32)
│ ├── [+0x14] Magic (float32)
│ ├── [+0x18] Magic Regen (float32)
│ ├── [+0x24] Rage (float32)
│ ├── [+0x28] Rage Regen (float32)
│ ├── [+0x30] Red Orbs (float32)
│ ├── [+0x38–0x43] X/Y/Z position (float32 × 3)
│ ├── [+0x5E] Swim state (0x50 = swimming)
│ ├── [+0xCC] Frozen (1 = frozen)
│ ├── [+0xD7] Inventory start (16-byte entries × N)
│ ├── [+0x2D7] Inventory count
│ ├── [+0x303] Gorgon Eyes
│ ├── [+0x307] Phoenix Feathers
│ └── [+0x317] Sub-weapon Ammo
└── [kptr2] Discovered via pattern {04 FF 00 00} − 4
├── [+0x17] Difficulty
└── [+0x1B] New Game+


---

## 🚀 Getting Started

### Prerequisites

- [.NET 11 SDK](https://dotnet.microsoft.com/download)
- Git
- RPCS3 save data (decrypted, in your `dev_hdd0/user/10000xxx/savedata/` folder)

### Build & Run

```bash
# Clone
git clone https://github.com/argyris-mpalampanisb/RPCS3-God-Of-War-Save-Editor-CSharp.git
cd RPCS3-God-Of-War-Save-Editor-CSharp

# Build
dotnet build -c Release

# Run
dotnet run
 

Publish (self-contained, no runtime required)

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
 

Each output folder contains a single executable — just copy it to the target machine.
📂 Project Structure

GodOfWarCSharpSE/
├── GodOfWarCSharpSE.sln
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
    │   ├── MainViewModel.cs
    │   ├── GoW1ViewModel.cs
    │   ├── GoW2ViewModel.cs
    │   ├── GoW3ViewModel.cs
    │   ├── GoWAViewModel.cs
    │   └── InventoryItem.cs
    ├── Models/
    │   ├── GoW1Save.cs
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
 

🔧 Usage

    Launch the app — you'll see four game buttons.

    Select a game — the editor window opens.

    Browse to your RPCS3 save folder (the one containing MASTER.BIN or DAT-1).

    Open — the save loads and all fields populate.

    Edit — change values, toggle checkboxes, select checkpoints.

    Save — writes back to disk with correct checksums.

    ⚠️ Always back up your saves before editing. Incorrect values can corrupt your file.

📷 Screenshots
📜 Changelog
Version	Date	Changes
1.0.0	2026-11-16	Initial release — full port of GoWSaveEdit to .NET 11 / Avalonia
🙏 Credits

    Original author: Wulf2k — save format research, offset tables, and original editor logic

    This rewrite: argyris-mpalampanisb

    UI Framework: Avalonia UI

    MVVM: ReactiveUI

📄 License

MIT — do whatever you want, just keep the license header.