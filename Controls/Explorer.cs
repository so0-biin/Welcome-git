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
    public class Explorer : ListView
    {
        public TreeNode SelectedNavigationNode { get; set; }

        public Size SmallIconSize { get; set; }
        public Size TileIconSize { get; set; }
        public Size LargeIconSize { get; set; }

        private List<FileType> FileTypes;
        private FileType Document;
        private FileType Folder;

        private System.Windows.Forms.ToolStripStatusLabel CurrentDirectory;

        public void Initialize()
        {
            SmallIconSize = new Size(16, 16);
            TileIconSize  = new Size(32, 32);
            LargeIconSize = new Size(128, 128);

            this.View = View.Details;
            this.Dock = DockStyle.Fill;
            this.LargeImageList = new ImageList();
            this.SmallImageList = new ImageList();
            this.LargeImageList.ImageSize = TileIconSize;
            this.SmallImageList.ImageSize = SmallIconSize;
            this.LargeImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.Columns.Add("Name", 150, HorizontalAlignment.Left);
            this.Columns.Add("Date Modified", 150, HorizontalAlignment.Left);
            this.Columns.Add("Type", 100, HorizontalAlignment.Left);
            this.Columns.Add("Status", 100, HorizontalAlignment.Left);
            this.FullRowSelect = true;

            this.MouseDoubleClick += FilesListView_MouseDoubleClick;
            this.MouseClick += m_ListView_MouseClick;

            InitializeFileTypes();
        }

        public void InitializeFileTypes()
        {
            FileType Picture = new FileType()
            {
                Name = "Picture",
                Description = " File",
                Extensions = new[] { "jpg", "jpeg", "bmp", "png", "gif", "tiff" },
                SearchPattern = new[] { "*.jpg", "*.jpeg", "*.bmp", "*.png", "*.gif", "*.tiff" },
                Image = Properties.Resources.Picture
            };

            FileType Pdf = new FileType()
            {
                Name = "Pdf",
                Description = " Document",
                Extensions = new[] { "pdf" },
                SearchPattern = new[] { "*.pdf" },
                Image = Properties.Resources.Pdf
            };

            FileType Word = new FileType()
            {
                Name = "Word",
                Description = " Document",
                Extensions = new[] { "docx" },
                SearchPattern = new[] { "*.docx" },
                Image = Properties.Resources.Word
            };

            FileType Excel = new FileType()
            {
                Name = "Excel",
                Description = " Document",
                Extensions = new[] { "xlsx" },
                SearchPattern = new[] { "*.xlsx" },
                Image = Properties.Resources.Excel
            };

            FileType PowerPoint = new FileType()
            {
                Name = "PowerPoint",
                Description = " Presentation",
                Extensions = new[] { "pptx" },
                SearchPattern = new[] { "*.pptx" },
                Image = Properties.Resources.Powerpoint
            };

            FileType Audio = new FileType()
            {
                Name = "Audio",
                Description = " File",
                Extensions = new[] { "m4a", "mp3", "wav" },
                SearchPattern = new[] { "*.m4a", "*.mp3", "*.wav" },
                Image = Properties.Resources.Audio
            };

            FileType Video = new FileType()
            {
                Name = "Video",
                Description = " File",
                Extensions = new[] { "mp4", "3gp", "wmv", "flv", "mkv" },
                SearchPattern = new[] { "*.mp4", "*.3gp", "*.wmv", "*.flv", "*.mkv" },
                Image = Properties.Resources.Video
            };

            FileType Text = new FileType()
            {
                Name = "Text",
                Description = " Document",
                Extensions = new[] { "txt" },
                SearchPattern = new[] { "*.txt" },
                Image = Properties.Resources.Doc
            };

            FileType Archive = new FileType()
            {
                Name = "Archive",
                Description = " Archive",
                Extensions = new[] { "rar", "zip", "7z", "gzip" },
                SearchPattern = new[] { "*.rar", "*.zip", "*.7z", "*.gzip" },
                Image = Properties.Resources.Archive
            };

            FileTypes = new List<FileType> { Picture, Pdf, Word, Excel, PowerPoint, Audio, Video, Text, Archive };

            Document = new FileType()
            {
                Name = "Document",
                Description = " Document",
                Extensions = new[] { "" },
                SearchPattern = new[] { "*" },
                Image = Properties.Resources.Doc
            };

            Folder = new FileType()
            {
                Name = "Folder",
                Description = "File Folder",
                Extensions = new[] { "" },
                SearchPattern = new[] { "" },
                Image = Properties.Resources.Folder
            };
        }
        public string[] GetStatus(string path)
        {
            
            if (CheckGit(path)) { 
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
                process.StandardInput.Write(@"git status -s" + Environment.NewLine);

                // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
                process.StandardInput.Close();
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();
                gitStatus = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                
                process.WaitForExit();
                process.Close();

                return gitStatus;
            }
            else {                 
                return null; }
            
        }
        public string FindStatus(string[] gitStatus, string fileName)
        {
            foreach(string status in gitStatus)
            { 
                if (status.Contains(fileName))
                {
                    return NameStatus(status);
                }
            }
            return "unmodified/committed";
        }

        public string NameStatus(string status)
        {
            if (status.Contains(@"?? "))
                return "untracked";
            if (status.Contains(@"A  "))
                return "staged";
            if (status.Contains(@" M "))
                return "modified";
            if (status.Contains(@"M  "))
                return "staged";
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

                    this.SmallImageList.Images.Add(fileType.Image);

                    if (fileType.Name == "Picture")
                        this.LargeImageList.Images.Add(Image.FromFile(file));
                    else
                        this.LargeImageList.Images.Add(fileType.Image);
                   

                    if(gitStatus != null)
                        status = FindStatus(gitStatus,Path.GetFileName(file));

                    ListViewItem listViewItem = new ListViewItem(
                    new string[] { Path.GetFileNameWithoutExtension(file),
                    File.GetLastWriteTime(file).ToString(),
                    Path.GetExtension(file).Substring(1).ToUpper() + fileType.Description, status},
                    this.SmallImageList.Images.Count - 1);

                    listViewItem.Tag = file;
                    listViewItem.UseItemStyleForSubItems = false;
                    listViewItem.SubItems[1].ForeColor = listViewItem.SubItems[2].ForeColor = Color.Gray;

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
            if (directoryPath== "C:\\") gitDirectoryPath = directoryPath + @".git";
            else gitDirectoryPath = directoryPath + @"\.git";
            return Directory.Exists(gitDirectoryPath);
        }

        public bool CheckGit(string directoryPath) {
            if (directoryPath == null) return false;
            while(!JudgeGit(directoryPath))
            {   
                directoryPath = directoryPath.Substring(0, directoryPath.LastIndexOf('\\'));
                if (directoryPath == @"C:") return false;
            }
            //directoryPath = System.IO.Directory.GetParent(Environment.CurrentDirectory).ToString();
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
                    new string[] { Path.GetFileName(directory),
                        File.GetLastWriteTime(directory).ToString(),
                        Folder.Description, ""},
                    this.SmallImageList.Images.Count - 1);
                listViewItem.Tag = directory;
                listViewItem.UseItemStyleForSubItems = false;
                listViewItem.SubItems[1].ForeColor = listViewItem.SubItems[2].ForeColor = Color.Gray;

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

                this.SelectedNavigationNode.Expand();
                this.SelectedNavigationNode.TreeView.SelectedNode = SelectedNavigationNode = this.SelectedNavigationNode.Nodes[Path.GetFileName(path)];

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

                if (!Directory.Exists(path))
                {
                    ContextMenuStrip m = new ContextMenuStrip();

                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    dict.Add("real_path", real_path);
                    dict.Add("file_name", file_name);

                    m.Tag = dict;

                    m.Items.Add("git add (untracked)");
                    m.Items.Add("git add (modified)");
                    m.Items.Add("git restore");
                    m.Items.Add("git restore --staged");
                    m.Items.Add("git rm --cached");
                    m.Items.Add("git rm");
                    m.Items.Add("git mv");
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
                case "git add (untracked)":
                    cmd_ex(real_path, file_name, "add");    
                    break;
                case "git add (modified)":
                    cmd_ex(real_path, file_name, "add");
                    break;
                case "git restore":
                    cmd_ex(real_path, file_name, "restore");
                    break;
                case "git restore --staged":
                    cmd_ex(real_path, file_name, "restore --staged");
                    break;
                case "git rm --cached":
                    cmd_ex(real_path, file_name, "rm --cached");
                    break;
                case "git rm":
                    cmd_ex(real_path, file_name, "rm");
                break;
                case "git mv":
                    cmd_ex(real_path, file_name, "mv");
                    break;
            }
        }

        void cmd_ex(String real_path, String file_name, String command)
        {
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
            //StringBuilder sb = new StringBuilder();
            //sb.Append(path);
            //sb.Append(this.CurrentDirectory.Text);
            //directoryPath = sb.ToString();

            process.Start(); 
            
            // cmd 명령 입히는거 시작                     
            
             process.StandardInput.Write(@"cd " + real_path + Environment.NewLine);
             process.StandardInput.Write(@"git " + command + " " + file_name + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음
        }
    }
}
