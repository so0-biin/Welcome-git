using FileManager.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
            splitContainer1.Panel1.Controls.Add(FilesListView);

            InitializeViewOptions();

            textBox1.ReadOnly = true;
            button1.Text = "Create git repository";
            button2.Text = "Commit";
            button3.Text = "Refresh";
            button4.Text = "Clone git repository";
            button5.Text = "History";

        }

        private void NavigationPanel_AfterSelect(object sender, TreeViewEventArgs e)
        {
            FilesListView.Items.Clear();
            FilesListView.LargeImageList.Images.Clear();
            FilesListView.SmallImageList.Images.Clear();
            textBox1.Text = string.Empty;

            string path = (string)e.Node.Tag;
            FilesListView.SelectedNavigationNode = e.Node;
            this.CurrentDirectory.Text = path;
            try
            {
                if (Directory.Exists(this.CurrentDirectory.Text + "\\.git"))
                {
                    button1.Enabled = false;
                }
                else
                {
                    button1.Enabled = true;
                }
                FilesListView.ShowFiles(path);
                FilesListView.ShowDirectories(path);
            }
            catch
            {

            }

            
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


            if (directoryPath.Length >= 1) {
                textBox1.Text = "Initialized empty Git repository in " + directoryPath + "\\.git\\";
                button1.Enabled = false;
            }
            else 
            {
                textBox1.Text = "Choose directory to initialize empty Git repository.";
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
           
            string[] gitStatus;
            string[] result;

            gitStatus = GetStatus(this.CurrentDirectory.Text);
            result = FindStatus(gitStatus);

            CommitMenu commitMenu = new CommitMenu(this);
            commitMenu.Show(); // commitmenu 닫기 전에는 form1 제어 불가
            commitMenu.SetTextBeforeCommit(this.CurrentDirectory.Text, result);
                       

        }


        private void Button1_MouseHover(object sender, EventArgs e)
        {
            this.toolTip1.ToolTipTitle = "Check please";
            this.toolTip1.IsBalloon = true;
            this.toolTip1.SetToolTip(this.button1, "Check exactly that the directory you want to initialize is correct.");
        }

        public string[] GetStatus(string path)
        {           
            string[] gitStatus;
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();

            cmd.FileName = @"cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;             // cmd창이 숨겨지도록 하기
            cmd.CreateNoWindow = true;                               // cmd창을 띄우지 안도록 하기
            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;        // cmd창에서 데이터를 가져오기
            cmd.RedirectStandardInput = true;          // cmd창으로 데이터 보내기
            cmd.RedirectStandardError = true;          // cmd창에서 오류 내용 가져오기

            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;
            process.Start();
            process.StandardInput.Write(@"cd " + path + Environment.NewLine);
            process.StandardInput.Write(@"git status" + Environment.NewLine);
            // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
            process.StandardInput.Close();

            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();            

            gitStatus = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            process.WaitForExit();
            process.Close();
            return gitStatus;
        }

        public string[] FindStatus(string[] gitStatus)
        {
            string[] result = new string[gitStatus.Length]; 

            bool flag = false;
            int i = 0;
            foreach (string status in gitStatus)
            {
                if (status.Equals("Changes to be committed:"))
                    flag = true;
                if (status.Equals("Changes not staged for commit:"))
                    flag = false;
                if (status.Equals("Untracked files:"))
                    flag = false;
                if (flag && status.Contains("modified: "))
                {
                    result[i++] = status;
                }                   
                if (flag && status.Contains("new file: "))
                {
                    result[i++] = status;
                }
                if (flag && status.Contains("deleted: "))
                {
                    result[i++] = status;
                }
                if (flag && status.Contains("renamed: "))
                {
                    result[i++] = status;
                }
            }

            if (result == null) //-> return 커밋할 파일이 존재하지 않아용
                return result;
            return result;
        }

        public void setTextAfterCommit(string[] result, string commitMsg)
        {
            textBox1.Text = "";
            textBox1.Text += "Successfully Committed!\r\n" + commitMsg;

            string directoryPath = this.CurrentDirectory.Text;

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

        private void SplitContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string directoryPath = this.CurrentDirectory.Text;
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
        private void button4_Click_1(Object sender, EventArgs e)
        {
            CloneMenu cloneMenu = new CloneMenu();
            cloneMenu.Show();
            cloneMenu.SetPathBeforeCommit(this.CurrentDirectory.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {            
            HistoryMenu historyMenu = new HistoryMenu();
            historyMenu.Show();
        }
    }
}