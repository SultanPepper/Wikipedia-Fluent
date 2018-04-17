using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml;
using Wikipedia_Fluent.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        public HomePage()
        {
            this.InitializeComponent();


        }


     protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            progressRing.IsActive = false;
            base.OnNavigatedFrom(e);
        }



        private async void submitbtn_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;

            //Get url to request data from
            string requestTitle;
            try
            {
                string input = searchQuery.Text;
                string replacement = "%20";
                Regex regex = new Regex("[-\\w.,&%@!+()#\\]\\[;]*(\\s)[-\\w.,&%@!+()#\\]\\[;]");
                requestTitle = regex.Replace(input, replacement);
            } catch (Exception)
            {
                requestTitle = "%20 couldn't replace spaces in requested article search.";
            }

            string URL = "https://en.wikipedia.org/w/api.php?"
                + "action=query"
                + "&titles=" + requestTitle
                + "&prop=revisions"
                + "&rvprop=content"
                + "&format=json"
                + "&formatversion=2";





            HttpClient http = new HttpClient();
            string pageTitle;
            string pageContent;

            try {

                var response = await http.GetAsync(URL);
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Rootobject>(result);

                pageTitle = data.query.pages[0].title;
                pageContent = data.query.pages[0].revisions[0].content;

            } catch (Exception) {
                pageTitle = URL;
                pageContent = URL;

            }

            
            WikiPageContentsToPass ParsedWikiPage = new WikiPageContentsToPass();
            ParsedWikiPage.PageTitle = pageTitle;
            ParsedWikiPage.PageContent = pageContent;

            Frame.Navigate(typeof(ContentPage), ParsedWikiPage);

            
            //passingToPage.PageContent = WikiAPINavigator.ParseQueryForWiki(searchQuery.Text);








        }
        
        
        
        

        }
}
    