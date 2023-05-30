using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Data;
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
        private string currentDirectory;
        TextBox commitTextBox;
        public void Initialize(TextBox textBox)
        {

            this.View = View.Details;
            this.Dock = DockStyle.Fill;

            this.Columns.Add("graph", 150, HorizontalAlignment.Left);
            this.Columns.Add("checksum", 150, HorizontalAlignment.Left);
            this.Columns.Add("commit message", 500, HorizontalAlignment.Left);
            this.Columns.Add("cm", 0, HorizontalAlignment.Left);

            this.Columns[3].Dispose();
            this.FullRowSelect = true;
            this.MouseClick += m_ListView_MouseClick;
            this.commitTextBox = textBox;
        }

        public string[] GetCommitLog(string path)
        {
            currentDirectory = path;
            string[] commitLog;

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
            process.StandardInput.Write(@"git log --pretty=oneline --graph" + Environment.NewLine);

            // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
            process.StandardInput.Close();
            StreamReader reader = process.StandardOutput;

            //String result = process.StandardOutput.ReadToEnd();
            //MessageBox.Show(result);
            string output = reader.ReadToEnd();
            int start = output.IndexOf('*');
            int length = path.Length;

            Console.WriteLine(output);
            
            output = output.Substring(start, output.Length - start - length - 1);


            commitLog = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            process.WaitForExit();
            process.Close();

            return commitLog;
        }

        public void showGraph(string[] commitLog)
        {
            List<string> commitList = new List<string>();

            this.BeginUpdate();
            
            foreach (string commit in commitLog)
            {
                int i = 0;
                int checksumIndex;
                int messageIndex;
                string transCommit;
                ListViewItem listViewItem;

                if (!commit.Contains('*'))
                {
                    transCommit=transGraph(commit);
                    listViewItem = new ListViewItem(
                    new string[] { transCommit, "", "",""});
                }

                else
                {
                    while (true)
                    {
                        if (commit[i] != ' ' && commit[i] != '*' && commit[i] != '\\' && commit[i] != '/' && commit[i] != '|')
                        {
                            checksumIndex = i;
                            break;
                        }
                        i++;
                    }

                    messageIndex = commit.IndexOf(' ', checksumIndex) + 1;
                    transCommit = transGraph(commit.Substring(0, checksumIndex - 1));
                    listViewItem = new ListViewItem(
                    new string[] { transCommit, commit.Substring(checksumIndex, 7), 
                        commit.Substring(messageIndex, commit.Length - messageIndex), commit.Substring(checksumIndex, 40)});

                }
                        
                listViewItem.Tag = commit;
                listViewItem.UseItemStyleForSubItems = false;
                listViewItem.SubItems[0].ForeColor = Color.Green;

                this.Items.Add(listViewItem);

            }

            this.EndUpdate();
        }

        private string transGraph(string commit)
        {
            string result = "";
            for (int i = 0; i< commit.Length; i++)
            {
                if (commit[i].Equals('*'))
                    result += " ⍥";
                if (commit[i].Equals('\\'))
                    result += " ⧹";
                if (commit[i].Equals('/'))
                    result += " ⧸";
                if (commit[i].Equals('|'))
                    result += " |";
            }
            return result;
        }

        private void m_ListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button.Equals(MouseButtons.Left))
            {
                ListViewItem item = (sender as CommitHistory).SelectedItems[0];
                string checksum = item.SubItems[3].Text;
                commitTextBox.Clear();
                if (checksum.Length != 0)
                {
                    GetChecksum(checksum);
                }
            }
        }
        public void GetChecksum(String checksum)
        {
            string[] result;
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

            process.StandardInput.Write(@"cd " + currentDirectory + Environment.NewLine);
            process.StandardInput.Write(@"git cat-file -p " + checksum + Environment.NewLine);


            process.StandardInput.Close(); // cmd  명령 입력 끝
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();
            result = output.Split(Environment.NewLine.ToCharArray());


            commitTextBox.Text += ("commit: " + checksum + " [" + checksum.Substring(0, 6)+"]\r\n");
            printCommitText(result);
            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

        }

        public void printCommitText(string[] catCommitObj)
        {
            string[] print;
            bool parentFlag = true, authorFlag = true, committerFlag = true, commitMsgFlag = false, readMeFlag = false;
            foreach (string line in catCommitObj)
            {
                if (commitMsgFlag)
                {
                    if (line.Equals(currentDirectory + ">"))
                        commitMsgFlag = false;
                    else if (line.Contains("-----BEGIN PGP SIGNATURE-----") || line.Contains("-----END PGP SIGNATURE-----"))
                    {
                        readMeFlag = !readMeFlag;
                    }
                    else if (!readMeFlag)
                    {
                        commitTextBox.Text += (line + "\r\n");
                    }


                }
                if (parentFlag && (line.IndexOf("parent") == 0))
                {
                    print = line.Split(' ');
                    commitTextBox.Text += ("parent: " + print[1].Substring(0, 7) + "\r\n");
                    parentFlag = false;
                }
                if (authorFlag && (line.IndexOf("author") == 0))
                {
                    print = line.Split(' ');
                    commitTextBox.Text += ("author: " + print[1] + " " + print[2] + "\r\n");
                    authorFlag = false;
                }
                if (committerFlag && (line.IndexOf("committer") == 0))
                {
                    print = line.Split(' ');
                    commitTextBox.Text += ("committer: " + print[1] + "\r\n");
                    committerFlag = false;
                    commitMsgFlag = true;
                }
            }
        }
    }
}
