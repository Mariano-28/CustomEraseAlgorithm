# 🔐 CustomEraser - Secure File Deletion Algorithm

**Author:** Mariano Ortu  
**Official Website:** [https://www.sicurpas.it](https://www.sicurpas.it)  
**License:** [GNU GPL v3 or later](https://www.gnu.org/licenses/gpl-3.0.html)  
**SPDX-License-Identifier:** `GPL-3.0-or-later`

---

## 📦 Overview

**CustomEraser** is an advanced algorithm for **secure file deletion**, designed to permanently and irreversibly erase data from traditional and solid-state drives.

The package includes:

- ✅ Complete source code of the algorithm in **C#**
- ✅ Visual Studio project to build the **DLL** (`CustomEraser.dll`)
- ✅ A working sample application (`CustomTest`) demonstrating how to use the DLL to erase files of any type and size
- ✅ **SHA-256 HASH** verification and **digital signature** to ensure file authenticity and integrity

---

## 🛠️ Build Instructions

### Requirements

- Microsoft Visual Studio 2019 or later
- .NET Framework 4.8

### Steps

1. Open the `.sln` file located in the main folder of the project.
2. Build the solution in **Release** mode.
3. The `CustomEraser.dll` will be generated inside the `bin\Release` directory.

---

## 💡 Usage Example

The provided demo (`CustomTest`) shows how to:

- Import and reference the DLL in a C# application
- Pass the path of the file to be securely erased
- Handle the result and notify the user of successful deletion

All source code is fully documented and ready to be integrated into your own solutions.

---

## 🔒 Security and Verification

Each official release includes:

- A `.sig` file containing the **PGP digital signature**
- A `.sha256` file containing the **SHA-256 checksum**

These files allow verification of integrity and authenticity of the downloaded materials.

---

## 📄 License

This project is released under the terms of the **GNU General Public License v3.0 or later**.

Please refer to the [`LICENSE`](./LICENSE) file for full license details.

---

## 📥 Download

You can download the full package from the official GitHub repository:  
[https://github.com/Mariano-28/CustomEraseAlgorithm](https://github.com/Mariano-28/CustomEraseAlgorithm)

---

## 🧑‍💻 Contact

**Mariano Ortu**  
Website: [https://www.sicurpas.it](https://www.sicurpas.it)  
Email: `sicurpas@sicurpas.it`

---

> *"Secure erasure is a right for those who work with responsibility. CustomEraser exists to guarantee that right."*
