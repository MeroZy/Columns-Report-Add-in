using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Columns_Report_Add_in.Revit;
using Columns_Report_Add_in.UI;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Columns_Report_Add_in
{
    [TransactionAttribute(TransactionMode.ReadOnly)]
    public class ExtCmd : IExternalCommand
    {
        public static UIDocument uidoc { get; set; }
        public static Document doc { get; set; }
        public static Columns_Report Mainform { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;
            //MAIN
            Mainform = new Columns_Report();
            Mainform.Show();

            //RVTUtlis.Run();
            //TaskDialog.Show("Test",RVTUtlis.sb.ToString()); //for test

            return Result.Succeeded;
        }
        
    }
}
