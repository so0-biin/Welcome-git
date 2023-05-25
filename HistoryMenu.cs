using FileManager.Controls;
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
        private BranchList BranchListView;
        private string currentDirectory;
        public HistoryMenu()
        {
            InitializeComponent();
        }

        public HistoryMenu(string data)
        {
            InitializeComponent();
            currentDirectory = data;

            BranchListView = new BranchList();
            BranchListView.Initialize();
            splitContainer1.Panel1.Controls.Add(BranchListView);

            BranchRefresh();

            button2.Text = "Create";
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
        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
