using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CrossFitCardGame.Converters
{
    public class ConvertSecondsToMMSS : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string returnValue = "00:00";

            int seconds = (int)value;

            int minutes = 0;
            int modSeconds = 0;

            string secondsToReturn = string.Empty;
            string minutesToReturn = string.Empty;

            //0's first.
            if (seconds == 0)
            {
                returnValue = "00:00";
            }
            else
            {
                modSeconds = seconds % 60;
                minutes = seconds / 60;

                if (modSeconds.ToString().Length == 0)
                {
                    secondsToReturn = "00";
                }
                else if (modSeconds.ToString().Length == 1)
                {
                    secondsToReturn = "0" + modSeconds.ToString();
                }
                else if (modSeconds.ToString().Length == 2)
                {
                    secondsToReturn = modSeconds.ToString();
                }

                if (minutes.ToString().Length == 0)
                {
                    minutesToReturn = "00";
                }
                else if (minutes.ToString().Length == 1)
                {
                    minutesToReturn = "0" + minutes.ToString();
                }
                else if (minutes.ToString().Length == 2)
                {
                    minutesToReturn = minutes.ToString();
                }
                if (minutesToReturn != string.Empty && secondsToReturn != string.Empty)
                {
                    returnValue = minutesToReturn + ":" + secondsToReturn;
                }
            }

            return returnValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
