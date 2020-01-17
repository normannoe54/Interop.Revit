#region Namespaces
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
#endregion

namespace QAQC
{
    public partial class QAQCForm : System.Windows.Forms.Form
    {
        //Event Handlers
        private ExternalEvent m_EventRun;
        private ExternalEventRun m_HandlerRun;

        //****************************************Get Document information [DO WE NEED THIS?]
        public UIDocument uidoc { get; set; }
        public Document doc { get; set; }
        public Autodesk.Revit.ApplicationServices.Application app { get; set; }

        //Internal point for movement
        private System.Drawing.Point lastPoint;

        //Opening File
        Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Event"></param>
        /// <param name="handler"></param>
        public QAQCForm(ExternalEvent Event, ExternalEventRun handler)
        {
            InitializeComponent();
            m_EventRun = Event;
            m_HandlerRun = handler;
        }

        /// <summary>
        /// Open File Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OFDButton_Click(object sender, EventArgs e)
        {
            {
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectElementButton_Click(object sender, EventArgs e)
        {
            //Ask User to select one structural framing element
            //This needs to be an external event?
            Reference selectionReference = uidoc.Selection.PickObject(ObjectType.Element, new SelectionFilterByCategory("Structural Framing"), "Select Elements");
            Element selectionElement = doc.GetElement(selectionReference);
            int ElementIDName = selectionElement.Id.IntegerValue;
            ElementIDText.Text = ElementIDName.ToString();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            //bubble the event up to the parent
            m_EventRun.Raise();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void QAQCForm_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new System.Drawing.Point(e.X, e.Y);
        }

        private void QAQCForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button== MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void FilenameText_TextChanged(object sender, EventArgs e)
        {
            FilenamePanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            FilenameText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
            OFDButton.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void RAMIDText_TextChanged(object sender, EventArgs e)
        {
            BeamNumberPanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            RAMIDText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void RAMStoryText_TextChanged(object sender, EventArgs e)
        {
            StoryNamePanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            RAMStoryText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void ToleranceText_TextChanged(object sender, EventArgs e)
        {
            TolerancePanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            ToleranceText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void ElementIDText_TextChanged(object sender, EventArgs e)
        {
            ElementIDPanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            ElementIDText.ForeColor= System.Drawing.Color.FromArgb(0, 130, 0);
            SelectElementButton.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void RotText_TextChanged(object sender, EventArgs e)
        {
            RotationPanel.BackColor = System.Drawing.Color.FromArgb(0, 130, 0);
            RotText.ForeColor = System.Drawing.Color.FromArgb(0, 130, 0);
        }

        private void QAQCForm_Load(object sender, EventArgs e)
        {

        }

        //Public influencer
        public string FilenamePublic
        {
            get { return FilenameText.Text; }
            set { FilenameText.Text = value; }
        }

        //Public influencer
        public string ElementIDPublic
        {
            get { return ElementIDText.Text; }
            set { ElementIDText.Text = value; }
        }
    }
}
