# HZ-Changer
# 🖥️ Refresh Rate Changer (Hz Changer)

A lightweight, high-performance Windows utility built in **C#** that allows users to instantly switch their monitor's refresh rate between the maximum supported frequency and the standard 60Hz.

![C#](https://img.shields.io/badge/Language-C%23-blue)
![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey)
![Win32 API](https://img.shields.io/badge/API-Win32-red)

## 🚀 Overview

Many gaming laptops and monitors don't always default to their highest refresh rate, or users might want to drop to 60Hz to save battery life. **Refresh Rate Optimizer** provides a simple "one-click" GUI to handle this without digging through deep Windows Display Settings.

## ✨ Features

* **Auto-Detection:** Scans hardware to find the absolute maximum refresh rate supported by your current resolution.
* **Quick Toggle:** Instant buttons to set Max Hz or Reset to 60Hz.
* **Win32 Integration:** Uses `user32.dll` (`EnumDisplaySettings` and `ChangeDisplaySettings`) for low-level, reliable display manipulation.
* **Safety First:** Only applies modes that your monitor officially reports as supported.

## 🛠️ Technical Details

The application interacts directly with the Windows OS using **P/Invoke** (Platform Invocation Services). 

### Key Components:
* **`EnumDisplaySettings`**: Iterates through all display modes supported by the graphics card and monitor.
* **`ChangeDisplaySettings`**: Updates the Windows Registry and applies the new frequency in real-time.
* **`DEVMODE` Struct**: A precisely mapped structure to handle display device data in memory.

## 📥 Downloads

You can download the latest compiled version (executable) directly from the **[Releases](https://github.com/Ali-Iyad-Durra/HZ-Changer/releases/tag/Hz)** page. Just download the `.exe` and Install it

## 🏗️ How to Build (For Developers)

1. Open the project in **Visual Studio**.
2. Ensure you have the **.NET Desktop Development** workload installed.
3. Build the solution as **Release**.
4. The `.exe` will be generated in the `bin/Release` folder.

## 📜 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.
