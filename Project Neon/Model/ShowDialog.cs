using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Project_Neon.Model
{
    public class ShowDialog
    {
        public async static void DisplayErrorMessage(string message)
        {
            var errorDialog = new MessageDialog(message);
            var x = await errorDialog.ShowAsync();
        }
    }
}
