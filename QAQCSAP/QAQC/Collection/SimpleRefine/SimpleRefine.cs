using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAQCSAP
{
    /// <summary>
    /// Parser class for data manipulation
    /// </summary>
    public static class SimpleRefine
    {
        /// <summary>
        /// Change decimal values to fractions
        /// </summary>
        /// <param name="decvalue"></param>
        /// <returns></returns>
        public static string Decimaltofractions(string decvalue)
        {
            string fracvalue = decvalue;

            if (decvalue.Length!=1)
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

        /// <summary>
        /// Rotation Parser
        /// </summary>
        /// <param name="rotationinput"></param>
        /// <returns></returns>
        public static double SymetricalRotation(double rotationoutput)
        { 
            //Need to convert negative angles to positive, then send to upper 2 quadrants on unit circle
            if (rotationoutput < 0)
            {
                rotationoutput = 360 + rotationoutput;
            }
            if (rotationoutput > 0 && rotationoutput <= 180)
            {
                rotationoutput = rotationoutput;
            }
            if (rotationoutput > 180 && rotationoutput <= 360)
            {
                rotationoutput = rotationoutput-180;
            }


            return rotationoutput;
        }

        /// <summary>
        /// Trim up the string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string StringTrimmer(string input)
        {
            //Remove nulls and empties for camber
            if (input == null || input == "")
            {
                input = "0";
            }
            else
            {
                if (input.Contains("\""))
                {
                    input = input.Trim().Replace("\"", "");
                }
            }
            return input;
        }
    }
}
