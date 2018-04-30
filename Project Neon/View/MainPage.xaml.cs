using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Project_Neon.View;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Project_Neon
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            InitFrame();
            SetTitleBar();
        }

        private void InitFrame()
        {
            myNavi.SelectedItem = welItem;
            myFrame.Navigate(typeof(WelPage));
        }

        private void SetTitleBar()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = "Neon";

            RegisterTitleBarChanged();
        }

        private void RegisterTitleBarChanged()
        {
            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

            // Register a handler for when the title bar visibility changes.
            // For example, when the title bar is invoked in full screen mode.
            coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;
        }

        private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (sender.IsVisible)
            {
                TitleBlock.Visibility = Visibility.Visible;
            }
            else
            {
                TitleBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void myNavi_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                myFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;

                switch (item.Tag.ToString())
                {
                    case "welpage":
                        myFrame.Navigate(typeof(WelPage));
                        //myNavi.Header = "Welcome";
                        break;

                }
            }
        }
    }
}
