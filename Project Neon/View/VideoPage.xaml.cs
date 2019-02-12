using FFmpegInterop;
using Project_Neon.Collection;
using Project_Neon.Helper;
using Project_Neon.Model;
using System;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private MediaPlaybackItem playbackItem;

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

        //protected override void OnNavigatedFrom(NavigationEventArgs e)
        //{
        //    MediaPlayerHelper.CleanUpMediaPlayerSource(VideoBox);
        //}

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
                VideoBox.SetPlaybackSource(MediaSource.CreateFromStorageFile(inputFile));
                //VideoBox.MediaPlayer.RealTimePlayback = true;
                VideoBox.Play();

                //VideoBox.MediaPlayer.MediaFailed += LocalMediaFailed;
            }
        }

        private void LocalMediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.Error == MediaPlayerError.SourceNotSupported)
            {
                TryToUseFFmpegLocal();
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
                TryToUseFFmpegURI();
            }

            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    VideoBox.MediaPlayer.MediaFailed -= URIMediaFailed;
            //});
        }

        private async void TryToUseFFmpegLocal()
        {
            if (inputFile != null)
            {
                VideoBox.Stop();

                // Open StorageFile as IRandomAccessStream to be passed to FFmpegInteropMSS
                IRandomAccessStream readStream = await inputFile.OpenAsync(FileAccessMode.Read);

                try
                {
                    // Instantiate FFmpegInteropMSS using the opened local file stream
                    ffmpegMss = await FFmpegInteropMSS.CreateFromStreamAsync(readStream);
                    
                    if (ffmpegMss != null)
                    {
                        playbackItem = ffmpegMss.CreateMediaPlaybackItem();

                        // Pass MediaStreamSource to Media Element
                        VideoBox.SetPlaybackSource(playbackItem);
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

            //try
            //{
            //    IRandomAccessStream readStream = await inputFile.OpenAsync(FileAccessMode.Read);

            //    ffmpegMss = await FFmpegInteropMSS.CreateFromStreamAsync(readStream);

            //    if (ffmpegMss != null)
            //    {
            //        MediaStreamSource mss = ffmpegMss.GetMediaStreamSource();

            //        if (mss != null)
            //        {
            //            mss.BufferTime = TimeSpan.Zero;
            //            sender.Source = MediaSource.CreateFromMediaStreamSource(mss);
            //            sender.RealTimePlayback = true;
            //            sender.Play();
            //        }
            //        else
            //        {
            //            ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
            //        }
            //    }
            //    else
            //    {
            //        ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowDialog.DisplayErrorMessage(ex.Message);
            //}
        }

        private async void TryToUseFFmpegURI()
        {
            try
            {
                // Set FFmpeg specific options. List of options can be found in https://www.ffmpeg.org/ffmpeg-protocols.html

                // Below are some sample options that you can set to configure RTSP streaming
                // Config.FFmpegOptions.Add("rtsp_flags", "prefer_tcp");
                // Config.FFmpegOptions.Add("stimeout", 100000);

                // Instantiate FFmpegInteropMSS using the URI
                VideoBox.Stop();
                ffmpegMss = await FFmpegInteropMSS.CreateFromUriAsync(inputURI);
                var source = ffmpegMss.CreateMediaPlaybackItem();

                // Pass MediaStreamSource to Media Element
                VideoBox.SetPlaybackSource(source);
            }
            catch (Exception ex)
            {
                ShowDialog.DisplayErrorMessage(ex.Message);
            }


            //try
            //{
            //    // Set FFmpeg specific options. List of options can be found in https://www.ffmpeg.org/ffmpeg-protocols.html
            //    PropertySet options = new PropertySet();

            //    // Below are some sample options that you can set to configure RTSP streaming
            //    // options.Add("rtsp_flags", "prefer_tcp");
            //    // options.Add("stimeout", 100000);

            //    // Instantiate FFmpegInteropMSS using the URI
            //    ffmpegMss = await FFmpegInteropMSS.CreateFromUriAsync(inputURI);
            //    if (ffmpegMss != null)
            //    {
            //        MediaStreamSource mss = ffmpegMss.GetMediaStreamSource();

            //        if (mss != null)
            //        {
            //            // Pass MediaStreamSource to Media Element
            //            VideoBox.Source = MediaSource.CreateFromMediaStreamSource(mss);
            //        }
            //        else
            //        {
            //            ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
            //        }
            //    }
            //    else
            //    {
            //        ShowDialog.DisplayErrorMessage(ErrorInfo.CannotOpen);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ShowDialog.DisplayErrorMessage(ex.Message);
            //}
        }

        private async void TryToLoadAndPlay(StorageFile file)
        {
            inputFile = file;
            var stream = await inputFile.OpenAsync(FileAccessMode.Read);
            VideoBox.SetPlaybackSource(MediaSource.CreateFromStorageFile(inputFile));

            VideoBox.Play();
            //VideoBox.MediaPlayer.RealTimePlayback = true;
            //VideoBox.MediaPlayer.Play();
            MediaTitleBlock.Text = inputFile.DisplayName;

            //VideoBox.MediaPlayer.MediaFailed += LocalMediaFailed;
        }

        private void TryToLoadAndPlay(string uri)
        {
            inputURI = uri;

            VideoBox.SetPlaybackSource(MediaSource.CreateFromUri(new Uri(inputURI)));
            VideoBox.Play();

            //VideoBox.MediaPlayer.MediaFailed += URIMediaFailed;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            //VideoBox.MediaPlayer.MediaFailed -= LocalMediaFailed;
            //VideoBox.MediaPlayer.MediaFailed -= URIMediaFailed;
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(MainPage));
        }

        private void MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            if (e.ErrorMessage == "MF_MEDIA_ENGINE_ERR_SRC_NOT_SUPPORTED : HRESULT - 0xC00D36C4")
            {
                TryToUseFFmpegLocal();
            }
        }
    }
}
