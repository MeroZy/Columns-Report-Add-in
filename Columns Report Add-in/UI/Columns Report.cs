using Columns_Report_Add_in.Revit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Columns_Report_Add_in.UI
{
    public partial class Columns_Report : Form
    {
        public Columns_Report()
        {
            InitializeComponent();
        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.linkedin.com/in/amrkhaled2/");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            RVTUtlis.Run();
        }

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Choose Output Folder";
                folderDialog.ShowNewFolderButton = true;

                if (!string.IsNullOrEmpty(Properties.Settings.Default.LastFolderDialogPath))
                {
                    folderDialog.SelectedPath = Properties.Settings.Default.LastFolderDialogPath;
                }

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    tb_location.Text = folderDialog.SelectedPath;

                    // Save this folder for next time
                    Properties.Settings.Default.LastFolderDialogPath = folderDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
