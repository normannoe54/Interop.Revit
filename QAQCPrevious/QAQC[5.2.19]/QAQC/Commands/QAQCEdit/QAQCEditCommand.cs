#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace QAQC
{


    ///  <summary>
    ///  QAQC Run Command code to be executed when button is clicked.
    ///  </summary>
    ///  <seealso cref="Autodesk.Revit.UI.IExternalApplication"/>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class QAQCEditCommand : IExternalCommand
    {
        
        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Application Context
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Check if we are in the Revit project, not in the family editor.
            if (doc.IsFamilyDocument)
            {
                Message.Display("Can't use command in family document", WindowType.Warning);
                return Result.Cancelled;
            }

            try
            {
                //Parameter Lists for Structural Framing and Structural Columns
                List <string> SCparameterlist = new List<string> {"Analysis.Section.Size", "Analysis.Type", "Analysis.Orient", "Analysis.Splice", "Flag.Size", "Flag.Orient"};
                List<string> SFparameterlist = new List<string> { "Analysis.Studs", "Analysis.Camber", "Analysis.ID", "Analysis.Story", "Flag.Studs", "Flag.Camber" };

                //Length of parameter lists
                int SClengthparameter = SCparameterlist.Count;
                int SFlengthparameter = SFparameterlist.Count;

                //Get all Categories in Project
                Categories groups = doc.Settings.Categories;

                //Get Structural Framing and Structural Column Categories
                Category SCcategory = groups.get_Item(BuiltInCategory.OST_StructuralColumns);
                Category SFcategory = groups.get_Item(BuiltInCategory.OST_StructuralFraming);

                //Define Porject Parameters initialization
                BindingMap bm = doc.ParameterBindings;
                DefinitionBindingMapIterator itor = bm.ForwardIterator();
                itor.Reset();
                Definition d = null;

                //Count Index for number of parameters that exist
                List<bool> Index = new List<bool>();

                //Iterate through project parameters to see if parameters have been added to the project
                //ADD PORTION TO CHECK IF SHARED PARAMETERS ARE CORRECTLY ASSIGNED TO CATEGORIES!!!!
                while (itor.MoveNext())
                {
                    d = itor.Key;
                    if (SCparameterlist.Contains(d.Name) || SFparameterlist.Contains(d.Name))
                    {
                        Index.Add(true);
                    }
                }

                //If none of the parameters are added, show warning message
                if (Index.Count == 0)
                {
                    Message.Display("QA/QC has not been run", WindowType.Warning);
                    return Result.Cancelled;
                }
                //Else show editing userform
                else
                {
                    //Active Revit Document and initialize new Userform
                    QAQCEdit myQAQCEdit = new QAQCEdit();
                    myQAQCEdit.uidoc = uiapp.ActiveUIDocument;
                    myQAQCEdit.doc = uidoc.Document;
                    myQAQCEdit.Show();
                    return Result.Succeeded;
                }

                // Show the number of all the categories to the user
                //String prompt = "Number of all categories in current Revit document:" + groups.Size;

                // get Floor category according to OST_Floors and show its name
                //prompt += SFcategory.Name;

                // Give the user some information
                //TaskDialog.Show("Revit", prompt);

                //for (int i = 0; i< lengthparameter; i++)
                //{

                //}
                        //if (AllParameterExists == true)
                //{

                //}
                //myQAQCEdit.Show();
                //ExternalEventRunApp.ShowForm(uiapp);
                //MainForm MainForm = new MainForm();
                //MainForm.Show();
                //ExternalEventSelectElementApp.ShowForm(uiapp);
                
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        public static string GetPath()
        {
            // Return constructed namespace path.
            return typeof(QAQCEditCommand).Namespace + "." + nameof(QAQCEditCommand);
        }
        #endregion
    }
}
