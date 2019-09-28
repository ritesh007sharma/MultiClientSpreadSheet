using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpreadsheetUtilities;
using SS;
using SC;

/// <summary>
/// Authors James Lee, Ryan Furukawa
/// </summary>

namespace SpreadsheetGUI
{
    /// <summary>
    /// Form showing the spreadsheet program
    /// </summary>
    public partial class Form1 : Form
    {

        // String to store path name for use to open and save
        // stays stored if the spreadsheet has been opened or saved at least once
        private String pathName = "";
        private String prevCell = "";
        private String prevContent;
        

        private SheetController controller;

        /// <summary>
        /// Method to convert the coordinates of selections to a variable name
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private static string columnString(int column)
        {
            return ((column < 26) ? "" : columnString(column / 26 - 1)) + Convert.ToChar('A' + column % 26);
        }

        private static string getCellName(int column, int row)
        {
            return columnString(column) + (row + 1);
        }

        /// <summary>
        /// Sets the numeric coordinates of a cell given the cell name
        /// </summary>
        /// <param name="CellName"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        private static void setCoordinates(string CellName, out int column, out int row)
        {
            column = 0;
            int letterCount = 0;

            foreach (char c in CellName)
            {
                // Exit the for loop if c is a digit
                if (Char.IsDigit(c))
                    break;

                // Increment the column value by the addition letter in the address
                column += (column * 26 + (int)(c - 'A'));
                letterCount++;
            }

            // Set row to a parsed (double) substring of the Cellname (without letters)
            if (!Int32.TryParse(CellName.Substring(letterCount), out row))
                row = 1;

            // Subtract row by 1 to...
            row -= 1;
        }

        /// <summary>
        /// Constructor for the form showing the spreadsheet program
        /// </summary>
        public Form1()
        {
           

            controller = new SheetController();

            // New spreadsheet for display
            sheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");
            InitializeComponent();
            //spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
            this.ActiveControl = usernameBox;
            ContentBox.ReadOnly = true;
            controller.SendSetupToView += loadFiles;
            controller.UpdateSpreadsheet += updateSpreadsheet;
            controller.ErrorCodeOne += invalidAuthorization;
            controller.ErrorCodeTwo += circularDependency;
            controller.ResetConnection += resetConnection;

         


            // Set the current cell in the panel to A1
            //  spreadsheetPanel1.SetSelection(0, 0);
            // Update the view of the panel
            //  OnSelectionChanged(spreadsheetPanel1);
        }


        /// <summary>
        /// adding files to listbox1.
        /// </summary>
        /// <param name="files"></param>
        private void loadFiles(List<string> files)
        {
            this.Invoke(new MethodInvoker(() => {
                listBox1.Items.Clear();
                foreach (string s in files)
                {
                    listBox1.Items.Add(s);
                }
                if (listBox1.Items.Count > 0)
                {
                    label6.Text = "Connected";
                }
                
            }));
            
        }

        private void updateSpreadsheet(Dictionary<string, string> cells)
        {
           this.Invoke(new MethodInvoker(() => {
               lock (sheet)
               {
                   if (cells != null)
                   {
                       foreach (KeyValuePair<string, string> cell in cells)
                       {
                           //sheet.SetContentsOfCell(cell.Key, cell.Value);
                           foreach (string changedCells in sheet.SetContentsOfCell(cell.Key, cell.Value))
                           {
                               // Get the coordinates of the cell that needs to be updated
                               setCoordinates(changedCells, out int c, out int r);
                               // Update the panel to show the recalculated cell
                               spreadsheetPanel1.SetValue(c, r, sheet.GetCellValue(changedCells).ToString());
                           }

                           setCoordinates(cell.Key, out int col, out int row);
                           spreadsheetPanel1.SetValue(col, row, sheet.GetCellValue(cell.Key).ToString());
                           // OnSelectionChanged(spreadsheetPanel1);
                       }
                       ContentBox.Text = sheet.GetCellContents(CellAddress.Text).ToString();
                       ValueBox.Text = sheet.GetCellContents(CellAddress.Text).ToString();
                   }
               }
            }));

        }

        public void invalidAuthorization()
        {
            this.Invoke(new MethodInvoker(() => {
                label3.Visible = true;
                label4.Visible = true;
                usernameBox.Visible = true;
                passwordBox.Visible = true;
                listBox1.Visible = true;
                ContentBox.ReadOnly = true;

                filenameBox.Visible = true;
                filenamebutton.Visible = true;
                newfilename.Visible = true;

                spreadsheetPanel1.Visible = false;
                ContentBox.Visible = false;
                ValueBox.Visible = false;
                Cell.Visible = false;
                CellAddress.Visible = false;
                label2.Visible = false;
                label1.Visible = false;
            }));

            MessageBox.Show("Invalid Authorization");

        }

        public void circularDependency(string cell)
        {
            this.Invoke(new MethodInvoker(() => {
                setCoordinates(cell, out int c, out int r);
                // Update the panel to show the recalculated cell
                spreadsheetPanel1.SetValue(c, r, sheet.GetCellValue(cell).ToString());
            }));
            MessageBox.Show("Circular Dependency on cell " + cell);
        }

        public void resetConnection()
        {
            this.Invoke(new MethodInvoker(() => {
                spreadsheetPanel1.Clear();
                sheet = new Spreadsheet(s => true, s => s.ToUpper(), "ps6");

                spreadsheetPanel1.Visible = false;
                ContentBox.Visible = false;
                ValueBox.Visible = false;
                Cell.Visible = false;
                CellAddress.Visible = false;
                label2.Visible = false;
                label1.Visible = false;

                ipBox.Visible = true;
                label5.Visible = true;
                button1.Visible = true;

                usernameBox.Visible = false;
                passwordBox.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                listBox1.Visible = false;

                filenameBox.Visible = false;
                filenamebutton.Visible = false;
                newfilename.Visible = false;

                label6.Text = "Not Connected";
            }));
            MessageBox.Show("No Connection");
        }
       
       
        /// <summary>
        /// What to do when the selected cell is changed. This deals with quite a bit of display and value updates
        /// </summary>
        /// <param name="ss"></param>
        private void OnSelectionChanged(SpreadsheetPanel ss)
        {

            prevContent = ContentBox.Text;
            // Store values for coordinates of selection
            int col, row;
            // If the selected cell isn't empty
            //if (!(CellAddress.Text.Equals("")))
            //{
            //    String cellContent = sheet.GetCellContents(CellAddress.Text).ToString();
            //    // If the content of the cell hasn't changed
            //    if (ContentBox.Text != cellContent)
            //    {
            //        try
            //        {
            //            // Iterate through each cell that needs to be updated with the cell changed
            //            foreach (string changedCells in sheet.SetContentsOfCell(CellAddress.Text, ContentBox.Text))
            //            {
            //                // Get the coordinates of the cell that needs to be updated
            //                setCoordinates(changedCells, out col, out row);
            //                // Update the panel to show the recalculated cell
            //                ss.SetValue(col, row, sheet.GetCellValue(changedCells).ToString());
            //            }
            //        }
            //        catch (CircularException)
            //        {
            //            int c, r;
            //            ss.GetSelection(out c, out r);
            //            ss.SetValue(c, r, "");
            //            sheet.SetContentsOfCell(CellAddress.Text, "");
            //            MessageBox.Show("Circular Dependency");
            //         //   OnSelectionChanged(ss);
            //        }
                  
            //    }
            //}

            // Set col and row to current coordinate
            ss.GetSelection(out col, out row);

            // Store coordinates as variable name
            string varName = getCellName(col, row);

            // Add an '=' to the beginning of the contents if the contents are a formula
            if (sheet.GetCellContents(varName) is Formula)
                ContentBox.Text = "=" + sheet.GetCellContents(varName).ToString();
            // Save non-Formulas' contents to the panel
            else
                ContentBox.Text = sheet.GetCellContents(varName).ToString();

            // Save the cell's value to the panel
            ValueBox.Text = sheet.GetCellValue(varName).ToString();

            // Set display text for variable name to the selected cell
            CellAddress.Text = varName;

            // select the ContentBox to type in
            ContentBox.Focus();



        }



      

        /// <summary>
        /// Event when text is changing in the ContentBox is to update the spreadsheet view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentBox_TextChanged(object sender, EventArgs e)
        {
            //prevContent = ContentBox.Text;
            //get coordinates and store them
            spreadsheetPanel1.GetSelection(out int col, out int row);

            // update the display of cell to show what is in the content box
            spreadsheetPanel1.SetValue(col, row, ContentBox.Text);
        }

        /// <summary>
        /// When leaving content box update display and deal with values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentBox_Leave(object sender, EventArgs e)
        {
            // get and store coordinates of the current cell
            spreadsheetPanel1.GetSelection(out int col, out int row);

            // get cell coords as variable name
            String varName = getCellName(col, row);

            //// update the contents of cell data
            //sheet.SetContentsOfCell(varName, ContentBox.Text);
            
            //// update the contents of cell display
            //spreadsheetPanel1.SetValue(col, row, sheet.GetCellValue(varName).ToString());

            // spreadsheetPanel1.SetSelection(col + 1, row);
            OnSelectionChanged(spreadsheetPanel1);
        }


        /// <summary>
        /// Actions to take when the save button is clicked in the menu strip. Deals with saving a spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // if there is a path name already in storage, save to the known path
            if (pathName != "")
            {
                sheet.Save(pathName);
            }
            else
            {
                // since there is not path to save to open a save dialogue, save logic as saveAs
                saveAsToolStripMenuItem_Click(sender, e);
            }
        }

        /// <summary>
        /// Brings up a new saveFileDialog for saving the current spreadsheet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //a saveFiledialogue object for saving documents
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "(*.sprd)|*.sprd"; //filter for file type
            saveFileDialog1.Title = "Save the spreadsheet"; // title of display window
            // show the dialog
            saveFileDialog1.ShowDialog();

            //save the file to given file path and update pathName
            if (saveFileDialog1.FileName != "")
            {
                pathName = saveFileDialog1.FileName;
                sheet.Save(pathName);
            }
        }

        /// <summary>
        /// Open a new spreadsheet when clicking new button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new spreadsheet form to display
            new Form1().Show();
        }

        /// <summary>
        /// Open the file browser when Open is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if the document has been saved before and
            // asks if it should be saved if it hasn't
            if (sheet.Changed)
            {
                // window to ask if user wants to save
                DialogResult result = MessageBox.Show("Would you like to save your changes",
                "Save?",
                 MessageBoxButtons.YesNoCancel,
                 MessageBoxIcon.Stop);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    // will cause dialog to close
                    return;
                }
            }

            // opens a file select dialogue for choosing a spreadsheet
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @".\", //starting directory to view
                Title = "Open Spreadsheet file", //label for window
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "xml", //limit file types to choose from
                Filter = "(*.sprd)|*.sprd", // display for file type to choose from
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pathName = openFileDialog1.FileName;
                sheet = new Spreadsheet(pathName, s => true, s => s.ToUpper(), "ps6");
                spreadsheetPanel1.Clear();

                // Get the name of the current cell
                spreadsheetPanel1.GetSelection(out int curCol, out int curRow);
                string curCellName = getCellName(curCol, curRow);

                // Update the content box for the current cell
                ContentBox.Text = sheet.GetCellContents(curCellName).ToString();
                // Update the Value box for the selected cll
                ValueBox.Text = sheet.GetCellValue(curCellName).ToString();

                foreach (string cell in sheet.GetNamesOfAllNonemptyCells())
                {
                    setCoordinates(cell, out int col, out int row);
                    spreadsheetPanel1.SetValue(col, row, sheet.GetCellValue(cell).ToString());
                }

            }

        }
        
        /// <summary>
        /// Action when file strip is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    // forces values to update when clicking on menu bar
        //    OnSelectionChanged(spreadsheetPanel1);
        //}

        /// <summary>
        /// Implement keyboard shortcuts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) //control
            {
                
                if (e.Shift) // control, shift
                {
                    // Ctrl + Shift + S is a shortcut to Save As
                    if (e.KeyCode == Keys.S)
                        saveAsToolStripMenuItem_Click(sender, e);
                }
                else // control, no shift
                {
                    // Ctrl + S is a shortcut to Save
                    if (e.KeyCode == Keys.S)
                    {
                        OnSelectionChanged(spreadsheetPanel1);
                        saveToolStripMenuItem_Click(sender, e);
                    }
                    // Ctrl + O is a shortcut to Open
                    else if (e.KeyCode == Keys.O)
                        openToolStripMenuItem_Click(sender, e);
                    // Ctrl + N is a shortcut to New
                    else if (e.KeyCode == Keys.N)
                        newToolStripMenuItem_Click(sender, e);
                    // Ctrl + C copies what is in the ContentBox
                    else if (e.KeyCode == Keys.C)
                    {
                        if (ContentBox.SelectedText == "")
                            Clipboard.SetDataObject(ContentBox.Text);
                    }
                    else if (e.KeyCode == Keys.T)
                    {
                        ToggleColors();
                    }
                    else if (e.KeyCode == Keys.R)
                    {
                        controller.Revert(CellAddress.Text);
                    }
                    else if (e.KeyCode == Keys.Z)
                    {
                        controller.Undo();
                    }

                }
            }
            else
            {
                if (e.Shift) // no control, shift
                {
                    // Shift + Enter makes cell select move up
                    if (e.KeyCode == Keys.Enter)
                    {
                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        spreadsheetPanel1.SetSelection(col, row - 1);
                        OnSelectionChanged(spreadsheetPanel1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    // Shift + tab makes cell select move left
                    else if (e.KeyCode == Keys.Tab)
                    {
                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        spreadsheetPanel1.SetSelection(col - 1, row);
                        OnSelectionChanged(spreadsheetPanel1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
                else  // no control, no shift
                {
                    // Enter or down arrow makes cell select move down
                    if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Down))
                    {
                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        spreadsheetPanel1.SetSelection(col, row + 1);
                        OnSelectionChanged(spreadsheetPanel1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    // tab makes cell select move right
                    else if (e.KeyCode == Keys.Tab)
                    {
                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        spreadsheetPanel1.SetSelection(col + 1, row);
                        OnSelectionChanged(spreadsheetPanel1);
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                    // up arrow makes cell select move up
                    else if (e.KeyCode == Keys.Up)
                    {
                        spreadsheetPanel1.GetSelection(out int col, out int row);
                        spreadsheetPanel1.SetSelection(col, row - 1);
                        OnSelectionChanged(spreadsheetPanel1);
                        e.Handled = true;
                    }

                }
            }
        }
        
        /// <summary>
        /// Toggles the color of items in the spreadsheet
        /// </summary>
        private void ToggleColors()
        {
            currentColor++;

            // Loops currentColor to 0
            if (currentColor == colors.Length)
                currentColor = 0;

            // Change the color of items in the spreadsheet;
            label1.ForeColor = colors[currentColor];
            label2.ForeColor = colors[currentColor];
            Cell.ForeColor = colors[currentColor];
            spreadsheetPanel1.BackColor = colors[currentColor];
           // fileToolStripMenuItem.ForeColor = colors[currentColor];
          //  helpToolStripMenuItem.ForeColor = colors[currentColor];
        }

        /// <summary>
        /// Dealing with closing the form and checking if file has been saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    // force cells to update
        //    OnSelectionChanged(spreadsheetPanel1);

        //    // check if the sheet has been modified to determine
        //    // if the user should be asked to save changes
        //    if (sheet.Changed)
        //    {
        //        DialogResult result = MessageBox.Show("Would you like to save your changes",
        //        "Save?",
        //         MessageBoxButtons.YesNoCancel,
        //         MessageBoxIcon.Stop);
        //        //save
        //        if (result == DialogResult.Yes)
        //        {
        //            saveToolStripMenuItem_Click(sender, e);
        //        }
        //        //will close the entire spreadsheet
        //        else if (result == DialogResult.No)
        //        {
        //        }
        //        // will close the dialog, but keep the spreadsheet up
        //        else if (result == DialogResult.Cancel)
        //        {
        //            e.Cancel = true;
        //        }
        //    }
        //}

        /// <summary>
        /// Catches the tab key from the content box to bypass default function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContentBox_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.KeyData == Keys.Tab) ||
                (e.KeyData == (Keys.Tab | Keys.Shift)))
                e.IsInputKey = true;
        }

        /// <summary>
        /// Show dialogue when help is clicked
        /// Help shows how to use the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        /// <summary>
        /// Closes the form if the Close button is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void spreadsheetPanel1_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                controller.Connect(ipBox.Text, usernameBox.Text, passwordBox.Text);
                //ContentBox.ReadOnly = false;
                //this.ActiveControl = ContentBox;

                ipBox.Visible = false;
                label5.Visible = false;
                button1.Visible = false;

                usernameBox.Visible = true;
                passwordBox.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                listBox1.Visible = true;


                filenameBox.Visible = true;
                filenamebutton.Visible = true;
                newfilename.Visible = true;

                //spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
                //// Set the current cell in the panel to A1
                // spreadsheetPanel1.SetSelection(0, 0);
                //// Update the view of the panel
                //  OnSelectionChanged(spreadsheetPanel1);
            }
            catch (Exception)
            {
                ContentBox.ReadOnly = true;
            }
        }

        private void usernameBox_TextChanged(object sender, EventArgs e)
        {
            usernameBox.Focus();
        }


        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            String selectedItem = "";
            if (listBox1.SelectedItem != null && (listBox1.SelectedItem.ToString() != ""))
            {
                selectedItem = listBox1.SelectedItem.ToString();
                //.WriteLine(selectedItem);
                controller.SendServerInfo(selectedItem, usernameBox.Text, passwordBox.Text);

                label3.Visible = false;
                label4.Visible = false;
                usernameBox.Visible = false;
                passwordBox.Visible = false;
                listBox1.Visible = false;
                ContentBox.ReadOnly = false;

                spreadsheetPanel1.Visible = true;
                ContentBox.Visible = true;
                ValueBox.Visible = true;
                Cell.Visible = true;
                CellAddress.Visible = true;
                label2.Visible = true;
                label1.Visible = true;

                filenameBox.Visible = false;
                filenamebutton.Visible = false;
                newfilename.Visible = false;

                this.ActiveControl = ContentBox;
                spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
                // Set the current cell in the panel to A1
                spreadsheetPanel1.SetSelection(0, 0);
                // Update the view of the panel
                OnSelectionChanged(spreadsheetPanel1);

            }

        }

        private void CellAddress_TextChanged(object sender, EventArgs e)
        {

            HashSet<String> hs = new HashSet<string>();
            // get and store coordinates of the current cell
            //        spreadsheetPanel1.GetSelection(out int col, out int row);

            // get cell coords as variable name
            //    String varName = getCellName(col, row);

            //// update the contents of cell data
            //sheet.SetContentsOfCell(varName, ContentBox.Text);
           if (prevCell != "" && prevContent != sheet.GetCellContents(prevCell).ToString())
           {
                // if the input is a formula
                if (prevContent.Length > 0 && prevContent[0] == '=')
                {
                    try
                    {
                        Formula f = new Formula(prevContent.Remove(0, 1), s => s.ToUpper(), s => true);
                        foreach (string var in f.GetVariables())
                        {
                            hs.Add(var);
                        }
                        controller.UpdateServer(prevCell, "=" + f.ToString(), hs);

                    }
                    catch (Exception)
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    controller.UpdateServer(prevCell, prevContent, hs);
                }
           }

            prevCell = CellAddress.Text;
            prevContent = ContentBox.Text;
            //// update the contents of cell display
            //spreadsheetPanel1.SetValue(col, row, sheet.GetCellValue(varName).ToString());

            // spreadsheetPanel1.SetSelection(col + 1, row);
        //    OnSelectionChanged(spreadsheetPanel1);

        }

        private void filenamebutton_Click(object sender, EventArgs e)
        {
        
            if (filenameBox.Text != "")
            {
            
                //.WriteLine(selectedItem);
                controller.SendServerInfo(filenameBox.Text, usernameBox.Text, passwordBox.Text);

                label3.Visible = false;
                label4.Visible = false;
                usernameBox.Visible = false;
                passwordBox.Visible = false;
                listBox1.Visible = false;
                ContentBox.ReadOnly = false;

                filenameBox.Visible = false;
                filenamebutton.Visible = false;
                newfilename.Visible = false;


                spreadsheetPanel1.Visible = true;
                ContentBox.Visible = true;
                ValueBox.Visible = true;
                Cell.Visible = true;
                CellAddress.Visible = true;
                label2.Visible = true;
                label1.Visible = true;


                this.ActiveControl = ContentBox;
                spreadsheetPanel1.SelectionChanged += OnSelectionChanged;
                // Set the current cell in the panel to A1
                spreadsheetPanel1.SetSelection(0, 0);
                // Update the view of the panel
                OnSelectionChanged(spreadsheetPanel1);

            }
        }
    }
}
