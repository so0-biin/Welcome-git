using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Merge : Form
    {
        string path;

        public Merge(string currentDirectory)
        {
            InitializeComponent();
            path = currentDirectory;

            label1.Text = "current branch";
            label2.Text = "merge branch";
            button1.Text = "Exit";
            button2.Text = "Merge";
            textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;

            string current_branch = currentBranch(path);

            string search_front = "--show-current";
            int index = current_branch.IndexOf(search_front);
            int index2 = index + search_front.Length; 
            current_branch = current_branch.Substring(index2).Trim();

            string search_back = path;
            int index_back = current_branch.IndexOf(path);
            current_branch = current_branch.Remove(index_back, path.Length+1);
            textBox2.Text = current_branch;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private string currentBranch(string path)
        {
            string result = "";

            try
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
                process.StandardInput.Write(@"cd " + path + Environment.NewLine);
                process.StandardInput.Write(@"git branch --show-current " + Environment.NewLine);

                process.StandardInput.Close(); // cmd  명령 입력 끝

                result = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close(); // cmd 창을 닫음

                return result;
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
