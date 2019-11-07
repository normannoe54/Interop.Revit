#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
#endregion

namespace QAQCSAP
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
            //Run handler to handle request posting by the dialog
            ExternalEventHighlight HandlerHighlight = new ExternalEventHighlight();
            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventHighlight = ExternalEvent.Create(HandlerHighlight);

            //Run handler to handle request posting by the dialog
            ExternalEventUpdate HandlerUpdate = new ExternalEventUpdate();
            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventUpdate = ExternalEvent.Create(HandlerUpdate);

            //Run handler to handle request posting by the dialog
            ExternalEventIgnore HandlerIgnore = new ExternalEventIgnore();
            //Run External Event for the dialog to use (to post requests)
            ExternalEvent EventIgnore = ExternalEvent.Create(HandlerIgnore);

            //Application Context
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Autodesk.Revit.ApplicationServices.Application app = uiapp.Application;
            Document doc = uidoc.Document;

            //Check if we are in the Revit project, not in the family editor.
            if (doc.IsFamilyDocument)
            {
                //If a family document dont show
                Message.Display("Can't use command in family document", WindowType.Warning);
                return Result.Cancelled;
            }

            try
            {
                //Collect Structural Framing Parameters
                List<string> SFParamaterNames = InternalConstants.StructuralFramingParameters();

                //Collect Structural Column Parameters
                //List<string> ColParamaterNames = InternalConstants.StructuralColumnParameters();

                //Collection Vertical Braces Parameters
                //List<string> VBParamaterNames = InternalConstants.StructuralVBParameters();

                //List of parameter names to create
                List<string> CreateParamaterNames = new List<string>();

                //Create complete list
                //CreateParamaterNames = SFParamaterNames.Concat(ColParamaterNames).Concat(VBParamaterNames).ToList();

                //Count Index for number of parameters that exist
                List<bool> Index = new List<bool>();

                //Collect all Project Parameters
                List<string> ProjectParameterList = RevitCollectElement.ProjectParameters(doc);

                foreach (string parametername in CreateParamaterNames)
                {
                    if (!ProjectParameterList.Contains(parametername))
                    {
                        Message.Display("QA/QC has not been run", WindowType.Warning);
                        return Result.Cancelled;
                    }
                }

                //Active Revit Document and initialize new Userform
                QAQCEdit myQAQCEdit = new QAQCEdit(EventHighlight,HandlerHighlight,EventUpdate,HandlerUpdate,EventIgnore,HandlerIgnore);
                //Initiate userform with event handler
                HandlerHighlight.qaqcedit = myQAQCEdit;
                HandlerUpdate.qaqcedit = myQAQCEdit;
                HandlerIgnore.qaqcedit = myQAQCEdit;

                //Do we need this portion? Need to have event handler
                myQAQCEdit.uidoc = uiapp.ActiveUIDocument;
                myQAQCEdit.doc = uidoc.Document;
                myQAQCEdit.Show();
                return Result.Succeeded;

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
