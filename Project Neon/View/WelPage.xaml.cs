using Project_Neon.Collection;
using Project_Neon.Model;
using Project_Neon.View.PlayHisView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class WelPage : Page
    {
        public delegate void EventHandler(object source, EventArgs e);
        public static event EventHandler OnOpenLocalFileReady;
        public static event EventHandler OnOpenUriReady;

        public WelPage()
        {
            this.InitializeComponent();
            InitPlayHis();
        }

        private void InitPlayHis()
        {
            if (PlayHistoryManager.IsEmpty())
            {
                WelPlayHisFrame.Navigate(typeof(PlayHisEmptyPage));
            }
            else
            {
                WelPlayHisFrame.Navigate(typeof(WelPlayHisPage));
            }
        }

        private async void openFileButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile inputFile = await OpenFile.LoadFileAsync();

            if (inputFile != null)
            {
                HandlerManager.OpenLocalFileStatus = true;
                NavigateToVideoPage();
                OnOpenLocalFileReady(inputFile, null);
            }
        }

        private void URICancelButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFromURIFlyout.Hide();
        }

        private void URIConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            string uri = UriTextBox.Text;
            if (uri != null && uri != string.Empty)
            {
                OpenFromURIFlyout.Hide();
                HandlerManager.OpenUriStatus = true;
                NavigateToVideoPage();
                OnOpenUriReady(uri, null);
            }
        }

        private void NavigateToVideoPage()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(VideoPage));
        }
    }
}
