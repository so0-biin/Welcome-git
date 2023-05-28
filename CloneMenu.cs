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
    public partial class CloneMenu : Form
    {
        private string path;
        
        public CloneMenu()
        {
            InitializeComponent();

            textBox2.ReadOnly = true;
            textBox3.ReadOnly = true;
            textBox4.ReadOnly = true;
            textBox5.ReadOnly = true;
            button1.Enabled = false;
            label1.Text = "Input Git repository address to CLONE";
            label2.Text = "Destination Path";
            label3.Text = "ID";
            label4.Text = "Access Token";
            label5.Text = "Check !";
            button1.Text = "Clone";
            button2.Text = "Exit";
            button3.Text = "Check";
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void SetPathBeforeCommit(string path)
        {
            this.path = path;
            textBox2.Text = path;   
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool validRepoAddress;
            bool validId;
            bool validAccessToken;
            validRepoAddress = repoAddressCheck();
            validId = idCheck();
            validAccessToken = accessTokenCheck();

            /* implementation 
 1. 입력받은 id, token 넣어서 명령어 입력
2. git clone 받기
3. id, access token 어딘가에 저장
 */

            if (!validRepoAddress) { 
                
            }
        }

        private bool repoAddressCheck() 
        {
            // repo address valid address check
            string pattern = @"^https:\/\/github\.com\/.+\/.+\.git$";
            bool isMatch = System.Text.RegularExpressions.Regex.IsMatch(textBox1.Text, pattern);

            return isMatch;
        }

        private bool idCheck()
        {
            bool isMatch = false;
            return isMatch;
        }

        private bool accessTokenCheck()
        {
            bool isMatch = false;   
            return isMatch;    
        }

        private void button3_Click(object sender, EventArgs e)
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
                    bool isPublic = repoPublicCheck(path, textBox1.Text);
                    if( isPublic ) // public repository
                    {
                        button1.Enabled = true; // clone button active
                        textBox5.Text += "Public Repository: You can clone " + "\"" + textBox1.Text + "\"" + " in " + textBox2.Text;
                        
                        button1.Focus();
                        textBox1.ReadOnly = true;
                    }
                    else // private repository
                    {
                        button1.Enabled = false;
                        textBox1.ReadOnly = true;
                        textBox3.ReadOnly = false; // input id
                        textBox4.ReadOnly = false; // input access token
                        textBox3.Focus();
                    }
                }
            }
        }

        private bool repoPublicCheck(string path, string address)
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
                    textBox5.Text += "Private Repository: Input your ID and Access Token.\n"; // private을 clone 시도할 때 id, token을 입력해야함
                }
            }           
            return flag;
        }                   
    }
}
