using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FetchingViewsOfViewSheetSets
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class GetViewCount : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;

                var sheetSets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheetSet));

                var result = "Total of " + sheetSets.Count().ToString() + " sheet sets in this project.\n\n";

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                foreach (var elem in sheetSets)
                {
                    var set = elem as ViewSheetSet;

                    result += set.Name;

                    // getting the Views property takes around 1.5 seconds on the given rvt file.
                    var views = set.Views;

                    result += " has " + views.Size.ToString() + " views.\n";
                }

                result += "\nOperation completed in " + Math.Round(stopWatch.ElapsedMilliseconds / 1000.0, 3) + " seconds.\n"
                    + "Average of " + stopWatch.ElapsedMilliseconds / sheetSets.Count() + " ms per loop iteration.";

                stopWatch.Stop();

                MessageBox.Show(result);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Result.Failed;
            }
        }
    }
}