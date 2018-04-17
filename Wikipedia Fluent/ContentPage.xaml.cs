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
using Wikipedia_Fluent.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page
    {
        public ContentPage()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WikiPageContentsToPass PassedThruPage = (WikiPageContentsToPass)e.Parameter;

            pageTitle.Text = PassedThruPage.PageTitle;
            pageContent.Text = PassedThruPage.PageContent;

            //pageTitle.Text = parametersPassed.PageTitle;
            //pageContent.Text = parametersPassed;

        }
    }
}
