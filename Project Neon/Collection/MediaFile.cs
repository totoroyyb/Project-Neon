using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Project_Neon.Collection
{
    public class MediaFile
    {
        private static StorageFile inputFile;

        public static void SetInputFile(StorageFile file)
        {
            inputFile = file;
        }

        public static StorageFile GetInputFile()
        {
            return inputFile;
        }
    }
}
