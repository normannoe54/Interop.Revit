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
        #region public methods
        public UIDocument uidoc { get; set; }
        public Document doc { get; set; }
        #endregion

        #region constructor
        /// <summary>
        /// Initialize userform
        /// </summary>
        public QAQCEdit()
        {
            InitializeComponent();

        }
        #endregion

        #region private methods

        private System.Drawing.Point lastPoint;

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

        /// <summary>
        /// Initiate public methods when userform loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QAQCEdit_Load(object sender, EventArgs e)
        {
            RecordBeamElements();
            RecordColumnElements();
            //RecordBraceElemeents();
        }

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
        /// Collect Beam Elements
        /// </summary>
        private void RecordBeamElements()
        {
            //Filter out Beams with built in camber parameter
            string camberexists = "";

            //Define Built in parameter
            BuiltInParameter camberParam = BuiltInParameter.STRUCTURAL_CAMBER;

            //Get parameter value
            ParameterValueProvider pvpcamber = new ParameterValueProvider(new ElementId((int)camberParam));

            //Create filter rule for element collector
            FilterStringRuleEvaluator fnrvStrcamber = new FilterStringContains();

            //Define filter with built in parameter camber and filter out beams without the parameter
            FilterStringRule paramFrcamber = new FilterStringRule(pvpcamber, fnrvStrcamber, camberexists, false);

            //Create element collector of structural framing category
            FilteredElementCollector SF = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralFraming);

            //Define element parameter filter
            ElementParameterFilter epfSteelBeam = new ElementParameterFilter(paramFrcamber);

            //filter out non steel beams
            SF.WherePasses(epfSteelBeam);

            //Define structural framing parameters that need to be filtered
            List<string> ParameterString = new List<string>(){"Studs", "Camber"};

            //List of shared parameters
            List<SharedParameterElement> shParamElements = new FilteredElementCollector(doc).OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>().ToList();

            //string name to filter beams by
            string flag = "false";

            //initialize parameters for filters and dictionary
            IList<FilterRule> FilterRules = new List<FilterRule>() { };

            //establish guid initializers for shared parameters
            Guid spguid = Guid.NewGuid();
            Guid spguidAnalysis = Guid.NewGuid();

            //parameter name starts with Flag
            string flagparametername = "Flag.";

            //parameter name starts with Flag
            string RAMparametername = "Analysis.";

            //parameter name 
            foreach (string parametername in ParameterString)
            {
                //Cycle through shared parameters to find the ones we need
                foreach (SharedParameterElement sharedparameter in shParamElements)
                {
                    //if shared parameter is found
                    if (sharedparameter.Name == (flagparametername + parametername))
                    {
                        //get Guid and filter element collector by Guid
                        spguid = sharedparameter.GuidValue;
                        SharedParameterElement param = SharedParameterElement.Lookup(doc, spguid);
                        ParameterValueProvider pvp = new ParameterValueProvider(param.Id);
                        FilterStringRuleEvaluator fnrvStr = new FilterStringContains();
                        FilterStringRule paramFr = new FilterStringRule(pvp, fnrvStr, flag, true);
                        FilterRules.Add(paramFr);
                    }

                    //Retrieve RAM shared parameter guid to collect later
                    if (sharedparameter.Name == (RAMparametername + parametername))
                    {
                        spguidAnalysis = sharedparameter.GuidValue;
                    }
                }

                //apply filter
                ElementParameterFilter epf = new ElementParameterFilter(FilterRules);
                IList<Element> Elementafterfilter = SF.WherePasses(epf).ToElements();

                //Collect information from elements
                foreach (Element element in Elementafterfilter)
                {
                    //constructor
                    string Revitvalue = "Error";

                    //for stud information
                    if (parametername == "Studs")
                    {
                        BuiltInParameter Revitvalueparam = BuiltInParameter.STRUCTURAL_NUMBER_OF_STUDS;
                        Revitvalue = element.get_Parameter(Revitvalueparam).AsString();
                    }

                    //for camber information
                    if (parametername == "Camber")
                    {
                        BuiltInParameter Revitvalueparam = BuiltInParameter.STRUCTURAL_CAMBER;
                        Revitvalue = element.get_Parameter(Revitvalueparam).AsString();
                    }

                    //write all the info to a row and add to the listview
                    var row = new string[] { "Beam", element.Id.IntegerValue.ToString(), element.Name, parametername, Revitvalue, element.get_Parameter(spguidAnalysis).AsString() };
                    var lvi = new ListViewItem(row);
                    LVDataList.Items.Add(lvi);
                }

                //change colors in listview
                for (int i = 0; i < LVDataList.Items.Count; i++)
                {
                    ListViewItem flaggedlist = LVDataList.Items[3];

                    if (LVDataList.Items[i].SubItems[3].Text == "Studs")
                    {
                        LVDataList.Items[i].ForeColor = System.Drawing.Color.Yellow;
                    }
                    if (LVDataList.Items[i].SubItems[3].Text == "Camber")
                    {
                        LVDataList.Items[i].ForeColor = System.Drawing.Color.Magenta;
                    }

                }
            }
        }

        /// <summary>
        /// Collect Column elements
        /// </summary>
        private void RecordColumnElements()
        {
            //Create element collector of structural framing category
            FilteredElementCollector SF = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_StructuralColumns);

            //Define structural framing parameters that need to be filtered
            List<string> ParameterString = new List<string>() { "Orient" };

            //List of shared parameters
            List<SharedParameterElement> shParamElements = new FilteredElementCollector(doc).OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>().ToList();

            //string name to filter beams by
            string flag = "false";

            //initialize parameters for filters and dictionary
            IList<FilterRule> FilterRules = new List<FilterRule>() { };

            //establish guid initializers for shared parameters
            Guid spguid = Guid.NewGuid();
            Guid spguidAnalysis = Guid.NewGuid();

            //parameter name starts with Flag
            string flagparametername = "Flag.";

            //parameter name starts with Flag
            string RAMparametername = "Analysis.";

            //parameter name 
            foreach (string parametername in ParameterString)
            {
                //Cycle through shared parameters to find the ones we need
                foreach (SharedParameterElement sharedparameter in shParamElements)
                {
                    //if shared parameter is found
                    if (sharedparameter.Name == (flagparametername + parametername))
                    {
                        //get Guid and filter element collector by Guid
                        spguid = sharedparameter.GuidValue;
                        SharedParameterElement param = SharedParameterElement.Lookup(doc, spguid);
                        ParameterValueProvider pvp = new ParameterValueProvider(param.Id);
                        FilterStringRuleEvaluator fnrvStr = new FilterStringContains();
                        FilterStringRule paramFr = new FilterStringRule(pvp, fnrvStr, flag, true);
                        FilterRules.Add(paramFr);
                    }

                    //Retrieve RAM shared parameter guid to collect later
                    if (sharedparameter.Name == (RAMparametername + parametername))
                    {
                        spguidAnalysis = sharedparameter.GuidValue;
                    }
                }

                //apply filter
                ElementParameterFilter epf = new ElementParameterFilter(FilterRules);
                IList<Element> Elementafterfilter = SF.WherePasses(epf).ToElements();

                //Collect information from elements
                foreach (Element element in Elementafterfilter)
                {
                    //constructor
                    string Revitvalue = "Error";

                    //Orient information
                    if (parametername == "Orient")
                    {                        
                        //locationpoint for vertical columns
                        LocationPoint LocationPoint = element.Location as LocationPoint;                       
                                       
                        //if null then its slanted column
                        if (LocationPoint==null)
                        {
                            //collect "Cross-Section Rotation" Parameter if slanted
                            BuiltInParameter Revitvalueparam = BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE;
                            double rotationdouble = element.get_Parameter(Revitvalueparam).AsDouble() * (180 / Math.PI);
                            Revitvalue = Math.Round(rotationdouble).ToString();
                        }
                        else
                        {
                            //collect rotation using method if vertical
                            double rotationdouble = LocationPoint.Rotation * (180 / Math.PI);
                            Revitvalue = Math.Round(rotationdouble).ToString();
                        }
                    }

                    //write all the info to a row and add to the listview
                    var row = new string[] { "Column", element.Id.IntegerValue.ToString(), element.Name, parametername, Revitvalue, element.get_Parameter(spguidAnalysis).AsString() };
                    var lvi = new ListViewItem(row);
                    LVDataList.Items.Add(lvi);
                }

                //change colors in listview
                for (int i = 0; i < LVDataList.Items.Count; i++)
                {
                    ListViewItem flaggedlist = LVDataList.Items[3];

                    if (LVDataList.Items[i].SubItems[3].Text == "Orient")
                    {
                        LVDataList.Items[i].ForeColor = System.Drawing.Color.Orange;
                    }
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

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            TaskDialog.Show("Test", "test");
        }

        private void IgnoreButton_Click(object sender, EventArgs e)
        {
            TaskDialog.Show("Test", "Test");
        }
    }
}
