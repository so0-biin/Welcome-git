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
    private string currentDirectory;
    public void Initialize()
    {
      this.View = View.Details;
      this.Dock = DockStyle.Fill;

      this.Columns.Add("Branch List", 200, HorizontalAlignment.Left);

      this.FullRowSelect = true;

      this.MouseClick += Branch_ListView_MouseClick;
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
      StreamReader readOutput = process.StandardOutput;
      string output = readOutput.ReadToEnd();

      branchList = output.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

      process.WaitForExit();
      process.Close();

      return branchList;
    }

    public string[] FilterBranch(string[] cmdResult)
    {
      string[] branchList = new string[cmdResult.Length];
      int i = 0;
      foreach (string result in cmdResult)
      {
        if (!result.Contains(@"\") && !result.Contains("Microsoft"))
        {
          branchList[i++] = result;
        }
      }

      return branchList;
    }
    public void ShowBranches(string path)
    {
      currentDirectory = path;
      string[] cmdResult = GetBranch(path);
      string[] branches = FilterBranch(cmdResult);

      this.BeginUpdate();
      foreach (string branch in branches)
      {
        if (branch != null)
        {
          ListViewItem item = new ListViewItem(branch);

          item.Tag = branch;
          item.UseItemStyleForSubItems = false;

          if (branch.Contains("*"))
            item.SubItems[0].ForeColor = Color.Green;
          this.Items.Add(item);
        }

      }
      this.EndUpdate();

    }

    private void Branch_ListView_MouseClick(object sender, MouseEventArgs e)
    {

      if (e.Button == MouseButtons.Right)
      {
        string currentBranch = (string)(sender as BranchList).SelectedItems[0].Tag;

        ContextMenuStrip menu = new ContextMenuStrip();

        menu.Tag = currentBranch;

        menu.Items.Add("branch delete");
        menu.Items.Add("branch rename");
        menu.Items.Add("branch checkout");

        menu.Show(PointToScreen(e.Location));
        menu.ItemClicked += Branch_Menu_ItemClicked;
      }
    }

    void Branch_Menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {
      string currentBranch = (sender as ContextMenuStrip).Tag as string;
      if (currentBranch.Contains("*"))
      {
        string[] split = currentBranch.Split('*');
        currentBranch = split[1];
      }
      switch (e.ClickedItem.Text)
      {
        case "branch delete":
          BranchCommand(currentDirectory, "branch -d", currentBranch);
          break;
        case "branch rename":
          Form inputForm = new Form();

          inputForm.Text = "rename branch";
          inputForm.Size = new Size(300, 100);

          TextBox inputBox = new TextBox();
          inputBox.Text = currentBranch.Trim();
          inputBox.Location = new Point(10, 10);
          inputForm.Controls.Add(inputBox);

          Button okButton = new Button();
          okButton.Text = "OK";

          okButton.DialogResult = DialogResult.OK;
          okButton.Location = new Point(150, 10);
          inputForm.Controls.Add(okButton);

          DialogResult result = inputForm.ShowDialog();
          string renamedBranch = "";
          string commandBranch = "";

          if (result == DialogResult.OK)
          {
            renamedBranch = inputBox.Text;
          }
          commandBranch = currentBranch + " " + renamedBranch;
          BranchCommand(currentDirectory, "branch -m", commandBranch);
          break;
        case "branch checkout":
          BranchCommand(currentDirectory, "checkout", currentBranch);
          break;
      }
    }
    public void BranchCommand(string path, string command, string branch)
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
      if (command.Equals("branch -m"))
      {
        string[] oldNewBranch = branch.Split(' ');
        //process.StandardInput.Write(@"git checkout " + oldNewBranch[0] + Environment.NewLine);
      }
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

      this.Items.Clear();
      try
      {
        this.ShowBranches(currentDirectory);
      }
      catch
      {

      }
    }
  }
}
