#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
#endregion

namespace QAQC
{
    public static class ColumnData
    {
        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColumnElement"></param>
        /// <returns></returns>
        public static double ColumnRotation(Element ColumnElement)
        {
            double RotationAdjusted = 0;

            //Get that location for column
            LocationPoint ColumnElementLocationPoint = ColumnElement.Location as LocationPoint;

            //if null then its slanted column or is not a 
            if (ColumnElementLocationPoint == null)
            {
                //Get that location for column
                LocationCurve ColumnElementLocationCurve = ColumnElement.Location as LocationCurve;

                //Make sure there is no null locations
                if (ColumnElementLocationCurve != null)
                {
                    Curve ColumnCurve = ColumnElementLocationCurve.Curve;

                    //collect "Cross-Section Rotation" Parameter if slanted
                    BuiltInParameter Revitvalueparam = BuiltInParameter.STRUCTURAL_BEND_DIR_ANGLE;
                    double rotationdouble = ColumnElement.get_Parameter(Revitvalueparam).AsDouble() * (180 / Math.PI);
                    rotationdouble = Math.Round(rotationdouble);

                    //Simple Refine
                    RotationAdjusted = SimpleRefine.SymetricalRotation(rotationdouble);
                }
            }
            //Else then its a vertical column
            else
            {
                //collect Rotation method if vertical
                double rotationdouble = ColumnElementLocationPoint.Rotation * (180 / Math.PI);
                rotationdouble = Math.Round(rotationdouble);

                //Simple Refine
                RotationAdjusted = SimpleRefine.SymetricalRotation(rotationdouble);
            }

            return RotationAdjusted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ColumnElement"></param>
        /// <param name="Rotation"></param>
        public static void SetColumnRotation(Element ColumnElement, double Rotation)
        {
            double currentrotation = ColumnRotation(ColumnElement);

            //Get that location for column
            LocationPoint ColumnElementLocationPoint = ColumnElement.Location as LocationPoint;

            //if null then its slanted column or is not a 
            if (ColumnElementLocationPoint == null)
            {
                //Get that location for column
                LocationCurve ColumnElementLocationCurve = ColumnElement.Location as LocationCurve;

                //Make sure there is no null locations
                if (ColumnElementLocationCurve != null)
                {
                    Curve ColumnCurve = ColumnElementLocationCurve.Curve;

                    XYZ aa = ColumnCurve.GetEndPoint(0);
                    XYZ cc = new XYZ(aa.X, aa.Y, aa.Z + 10);
                    Line axis = Line.CreateBound(aa, cc);
                    bool rotated = ColumnElementLocationCurve.Rotate(axis, (currentrotation - Rotation)*(Math.PI / 180));
                }
            }
            //Else then its a vertical column
            else
            {
                XYZ aa = ColumnElementLocationPoint.Point;
                XYZ cc = new XYZ(aa.X, aa.Y, aa.Z + 10);
                Line axis = Line.CreateBound(aa, cc);
                bool rotated = ColumnElementLocationPoint.Rotate(axis, (currentrotation - Rotation) * (Math.PI / 180));
            }
        }
        #endregion
    }
}
