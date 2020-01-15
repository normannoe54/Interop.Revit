using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAMSSWrapper;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading;
using System.IO;
using System.Reflection;

namespace RAMSSTransfer
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Debugger.Launch();

            string FilenameUser = args[0];
            //string FilenameUser = @"C:\Users\nnoe\Documents\QAQCTesting\R to R\ECB3.rss";
            //string FilenameUser = @"C:\Users\nnoe\Documents\QAQCTesting\R to R\Podium_v10.rss";

            //Initialize method
            CollectInfo RAMCollection = new CollectInfo(FilenameUser);
            
            List <BeamDataModel> RAMStlBeams = new List<BeamDataModel>();
            List<BeamDataModel> RAMJoists = new List<BeamDataModel>();
            List<ColumnDataModel> RAMColumns = new List<ColumnDataModel>();
            List<VBDataModel> RAMVBs = new List<VBDataModel>();

            //Get the beams
            if (args[1] == "true")
            {
                RAMStlBeams = RAMCollection.GetBeams();
            }

            //Get the joists
            if (args[2] == "true")
            {
                RAMJoists = RAMCollection.GetJoists();
            }

            //Get the Columns
            if (args[3] == "true")
            {
                RAMColumns = RAMCollection.GetColumns();
            }

            //Get the VBs
            if (args[4] == "true")
            {
                RAMVBs = RAMCollection.GetVB();
            }

            Payload payload = new Payload(RAMStlBeams, RAMJoists, RAMColumns, RAMVBs);

            //Need to serialize data here:
            string output = JsonConvert.SerializeObject(payload);

            //In EXE FILEPATH
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string jsonfilepath = assemblyFolder + @"\MetaData.json";

            //string jsonfilepath = @"C:\Users\nnoe\Documents\QAQCTesting\MetaData.json";

            File.WriteAllText(jsonfilepath, output);

            //Close the database
            RAMCollection.CloseModel();
        }
    }
}
