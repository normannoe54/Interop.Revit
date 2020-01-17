using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAQC
{
    /// <summary>
    /// Parser class for data manipulation
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Change decimal values to fractions
        /// </summary>
        /// <param name="decvalue"></param>
        /// <returns></returns>
        public string Decimaltofractions(string decvalue)
        {
            string fracvalue = "0";

            if (decvalue != "0")
            {
                Decimal FirstNumber = Decimal.Parse(decvalue);
                Decimal SecondNumber = Math.Floor(Decimal.Parse(decvalue));
                Decimal ThirdNumber = FirstNumber - SecondNumber;
                int Times = 0;

                while (ThirdNumber % 1 != 0)
                {
                    ThirdNumber = ThirdNumber * 10;
                    Times++;
                }

                int Denominator = Convert.ToInt32(Math.Pow(10, Times));
                int Numerator = Convert.ToInt32(ThirdNumber);
                int i = 2;

                while (Numerator >= i && Denominator >= i)
                {
                    if (Numerator % i == 0 && Denominator % i == 0)
                    {
                        Numerator = Numerator / i;
                        Denominator = Denominator / i;
                    }
                    else
                    {
                        i = i + 1;
                    }
                }

                fracvalue = SecondNumber + " " + Numerator + "/" + Denominator;
            }
            return fracvalue;
        }

        public double SymetricalRotation(double rotationinput)
        {
            double rotationoutput = rotationinput;

            if (rotationinput == 180)
            {
                rotationoutput = 0;
            }
            if (rotationinput==270)
            {
                rotationoutput = 90;
            }
            if (rotationinput == 360)
            {
                rotationoutput = 0;
            }
            //if (rotationinput > 90)
            //{
            //    rotationoutput = rotationinput - 90;
            //}

            //if (rotationinput > 180)
            //{
            //    rotationoutput = rotationinput - 180;
            //}

            //if (rotationinput > 270)
            //{
            //    rotationoutput = rotationinput - 270;
            //}

            return rotationoutput;
        }
    }
}
