#region Namespace
using System;
using Autodesk.Revit.DB;
#endregion

namespace QAQC
{
    public static class CollectLocation
    {
        #region public methods
        /// <summary>
        /// Structural Framing Location
        /// </summary>
        /// <param name="Beam"></param>
        /// <returns></returns>
        public static XYZ StructuralFramingMidpoint(Element BeamElement)
        {
            //Get that location for Beam
            LocationCurve BeamElementLocationCurve = BeamElement.Location as LocationCurve;
            Curve BeamCurve = BeamElementLocationCurve.Curve;

            //Normalized parameter at 0.5 for midpoint
            XYZ BeamMidpoint = BeamCurve.Evaluate(0.5, true);

            return BeamMidpoint;
        }

        /// <summary>
        /// Structural Column Location
        /// </summary>
        /// <param name="ColumnElement"></param>
        /// <returns></returns>
        public static XYZ StructuralColumnMidpoint(Element ColumnElement)
        {
            //Get that location for column
            LocationPoint ColumnElementLocationPoint = ColumnElement.Location as LocationPoint;

            //Initialize Column Point
            XYZ ColumnPoint = new XYZ(0, 0, 0);

            //if null then its slanted column or is not a 
            if (ColumnElementLocationPoint == null)
            {
                //Get that location for column
                LocationCurve ColumnElementLocationCurve = ColumnElement.Location as LocationCurve;

                //Make sure there is no null locations
                if (ColumnElementLocationCurve != null)
                {
                    //Curve
                    Curve ColumnCurve = ColumnElementLocationCurve.Curve;

                    //Normalized parameter at 0.5 for midpoint -- ***** MAKE SURE WE DONT FORGET THIS ONLY WORKS FOR STRAIGHT LINES!
                    ColumnPoint = ColumnCurve.Evaluate(0.5, true);
                }
            }
            //Else then its a vertical column
            else
            {
                //Basepoint for X and Y coords
                XYZ ColumnBasePoint = ColumnElementLocationPoint.Point;

                //Double
                double X = ColumnBasePoint.X;
                double Y = ColumnBasePoint.Y;

                //Bounding box for Z coords
                BoundingBoxXYZ bb = ColumnElement.get_BoundingBox(null);

                //Bounding Box catch
                if (null == bb)
                {
                    throw new ArgumentException(
                      "Expected Element 'Column' to have a valid bounding box.");
                }

                //Z value specified by bounding box
                double Z = (((bb.Max.Z - bb.Min.Z) * 0.5) + bb.Min.Z);

                //Reestablish XYZ Point
                ColumnPoint = new XYZ(X,Y,Z);
            }

            return ColumnPoint;
        }
        #endregion
    }
}
