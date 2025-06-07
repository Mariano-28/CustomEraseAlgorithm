# 🔐 CustomEraser - Secure File Deletion Algorithm

**Author:** Mariano Ortu  
**Official Website:** [https://www.sicurpas.it](https://www.sicurpas.it)  
**License:** [GNU GPL v3 or later](https://www.gnu.org/licenses/gpl-3.0.html)  
**SPDX-License-Identifier:** `GPL-3.0-or-later`

---

## 📦 Overview

**CustomEraser** is an advanced algorithm for **secure file deletion**, designed to permanently and irreversibly erase data from traditional and solid-state drives.

The package includes:

- ✅ Complete source code of the algorithm
- ✅ Visual Studio project to build the **DLL**
- ✅ A working sample application demonstrating how to use the DLL to erase files of any type and size
- ✅ **SHA-256 HASH** verification and **digital signature** to ensure file authenticity and integrity

---

## 🛠️ Build Instructions

### Requirements

- Microsoft Visual Studio 2019 or later
- .NET Framework 4.8 (for the C# demo)
- C++17 standard or later

### Steps

1. Open the `.sln` file from the main project folder.
2. Build the solution in **Release** mode.
3. The `CustomEraser.dll` will be generated inside the `bin\Release` folder.

---

## 💡 Usage Example

The provided demo shows how to:

- Import the DLL into a C# application
- Pass the path of the file to be securely erased
- Handle the result and inform the user of successful deletion

The code is fully documented and ready to be integrated into your own applications.

---

## 🔒 Security and Verification

For each official release, the following are provided:

- A `.sig` file containing the **PGP digital signature**
- A `.sha256` file containing the **hash checksum**

These allow users to verify that the files have not been tampered with.

---

## 📄 License

This project is released under the terms of the **GNU General Public License v3.0 or later**.

Please refer to the [`LICENSE`](./LICENSE) file for full license details.

---

## 🧑‍💻 Contact

**Mariano Ortu**  
Website: [https://www.sicurpas.it](https://www.sicurpas.it)  
Email: *(sicurpas@sicurpas.it)*

---

> *"Secure erasure is a right for those who work with responsibility. CustomEraser exists to guarantee that right."*
