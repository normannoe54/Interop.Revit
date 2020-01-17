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

namespace QAQC
{
    public partial class QAQCEdit : System.Windows.Forms.Form
    {
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

        }

        /// <summary>
        /// Initiate public methods when userform loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCEdit_Load(object sender,EventArgs e)
        {
            UpdateTable();
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
        }

        //Ignore the elements
        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            m_EventIgnore.Raise();
        }

        private void HighlightRow_Click(object sender,EventArgs e)
        {
            m_EventHighlight.Raise();
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

        /// <summary>
        /// Initiate public methods when userform loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateTable()
        {
            LVDataList.Items.Clear();
            //Retrieve errorneous columns
            List<EditDataModel> Columns = EditDataCollect.RecordColumnElements(doc);
            foreach (EditDataModel Column in Columns)
            {
                var row = new string[] { Column.elementtype, Column.elementId, Column.name, Column.concern, Column.RevitValue, Column.RAMValue };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            //Retrieve erroneous beams
            List<EditDataModel> Beams = EditDataCollect.RecordBeamElements(doc);
            foreach (EditDataModel Beam in Beams)
            {
                var row = new string[] { Beam.elementtype, Beam.elementId, Beam.name, Beam.concern, Beam.RevitValue, Beam.RAMValue };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            //Retrieve erroneous braces
            List<EditDataModel> VBs = EditDataCollect.RecordVBElements(doc);
            foreach (EditDataModel VB in VBs)
            {
                var row = new string[] { VB.elementtype, VB.elementId, VB.name, VB.concern, VB.RevitValue, VB.RAMValue };
                var lvi = new ListViewItem(row);
                LVDataList.Items.Add(lvi);
            }

            //Retrieve erroneous vertical braces

            //Color
            //change colors in listview
            for (int i = 0; i < LVDataList.Items.Count; i++)
            {
                ListViewItem flaggedlist = LVDataList.Items[3];

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

        #endregion
    }
}
