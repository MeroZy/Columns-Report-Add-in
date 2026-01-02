using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Columns_Report_Add_in.Revit
{
    public class ExtApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication uicApp)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uicApp)
        {
            string tabName = "KAITECH-BD-R09";
            string panelName = "Structure";

            try
            {
                uicApp.CreateRibbonTab(tabName);
            }
            catch (Exception) { }

            RibbonPanel panel = uicApp.GetRibbonPanels(tabName)
                                     .FirstOrDefault(p => p.Name == panelName);

            if (panel == null)
            {
                panel = uicApp.CreateRibbonPanel(tabName, panelName);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            PushButtonData pb_Data = new PushButtonData(
                "Export",
                "Columns Report",
                assembly.Location,
                "Columns_Report_Add_in.ExtCmd"
            );

            PushButton pb = panel.AddItem(pb_Data) as PushButton;
            pb.ToolTip = "This is Columns Report Add-in Developed By Amr Khaled";
            pb.LargeImage = GetImageSource(Properties.Resources.icons8_report_94);
            
            return Result.Succeeded;
        }
        public ImageSource GetImageSource(System.Drawing.Bitmap bitmap) //gpt help
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;
                PngBitmapDecoder decoder = new PngBitmapDecoder(ms, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return decoder.Frames[0];
            }
        }
    }
}
