using System.Drawing;

namespace SpreadsheetGUI
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
            this.spreadsheetPanel1 = new SS.SpreadsheetPanel();
            this.ContentBox = new System.Windows.Forms.TextBox();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.CellAddress = new System.Windows.Forms.TextBox();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Cell = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.usernameBox = new System.Windows.Forms.TextBox();
            this.passwordBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ipBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ListPrompt = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filenameBox = new System.Windows.Forms.TextBox();
            this.newfilename = new System.Windows.Forms.Label();
            this.filenamebutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // spreadsheetPanel1
            // 
            this.spreadsheetPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel1.Location = new System.Drawing.Point(0, 69);
            this.spreadsheetPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.spreadsheetPanel1.Name = "spreadsheetPanel1";
            this.spreadsheetPanel1.Size = new System.Drawing.Size(893, 410);
            this.spreadsheetPanel1.TabIndex = 0;
            this.spreadsheetPanel1.Visible = false;
            this.spreadsheetPanel1.Load += new System.EventHandler(this.spreadsheetPanel1_Load);
            // 
            // ContentBox
            // 
            this.ContentBox.AcceptsReturn = true;
            this.ContentBox.AcceptsTab = true;
            this.ContentBox.Location = new System.Drawing.Point(176, 44);
            this.ContentBox.Name = "ContentBox";
            this.ContentBox.Size = new System.Drawing.Size(159, 20);
            this.ContentBox.TabIndex = 0;
            this.ContentBox.Visible = false;
            this.ContentBox.TextChanged += new System.EventHandler(this.ContentBox_TextChanged);
            this.ContentBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContentBox_KeyDown);
            this.ContentBox.Leave += new System.EventHandler(this.ContentBox_Leave);
            this.ContentBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.ContentBox_KeyDown);
            // 
            // ValueBox
            // 
            this.ValueBox.Location = new System.Drawing.Point(70, 44);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.ReadOnly = true;
            this.ValueBox.Size = new System.Drawing.Size(100, 20);
            this.ValueBox.TabIndex = 5;
            this.ValueBox.Visible = false;
            // 
            // CellAddress
            // 
            this.CellAddress.Location = new System.Drawing.Point(0, 44);
            this.CellAddress.Name = "CellAddress";
            this.CellAddress.ReadOnly = true;
            this.CellAddress.Size = new System.Drawing.Size(64, 20);
            this.CellAddress.TabIndex = 6;
            this.CellAddress.Visible = false;
            this.CellAddress.TextChanged += new System.EventHandler(this.CellAddress_TextChanged);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // Cell
            // 
            this.Cell.AutoSize = true;
            this.Cell.Location = new System.Drawing.Point(20, 28);
            this.Cell.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Cell.Name = "Cell";
            this.Cell.Size = new System.Drawing.Size(24, 13);
            this.Cell.TabIndex = 8;
            this.Cell.Text = "Cell";
            this.Cell.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Value";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(240, 28);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Content";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Username";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Password";
            this.label4.Visible = false;
            // 
            // usernameBox
            // 
            this.usernameBox.AcceptsReturn = true;
            this.usernameBox.AcceptsTab = true;
            this.usernameBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.usernameBox.Location = new System.Drawing.Point(82, 33);
            this.usernameBox.Name = "usernameBox";
            this.usernameBox.Size = new System.Drawing.Size(109, 20);
            this.usernameBox.TabIndex = 13;
            this.usernameBox.Visible = false;
            this.usernameBox.TextChanged += new System.EventHandler(this.usernameBox_TextChanged);
            // 
            // passwordBox
            // 
            this.passwordBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.passwordBox.Location = new System.Drawing.Point(82, 73);
            this.passwordBox.Name = "passwordBox";
            this.passwordBox.Size = new System.Drawing.Size(109, 20);
            this.passwordBox.TabIndex = 14;
            this.passwordBox.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(170, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ipBox
            // 
            this.ipBox.Location = new System.Drawing.Point(59, 35);
            this.ipBox.Name = "ipBox";
            this.ipBox.Size = new System.Drawing.Size(100, 20);
            this.ipBox.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 35);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "IP";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(23, 110);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(432, 303);
            this.listBox1.TabIndex = 18;
            this.listBox1.Visible = false;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F);
            this.label6.Location = new System.Drawing.Point(824, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 9);
            this.label6.TabIndex = 20;
            this.label6.Text = "Not Connected";
            // 
            // ListPrompt
            // 
            this.ListPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ListPrompt.AutoSize = true;
            this.ListPrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.ListPrompt.Location = new System.Drawing.Point(711, 149);
            this.ListPrompt.Name = "ListPrompt";
            this.ListPrompt.Size = new System.Drawing.Size(0, 13);
            this.ListPrompt.TabIndex = 21;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(893, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filenameBox
            // 
            this.filenameBox.Location = new System.Drawing.Point(646, 110);
            this.filenameBox.Name = "filenameBox";
            this.filenameBox.Size = new System.Drawing.Size(100, 20);
            this.filenameBox.TabIndex = 22;
            this.filenameBox.Visible = false;
            // 
            // newfilename
            // 
            this.newfilename.AutoSize = true;
            this.newfilename.Location = new System.Drawing.Point(584, 113);
            this.newfilename.Name = "newfilename";
            this.newfilename.Size = new System.Drawing.Size(48, 13);
            this.newfilename.TabIndex = 23;
            this.newfilename.Text = "New File";
            this.newfilename.Visible = false;
            // 
            // filenamebutton
            // 
            this.filenamebutton.Location = new System.Drawing.Point(762, 110);
            this.filenamebutton.Name = "filenamebutton";
            this.filenamebutton.Size = new System.Drawing.Size(75, 23);
            this.filenamebutton.TabIndex = 24;
            this.filenamebutton.Text = "Create";
            this.filenamebutton.UseVisualStyleBackColor = true;
            this.filenamebutton.Visible = false;
            this.filenamebutton.Click += new System.EventHandler(this.filenamebutton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 496);
            this.Controls.Add(this.filenamebutton);
            this.Controls.Add(this.newfilename);
            this.Controls.Add(this.filenameBox);
            this.Controls.Add(this.ListPrompt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ipBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.passwordBox);
            this.Controls.Add(this.usernameBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cell);
            this.Controls.Add(this.CellAddress);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.ContentBox);
            this.Controls.Add(this.spreadsheetPanel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel1;
        private SS.AbstractSpreadsheet sheet;
        private System.Windows.Forms.TextBox ContentBox;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.TextBox CellAddress;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label Cell;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

        // Array containing the list of colors that are toggled
        public Color [] colors = new Color [] { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple };
        // Integers showing the current location in the color array
        public int currentColor = 0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox usernameBox;
        private System.Windows.Forms.TextBox passwordBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ipBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label ListPrompt;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TextBox filenameBox;
        private System.Windows.Forms.Label newfilename;
        private System.Windows.Forms.Button filenamebutton;
    }
}

