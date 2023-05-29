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
            
        }

        public string[] GetCommitLog(string path)
        {
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
            output = output.Substring(start, output.Length - start - 1);

            commitLog = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            process.WaitForExit();
            process.Close();

            return commitLog;
        }

        public void showGraph(string[] commitLog)
        {
            List<string> commitList = new List<string>();
            
            foreach (string commit in commitLog)
            {
                int graphIndex = commit.IndexOf(' ');
                int checksumIndex = commit.IndexOf(' ', commit.IndexOf(' ') + 1);

                Console.WriteLine(commit);
                Console.WriteLine(graphIndex);
                Console.WriteLine(checksumIndex-graphIndex-1);
                Console.WriteLine(commit.Length - checksumIndex - 1);

                ListViewItem listViewItem = new ListViewItem(
                    new string[] {commit.Substring(0, graphIndex), commit.Substring(graphIndex + 1, checksumIndex - graphIndex - 1), commit.Substring(checksumIndex + 1, commit.Length - checksumIndex-  1)});
                //listViewItem.Tag = commit;
                //listViewItem.UseItemStyleForSubItems = false;
                //listViewItem.SubItems[2].ForeColor = listViewItem.SubItems[3].ForeColor = Color.Gray;

            }
        }

        void cmd_ex(String real_path, String command)
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
            
            process.StandardInput.Close(); // cmd  명령 입력 끝

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

            this.Items.Clear();
            this.LargeImageList.Images.Clear();
            this.SmallImageList.Images.Clear();

        }
    }
}
