using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class CommitMenu : Form
    {

        private string path;

        public CommitMenu()
        {
            InitializeComponent();

            label1.Text = "The list of Staged Changes";
            label2.Text = "Enter a commit message";
            button1.Text = "Commit";    
            button2.Text = "Exit";
            textBox1.ReadOnly = true;
            textBox1.Enabled = false;
            textBox1.Enabled = true;
            
        }
        public void SetText(string directoryPath, string[] result)
        {
            this.path = directoryPath;
            textBox1.Text += directoryPath + "\r\n";

            foreach (string staged in result)
            {
                if (String.IsNullOrEmpty(staged)) continue;
                else
                {
                    textBox1.Text += staged + "\r\n";
                }
            }
                   
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close(); // exit and return to Form1
        }

        private void button1_Click(object sender, EventArgs e) // git commit button click
        {
            // cmd를 사용하기 위한 준비
            ProcessStartInfo cmd = new ProcessStartInfo();
            Process process = new Process();
            string directoryPath;
            directoryPath = this.path;

            cmd.FileName = @"cmd";
            cmd.WindowStyle = ProcessWindowStyle.Hidden;             // cmd창이 숨겨지도록 하기
            cmd.CreateNoWindow = true;                               // cmd창을 띄우지 안도록 하기

            cmd.UseShellExecute = false;
            cmd.RedirectStandardOutput = true;        // cmd창에서 데이터를 가져오기
            cmd.RedirectStandardInput = true;          // cmd창으로 데이터 보내기
            cmd.RedirectStandardError = true;          // cmd창에서 오류 내용 가져오기

            process.EnableRaisingEvents = false;
            process.StartInfo = cmd;

            process.Start(); // cmd 명령 입히는거 시작                     
            process.StandardInput.Write(@"cd " + directoryPath + Environment.NewLine);

            //textBox2.Text = "git commit -m " + "\"" + textBox2.Text + "\"" +  Environment.NewLine;

            process.StandardInput.Write(@"git commit -m " + "\"" + textBox2.Text + "\"" + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음*/
        }
    }
}
