# Columns Report – Revit Add-in

A Revit add-in that exports detailed structural column data to Excel, providing accurate geometric, level, and volume information in a clean and readable format.

---

## 🔍 Overview

**Columns Report** is designed to automate the extraction of structural column data from Revit projects.  
It generates a fully formatted Excel file containing all essential parameters needed for BIM analysis, coordination, and quantity takeoffs.

---

## ✨ Features

- Exports **Family & Type** for each column
- Retrieves **Revit Element ID**
- Calculates **Easting & Northing** coordinates
- Extracts:
  - Base Level
  - Top Level
  - Base Offset
  - Top Offset
- Computes **Column Height**
- Computes **Volume (m³)**
- Automatically formats Excel sheets for readability
- Opens the Excel file automatically after export

---

## 🧩 Supported Revit Versions

- Revit 2020  
- Revit 2021  
- Revit 2022  
- Revit 2023  
- Revit 2024  

---

## ⚙️ Installation

1. Run **Columns Report Add-in Setup**.
2. The installer automatically copies:
   - `.dll`
   - `.addin`
   files to the correct Revit Add-ins folders.
3. Launch Revit.
4. The **Columns Report** button will appear under the **Structure** panel.

To uninstall, use Windows **Apps & Features**.

---

## 🧪 How to Use

1. Open a Revit project containing structural columns.
2. Click **Columns Report** from the ribbon.
3. Choose an output folder.
4. The add-in exports all structural columns to Excel.
5. The Excel file opens automatically after creation.

---

## ⚠️ Notes

- Works only with **Structural Columns**  
  (`OST_StructuralColumns`)
- Uses **metric units**:
  - Meters for coordinates
  - Cubic meters for volume
- No geometry is modified
- Revit should be closed during installation

---

## 👤 Author

**Amr Khaled**  
Computational Architect  

- LinkedIn: https://www.linkedin.com/in/amrkhaled2/
- Email: mero.mero203512@gmail.com

---

## 📄 License

Released for educational and professional use.  
Contributions and suggestions are welcome.
