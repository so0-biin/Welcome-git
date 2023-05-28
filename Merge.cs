﻿using System;
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
        string current_branch;

        public Merge(string currentDirectory)
        {
            InitializeComponent();
            path = currentDirectory;

            label1.Text = "current branch";
            label2.Text = "merge branch";
            button1.Text = "Exit";
            button2.Text = "Merge";
            //textBox1.ReadOnly = true;
            textBox2.ReadOnly = true;

            current_branch = currentBranch(path);

            string search_front = "--show-current";
            int index = current_branch.IndexOf(search_front);
            int index2 = index + search_front.Length; 
            current_branch = current_branch.Substring(index2).Trim();

            int index_back = current_branch.IndexOf(path);
            current_branch = current_branch.Remove(index_back, path.Length+1);
            textBox2.Text = current_branch;

            comboBox1.Text = "select branch";
            comboBox_show(path);
        }

        private void button2_Click(object sender, EventArgs e)  //merge button
        {
            string selected_branch = comboBox1.SelectedItem.ToString();
            string result = "";

            try
            {
                result = cmd_ex(path, "merge " + selected_branch);
            }
            catch (Exception ex)
            {
            }

            if (result.Contains("merge failed")||result.Contains("CONFLICT")||result.Contains("Merge conflict")){
                result = cmd_ex(path, "status");
                string search_front = "git status";
                int index = result.IndexOf(search_front);
                int index2 = index + search_front.Length;
                result = result.Substring(index2).Trim();

                string result_back = result.Remove(result.IndexOf("no changes"));
                MessageBox.Show(result_back + "conflict occured and automatically aborted merge");
                //textBox1.Text = result + " conflict occured and automatically aborted merge";
                //textBox1.Text = "conflict occured and automatically aborted merge";
                cmd_ex(path, "merge --abort");
            }
            else
            {
                //textBox1.Text = "successfully merged";
                MessageBox.Show("successfully merged");
            }
        }

        private void button1_Click(object sender, EventArgs e)  //exit button
        {
            this.Close();
        }

        private void comboBox_show(string path)
        {
            comboBox1.Items.Clear();
            string result = "";

            try
            {
                result = cmd_ex(path, "branch");
            }
            catch (Exception ex)
            {
            }
            string search_front = "git branch";
            int index = result.IndexOf(search_front);
            int index2 = index + search_front.Length;
            result = result.Substring(index2).Trim();

            int index_back = result.IndexOf(path);
            result = result.Remove(index_back, path.Length + 1);
            result = result.Replace("*", " ");

            string[] branchList = result.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string branch in branchList)
            {
                string branchName = branch.Trim();
                string current_branchName = current_branch.Trim();
                if (!branchName.Equals(current_branchName))
                {
                    comboBox1.Items.Add(branchName);
                }
            }
        }

        private string currentBranch(string path)
        {
            string result = "";

            try
            {
                result = cmd_ex(path, "branch --show-current");
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        private string cmd_ex(string path, string command)
        {
            string result = "";

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
            process.StandardInput.Write(@"git " + command + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            result = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

            return result;
        }
    }
}
