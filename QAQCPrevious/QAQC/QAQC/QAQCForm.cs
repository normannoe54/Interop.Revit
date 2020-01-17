#region Namespaces
using System;
using System.Windows.Forms;
using Autodesk.Revit.UI;
#endregion

namespace QAQC
{
    public partial class QAQCForm : System.Windows.Forms.Form
    {
        #region Event Handlers
        //Event Handlers
        private ExternalEvent m_EventRun;
        private ExternalEventRun m_HandlerRun;
        private ExternalEvent m_EventSelection;
        private ExternalEventSelection m_HandlerSelection;
        #endregion

        //Internal point for movement
        private System.Drawing.Point lastPoint;

        #region Form Controls

        /// <summary>
        /// Initialize Userform
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="handler"></param>
        public QAQCForm(ExternalEvent EventRun, ExternalEventRun HandlerRun, ExternalEvent EventSelection, ExternalEventSelection HandlerSelection)
        {
            InitializeComponent();
            m_EventRun = EventRun;
            m_HandlerRun = HandlerRun;
            m_EventSelection = EventSelection;
            m_HandlerSelection = HandlerSelection;
        }

        /// <summary>
        /// Load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCForm_Load(object sender, EventArgs e)
        { }

        /// <summary>
        /// UserForm clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCForm_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);
        }
        #endregion

        #region Filename Text
        /// <summary>
        /// Open File Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OFDButton_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);

            //Opening File
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            {
                //Set the dialog box with the following constraints
                ofd.DefaultExt = ".rss";
                ofd.Filter = "RAM SS FILES (*.rss)|*.rss";
                Nullable<bool> result = ofd.ShowDialog();
                if (result == true)
                {
                    // Open document 
                    string filename = ofd.FileName;
                    FilenameText.Text = filename;
                }
            }
        }
        
        /// <summary>
        /// Filename Text box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilenameText_TextChanged(object sender, EventArgs e)
        {
            FilenameText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
            OFDButton.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }
        #endregion

        #region Select Element Button
        /// <summary>
        /// Select element button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectElementButton_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);
            //bubble the event up to the parent
            m_EventSelection.Raise();

            //Ask User to select one structural framing element
            //This needs to be an external event?
            //Reference selectionReference = uidoc.Selection.PickObject(ObjectType.Element, new SelectionFilterByCategory("Structural Framing"), "Select Elements");
            //Element selectionElement = doc.GetElement(selectionReference);
            //int ElementIDName = selectionElement.Id.IntegerValue;
            //ElementIDText.Text = ElementIDName.ToString();
        }

        private void ElementIDText_TextChanged(object sender, EventArgs e)
        {
            ElementIDText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
            SelectElementButton.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }
        #endregion

        #region Click Buttons
        /// <summary>
        /// Run external event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunButton_Click(object sender, EventArgs e)
        {
            //Error finder index
            bool found = false;

            //Go through each textbox control
            foreach (System.Windows.Forms.Control control in this.Controls)
            {
                //Set control as textbox
                if (control is System.Windows.Forms.TextBox)
                {
                    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)control;

                    //null catch
                    if (textBox.Tag != null)
                    {
                        //If the textbox is not selected and is empty
                        if (textBox.Text == textBox.Tag.ToString() || (string.IsNullOrWhiteSpace(textBox.Text)))
                        {
                            found = true;
                            break;
                        }
                    }
                }
            }

            //If there is no error found
            if (!found)
            {
                //bubble the event up to the parent
                m_EventRun.Raise();
            }
            else
            {
                //Display Error Message for missing inputs
                Message.Display("Missing inputs, please revise", WindowType.Error);
            }
        }

        /// <summary>
        /// Close Userform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Movement
        /// <summary>
        /// Move Down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new System.Drawing.Point(e.X, e.Y);
        }

        /// <summary>
        /// Move left and up
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }
        #endregion

        #region RAMID Text Box
        /// <summary>
        /// RAMID Textbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RAMIDText_TextChanged(object sender, EventArgs e)
        {
            RAMIDText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        /// <summary>
        /// RAMID Textbox clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RAMIDText_Click(object sender, EventArgs e)
        { 
            ResetDefaultTextBoxes(this);

            if (RAMIDText.Text == RAMIDText.Tag.ToString())
            {
                RAMIDText.Text = "";
            }
        }

        #endregion

        #region RAMStory Text Box

        /// <summary>
        /// RAM Story Textbox click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RAMStoryText_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);

            if (RAMStoryText.Text== RAMStoryText.Tag.ToString())
            {
                RAMStoryText.Text = "";
            }
        }
        
        /// <summary>
        /// RAM Story Textbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RAMStoryText_TextChanged(object sender, EventArgs e)
        {
            RAMStoryText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        #endregion

        #region Tolerance Text Box

        /// <summary>
        /// Tolerance Textbox click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToleranceText_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);

            if (ToleranceText.Text == ToleranceText.Tag.ToString())
            {
                ToleranceText.Text = "";
            }          
        }

        /// <summary>
        /// Tolerance Textbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToleranceText_TextChanged(object sender, EventArgs e)
        {
            ToleranceText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }
        #endregion

        #region Rotation Text Box

        /// <summary>
        /// Rotation Textbox clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotText_Click(object sender, EventArgs e)
        {
            ResetDefaultTextBoxes(this);

            if (RotText.Text == RotText.Tag.ToString())
            {
                RotText.Text = "";
            }
        }

        /// <summary>
        /// Rotation Textbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RotText_TextChanged(object sender, EventArgs e)
        {
            RotText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        #endregion

        #region Userform general functions
        /// <summary>
        /// Reset text box controls to tag values
        /// </summary>
        /// <param name="form"></param>
        private void ResetDefaultTextBoxes(System.Windows.Forms.Control form)
        {
            foreach (System.Windows.Forms.Control control in form.Controls)
            {
                //Set control as textbox
                if (control is System.Windows.Forms.TextBox)
                {
                    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)control;

                    //If the textbox is not selected and is empty
                    if (!textBox.Focused && (string.IsNullOrWhiteSpace(textBox.Text)))
                    {
                        textBox.Text = textBox.Tag.ToString();
                        textBox.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }

        /// <summary>
        /// RAMIDText Key Pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Integer_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }
        #endregion

    }
}
