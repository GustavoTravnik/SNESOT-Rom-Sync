using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace SNESOT_Sync
{
    public partial class Principal : Form
    {
        public static String ROM_DIRECTORY = Environment.CurrentDirectory + "\\Roms";
        public static String EMU_DIRECTORY = Environment.CurrentDirectory + "\\Emu";
        List<String> localRomsList = new List<string>();
        const String ONLINE_ROMS_LOCATION = "http://soniclethal.esy.es/Roms/";
        const String ONLINE_ROMS_LIST_LOCATION = "http://soniclethal.esy.es/Roms/list.txt";
        const String SNES_CONFIG_FILE = "zsnesw.cfg";
        public volatile List<Rom> romsParaSeremBaixadas = new List<Rom>();
        
        public Principal()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            FillEmuVersions();
            FillLocalRomsList();
            LoadRoms();
        }

        private String GetRomNameByRom(Rom r)
        {
            return r.pathToDownload.Split('/')[r.pathToDownload.Split('/').Length - 1];
        }

        private String RemoveExtension(String s)
        {
            return s.Remove(s.Length - 4, 4);
        }

        private void LoadRoms()
        {
            try
            {
                panel.Controls.Clear();
                WebClient wb = new WebClient();
                wb.Headers.Add("User-Agent", "Mozilla/4.0+");
                String[] listaDeRoms = wb.DownloadString(ONLINE_ROMS_LIST_LOCATION).Replace("\n", "").Split('|');

                DirectoryInfo romDirectory = new DirectoryInfo(ROM_DIRECTORY);
                foreach (FileInfo f in romDirectory.GetFiles("*png", SearchOption.TopDirectoryOnly))
                {
                    Boolean baixado = false;
                    String fileName = "";
                    foreach (String s in listaDeRoms)
                    {
                        if (RemoveExtension(f.Name).Equals(s))
                        {
                            baixado = true;
                            fileName = s;
                            break;
                        }
                    }
                    Image image;
                    using (FileStream myStream = new FileStream(f.FullName, FileMode.Open, FileAccess.Read))
                    {
                        image = Image.FromStream(myStream);
                    }
                    Rom rom = new Rom(image, f, baixado, ONLINE_ROMS_LOCATION + "/" + fileName);
                    panel.Controls.Add(rom);
                }

                foreach (String s in listaDeRoms)
                {
                    Boolean existe = false;
                    foreach (Rom r in panel.Controls)
                    {
                        if ((r.file != null ? RemoveExtension(r.file.Name) : GetRomNameByRom(r)).Equals(s))
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (!existe)
                    {
                        Rom rom = new Rom(null, null, false, ONLINE_ROMS_LOCATION + "/" + s);
                        panel.Controls.Add(rom);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                button1.Enabled = true;
                panelSync.Visible = false;
                panelSync.BringToFront();
                button3.Enabled = true;
            }
        }

        private void FillEmuVersions()
        {
            comboVersion.Items.Clear();
            foreach (DirectoryInfo d in new DirectoryInfo(EMU_DIRECTORY).GetDirectories())
            {
                comboVersion.Items.Add(d.Name);
                StreamReader sr = new StreamReader(EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE);
                StreamWriter sw = new StreamWriter(EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE + ".tmp");
                while (!sr.EndOfStream)
                {
                    String line = sr.ReadLine();
                    if (line.Contains("GameDirectory"))
                    {
                        sw.WriteLine("GameDirectory = " + ROM_DIRECTORY.ToUpper());
                    }
                    else
                    {
                        sw.WriteLine(line);
                    }
                }
                sr.Close();
                sw.Close();

                File.Delete(EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE);
                File.Copy(EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE + ".tmp", EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE);
                File.Delete(EMU_DIRECTORY + "\\" + d.Name + "\\" + SNES_CONFIG_FILE + ".tmp");

            }
        }

        private void FillLocalRomsList()
        {
            localRomsList.Clear();
            foreach (FileInfo f in new DirectoryInfo(ROM_DIRECTORY).GetFiles())
            {
                localRomsList.Add(f.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboVersion.Items.Contains(comboVersion.Text))
            {
                Process proc = new Process();
                proc.StartInfo.FileName = EMU_DIRECTORY + "\\" + comboVersion.Text + "\\" + "ZSNESW.EXE";
                proc.StartInfo.WorkingDirectory = EMU_DIRECTORY + "\\" + comboVersion.Text;
                proc.Start();
            }
            else
            {
                MessageBox.Show("Selecione Um Item Válido");
            }
        }

        private readonly Object LOCKER = new Object();

        private void SyncRoms()
        {
            lock (LOCKER)
            {
                foreach (Rom r in romsParaSeremBaixadas)
                {
                    WebClient wb = new WebClient();
                    wb.Headers.Add("User-Agent", "Mozilla/4.0+");

                    lblGameName.Text = r.pathToDownload.Split('/')[r.pathToDownload.Split('/').Length - 1];
                    if (File.Exists(ROM_DIRECTORY + "\\" + GetRomNameByRom(r) + ".sfc"))
                    {
                        File.Delete(ROM_DIRECTORY + "\\" + GetRomNameByRom(r) + ".sfc");
                    }
                    wb.DownloadFile(r.pathToDownload + ".sfc", ROM_DIRECTORY + "\\" + GetRomNameByRom(r) + ".sfc");
                    wb.DownloadFile(r.pathToDownload + ".png", ROM_DIRECTORY + "\\" + GetRomNameByRom(r) + ".png");

                    progress.Value++;
                }
                progress.Value = 0;
                Invoke(new MethodInvoker(LoadRoms));
                MessageBox.Show("Todas as roms sincronizadas!");
            }
        }

        private void RemoverDesmarcados()
        {
            foreach (Rom r in panel.Controls)
            {
                if (!r.checkedRom.Checked)
                {
                    List<FileInfo> listaJaBaixada = new DirectoryInfo(ROM_DIRECTORY).GetFiles().ToList();
                    foreach (FileInfo f in listaJaBaixada)
                    {
                        if (RemoveExtension(f.Name).Equals(GetRomNameByRom(r)))
                        {
                            r.pic.Image = null;
                            File.Delete(RemoveExtension(f.FullName) + ".sfc");
                            File.Delete(RemoveExtension(f.FullName) + ".png");
                            break;
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            panelSync.Visible = true;
            panelSync.BringToFront();
            panelSync.Location = new Point(12, 225);
            romsParaSeremBaixadas = new List<Rom>();
            List<FileInfo> listaJaBaixada = new DirectoryInfo(ROM_DIRECTORY).GetFiles().ToList();
            RemoverDesmarcados();
            foreach (Rom r in panel.Controls)
            {
                if (r.checkedRom.Checked)
                {
                    Boolean jaBaixada = false;

                    foreach (FileInfo f in listaJaBaixada)
                    {
                        if (RemoveExtension(f.Name).Equals(GetRomNameByRom(r)))
                        {
                            jaBaixada = true;
                            break;
                        }
                    }

                    if (!jaBaixada)
                        romsParaSeremBaixadas.Add(r);
                }
            }
            
            progress.Maximum = romsParaSeremBaixadas.Count;
            progress.Value = 0;

            Thread sync = new Thread(SyncRoms);
            sync.IsBackground = true;
            sync.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Enabled = false;
            foreach (Rom r in panel.Controls)
            {
                r.checkedRom.Checked = true;
            }
            button1_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("http://snesot.com");
        }
    }
}
