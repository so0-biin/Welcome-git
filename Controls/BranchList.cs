using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager.Controls
{
    public class BranchList : ListView
    {
        public void Initialize()
        {
            this.View = View.Details;
            this.Dock = DockStyle.Fill;

            this.Columns.Add("Branch List", 200, HorizontalAlignment.Left);
        }

        public string[] GetBranch(string path)
        {
            string[] branchList;
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
            process.StandardInput.Write(@"git branch" + Environment.NewLine);

            // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
            process.StandardInput.Close();
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();
            branchList = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            process.WaitForExit();
            process.Close();

            return branchList;
        }

        public void ShowBranches(string path)
        {
            string[] branches = GetBranch(path);

            this.BeginUpdate();
            foreach (string branch in branches)
            {
                ListViewItem item = new ListViewItem(branch);

                item.Tag = branch;
                item.UseItemStyleForSubItems = false;
                if (branch.Contains("*"))
                    item.SubItems[0].ForeColor = Color.Green;
                this.Items.Add(item);
            }
            this.EndUpdate();

        }
    }
}
