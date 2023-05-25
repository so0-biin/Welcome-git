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
    public partial class HistoryMenu : Form
    {
        private string currentDirectory;
        public HistoryMenu()
        {
            InitializeComponent();
        }

        public HistoryMenu(string data)
        {
            InitializeComponent();
            currentDirectory = data;
            Console.WriteLine(currentDirectory);
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
    }
}
