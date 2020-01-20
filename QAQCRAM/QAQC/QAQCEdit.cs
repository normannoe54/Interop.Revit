using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using RAMSSWrapper;
using System.Runtime.InteropServices;
using Autodesk.Windows;

namespace QAQCRAM
{
    public partial class QAQCEdit : System.Windows.Forms.Form
    {
        // keep list of listview items 
        List<EditDataModel> Items = new List<EditDataModel>();

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Move the window associated with the passed 
        /// handle to the front.
        /// </summary>
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #region public methods -- DO I NEED TO REF THE DOC?
        public UIDocument uidoc { get; set; }
        public Document doc { get; set; }
        #endregion

        #region Event Handlers
        //Event Handlers
        private ExternalEvent m_EventHighlight;
        private ExternalEventHighlight m_HandlerHighlight;
        private ExternalEvent m_EventUpdate;
        private ExternalEventUpdate m_HandlerUpdate;
        private ExternalEvent m_EventIgnore;
        private ExternalEventIgnore m_HandlerIgnore;
        #endregion

        //Internal point for movement
        private System.Drawing.Point lastPoint;

        #region Form Controls
        /// <summary>
        /// Initialize userform
        /// </summary>
        public QAQCEdit(ExternalEvent EventHighlight, ExternalEventHighlight HandlerHighlight, ExternalEvent EventUpdate, ExternalEventUpdate HandlerUpdate,ExternalEvent EventIgnore,ExternalEventIgnore HandlerIgnore)
        {
            InitializeComponent();
            m_EventHighlight = EventHighlight;
            m_HandlerHighlight = HandlerHighlight;
            m_EventUpdate = EventUpdate;
            m_HandlerUpdate = HandlerUpdate;
            m_EventIgnore = EventIgnore;
            m_HandlerIgnore = HandlerIgnore;
            progressBar1.Visible = false;

        }

        /// <summary>
        /// Initiate public methods when userform loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCEdit_Load(object sender,EventArgs e)
        {
            UpdateTable();
            progressBar1.Visible = false;
        }

        #endregion

        #region Buttons
        /// <summary>
        /// close Userform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Update the elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            m_EventUpdate.Raise();

            progressBar1.Visible = true;
            //Maximum number is selected
            progressBar1.Maximum = LVDataList.SelectedItems.Count;
        }

        //Ignore the elements
        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            m_EventIgnore.Raise();

            progressBar1.Visible = true;
            //Maximum number is selected
            progressBar1.Maximum = LVDataList.SelectedItems.Count;
        }

        private void HighlightRow_Click(object sender,EventArgs e)
        {
            m_EventHighlight.Raise();

            SetForegroundWindow(ComponentManager.ApplicationWindow);         
        }
        #endregion

        #region Movement
        /// <summary>
        /// allows mouse movement for userform
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCEdit_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new System.Drawing.Point(e.X, e.Y);
        }

        /// <summary>
        /// allows mouse movement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCEdit_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        #endregion

        #region Asthetics
        /// <summary>
        /// Define header background and header text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LVDataList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            Brush brush = new SolidBrush(System.Drawing.Color.FromArgb(34, 36, 49));
            e.Graphics.FillRectangle(brush, e.Bounds);
            
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Underline))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont, Brushes.White, e.Bounds, sf);
                }
            }
        }

        /// <summary>
        /// Define the ability to draw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LVDataList_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        /// <summary>
        /// Disallow changing the column width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LVDataList_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = LVDataList.Columns[e.ColumnIndex].Width;
        }

        #endregion

        #region private update method

        public void updateProgressBar(int progress)
        {
            progressBar1.Increment(progress);
        }

        /// <summary>
        /// Initiate public methods when userform loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateTable()
        {
            LVDataList.HideSelection = false;
            LVDataList.Items.Clear();
            //Retrieve errorneous columns
            List<EditDataModel> Columns = EditDataCollect.RecordColumnElements(doc);
            Items = Columns;
            foreach (EditDataModel Column in Columns)
            {
                var row = new string[] { Column.elementtype, Column.elementId, Column.name, Column.concern, Column.RevitValue, Column.RAMValue, Column.RAMStory };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            //Retrieve erroneous beams
            List<EditDataModel> Beams = EditDataCollect.RecordBeamElements(doc);
            Items.AddRange(Beams);
            foreach (EditDataModel Beam in Beams)
            {
                var row = new string[] { Beam.elementtype, Beam.elementId, Beam.name, Beam.concern, Beam.RevitValue, Beam.RAMValue, Beam.RAMStory };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            //Retrieve erroneous braces
            List<EditDataModel> VBs = EditDataCollect.RecordVBElements(doc);
            Items.AddRange(VBs);
            foreach (EditDataModel VB in VBs)
            {
                var row = new string[] { VB.elementtype, VB.elementId, VB.name, VB.concern, VB.RevitValue, VB.RAMValue, VB.RAMStory };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }
          
            UpdateColors();

            //Reset
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }

        #endregion

        private void SearchBar_TextChanged(object sender, EventArgs e)
        {
            LVDataList.Items.Clear();

            foreach(EditDataModel items in Items)
            {

                if ((items.concern?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.elementId?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.elementtype?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.name?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.RAMStory?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.RAMValue?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault()
                    || (items.RevitValue?.ToLower().Contains(SearchBar.Text.ToLower())).GetValueOrDefault())
                {
                    var row = new string[] { items.elementtype, items.elementId, items.name, items.concern, items.RevitValue, items.RAMValue, items.RAMStory };
                    var lvi = new ListViewItem(row);
                    LVDataList.Items.Add(lvi);
                }
            }

            UpdateColors();
        }

        private void UpdateColors()
        {
            //Color
            //change colors in listview
            for (int i = 0; i < LVDataList.Items.Count; i++)
            {
                if (LVDataList.Items[i].SubItems[3].Text == "Size")
                {
                    LVDataList.Items[i].ForeColor = System.Drawing.Color.Red;
                }
                if (LVDataList.Items[i].SubItems[3].Text == "Rotation")
                {
                    LVDataList.Items[i].ForeColor = System.Drawing.Color.Orange;
                }
                if (LVDataList.Items[i].SubItems[3].Text == "Studs")
                {
                    LVDataList.Items[i].ForeColor = System.Drawing.Color.Aqua;
                }
                if (LVDataList.Items[i].SubItems[3].Text == "Camber")
                {
                    LVDataList.Items[i].ForeColor = System.Drawing.Color.Magenta;
                }

            }
        }

        private void Isolate_Click(object sender, EventArgs e)
        {
            SearchBar.Text = "";
            LVDataList.Items.Clear();

            //Retrieve erroneous beams
            List<EditDataModel> Beams = EditDataCollect.RecordSelectedBeamElements(uidoc);

            if (!Beams.Any())
            {
                UpdateTable();
                return;
            }

            Items.AddRange(Beams);
            foreach (EditDataModel Beam in Beams)
            {
                var row = new string[] { Beam.elementtype, Beam.elementId, Beam.name, Beam.concern, Beam.RevitValue, Beam.RAMValue, Beam.RAMStory };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            UpdateColors();

            //Reset
            progressBar1.Value = 0;
            progressBar1.Visible = false;
        }
    }
}
