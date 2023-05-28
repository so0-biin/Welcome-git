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

        

        

        void cmd_ex(String real_path, String file_name, String command)
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
            process.StandardInput.Write(@"git " + command + " " + file_name + Environment.NewLine);

            process.StandardInput.Close(); // cmd  명령 입력 끝

            process.WaitForExit();
            process.Close(); // cmd 창을 닫음

            this.Items.Clear();
            this.LargeImageList.Images.Clear();
            this.SmallImageList.Images.Clear();

        }
    }
}
