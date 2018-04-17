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



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        public string titleofPage;


        public MainPage()
        {
            this.InitializeComponent();
            backbtn.Visibility = Visibility.Visible;
            forwardbtn.Foreground = new SolidColorBrush(Windows.UI.Colors.DimGray);
            //forwardbtn.Visibility = Visibility.Collapsed;
            ContentFrame.Navigate(typeof(HomePage));


            /*          
             if (Frame.CanGoBack)
                backbtn.Visibility = Visibility.Visible;

            else
                backbtn.Visibility = Visibility.Collapsed;

            if (Frame.CanGoForward)
                forwardbtn.Visibility = Visibility.Visible;
            else
                forwardbtn.Visibility = Visibility.Collapsed;
             */
           
        }



        private void hamburgerbtn_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void splitviewtogglebtn_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void settingbtn_Click(object sender, RoutedEventArgs e)
        {
            forwardbtn.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            ContentFrame.Navigate(typeof(SettingsPage));
        }

        private void backbtn_Click(object sender, RoutedEventArgs e)
        {
            forwardbtn.Visibility = Visibility.Visible;
            if (ContentFrame.CanGoBack)
                ContentFrame.GoBack();
        }

        private void forwardbtn_Click(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.CanGoForward)
                ContentFrame.GoForward();
        }


    }       
}
