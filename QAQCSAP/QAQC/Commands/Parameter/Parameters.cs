#region Namespaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
#endregion

namespace QAQCSAP
{
    /// <summary>
    /// Class used to manipulate parameters
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Create a new shared parameter
        /// </summary>
        /// <param name="SharedParameterName"></param>
        /// <param name="CategorySet"></param>
        /// <param name="doc"></param>
        /// <param name="app"></param>
        /// <param name="group"></param>
        public static void CreateSharedParameters(List<string> SharedParameterName, CategorySet CategorySet, Document doc, Application app, DefinitionGroup group)
        { 
            //Collect all Project Parameters
            List<string> ProjectParameterList = RevitCollectElement.ProjectParameters(doc);

            foreach (string parametername in SharedParameterName)
            {
                if (!ProjectParameterList.Contains(parametername))
                {
                    //Locate definition of shared parameter
                    ExternalDefinition exdef = group.Definitions.get_Item(parametername) as ExternalDefinition;

                    //make shared parameter a project parameter
                    using (Transaction t = new Transaction(doc))
                    {
                        t.Start("Add Shared Parameters");
                        InstanceBinding ib = app.Create.NewInstanceBinding(CategorySet);
                        doc.ParameterBindings.Insert(exdef, ib, BuiltInParameterGroup.PG_TEXT);
                        t.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the shared parameter guid
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Guid SharedParameterGuid(Document doc, string name)
        {
            Guid spguid = Guid.Empty;

            //List of shared parameters
            List<SharedParameterElement> shParamElements = new FilteredElementCollector(doc).OfClass(typeof(SharedParameterElement)).Cast<SharedParameterElement>().ToList();

            //Cycle through shared parameters to find the ones we need
            foreach (SharedParameterElement sharedparameter in shParamElements)
            {

                //Retrieve shared parameter guid to collect later
                if (sharedparameter.Name == name)
                {
                    spguid = sharedparameter.GuidValue;
                    break;
                }
            }

            return spguid;
        }
    }
}
