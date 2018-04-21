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
using Windows.System;
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


        HttpClient http = new HttpClient(); //Webclient to be used
        WikiPageContentsToPass ParsedWikiPage = new WikiPageContentsToPass(); //Where PageTitle and PageContent will be saved


        string Intro_Paragraph;
        string[] Content_Under_Header = new string[] { };


        

        string output;
        int i = 0;



        public HomePage()
        {
            this.InitializeComponent();

            KeyEventHandler EnterToSubmitHandler = new KeyEventHandler(searchQuery_KeyDown);
            searchQuery.AddHandler(AutoSuggestBox.KeyDownEvent, EnterToSubmitHandler, true);

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //No title when navigate back to page
            DataBinding.CreateTitleText(string.Empty);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            progressRing.IsActive = false;
            base.OnNavigatedFrom(e);
        }

        public async void SubmitButtonEvent() {
            
            progressRing.IsActive = true;

            string parsedtitle = ParseTitle(searchQuery.Text);
            string URL = API_URL(parsedtitle);

            var response = await http.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            string input = data.query.pages[0].revisions[0].content;
            WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
            Introduction intro = new Introduction();

            string unparsed_content = data.query.pages[0].revisions[0].content;
            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            //Get page title
            wikiContent.pagetitle = data.query.pages[0].title;

            //Intro tables/data
            string pattern = @"({{(.|\n)*?\n}}\n\n)'";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success) {
                intro.datatables = match.Groups[1].Value;
            } else {
                intro.datatables = "There was difficulty extracting the data tables";
            }

            //Extract out introductory paragraph
            pattern = @"(\n}}\n\n)('''(.|\s|\d)*?)(\n\n==\s?(\w|\d))";
            regex = new Regex(pattern);
            match = regex.Match(input);

            if (match.Success) {
                ParsedWikiPage.PageContent = match.Groups[2].Value;
                intro.content = match.Groups[2].Value;

                //input = regex.Replace(input, @"$1$4"); //$3 = A mysterious unaccounted for '}'
            } else {
                intro.content = "Intro paragraph could not be parsed with RegEx";
            }

            //Remove intro data from output
            pattern = @"(^{{.*?}}\\n\\n'''.*?\\n\\n)(==)";
            regex = new Regex(pattern);
            input = regex.Replace(input, "$2");
            wikiContent.remainingcontent = input;


            //Extract out header content into array
            
                pattern = @"(\n==\s?(\w|\d|\s)*?\s?==[^=](.|\n)*?)(?=\n\n==(\s|\w|\d))";
                string[] Content_Under_Headers = 
                    Regex.Matches(input, pattern)
                    .OfType<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToArray();



            
            int array_length = Content_Under_Header.Length;
            List<Headers> headers = new List<Headers>() { };
            int i = 0;
            foreach (var Var in Content_Under_Headers) {



                string sh_input = Var.ToString();
                string sh_pattern = @"(\n\n===.*?===\n(.|\n)*)[^===]";
                

                string[] array_of_subheaders = Regex.Matches(sh_input, sh_pattern)
                                        .OfType<Match>()
                                        .Select(m => m.Groups[1].Value)
                                        .ToArray();

                Regex sh_regex = new Regex(sh_pattern);

                string headers_without_subheaders = sh_regex.Replace(sh_input, "");

                if (array_of_subheaders.Length == 0) {
                    string[] empty_array = new string[1] { "" };
                    empty_array = array_of_subheaders;
                }
                    

                headers.Add(new Headers() { content = headers_without_subheaders, subheaders = array_of_subheaders });

  
            }
             
                
            
                foreach (Headers Var in headers) {
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += "============";
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += Var.content;
                        foreach (var Var1 in Var.subheaders) {
                    ParsedWikiPage.PageContent += Var1.ToString();
                    ParsedWikiPage.PageContent += "@@@@@@@@@@@@@@@@@@";
                }
                      
                    
                    ParsedWikiPage.PageContent += Environment.NewLine;
                    ParsedWikiPage.PageContent += Environment.NewLine;
                }
            

            



            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            PassThruContent.PageTitle = wikiContent.pagetitle;
            PassThruContent.PageContent = intro.datatables + intro.content + ParsedWikiPage.PageContent;

                Frame.Navigate(typeof(ContentPage), PassThruContent);
            }
        


        private void submitbtn_Click(object sender, RoutedEventArgs e)
        {
            SubmitButtonEvent();
        }
        private void searchQuery_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            string searchtext = searchQuery.Text;
            string empty = "";
            string space = " ";

            if (e.Key == VirtualKey.Enter 
                && searchtext != empty 
                && searchtext !=space)
            {
                SubmitButtonEvent();
            }
                       
        }

        public static string API_URL(string Parsed_Search_Input)
        {
            string ParsedURL = "https://en.wikipedia.org/w/api.php"
                + "?action=query"
                + "&titles=" + Parsed_Search_Input
                + "&prop=revisions"
                + "&rvprop=content"
                + "&format=json"
                + "&formatversion=2"
                + "&redirects";

            return ParsedURL;
        }
        public static string ParseTitle(string Unparsed_Search_Input) {
            string input = Unparsed_Search_Input;
            string replacement = "%20";
            Regex regex = new Regex(@"\s");
            string formatted_query = regex.Replace(input, replacement);

            return formatted_query;
        }

       
    }
 
}
    