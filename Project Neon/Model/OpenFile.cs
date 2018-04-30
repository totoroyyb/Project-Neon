using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Project_Neon.Model
{
    public class OpenFile
    {
        public static async Task<StorageFile> LoadFileAsync()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.Thumbnail;
            fileOpenPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            fileOpenPicker.FileTypeFilter.Add("*");
            //fileOpenPicker.FileTypeFilter.Add(".mkv");
            //fileOpenPicker.FileTypeFilter.Add(".rmvb");
            //fileOpenPicker.FileTypeFilter.Add(".flv");

            var inputFile = await fileOpenPicker.PickSingleFileAsync();

            if (inputFile != null)
            {
                return inputFile;
            }
            else
            {
                return null;
            }
        }
    }
}
