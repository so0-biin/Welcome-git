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
    public partial class CommitMenu : Form
    {
        public CommitMenu()
        {
            InitializeComponent();

            label1.Text = "The list of Staged Changes";
            label2.Text = "Enter a commit message";
            button1.Text = "Commit";    
            button2.Text = "Exit";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
