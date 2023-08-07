using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace FactoryLogic
{
    public static class Helper
    {
        public static string unValidMsg = "unvalid Number";
        public static string AddComplete = "Add item secsusful";
        public static double CheckDounle(string num)
        {
            bool good = double.TryParse(num, out double validNum);
            if (good && validNum > 0)
            {
                return validNum;
            }
            throw new Exception(unValidMsg);
        }
        public static int Checkint(string num)
        {
            bool good = int.TryParse(num, out int validNum);
            if (good && validNum > 0)
            {
                return validNum;
            }
            throw new Exception(unValidMsg);
        }

        public static async void userMsgs(string msg)
        {
            await new MessageDialog(msg).ShowAsync();
        }

    }
}
