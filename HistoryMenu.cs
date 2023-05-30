using FileManager.Controls;
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

namespace FileManager
{
    public partial class HistoryMenu : Form
    {
        private BranchList BranchListView;
        private CommitHistory GraphListView;
        private string currentDirectory;

        public HistoryMenu(string data)
        {
            InitializeComponent();
            GraphListView = new CommitHistory();
            GraphListView.Initialize(textBox1);
            currentDirectory = data;
           
            BranchListView = new BranchList();
            BranchListView.Initialize(GraphListView);
            splitContainer1.Panel1.Controls.Add(BranchListView);

            BranchRefresh();

            button1.Text = "Merge";
            button2.Text = "Create";

            splitContainer2.Panel1.Controls.Add(GraphListView);

            
            GraphRefresh(currentDirectory);

        }

        public void GraphRefresh(string path)
        {
            string[] commitLog = GraphListView.GetCommitLog(path);
            
            GraphListView.Items.Clear();
            try 
            {
                GraphListView.showGraph(commitLog);
            }
            catch
            {

            }
        }


        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void branchCmd(string path, string command, string branch)
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
          process.Start();
          process.StandardInput.Write(@"cd " + path + Environment.NewLine);
          process.StandardInput.Write(@"git " + command + " " + branch + Environment.NewLine);

          // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
          process.StandardInput.Close();
          StreamReader readError = process.StandardError;
          string error = readError.ReadToEnd();
          if (error.Contains("error") || error.Contains("fatal"))
          {
            Form errorForm = new Form();

            errorForm.Text = "branch error";
            errorForm.Size = new Size(400, 150);

            Label errorLabel = new Label();
            errorLabel.Text = error;
            errorLabel.AutoSize = true;
            errorLabel.MaximumSize = new Size(390, 0);
            errorLabel.Location = new Point(10, 10);
            errorForm.Controls.Add(errorLabel);

            Button okButton = new Button();
            okButton.Text = "OK";

            okButton.DialogResult = DialogResult.OK;
            okButton.Location = new Point(300, 80);
            errorForm.Controls.Add(okButton);

            DialogResult result = errorForm.ShowDialog();

          }

          process.WaitForExit();
          process.Close();
        }

        private void BranchCreate()
        {
          Form inputForm = new Form();

          inputForm.Text = "write new branch name";
          inputForm.Size = new Size(350, 100);

          TextBox inputBox = new TextBox();
          inputBox.Location = new Point(10, 10);
          inputForm.Controls.Add(inputBox);

          Button okButton = new Button();
          okButton.Text = "OK";

          okButton.DialogResult = DialogResult.OK;
          okButton.Location = new Point(150, 10);
          inputForm.Controls.Add(okButton);

          DialogResult result = inputForm.ShowDialog();
          string branchName = "";

          if (result == DialogResult.OK)
          {
            branchName = inputBox.Text;
          }
          branchCmd(currentDirectory, "branch", branchName);
          BranchRefresh();
        }

        public void BranchRefresh()
        {
          BranchListView.Items.Clear();
          try
          {
            BranchListView.ShowBranches(currentDirectory);
          }
          catch
          {

          }
        }



        private void button2_Click(object sender, EventArgs e)
        {
          BranchCreate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          Merge merge = new Merge(currentDirectory);
          merge.Show();
        }
    }

 }