namespace FileManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            //this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            //this.EditMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.CurrentDirectoryLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.CurrentDirectory = new System.Windows.Forms.ToolStripStatusLabel();
            this.SplitContainer = new System.Windows.Forms.SplitContainer();
            this.NavigationPanel = new FileManager.Controls.Navigation();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ViewDetails = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLargeIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewList = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewTiles = new System.Windows.Forms.ToolStripMenuItem();
            //this.ViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuStrip.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.Panel1.SuspendLayout();
            this.SplitContainer.Panel2.SuspendLayout();
            this.SplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            //this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.FileMenuItem,
            //this.EditMenuItem,
            //this.ViewMenuItem});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MenuStrip.Size = new System.Drawing.Size(935, 24);
            this.MenuStrip.TabIndex = 0;
            this.MenuStrip.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            //this.FileMenuItem.Name = "FileMenuItem";
            //this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            //this.FileMenuItem.Text = "&File";
            // 
            // EditMenuItem
            // 
            //this.EditMenuItem.Name = "EditMenuItem";
            //this.EditMenuItem.Size = new System.Drawing.Size(39, 20);
            //this.EditMenuItem.Text = "&Edit";
            // 
            // StatusStrip
            // 
            this.StatusStrip.BackColor = System.Drawing.SystemColors.Control;
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurrentDirectoryLabel,
            this.CurrentDirectory});
            this.StatusStrip.Location = new System.Drawing.Point(0, 393);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 17, 0);
            this.StatusStrip.Size = new System.Drawing.Size(935, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // CurrentDirectoryLabel
            // 
            this.CurrentDirectoryLabel.AutoSize = false;
            this.CurrentDirectoryLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.CurrentDirectoryLabel.Name = "CurrentDirectoryLabel";
            this.CurrentDirectoryLabel.Size = new System.Drawing.Size(101, 17);
            this.CurrentDirectoryLabel.Text = "Current Directory:";
            // 
            // CurrentDirectory
            // 
            this.CurrentDirectory.AutoSize = false;
            this.CurrentDirectory.Name = "CurrentDirectory";
            this.CurrentDirectory.Size = new System.Drawing.Size(680, 17);
            this.CurrentDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplitContainer
            // 
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 24);
            this.SplitContainer.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.SplitContainer.Name = "SplitContainer";
            // 
            // SplitContainer.Panel1
            // 
            this.SplitContainer.Panel1.Controls.Add(this.NavigationPanel);
            // 
            // SplitContainer.Panel2
            // 
            this.SplitContainer.Panel2.AutoScroll = true;
            this.SplitContainer.Panel2.Controls.Add(this.textBox1);
            this.SplitContainer.Panel2.MouseHover += new System.EventHandler(this.Button1_MouseHover);
            this.SplitContainer.Size = new System.Drawing.Size(935, 369);
            this.SplitContainer.SplitterDistance = 229;
            this.SplitContainer.TabIndex = 2;
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.NavigationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NavigationPanel.Location = new System.Drawing.Point(0, 0);
            this.NavigationPanel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(229, 369);
            this.NavigationPanel.TabIndex = 0;
            this.NavigationPanel.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.NavigationPanel_AfterSelect);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 297);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(702, 72);
            this.textBox1.TabIndex = 5;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(710, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(107, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseHover += new System.EventHandler(this.Button1_MouseHover);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(822, 0);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(107, 22);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ViewDetails
            // 
            this.ViewDetails.Checked = true;
            this.ViewDetails.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ViewDetails.Name = "ViewDetails";
            this.ViewDetails.Size = new System.Drawing.Size(180, 22);
            this.ViewDetails.Text = "Details";
            // 
            // ViewLargeIcons
            // 
            this.ViewLargeIcons.Name = "ViewLargeIcons";
            this.ViewLargeIcons.Size = new System.Drawing.Size(180, 22);
            this.ViewLargeIcons.Text = "Large icons";
            // 
            // ViewList
            // 
            this.ViewList.Name = "ViewList";
            this.ViewList.Size = new System.Drawing.Size(180, 22);
            this.ViewList.Text = "List";
            // 
            // ViewSmallIcons
            // 
            this.ViewSmallIcons.Name = "ViewSmallIcons";
            this.ViewSmallIcons.Size = new System.Drawing.Size(180, 22);
            this.ViewSmallIcons.Text = "Small icons";
            // 
            // ViewTiles
            // 
            this.ViewTiles.Name = "ViewTiles";
            this.ViewTiles.Size = new System.Drawing.Size(180, 22);
            this.ViewTiles.Text = "Tiles";
            // 
            // ViewMenuItem
            // 
            //this.ViewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            //this.ViewDetails,
            //this.ViewLargeIcons,
            //this.ViewList,
            //this.ViewSmallIcons,
            //this.ViewTiles});
            //this.ViewMenuItem.Name = "ViewMenuItem";
            //this.ViewMenuItem.Size = new System.Drawing.Size(45, 20);
            //this.ViewMenuItem.Text = "&View";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 415);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.MinimumSize = new System.Drawing.Size(951, 238);
            this.Name = "Form1";
            this.Text = "File Manager";
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.SplitContainer.Panel1.ResumeLayout(false);
            this.SplitContainer.Panel2.ResumeLayout(false);
            this.SplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel CurrentDirectoryLabel;
        private System.Windows.Forms.ToolStripStatusLabel CurrentDirectory;
        private System.Windows.Forms.SplitContainer SplitContainer;
        private Controls.Navigation NavigationPanel;
        private System.Windows.Forms.ToolStripMenuItem EditMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem ViewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ViewDetails;
        private System.Windows.Forms.ToolStripMenuItem ViewLargeIcons;
        private System.Windows.Forms.ToolStripMenuItem ViewList;
        private System.Windows.Forms.ToolStripMenuItem ViewSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem ViewTiles;
    }
}

