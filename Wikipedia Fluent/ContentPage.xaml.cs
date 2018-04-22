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
using System.Text.RegularExpressions;
using System.Text;
using Windows.UI.Xaml.Documents;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page
    {

        string headers_AsString;
        StringBuilder header_sb = new StringBuilder();

        public ContentPage()
        {
            this.InitializeComponent();

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            WikiPageContentsToPass parametersPassed = (WikiPageContentsToPass)e.Parameter;

            //string headers = ParseWikiText.GetHeaders(PassedThruPage.PageContent);









            pageTitle.Text = parametersPassed.PageTitle;
            pageContent.Text = parametersPassed.PageContent;

        }
    }
}
