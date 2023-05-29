using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class CloneMenu : Form
    {
        private string path;
        Form1 form1;
        
        public CloneMenu(Form1 form)
        {
            InitializeComponent();
            form1 = form;

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            button1.Enabled = false; // clone button
            //button4.Enabled = false; // id-token button
            label1.Text = "Input Git repository address to CLONE and click Check button";
            label2.Text = "Destination Path";
            label3.Text = "ID";
            label4.Text = "Access Token";
            label5.Text = "Check !";
            button1.Text = "Clone";
            button2.Text = "Exit";
            button3.Text = "Check";
            //button4.Text = "Check";
            string[] data = { "public", "private" };
            comboBox1.Items.AddRange(data);
            label6.Text = "Repository property";
            comboBox1.Enabled = false;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void SetTextBeforeClone(string path)
        {
            textBox2.Text = path;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e) // clone button click
        {
            repoClone(textBox2.Text, textBox1.Text); // clone 버튼 클릭하면 clone하기 - private / public, 경로, 주소



        }

        private void cloneCmd(string path, string repoAddress)
        {
            // repoAddress가 public이면 입력받은 address로 그냥 오고 private이면 변형되어서 올 것임

            string[] cloneError;
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
            process.StandardInput.Write(@"git clone " + repoAddress + Environment.NewLine);
            // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다
            process.StandardInput.Close();

            string errorOutput = process.StandardError.ReadToEnd();
            cloneError = errorOutput.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            // git clone 후 error 메시지를 받음


            process.WaitForExit();
            process.Close();
            this.Close();

            bool successClone = true;
            foreach(string output in cloneError)
            {

                if(output.Contains("fatal")) {
                    successClone = false; // success 하게 clone 실패했다고 출력
                    
                }
            }
            form1.setTextAfterClone(successClone, cloneError);


        }

        private void repoClone(string path, string address)
        {
            string repoAddress = address;
            //bool publicRepo = true;
            if(comboBox1.SelectedItem.ToString() == "private") // private이면 주소에 수정 필요하다
            {
                //publicRepo = false;
                int Index = address.IndexOf("/");
                // id, token 입력한 것 확인하기
                if (textBox3.Text.Equals("") || textBox4.Text.Equals("")) // 둘 중 하나라도 empty이면 안됨
                {
                    textBox5.Text = "Check your id, access token. Invalid input exists.";
                }
                else // 둘 다 입력이 되었음
                {
                    // "https://"github.com, id, token 넣기 위해서 잘라서 넣기
                    repoAddress = "https://" + textBox3.Text + ":" + textBox4.Text + "@" + address.Substring(Index + 2);
                    cloneCmd(path, repoAddress);
                }               
            }
            else // public일 때 그냥 수행
            {
                cloneCmd(path, repoAddress);
            }
                               
        }

        private void button3_Click(object sender, EventArgs e) // 주소 옆에 있는 check button click
        {
            textBox5.Text = string.Empty;
            if(textBox1.Text == "") // repository input nothing
            {
                textBox5.Text = "fatal: You must specify a repository to clone.";
            }
            else
            {
                string pattern = @"^https:\/\/github\.com\/.+\/.+\.git$"; // valid address check
                bool isMatch = System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, pattern);
                if (!isMatch ) { // invalid address
                    textBox5.Text = "fatal: repository \'" + textBox1.Text + "\'" + " does not exist";
                }
                else // valid address, check private or public 
                {
                    comboBox1.Enabled = true; // valid address니까 public/private check 할 수 있도록 checkbox active
                    textBox1.ReadOnly = true;
                    textBox5.Text = "Valid address. Choose your Repository property.";
                    comboBox1.Focus();                  
                }
            }
        }

        private bool repoPublicCheck(string path, string address) // invitation 받지 않은 private repo 확인용
        {
            string[] privateRepo;
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
            process.StandardInput.Write(@"git ls-remote --exit-code --quiet " + address + Environment.NewLine);
            // 명령어를 보낼때는 꼭 마무리를 해줘야 한다. 그래서 마지막에 NewLine가 필요하다  ls-remote --exit-code --quiet
            process.StandardInput.Close();

            string errorOutput = process.StandardError.ReadToEnd();

            privateRepo = errorOutput.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            process.WaitForExit();
            process.Close();

            bool flag = true; 
            foreach(string line in privateRepo)
            {
                if (line.Equals("remote: Repository not found.")) // private이면 error가 출력됨
                {
                    flag = false;
                    textBox5.Text = "Private Repository: Input your ID and Access Token."; // private을 clone 시도할 때 id, token을 입력해야함
                }
            }           
            return flag;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(textBox3.Text.Equals("") || textBox4.Text.Equals("")) // 둘 중 하나라도 empty이면 안됨
            {
                textBox5.Text = "Private Repository: Check your id, access token. Invalid input exists.";
            }
            else // 둘 다 값이 채워져 있을 때
            {
                textBox5.Text = "You can clone " + "\"" + textBox1.Text + "\"" + " in " + textBox2.Text;
                button1.Enabled = true; // clone 버튼 누를 수 있도록 하기
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // combobox의 값이 변경될 때 호출되는 이벤트
            if(comboBox1.SelectedItem.ToString() == "public") //public 선택하면 clone 버튼 활성화
            {
                textBox3.ReadOnly = true;
                textBox4.ReadOnly = true; // id, token 활성화
                button1.Enabled = true; //clone 버튼 활성화
                textBox5.Text = "You can clone " + "\"" + textBox1.Text + "\"" + " in " + textBox2.Text;
                button1.Focus();
            }
            else if(comboBox1.SelectedItem.ToString() == "private") // private 선택하면 id, token 활성화
            {
                textBox3.ReadOnly = false;
                textBox4.ReadOnly = false; // id, token 활성화
                //button4.Enabled = true; // check button, 없애
                button1.Enabled = true; //clone 버튼 활성화
                textBox5.Text = "Input your ID and Access Token to clone private repository.";
                textBox3.Focus();
            }

        }
    }
}
