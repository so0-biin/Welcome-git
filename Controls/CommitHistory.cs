using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace FileManager.Controls
{
    public class CommitHistory : ListView
    {

        private List<FileType> FileTypes;
        private FileType Document;
        private FileType Folder;

        public void Initialize()
        {

            this.View = View.Details;
            this.Dock = DockStyle.Fill;

            this.Columns.Add("graph", 150, HorizontalAlignment.Left);
            this.Columns.Add("checksum", 150, HorizontalAlignment.Left);
            this.Columns.Add("commit message", 150, HorizontalAlignment.Left);

            this.FullRowSelect = true;

            this.MouseDoubleClick += FilesListView_MouseDoubleClick;
            this.MouseClick += m_ListView_MouseClick;

            
        }

        public void SetTextBeforeCommit(string directoryPath, string[] result)
        {
            this.path = directoryPath;
            textBox1.Text += directoryPath + "\r\n";
            int noneFilesInStage = 0;

            foreach (string staged in result)
            {
                if (String.IsNullOrEmpty(staged))
                {
                    noneFilesInStage++;
                    continue;
                }
                else
                {
                    textBox1.Text += staged + "\r\n";
                }
            }

            if (noneFilesInStage == result.Length)
            {
                button1.Enabled = false; // stage에 아무 파일이 없으면 commit button 비활성화
                textBox1.Text = "The File to commit does not exist on Stage.\r\n" +
                    "Press Exit to return to the main screen.";
                textBox2.ReadOnly = true;
                textBox2.Enabled = false;
                textBox2.Enabled = true;
            }


        }

        public string FindStatus(string[] gitStatus, string fileName)
        {
            foreach (string status in gitStatus)
            {
                if (status.Contains(" " + fileName))
                {
                    return NameStatus(status);
                }
            }
            return "Unmodified/Committed";
        }

        public string NameStatus(string status)
        {
            if (status.Contains(@"D  "))
                return "Untracked";
            if (status.Contains(@"?? "))
                return "Untracked";
            if (status.Contains(@"A  "))
                return "Staged";
            if (status.Contains(@"R  "))
                return "Staged";
            if (status.Contains(@"M  "))
                return "Staged";
            if (status.Contains(@" M "))
                return "Modified";

            if (status.Contains(@"MM "))
                return "Modified & Staged";
            if (status.Contains(@"AM "))
                return "Modified & Staged";
            if (status.Contains(@"RM "))
                return "Modified & Staged";
            return "";
        }
        public void ShowFiles(string path)
        {
            List<string> files = new List<string>();
            FileType fileType;
            string[] gitStatus = GetStatus(path);

            files.AddRange(Directory.GetFiles(path));

            this.BeginUpdate();

            foreach (string file in files)
            {
                try
                {
                    string status = "";

                    fileType = DetectFileType(Path.GetExtension(file).Substring(1).ToLower());

                    if (gitStatus != null)
                    {
                        status = FindStatus(gitStatus, Path.GetFileName(file));
                    }
                    
                    
                    this.SmallImageList.Images.Add(fileType.Image);
                    

                    if (fileType.Name == "Picture")
                        this.LargeImageList.Images.Add(Image.FromFile(file));
                    else
                        this.LargeImageList.Images.Add(fileType.Image);


                    ListViewItem listViewItem = new ListViewItem(
                    new string[] { Path.GetFileNameWithoutExtension(file), status,
                    File.GetLastWriteTime(file).ToString(),
                    Path.GetExtension(file).Substring(1).ToUpper() + fileType.Description},
                    this.SmallImageList.Images.Count - 1);


                    listViewItem.Tag = file;
                    listViewItem.UseItemStyleForSubItems = false;
                    if (status.Equals("Untracked"))
                    {
                        listViewItem.SubItems[0].ForeColor = Color.Red;
                        listViewItem.SubItems[1].ForeColor = Color.Red;
                    }
                    else if (status.Equals("Staged"))
                    {
                        listViewItem.SubItems[0].ForeColor = Color.Green;
                        listViewItem.SubItems[1].ForeColor = Color.Green;
                    }
                    else if (status.Equals("Modified"))
                    {
                        listViewItem.SubItems[0].ForeColor = Color.Blue;
                        listViewItem.SubItems[1].ForeColor = Color.Blue;
                    }
                    else if (status.Equals("Modified & Staged"))
                    {
                        listViewItem.SubItems[0].ForeColor = Color.Green; 
                        listViewItem.SubItems[1].ForeColor = Color.Red;
                    }
                    else
                    {
                        listViewItem.SubItems[0].ForeColor = Color.Black;
                        listViewItem.SubItems[1].ForeColor = Color.Black;
                    }
                    listViewItem.SubItems[2].ForeColor = listViewItem.SubItems[3].ForeColor = Color.Gray;

                    this.Items.Add(listViewItem);
                }
                catch
                {
                    if (this.SmallImageList.Images.Count != this.LargeImageList.Images.Count)
                        this.SmallImageList.Images.RemoveAt(this.SmallImageList.Images.Count - 1);
                }
            }

            this.EndUpdate();
        }

        public bool JudgeGit(string directoryPath)
        {
            string gitDirectoryPath;
            if (directoryPath == "C:\\") gitDirectoryPath = directoryPath + @".git";
            else gitDirectoryPath = directoryPath + @"\.git";
            return Directory.Exists(gitDirectoryPath);
        }

        public bool CheckGit(string directoryPath)
        {
            if (directoryPath == null) return false;
            while (!JudgeGit(directoryPath))
            {
                directoryPath = directoryPath.Substring(0, directoryPath.LastIndexOf('\\'));
                if (directoryPath == @"C:") return false;
            }
            return true;
        }

        public void ShowDirectories(string path)
        {
            List<string> directories = new List<string>();

            directories.AddRange(Directory.GetDirectories(path));

            this.BeginUpdate();

            foreach (string directory in directories)
            {
                this.SmallImageList.Images.Add(Folder.Image);
                this.LargeImageList.Images.Add(Folder.Image);

                ListViewItem listViewItem = new ListViewItem(
                    new string[] {Path.GetFileName(directory),"",
                        File.GetLastWriteTime(directory).ToString(),
                        Folder.Description},
                    this.SmallImageList.Images.Count - 1);
                listViewItem.Tag = directory;
                listViewItem.UseItemStyleForSubItems = false;
                listViewItem.SubItems[2].ForeColor = listViewItem.SubItems[3].ForeColor = Color.Gray;

                this.Items.Add(listViewItem);
            }

            this.EndUpdate();
        }

        public void UpdateIconImages()
        {
            FileType fileType;

            this.BeginUpdate();

            this.LargeImageList.Images.Clear();
            foreach (ListViewItem item in this.Items)
            {
                if (item.SubItems[2].Text == "File Folder")
                    this.LargeImageList.Images.Add(Folder.Image);
                else
                {
                    fileType = DetectFileType(Path.GetExtension(((string)item.Tag)).Substring(1).ToLower());

                    if (fileType.Name == "Picture")
                        this.LargeImageList.Images.Add(Image.FromFile((string)item.Tag));
                    else
                        this.LargeImageList.Images.Add(fileType.Image);
                }
            }

            this.EndUpdate();
        }

        public FileType DetectFileType(string searchExtension)
        {
            foreach (FileType type in FileTypes)
            {
                foreach (string extension in type.Extensions)
                {
                    if (searchExtension == extension)
                        return type;
                }
            }

            return Document;
        }

        private void FilesListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string path = (string)(sender as Explorer).SelectedItems[0].Tag;

            if (!Directory.Exists(path))
            {
                // if the path represents a file

                System.Diagnostics.Process.Start(path);
            }
            else
            {
                // if the path represents a folder

                this.Items.Clear();
                this.LargeImageList.Images.Clear();
                this.SmallImageList.Images.Clear();

                try
                {
                    this.ShowFiles(path);
                    this.ShowDirectories(path);
                }
                catch
                {

                }

                

                // search for Form1 Control
                Control parent = this.Parent;
                while (parent.Name != "Form1")
                    parent = parent.Parent;

                // parent is now Form1

                // set CurrentDirectory's Text
                (parent.Controls["StatusStrip"] as StatusStrip).Items["CurrentDirectory"].Text = path;

                // Focus on NavigationPanel
                (parent.Controls["SplitContainer"] as SplitContainer).Panel1.Controls["NavigationPanel"].Focus();
            }
        }


        private void m_ListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Right))
            {
                string path = (string)(sender as Explorer).SelectedItems[0].Tag;
                string file_name = Path.GetFileName(path);
                int index = path.IndexOf(file_name);
                string real_path = path.Remove(index, file_name.Length);
                real_path = real_path.TrimEnd('\\');


                ListViewItem item = (sender as Explorer).SelectedItems[0];
                string status = item.SubItems[1].Text;

                if (!Directory.Exists(path))
                {
                    ContextMenuStrip m = new ContextMenuStrip();

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("real_path", real_path);
                    dict.Add("file_name", file_name);

                    m.Tag = dict;

                    if (status.Equals("Untracked"))
                    {
                        m.Items.Add("git add (on stage)");
                    }
                    else if (status.Equals("Modified"))
                    {
                        m.Items.Add("git add (on stage)");
                        m.Items.Add("git restore (undo change)");
                        m.Items.Add("git rm --cached (untrack file)");
                    }
                    else if (status.Equals("Staged"))
                    {
                        m.Items.Add("git restore --staged (off stage)");
                        m.Items.Add("git rm --cached (untrack file)");
                    }
                    else if (status.Equals("Modified & Staged"))
                    {
                        m.Items.Add("git add (on stage)");
                        m.Items.Add("git restore (undo change)");
                        m.Items.Add("git restore --staged (off stage)");
                    }
                    else
                    {
                        m.Items.Add("git rm --cached (untrack file)");
                        m.Items.Add("git rm (delete)");
                        m.Items.Add("git mv (change file name)");
                    }

                    m.Show(PointToScreen(e.Location));

                    m.ItemClicked += m_ItemClicked;
                }
            }
        }

        void m_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Dictionary<string, string> dict = (sender as ContextMenuStrip).Tag as Dictionary<string, string>;

            string real_path = dict["real_path"];
            string file_name = dict["file_name"];

            switch (e.ClickedItem.Text)
            {
                case "git add (on stage)":
                    cmd_ex(real_path, file_name, "add");
                    break;
                case "git restore (undo change)":
                    cmd_ex(real_path, file_name, "restore");
                    break;
                case "git restore --staged (off stage)":
                    cmd_ex(real_path, file_name, "restore --staged");
                    break;
                case "git rm --cached (untrack file)":
                    cmd_ex(real_path, file_name, "rm --cached");
                    break;
                case "git rm (delete)":
                    cmd_ex(real_path, file_name, "rm");
                    break;
                case "git mv (change file name)":
                    Form inputForm = new Form();

                    inputForm.Text = "write new file name";
                    inputForm.Size = new Size(300, 100);

                    TextBox inputBox = new TextBox();
                    inputBox.Location = new Point(10, 10);
                    inputForm.Controls.Add(inputBox);

                    Button okButton = new Button();
                    okButton.Text = "OK";

                    okButton.DialogResult = DialogResult.OK;
                    okButton.Location = new Point(150, 10);
                    inputForm.Controls.Add(okButton);

                    DialogResult result = inputForm.ShowDialog();
                    string renameFile = "";
                    string newFileName = "";
                    string newFileExtension = "";

                    if (result == DialogResult.OK)
                    {
                        newFileExtension = Path.GetExtension(real_path + @"\" + file_name);
                        newFileName = inputBox.Text + newFileExtension;
                    }
                    renameFile = file_name + " " + newFileName;
                    cmd_ex(real_path, renameFile, "mv");
                    break;
            }
        }

        void cmd_ex(String real_path, String file_name, String command)
        {
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

            // cmd 다루기
            process.Start();

            // cmd 명령 입히는거 시작                     

            process.StandardInput.Write(@"cd " + real_path + Environment.NewLine);
            process.StandardInput.Write(@"git " + command + " " + file_name + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

            this.Items.Clear();
            this.LargeImageList.Images.Clear();
            this.SmallImageList.Images.Clear();

            try
            {
                this.ShowFiles(real_path);
                this.ShowDirectories(real_path);
            }
            catch
            {

            }
        }
    }
}
