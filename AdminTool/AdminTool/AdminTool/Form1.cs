using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AC;

namespace AdminTool
{
    public partial class AdminTool : Form
    {
        //Testing information
        List<string> currentUsers = new List<string>();
        List<string> currentSpreadsheets = new List<string>();
        string selectedUser;
        string selectedSpreadsheet;
        string selectedUserDefault = "Selected User : ";
        string selectedSpreadsheetDefault = "Selected Spreadsheet : ";

        private AdminController controller;

        /// <summary>
        /// Clear textboxes and load up all users and spreadsheets
        /// </summary>
        /// <param name="files"></param>
        private void loadUsersAndSpreadsheets(List<string> Users, List<string> Spreadsheets)
        {
            ClearListBoxes();
            this.Invoke(new MethodInvoker(() => {
                foreach (string s in Users)
                {
                    currentUsers.Add(s);
                    ListOfUsers_LISTBOX.Items.Add(s);
                }
                foreach (string s in Spreadsheets)
                {
                    currentSpreadsheets.Add(s);
                    ListOfSpreadsheets_LISTBOX.Items.Add(s);
                }
            }));

        }

        private void LoadSpreadsheets(List<string> Spreadsheets)
        {
            this.Invoke(new MethodInvoker(() => {
                ListOfSpreadsheets_LISTBOX.Items.Clear();
                foreach (string s in Spreadsheets)
                {
                    currentSpreadsheets.Add(s);
                    ListOfSpreadsheets_LISTBOX.Items.Add(s);
                }
            }));
        }

        private void LoadUsers(Dictionary<string,string> users)
        {
            this.Invoke(new MethodInvoker(() => {
                ListOfUsers_LISTBOX.Items.Clear();
                foreach (string s in users.Keys)
                {
                    currentUsers.Add(s);
                    ListOfUsers_LISTBOX.Items.Add(s);
                }
            }));
        }

        private void ClearListBoxes()
        {
            this.Invoke(new MethodInvoker(() => {
                ListOfUsers_LISTBOX.Items.Clear();
                ListOfSpreadsheets_LISTBOX.Items.Clear();
            }));
        }

        public AdminTool()
        {
            controller = new AdminController();
            InitializeComponent();
            controller.SendSetupToView += LoadSpreadsheets;
            controller.UpdateSpreadsheet += LoadUsers;
            controller.ResetConnection += HardReset;
            controller.InformViewConnectionFailed += Connectionfailed;
            //controller.SendSetupToView += loadUsersAndSpreadsheets;

        }

        private void AdminTool_Load(object sender, EventArgs e)
        {

        }

        private void ListOfUsers_CHECKBOX_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void HardReset()
        {
            MessageBox.Show("Connection to Server Lost...");
            this.Invoke(new MethodInvoker(() => {
                ShutDown_BUTTON.Enabled = false;
                Connect_BUTTON.Enabled = true;
                ListOfSpreadsheets_LISTBOX.Items.Clear();
                ListOfUsers_LISTBOX.Items.Clear();
            }));
            
        }
        
        public void Connectionfailed()
        {
            MessageBox.Show("Connection Failed..");
        }

        #region Clicking UI

        /// <summary>
        /// Update the selected user labe at the bottom of the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfUsers_LISTBOX_Click(object sender, EventArgs e)
        {
            if (ListOfUsers_LISTBOX.SelectedItem != null)
            {
                selectedUser = ListOfUsers_LISTBOX.SelectedItem.ToString();
                selectedUser_LABEL.Text = selectedUserDefault + selectedUser;
            }
        }

        /// <summary>
        /// Update the selected spreadsheet label at the bottom of the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListOfSpreadsheets_LISTBOX_Click(object sender, EventArgs e)
        {
            if (ListOfSpreadsheets_LISTBOX.SelectedItem != null)
            {
                selectedSpreadsheet = ListOfSpreadsheets_LISTBOX.SelectedItem.ToString();
                selectedSpreadsheet_LABEL.Text = selectedSpreadsheetDefault + selectedSpreadsheet;
            }
        }

        /// <summary>
        /// Delete a user from the Server.
        /// Check if there is a selected user. If there is then delete the selected user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteUser_BUTTON_Click(object sender, EventArgs e)
        {
            if (ListOfUsers_LISTBOX.SelectedItem != null)
            {
                controller.DeleteThisUser(selectedUser);
                ListOfUsers_LISTBOX.Items.Remove(ListOfUsers_LISTBOX.SelectedItem);
                currentUsers.Remove(selectedUser);
                selectedUser_LABEL.Text = selectedUserDefault;
            }
        }

        /// <summary>
        /// Delete a spreadsheet from the server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteSpreadsheet_BUTTON_Click(object sender, EventArgs e)
        {
            if (ListOfSpreadsheets_LISTBOX.SelectedItem != null)
            {
                controller.DeleteThisSpreadsheet(selectedSpreadsheet);
                ListOfSpreadsheets_LISTBOX.Items.Remove(ListOfSpreadsheets_LISTBOX.SelectedItem);
                currentSpreadsheets.Remove(selectedSpreadsheet);
                selectedSpreadsheet_LABEL.Text = selectedSpreadsheetDefault;
            }
        }

        private void Connect_BUTTON_Click(object sender, EventArgs e)
        {
            try
            {
                controller.Connect(connectToServer_TEXTBOX.Text);

                Connect_BUTTON.Enabled = false;
                connectToServer_TEXTBOX.ReadOnly = true;
                controller.RequestUsers();
                ShutDown_BUTTON.Enabled = true;
            }
            catch (Exception)
            {
                Connect_BUTTON.Enabled = true;
                connectToServer_TEXTBOX.ReadOnly = false;
                ShutDown_BUTTON.Enabled = false;
            }
        }

        private void AddUser_BUTTON_Click(object sender, EventArgs e)
        {
            if((NewUsername_TEXTBOX.Text != "") && (NewPassword_TEXTBOX.Text != "") )
            {
                string spreadsheet = "blank"; 
                if(ListOfSpreadsheets_LISTBOX.Items[0].ToString() != null)
                {
                    spreadsheet = ListOfSpreadsheets_LISTBOX.Items[0].ToString();
                }
                controller.AddNewUser(NewUsername_TEXTBOX.Text, NewPassword_TEXTBOX.Text, spreadsheet);
                currentUsers.Add(NewUsername_TEXTBOX.Text);
                ListOfUsers_LISTBOX.Items.Add(NewUsername_TEXTBOX.Text);
                NewUsername_TEXTBOX.Text = "";
                NewPassword_TEXTBOX.Text = "";
            }
        }

        private void EditUser_BUTTON_Click(object sender, EventArgs e)
        {
            string spreadsheet = "blank";
            bool mustdelete = true;
            if (ListOfSpreadsheets_LISTBOX.Items.Count != 0)
            {
                spreadsheet = ListOfSpreadsheets_LISTBOX.Items[0].ToString();
                mustdelete = false;
            }
            if ((NewUsername_TEXTBOX.Text != "") && (NewPassword_TEXTBOX.Text != "") && (ListOfUsers_LISTBOX.SelectedItem != null))
            {
                controller.EditThisUser(selectedUser, NewUsername_TEXTBOX.Text, NewPassword_TEXTBOX.Text, spreadsheet);
                ListOfUsers_LISTBOX.Items.Remove(selectedUser);
                ListOfUsers_LISTBOX.Items.Add(NewUsername_TEXTBOX.Text);
                selectedUser = NewUsername_TEXTBOX.Text;
                NewUsername_TEXTBOX.Text = "";
                NewPassword_TEXTBOX.Text = "";
            }
            if(mustdelete)
            {
                controller.DeleteThisSpreadsheet("blank");
            }
        }

        private void AddSpreadsheet_Button_Click(object sender, EventArgs e)
        {
            if ((NewUsername_TEXTBOX.Text != ""))
            {
                controller.AddThisSpreadsheet(NewUsername_TEXTBOX.Text);
                currentSpreadsheets.Add(NewUsername_TEXTBOX.Text);
                ListOfSpreadsheets_LISTBOX.Items.Add(NewUsername_TEXTBOX.Text);
                NewUsername_TEXTBOX.Text = "";
            }
        }

        #endregion

        private void ShutDown_BUTTON_Click(object sender, EventArgs e)
        {
            controller.ShutDownThisServer();
            ShutDown_BUTTON.Enabled = false;
            Connect_BUTTON.Enabled = true;
            ListOfSpreadsheets_LISTBOX.Items.Clear();
            ListOfUsers_LISTBOX.Items.Clear();
        }
    }
    
}
