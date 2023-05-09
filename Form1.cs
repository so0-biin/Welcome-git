using FileManager.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Form1 : Form
    {
        private Explorer FilesListView;

        public Form1()
        {
            InitializeComponent();



            FilesListView = new Explorer();
            FilesListView.Initialize();
            SplitContainer.Panel2.Controls.Add(FilesListView);

            InitializeViewOptions();

            textBox1.ReadOnly = true;
            button1.Text = "git init";
            button2.Text = "git commit";

        }

        private void NavigationPanel_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FilesListView.Items.Clear();
            FilesListView.LargeImageList.Images.Clear();
            FilesListView.SmallImageList.Images.Clear();

            string path = (string)e.Node.Tag;
            try
            {
                FilesListView.ShowFiles(path);
                FilesListView.ShowDirectories(path);
            }
            catch
            {

            }

            FilesListView.SelectedNavigationNode = e.Node;

            this.CurrentDirectory.Text = path;
        }

        private void InitializeViewOptions()
        {
            ViewDetails.Tag = View.Details;
            ViewLargeIcons.Tag = View.LargeIcon;
            ViewList.Tag = View.List;
            ViewSmallIcons.Tag = View.SmallIcon;
            ViewTiles.Tag = View.Tile;

            EventHandler viewOptionClickHandler = new EventHandler(ViewOptions_Click);
            ViewDetails.Click += viewOptionClickHandler;
            ViewLargeIcons.Click += viewOptionClickHandler;
            ViewList.Click += viewOptionClickHandler;
            ViewSmallIcons.Click += viewOptionClickHandler;
            ViewTiles.Click += viewOptionClickHandler;
        }

        private void ViewOptions_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem ViewOption = (sender as ToolStripMenuItem);
            if (ViewOption.Checked == true)
                return;

            foreach (ToolStripMenuItem item in this.ViewMenuItem.DropDownItems)
            {
                if (item.Checked == true)
                {
                    item.Checked = false;
                }
            }

            ViewOption.Checked = true;

            switch ((View)ViewOption.Tag)
            {
                case View.Details:
                    FilesListView.View = View.Details;
                    break;

                case View.LargeIcon:
                    FilesListView.View = View.LargeIcon;
                    if (FilesListView.LargeImageList.ImageSize != FilesListView.LargeIconSize)
                    {
                        FilesListView.LargeImageList.ImageSize = FilesListView.LargeIconSize;

                        // change in Imagesize removes elements of LargeImageList.Images, thus we need the following
                        FilesListView.UpdateIconImages();
                    }
                    break;

                case View.List:
                    FilesListView.View = View.List;
                    break;

                case View.SmallIcon:
                    FilesListView.View = View.SmallIcon;
                    break;

                case View.Tile:
                    FilesListView.View = View.Tile;
                    if (FilesListView.LargeImageList.ImageSize != FilesListView.TileIconSize)
                    {
                        FilesListView.LargeImageList.ImageSize = FilesListView.TileIconSize;

                        // change in Imagesize removes elements of LargeImageList.Images, thus we need the following
                        FilesListView.UpdateIconImages();
                    }
                    break;
            }
        }

        private void SearchMenuItem_Click(object sender, EventArgs e)
        {
            SearchDialog searchDialog = new SearchDialog();
            searchDialog.CurrentDirectory = this.CurrentDirectory.Text;
            searchDialog.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //http://redreans.tistory.com/58
            
            
            // cmd를 사용하기 위한 준비
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            String directoryPath;
            cmd.FileName = @"cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;             // cmd창이 숨겨지도록 하기
            cmd.CreateNoWindow = true;                               // cmd창을 띄우지 안도록 하기

            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;        // cmd창에서 데이터를 가져오기
            cmd.RedirectStandardInput = true;          // cmd창으로 데이터 보내기
            cmd.RedirectStandardError = true;          // cmd창에서 오류 내용 가져오기

            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;


            // cmd 다루기
            StringBuilder sb = new StringBuilder();
            sb.Append(this.CurrentDirectory.Text);
            directoryPath = sb.ToString();
            
            process.Start(); // cmd 명령 입히는거 시작                     
            process.StandardInput.Write(@"cd " + directoryPath + Environment.NewLine);
            StringBuilder sb2 = new StringBuilder();
            process.StandardInput.Write(@"git init" + Environment.NewLine);

            //fileType = DetectFileType(directoryPath.GetExtension(((string)item.Tag)).Substring(1).ToLower());

            if (directoryPath.Length >= 1 && Directory.Exists(directoryPath + "\\.git")) {
                //StreamReader reader = process.StandardOutput;
                //string output = reader.ReadToEnd();
                //textBox1.Text = output;
                textBox1.Text = "Reinitialized existing Git repository in " + directoryPath + "\\.git\\";
            }
            else if (directoryPath.Length == 0)
            {
                textBox1.Text = "Choose directory to initialize empty Git repository.";
            }

            else
            {               
                textBox1.Text = "Initialized empty Git repository in " + directoryPath + "\\.git\\";
            }
            
            process.StandardInput.Close(); // cmd  명령 입력 끝
            

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음
            
            // git init file Redirection
            FilesListView.Items.Clear();
            try
            {
                FilesListView.ShowFiles(directoryPath);
                FilesListView.ShowDirectories(directoryPath);
            }
            catch
            {

            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }


        private void Button1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "Check please";
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.button1, "Check exactly that the directory you want to initialize is correct.");
        }
    }
}