using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            label1.Text = "Input Git repository address to CLONE";
            label2.Text = "Destination Path";
            label3.Text = "ID";
            label4.Text = "Access Token";
            button1.Text = "Clone";
            button2.Text = "Exit";
            
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
    }
}
