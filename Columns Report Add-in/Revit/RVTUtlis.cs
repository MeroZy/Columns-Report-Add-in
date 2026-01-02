
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Columns_Report_Add_in.Revit
{
    public class RVTUtlis
    {
        public static StringBuilder sb { get; set; }
        public static void Run()
        {
            sb = new StringBuilder();
            //Vars
            List<string> x_Family = new List<string>(); //
            List<string> x_Type = new List<string>(); //

            List<int> x_ID = new List<int>(); //

            List<double> x_Easting = new List<double>(); //
            List<double> x_Northing = new List<double>(); //

            List<string> x_BaseLevel = new List<string>(); //
            List<double> x_BaseOffset = new List<double>(); //

            List<string> x_TopLevel = new List<string>(); //
            List<double> x_TopOffset = new List<double>(); //

            List<double> x_Height = new List<double>(); //
            List<double> x_Volume = new List<double>(); //


            var Cols = Columns_Collector(ExtCmd.doc);
            foreach (var col in Cols)
            {
                FamilyInstance fi = col as FamilyInstance;
                if (fi != null)
                {

                    x_Family.Add(fi.Symbol.Family.Name);
                    x_Type.Add(fi.Symbol.Name);
                    x_ID.Add((int)fi.Id.Value);

                    //x_Northing.Add(fi.Location.ToString);
                    LocationPoint lp = fi.Location as LocationPoint;
                    XYZ p = lp.Point;

                    double easting = ConvertUnit_m(p.X);
                    double northing = ConvertUnit_m(p.Y);

                    x_Easting.Add(easting);
                    x_Northing.Add(northing);

                    // base and top leve
                    Parameter baseLevel_p = fi.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);
                    Parameter topLevel_p = fi.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
                    Parameter baseoffset_p = fi.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM);
                    Parameter topoffset_p = fi.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM);
                    Parameter heightParam = fi.get_Parameter(BuiltInParameter.FAMILY_HEIGHT_PARAM);
                    Parameter volume_p = fi.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);

                    Level baseLevel = ExtCmd.doc.GetElement(baseLevel_p.AsElementId()) as Level;
                    Level topLevel = ExtCmd.doc.GetElement(topLevel_p.AsElementId()) as Level;

                    x_BaseLevel.Add(baseLevel.Name);
                    x_TopLevel.Add(topLevel.Name);

                    double baseoffset = ConvertUnit_m(baseoffset_p.AsDouble());
                    double topoffset = ConvertUnit_m(topoffset_p.AsDouble());

                    x_BaseOffset.Add(baseoffset);
                    x_TopOffset.Add(topoffset);

                    //calc height
                    double height;
                    if (heightParam != null)
                    {
                        height = heightParam.AsDouble();
                    }
                    else
                    {
                        double bottom = ConvertUnit_m(baseLevel.Elevation);
                        double top = ConvertUnit_m(topLevel.Elevation);

                        height = (top + topoffset) - (bottom + baseoffset);
                    }
                    x_Height.Add(height);

                    x_Volume.Add(ConvertUnit_m3(volume_p.AsDouble()));

                    sb.AppendLine($"{fi.Id.Value}        " +
                        $"{easting.ToString()}        {baseLevel.Name}    {topLevel.Name}    {height.ToString()}   " +
                        $"{ConvertUnit_m3(volume_p.AsDouble())}"); //for test
                }
            }
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Amr Khaled";
                excelPackage.Workbook.Properties.Title = "Columns Report";
                excelPackage.Workbook.Properties.Subject = "Export columns data";
                excelPackage.Workbook.Properties.Created = DateTime.Now;

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Columns");

                worksheet.Cells["A1"].Value = "Family";
                worksheet.Cells["B1"].Value = "Type";
                worksheet.Cells["C1"].Value = "ID";
                worksheet.Cells["D1"].Value = "Easting (m)";
                worksheet.Cells["E1"].Value = "Northing (m)";
                worksheet.Cells["F1"].Value = "Base Level";
                worksheet.Cells["G1"].Value = "Base Offset (m)";
                worksheet.Cells["H1"].Value = "Top Level";
                worksheet.Cells["I1"].Value = "Top Offset (m)";
                worksheet.Cells["J1"].Value = "Height (m)";
                worksheet.Cells["K1"].Value = "Volume (m³)";

                using (var headerRange = worksheet.Cells["A1:K1"])
                {
                    headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerRange.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.FromArgb(191, 191, 191));
                    headerRange.Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    headerRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
                

                //fill data
                for(int i = 0; i < x_ID.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = x_Family[i];
                    worksheet.Cells[i + 2, 2].Value = x_Type[i];
                    worksheet.Cells[i + 2, 3].Value = x_ID[i];
                    worksheet.Cells[i + 2, 4].Value = x_Easting[i];
                    worksheet.Cells[i + 2, 5].Value = x_Northing[i];
                    worksheet.Cells[i + 2, 6].Value = x_BaseLevel[i];
                    worksheet.Cells[i + 2, 7].Value = x_BaseOffset[i];
                    worksheet.Cells[i + 2, 8].Value = x_TopLevel[i];
                    worksheet.Cells[i + 2, 9].Value = x_TopOffset[i];
                    worksheet.Cells[i + 2, 10].Value = x_Height[i];
                    worksheet.Cells[i + 2, 11].Value = x_Volume[i];
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                using (var allCells = worksheet.Cells[worksheet.Dimension.Address]) //from gpt
                {
                    allCells.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    allCells.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }
                
                //RVTFileName-Columns Report [yyyy-mm-dd_hh-mm-ss XM]

                string RVTFileName = ExtCmd.doc.Title;
                string timenow = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss tt"); //gpt
                string filefullname = $"{RVTFileName}-Columns Report [{timenow}]";

                FileInfo fInfo = new FileInfo($"{ExtCmd.Mainform.tb_location.Text}\\{filefullname}.xlsx");
                excelPackage.SaveAs(fInfo);
                Process.Start(fInfo.ToString());
                MessageBox.Show("Excel file created", "Done");
            }
        }

        public static FilteredElementCollector Columns_Collector(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsNotElementType();
        }
        public static double ConvertUnit_m(double num)
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(num, UnitTypeId.Meters), 2);
        }
        public static double ConvertUnit_m3(double num) //for cubiv meter volume
        {
            return Math.Round(UnitUtils.ConvertFromInternalUnits(num, UnitTypeId.CubicMeters), 2);
        }
    }
}
