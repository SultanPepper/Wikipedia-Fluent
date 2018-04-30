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
using System.Net.Http;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using Wikipedia_Fluent.Models;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows;
using System.ComponentModel;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        public string titleofPage;
        public bool splitviewtogglebtn_ClickedAndHidden = false;



        public MainPage()
        {
            this.InitializeComponent();
            backbtn.Visibility = Visibility.Visible;

            forwardbtn.Foreground = new SolidColorBrush(Windows.UI.Colors.DimGray);

            ApplicationViewTitleBar formattedTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            formattedTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            ContentFrame.Navigate(typeof(HomePage));

            


           
        }



        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void homebtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(HomePage));
            mySplitView.IsPaneOpen = false;
        }

        private void tablesbtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(MyTables));
            mySplitView.IsPaneOpen = false;
        }

        private void settingbtn_Click(object sender, RoutedEventArgs e)
        {
            forwardbtn.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            ContentFrame.Navigate(typeof(SettingsPage));
            mySplitView.IsPaneOpen = false;
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            forwardbtn.Visibility = Visibility.Visible;
            if (ContentFrame.CanGoBack)
                ContentFrame.GoBack();
        }

        private void splitviewtogglebtn_Click(object sender, RoutedEventArgs e)
        {
            splitviewtogglebtn.Visibility = Visibility.Collapsed;
            splitviewtogglebtn_ClickedAndHidden = true;

            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void forwardbtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoForward)
                ContentFrame.GoForward();
        }

        private void mySplitView_PaneIsClosing(SplitView sender, SplitViewPaneClosingEventArgs args)
        {
            if (splitviewtogglebtn_ClickedAndHidden == true && splitviewtogglebtn.Visibility == Visibility.Collapsed)
            {
                splitviewtogglebtn.Visibility = Visibility.Visible;
                splitviewtogglebtn_ClickedAndHidden = false;
            }
        }

        private void mySplitView_PaneIsOpening(SplitView sender, object args)
        {

        }

        private void splitviewPane_LostFocus(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = false;
        }


    }       
}
