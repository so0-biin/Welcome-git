using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace FileManager
{
    public partial class CommitMenu : Form
    {

        private string path;
        Form1 form1;

        public CommitMenu(Form1 form)
        {
            InitializeComponent();
            form1 = form;

            label1.Text = "The list of Staged Changes";
            label2.Text = "Enter a commit message";
            button1.Text = "Commit";    
            button2.Text = "Exit";
            textBox1.ReadOnly = true;
            textBox1.Enabled = false;
            textBox1.Enabled = true;
            
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

            if(noneFilesInStage == result.Length)
            {
                button1.Enabled = false; // stage에 아무 파일이 없으면 commit button 비활성화
                textBox1.Text = "The File to commit does not exist on Stage.\r\n" +
                    "Press Exit to return to the main screen.";
                textBox2.ReadOnly = true;
                textBox2.Enabled = false;
                textBox2.Enabled = true;
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
            string[] gitAfterCommit;

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

            string commitMsg = textBox2.Text.Replace("\r\n", " / ");
            process.StandardInput.Write(@"git commit -m " + "\"" + commitMsg + "\"" + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            gitAfterCommit = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            form1.setTextAfterCommit(gitAfterCommit, textBox2.Text);
 
            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

            this.Close(); // commitMenu close
        }
    }
}
