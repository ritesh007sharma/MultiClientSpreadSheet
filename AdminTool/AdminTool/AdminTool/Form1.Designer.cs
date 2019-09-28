namespace AdminTool
{
    partial class AdminTool
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
            this.ListOfUsers_LABEL = new System.Windows.Forms.Label();
            this.ListOfSpreadsheets_LABEL = new System.Windows.Forms.Label();
            this.AddUser_BUTTON = new System.Windows.Forms.Button();
            this.DeleteUser_BUTTON = new System.Windows.Forms.Button();
            this.Connect_BUTTON = new System.Windows.Forms.Button();
            this.connectToServer_TEXTBOX = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DeleteSpreadsheet_BUTTON = new System.Windows.Forms.Button();
            this.AddSpreadsheet_Button = new System.Windows.Forms.Button();
            this.EditUser_BUTTON = new System.Windows.Forms.Button();
            this.ListOfUsers_LISTBOX = new System.Windows.Forms.ListBox();
            this.ListOfSpreadsheets_LISTBOX = new System.Windows.Forms.ListBox();
            this.ShutDown_BUTTON = new System.Windows.Forms.Button();
            this.selectedUser_LABEL = new System.Windows.Forms.Label();
            this.NumOfConnectedUsers_LABEL = new System.Windows.Forms.Label();
            this.selectedSpreadsheet_LABEL = new System.Windows.Forms.Label();
            this.NewUsername_TEXTBOX = new System.Windows.Forms.TextBox();
            this.NewPassword_TEXTBOX = new System.Windows.Forms.TextBox();
            this.NewUsername_LABEL = new System.Windows.Forms.Label();
            this.NewPassword_Label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ListOfUsers_LABEL
            // 
            this.ListOfUsers_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ListOfUsers_LABEL.AutoSize = true;
            this.ListOfUsers_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOfUsers_LABEL.Location = new System.Drawing.Point(74, 126);
            this.ListOfUsers_LABEL.Name = "ListOfUsers_LABEL";
            this.ListOfUsers_LABEL.Size = new System.Drawing.Size(182, 25);
            this.ListOfUsers_LABEL.TabIndex = 2;
            this.ListOfUsers_LABEL.Text = "LIST OF USERS";
            this.ListOfUsers_LABEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListOfSpreadsheets_LABEL
            // 
            this.ListOfSpreadsheets_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ListOfSpreadsheets_LABEL.AutoSize = true;
            this.ListOfSpreadsheets_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListOfSpreadsheets_LABEL.Location = new System.Drawing.Point(490, 126);
            this.ListOfSpreadsheets_LABEL.Name = "ListOfSpreadsheets_LABEL";
            this.ListOfSpreadsheets_LABEL.Size = new System.Drawing.Size(287, 25);
            this.ListOfSpreadsheets_LABEL.TabIndex = 4;
            this.ListOfSpreadsheets_LABEL.Text = "LIST OF SPREADSHEETS";
            this.ListOfSpreadsheets_LABEL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AddUser_BUTTON
            // 
            this.AddUser_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddUser_BUTTON.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddUser_BUTTON.Location = new System.Drawing.Point(358, 177);
            this.AddUser_BUTTON.Name = "AddUser_BUTTON";
            this.AddUser_BUTTON.Size = new System.Drawing.Size(86, 34);
            this.AddUser_BUTTON.TabIndex = 5;
            this.AddUser_BUTTON.Text = "Add...";
            this.AddUser_BUTTON.UseVisualStyleBackColor = true;
            this.AddUser_BUTTON.Click += new System.EventHandler(this.AddUser_BUTTON_Click);
            // 
            // DeleteUser_BUTTON
            // 
            this.DeleteUser_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DeleteUser_BUTTON.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteUser_BUTTON.Location = new System.Drawing.Point(358, 255);
            this.DeleteUser_BUTTON.Name = "DeleteUser_BUTTON";
            this.DeleteUser_BUTTON.Size = new System.Drawing.Size(86, 34);
            this.DeleteUser_BUTTON.TabIndex = 6;
            this.DeleteUser_BUTTON.Text = "Delete...";
            this.DeleteUser_BUTTON.UseVisualStyleBackColor = true;
            this.DeleteUser_BUTTON.Click += new System.EventHandler(this.DeleteUser_BUTTON_Click);
            // 
            // Connect_BUTTON
            // 
            this.Connect_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Connect_BUTTON.Location = new System.Drawing.Point(311, 23);
            this.Connect_BUTTON.Name = "Connect_BUTTON";
            this.Connect_BUTTON.Size = new System.Drawing.Size(75, 31);
            this.Connect_BUTTON.TabIndex = 7;
            this.Connect_BUTTON.Text = "Connect";
            this.Connect_BUTTON.UseVisualStyleBackColor = true;
            this.Connect_BUTTON.Click += new System.EventHandler(this.Connect_BUTTON_Click);
            // 
            // connectToServer_TEXTBOX
            // 
            this.connectToServer_TEXTBOX.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.connectToServer_TEXTBOX.Location = new System.Drawing.Point(39, 29);
            this.connectToServer_TEXTBOX.Name = "connectToServer_TEXTBOX";
            this.connectToServer_TEXTBOX.Size = new System.Drawing.Size(256, 20);
            this.connectToServer_TEXTBOX.TabIndex = 8;
            this.connectToServer_TEXTBOX.Text = "Enter Server...";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(374, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 9;
            this.label1.Text = "Users";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(349, 332);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Spreadsheets";
            // 
            // DeleteSpreadsheet_BUTTON
            // 
            this.DeleteSpreadsheet_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DeleteSpreadsheet_BUTTON.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSpreadsheet_BUTTON.Location = new System.Drawing.Point(358, 391);
            this.DeleteSpreadsheet_BUTTON.Name = "DeleteSpreadsheet_BUTTON";
            this.DeleteSpreadsheet_BUTTON.Size = new System.Drawing.Size(86, 34);
            this.DeleteSpreadsheet_BUTTON.TabIndex = 11;
            this.DeleteSpreadsheet_BUTTON.Text = "Delete...";
            this.DeleteSpreadsheet_BUTTON.UseVisualStyleBackColor = true;
            this.DeleteSpreadsheet_BUTTON.Click += new System.EventHandler(this.DeleteSpreadsheet_BUTTON_Click);
            // 
            // AddSpreadsheet_Button
            // 
            this.AddSpreadsheet_Button.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddSpreadsheet_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddSpreadsheet_Button.Location = new System.Drawing.Point(358, 351);
            this.AddSpreadsheet_Button.Name = "AddSpreadsheet_Button";
            this.AddSpreadsheet_Button.Size = new System.Drawing.Size(86, 34);
            this.AddSpreadsheet_Button.TabIndex = 10;
            this.AddSpreadsheet_Button.Text = "Add...";
            this.AddSpreadsheet_Button.UseVisualStyleBackColor = true;
            this.AddSpreadsheet_Button.Click += new System.EventHandler(this.AddSpreadsheet_Button_Click);
            // 
            // EditUser_BUTTON
            // 
            this.EditUser_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.EditUser_BUTTON.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EditUser_BUTTON.Location = new System.Drawing.Point(357, 216);
            this.EditUser_BUTTON.Name = "EditUser_BUTTON";
            this.EditUser_BUTTON.Size = new System.Drawing.Size(86, 34);
            this.EditUser_BUTTON.TabIndex = 13;
            this.EditUser_BUTTON.Text = "Edit...";
            this.EditUser_BUTTON.UseVisualStyleBackColor = true;
            this.EditUser_BUTTON.Click += new System.EventHandler(this.EditUser_BUTTON_Click);
            // 
            // ListOfUsers_LISTBOX
            // 
            this.ListOfUsers_LISTBOX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ListOfUsers_LISTBOX.FormattingEnabled = true;
            this.ListOfUsers_LISTBOX.Location = new System.Drawing.Point(12, 154);
            this.ListOfUsers_LISTBOX.Name = "ListOfUsers_LISTBOX";
            this.ListOfUsers_LISTBOX.Size = new System.Drawing.Size(308, 277);
            this.ListOfUsers_LISTBOX.TabIndex = 14;
            this.ListOfUsers_LISTBOX.Click += new System.EventHandler(this.ListOfUsers_LISTBOX_Click);
            // 
            // ListOfSpreadsheets_LISTBOX
            // 
            this.ListOfSpreadsheets_LISTBOX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.ListOfSpreadsheets_LISTBOX.FormattingEnabled = true;
            this.ListOfSpreadsheets_LISTBOX.Location = new System.Drawing.Point(478, 154);
            this.ListOfSpreadsheets_LISTBOX.Name = "ListOfSpreadsheets_LISTBOX";
            this.ListOfSpreadsheets_LISTBOX.Size = new System.Drawing.Size(308, 277);
            this.ListOfSpreadsheets_LISTBOX.TabIndex = 15;
            this.ListOfSpreadsheets_LISTBOX.Click += new System.EventHandler(this.ListOfSpreadsheets_LISTBOX_Click);
            // 
            // ShutDown_BUTTON
            // 
            this.ShutDown_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ShutDown_BUTTON.Enabled = false;
            this.ShutDown_BUTTON.Location = new System.Drawing.Point(392, 23);
            this.ShutDown_BUTTON.Name = "ShutDown_BUTTON";
            this.ShutDown_BUTTON.Size = new System.Drawing.Size(75, 31);
            this.ShutDown_BUTTON.TabIndex = 16;
            this.ShutDown_BUTTON.Text = "Shut Down";
            this.ShutDown_BUTTON.UseVisualStyleBackColor = true;
            this.ShutDown_BUTTON.Click += new System.EventHandler(this.ShutDown_BUTTON_Click);
            // 
            // selectedUser_LABEL
            // 
            this.selectedUser_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.selectedUser_LABEL.AutoSize = true;
            this.selectedUser_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedUser_LABEL.Location = new System.Drawing.Point(12, 434);
            this.selectedUser_LABEL.Name = "selectedUser_LABEL";
            this.selectedUser_LABEL.Size = new System.Drawing.Size(99, 13);
            this.selectedUser_LABEL.TabIndex = 19;
            this.selectedUser_LABEL.Text = "Selected User : ";
            // 
            // NumOfConnectedUsers_LABEL
            // 
            this.NumOfConnectedUsers_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.NumOfConnectedUsers_LABEL.AutoSize = true;
            this.NumOfConnectedUsers_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumOfConnectedUsers_LABEL.Location = new System.Drawing.Point(473, 23);
            this.NumOfConnectedUsers_LABEL.Name = "NumOfConnectedUsers_LABEL";
            this.NumOfConnectedUsers_LABEL.Size = new System.Drawing.Size(158, 13);
            this.NumOfConnectedUsers_LABEL.TabIndex = 20;
            this.NumOfConnectedUsers_LABEL.Text = "Num Of Connected Users: ";
            // 
            // selectedSpreadsheet_LABEL
            // 
            this.selectedSpreadsheet_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.selectedSpreadsheet_LABEL.AutoSize = true;
            this.selectedSpreadsheet_LABEL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedSpreadsheet_LABEL.Location = new System.Drawing.Point(475, 434);
            this.selectedSpreadsheet_LABEL.Name = "selectedSpreadsheet_LABEL";
            this.selectedSpreadsheet_LABEL.Size = new System.Drawing.Size(144, 13);
            this.selectedSpreadsheet_LABEL.TabIndex = 21;
            this.selectedSpreadsheet_LABEL.Text = "Selected Spreadsheet : ";
            // 
            // NewUsername_TEXTBOX
            // 
            this.NewUsername_TEXTBOX.Location = new System.Drawing.Point(311, 71);
            this.NewUsername_TEXTBOX.Name = "NewUsername_TEXTBOX";
            this.NewUsername_TEXTBOX.Size = new System.Drawing.Size(100, 20);
            this.NewUsername_TEXTBOX.TabIndex = 22;
            // 
            // NewPassword_TEXTBOX
            // 
            this.NewPassword_TEXTBOX.Location = new System.Drawing.Point(311, 97);
            this.NewPassword_TEXTBOX.Name = "NewPassword_TEXTBOX";
            this.NewPassword_TEXTBOX.Size = new System.Drawing.Size(100, 20);
            this.NewPassword_TEXTBOX.TabIndex = 23;
            // 
            // NewUsername_LABEL
            // 
            this.NewUsername_LABEL.AutoSize = true;
            this.NewUsername_LABEL.Location = new System.Drawing.Point(418, 72);
            this.NewUsername_LABEL.Name = "NewUsername_LABEL";
            this.NewUsername_LABEL.Size = new System.Drawing.Size(92, 13);
            this.NewUsername_LABEL.TabIndex = 24;
            this.NewUsername_LABEL.Text = "<- New Username";
            // 
            // NewPassword_Label
            // 
            this.NewPassword_Label.AutoSize = true;
            this.NewPassword_Label.Location = new System.Drawing.Point(418, 100);
            this.NewPassword_Label.Name = "NewPassword_Label";
            this.NewPassword_Label.Size = new System.Drawing.Size(90, 13);
            this.NewPassword_Label.TabIndex = 25;
            this.NewPassword_Label.Text = "<- New Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "New Spreadsheet ->";
            // 
            // AdminTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NewPassword_Label);
            this.Controls.Add(this.NewUsername_LABEL);
            this.Controls.Add(this.NewPassword_TEXTBOX);
            this.Controls.Add(this.NewUsername_TEXTBOX);
            this.Controls.Add(this.selectedSpreadsheet_LABEL);
            this.Controls.Add(this.NumOfConnectedUsers_LABEL);
            this.Controls.Add(this.selectedUser_LABEL);
            this.Controls.Add(this.ShutDown_BUTTON);
            this.Controls.Add(this.ListOfSpreadsheets_LISTBOX);
            this.Controls.Add(this.ListOfUsers_LISTBOX);
            this.Controls.Add(this.EditUser_BUTTON);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DeleteSpreadsheet_BUTTON);
            this.Controls.Add(this.AddSpreadsheet_Button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectToServer_TEXTBOX);
            this.Controls.Add(this.Connect_BUTTON);
            this.Controls.Add(this.DeleteUser_BUTTON);
            this.Controls.Add(this.AddUser_BUTTON);
            this.Controls.Add(this.ListOfSpreadsheets_LABEL);
            this.Controls.Add(this.ListOfUsers_LABEL);
            this.Name = "AdminTool";
            this.Text = "AdminTool";
            this.Load += new System.EventHandler(this.AdminTool_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label ListOfUsers_LABEL;
        private System.Windows.Forms.Label ListOfSpreadsheets_LABEL;
        private System.Windows.Forms.Button AddUser_BUTTON;
        private System.Windows.Forms.Button DeleteUser_BUTTON;
        private System.Windows.Forms.Button Connect_BUTTON;
        private System.Windows.Forms.TextBox connectToServer_TEXTBOX;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DeleteSpreadsheet_BUTTON;
        private System.Windows.Forms.Button AddSpreadsheet_Button;
        private System.Windows.Forms.Button EditUser_BUTTON;
        private System.Windows.Forms.ListBox ListOfUsers_LISTBOX;
        private System.Windows.Forms.ListBox ListOfSpreadsheets_LISTBOX;
        private System.Windows.Forms.Button ShutDown_BUTTON;
        private System.Windows.Forms.Label selectedUser_LABEL;
        private System.Windows.Forms.Label NumOfConnectedUsers_LABEL;
        private System.Windows.Forms.Label selectedSpreadsheet_LABEL;
        private System.Windows.Forms.TextBox NewUsername_TEXTBOX;
        private System.Windows.Forms.TextBox NewPassword_TEXTBOX;
        private System.Windows.Forms.Label NewUsername_LABEL;
        private System.Windows.Forms.Label NewPassword_Label;
        private System.Windows.Forms.Label label3;
    }
}

