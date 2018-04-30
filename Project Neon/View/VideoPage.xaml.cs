using FFmpegInterop;
using Project_Neon.Collection;
using Project_Neon.Helper;
using Project_Neon.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.DirectX.Direct3D11;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Project_Neon.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoPage : Page
    {
        private StorageFile inputFile;
        private FFmpegInteropMSS ffmpegMss;
        private string inputURI;

        public VideoPage()
        {
            this.InitializeComponent();
            RegOpenMediaHandler();
        }

        private void RegOpenMediaHandler()
        {
            if (HandlerManager.OpenLocalFileStatus)
            {
                WelPage.OnOpenLocalFileReady += WelPage_OnOpenLocalFileReady;
                HandlerManager.OpenLocalFileStatus = false;
            }
            if (HandlerManager.OpenUriStatus)
            {
                WelPage.OnOpenUriReady += WelPage_OnOpenUriReady;
                HandlerManager.OpenUriStatus = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MediaPlayerHelper.CleanUpMediaPlayerSource(VideoBox.MediaPlayer);
        }

        private void WelPage_OnOpenUriReady(object source, EventArgs e)
        {
            TryToLoadAndPlay((string)source);
            WelPage.OnOpenUriReady -= WelPage_OnOpenUriReady;
        }

        private void WelPage_OnOpenLocalFileReady(object source, EventArgs e)
        {
            TryToLoadAndPlay((StorageFile)source);
            WelPage.OnOpenLocalFileReady -= WelPage_OnOpenLocalFileReady;
        }

        private async void picker_Click(object sender, RoutedEventArgs e)
        {
            inputFile = await OpenFile.LoadFileAsync();

            if (inputFile != null)
            {
                VideoBox.Source = MediaSource.CreateFromStorageFile(inputFile);
                VideoBox.MediaPlayer.RealTimePlayback = true;
                VideoBox.MediaPlayer.Play();

                VideoBox.MediaPlayer.MediaFailed += LocalMediaFailed;
            }
        }

        private void LocalMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.Error == MediaPlayerError.SourceNotSupported)
            {
                TryToUseFFmpegLocal(sender, args);
            }

            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    VideoBox.MediaPlayer.MediaFailed -= LocalMediaFailed;
            //});
        }

        private void URIMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.Error == MediaPlayerError.SourceNotSupported)
            {
                TryToUseFFmpegURI(sender, args);
            }
            
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    VideoBox.MediaPlayer.MediaFailed -= URIMediaFailed;
            //});
        }

        private async void TryToUseFFmpegLocal(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            try
            {
                IRandomAccessStream readStream = await inputFile.OpenAsync(FileAccessMode.Read);

                ffmpegMss = FFmpegInteropMSS.CreateFFmpegInteropMSSFromStream(readStream, false, false);

                if (ffmpegMss != null)
                {
                    MediaStreamSource mss = ffmpegMss.GetMediaStreamSource();

                    if (mss != null)
                    {
                        mss.BufferTime = TimeSpan.Zero;
                        sender.Source = MediaSource.CreateFromMediaStreamSource(mss);
                        sender.RealTimePlayback = true;
                        sender.Play();
                    }
                    else
                    {
                        ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
                    }
                }
                else
                {
                    ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
                }
            }
            catch (Exception ex)
            {
                ShowDialog.DisplayErrorMessage(ex.Message);
            }
        }

        private void TryToUseFFmpegURI(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            try
            {
                // Set FFmpeg specific options. List of options can be found in https://www.ffmpeg.org/ffmpeg-protocols.html
                PropertySet options = new PropertySet();

                // Below are some sample options that you can set to configure RTSP streaming
                // options.Add("rtsp_flags", "prefer_tcp");
                // options.Add("stimeout", 100000);

                // Instantiate FFmpegInteropMSS using the URI
                ffmpegMss = FFmpegInteropMSS.CreateFFmpegInteropMSSFromUri(inputURI, false, false, options);
                if (ffmpegMss != null)
                {
                    MediaStreamSource mss = ffmpegMss.GetMediaStreamSource();

                    if (mss != null)
                    {
                        // Pass MediaStreamSource to Media Element
                        VideoBox.Source = MediaSource.CreateFromMediaStreamSource(mss);
                    }
                    else
                    {
                        ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
                    }
                }
                else
                {
                    ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
                }
            }
            catch (Exception ex)
            {
                ShowDialog.DisplayErrorMessage(ex.Message);
            }
        }

        private void TryToLoadAndPlay(StorageFile file)
        {   
            inputFile = file;

            VideoBox.Source = MediaSource.CreateFromStorageFile(inputFile);
            VideoBox.MediaPlayer.RealTimePlayback = true;
            VideoBox.MediaPlayer.Play();
            MediaTitleBlock.Text = inputFile.DisplayName;
            
            VideoBox.MediaPlayer.MediaFailed += LocalMediaFailed;
        }

        private void TryToLoadAndPlay(string uri)
        {
            inputURI = uri;

            VideoBox.Source = MediaSource.CreateFromUri(new Uri(inputURI));
            VideoBox.MediaPlayer.Play();
            
            VideoBox.MediaPlayer.MediaFailed += URIMediaFailed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            VideoBox.MediaPlayer.MediaFailed -= LocalMediaFailed;
            VideoBox.MediaPlayer.MediaFailed -= URIMediaFailed;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }
    }
}
