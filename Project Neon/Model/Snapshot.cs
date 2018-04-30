using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Project_Neon.Model
{
    public class Snapshot
    {
        public static async Task<string> Capture(StorageFile file, TimeSpan timeOfFrame)
        {
            if (file == null)
            {
                return null;
            }

            //Get FrameWidth & FrameHeight
            List<string> encodingPropertiesToRetrieve = new List<string>();
            encodingPropertiesToRetrieve.Add("System.Video.FrameHeight");
            encodingPropertiesToRetrieve.Add("System.Video.FrameWidth");
            IDictionary<string, object> encodingProperties = await file.Properties.RetrievePropertiesAsync(encodingPropertiesToRetrieve);
            uint frameHeight = (uint)encodingProperties["System.Video.FrameHeight"];
            uint frameWidth = (uint)encodingProperties["System.Video.FrameWidth"];

            //Get image stream
            var clip = await MediaClip.CreateFromFileAsync(file);
            var composition = new MediaComposition();
            composition.Clips.Add(clip);
            var imageStream = await composition.GetThumbnailAsync(timeOfFrame, (int)frameWidth, (int)frameHeight, VideoFramePrecision.NearestFrame);

            //Create BMP
            var writableBitmap = new WriteableBitmap((int)frameWidth, (int)frameHeight);
            writableBitmap.SetSource(imageStream);

            //Get stream from BMP
            string mediaCaptureFileName = "IMG" + Guid.NewGuid().ToString().Substring(0, 4) + ".jpg";
            var saveAsTarget = await CreateMediaFile(mediaCaptureFileName);

            if (saveAsTarget != null)
            {
                Stream stream = writableBitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[(uint)stream.Length];
                await stream.ReadAsync(pixels, 0, pixels.Length);

                using (var writeStream = await saveAsTarget.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Premultiplied,
                        (uint)writableBitmap.PixelWidth,
                        (uint)writableBitmap.PixelHeight,
                        96,
                        96,
                        pixels);
                    await encoder.FlushAsync();
                    using (var outputStream = writeStream.GetOutputStreamAt(0))
                    {
                        await outputStream.FlushAsync();
                    }
                }
                return saveAsTarget.Name;
            }
            else
            {
                return null;
            }

        }

        public static async Task<StorageFile> CreateMediaFile(string filename)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Photo", new List<string>() { ".jpg" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = filename;

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                return file;
                //// Prevent updates to the remote version of the file until
                //// we finish making changes and call CompleteUpdatesAsync.
                //Windows.Storage.CachedFileManager.DeferUpdates(file);
                //// write to file
                //await Windows.Storage.FileIO.WriteTextAsync(file, file.Name);
                //// Let Windows know that we're finished changing the file so
                //// the other app can update the remote version of the file.
                //// Completing updates may require Windows to ask for user input.
                //Windows.Storage.Provider.FileUpdateStatus status =
                //    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                //if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                //{
                //    this.textBlock.Text = "File " + file.Name + " was saved.";
                //}
                //else
                //{
                //    this.textBlock.Text = "File " + file.Name + " couldn't be saved.";
                //}
            }
            else
            {
                return null;
                //this.textBlock.Text = "Operation cancelled.";
            }

            //StorageFolder _mediaFolder = KnownFolders.PicturesLibrary;
            //return await _mediaFolder.CreateFileAsync(filename);
        }
    }
}
