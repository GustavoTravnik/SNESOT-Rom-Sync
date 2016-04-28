using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SNESOT_Sync
{
    public partial class Rom : UserControl
    {
        public FileInfo file;
        public String pathToDownload;

        public Rom(Image img, FileInfo file, Boolean baixado, String pathToDownload)
        {
            InitializeComponent();
            this.pathToDownload = pathToDownload;
            if (file != null)
            {
                this.file = file;
                file = new FileInfo(file.FullName.Replace("png", "sfc"));
                lblName.Text = file.Name;
            }
            else
            {
                lblName.Text = pathToDownload.Split('/')[pathToDownload.Split('/').Length - 1];
            }
            if (img != null)
            {
                pic.Image = img;
            }
            checkedRom.Checked = baixado;
        }
    }
}
