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

            /*

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
            wikiContent.PageTitle = data.query.pages[0].title;

            //Intro tables/data
            string pattern = @"({{(.|\n)*?\n}}\n\n)'";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success) {
                wikiContent.Introduction.DataTable = match.Groups[1].Value;
                //intro.datatable = match.Groups[1].Value;
            } else {
                wikiContent.Introduction.DataTable = "There was difficulty extracting the data tables";
            }

            //Extract out introductory paragraph
            pattern = @"(\n}}\n\n)('''(.|\s|\d)*?)(\n\n==\s?(\w|\d))";
            regex = new Regex(pattern);
            match = regex.Match(input);

            if (match.Success) {
                ParsedWikiPage.PageContent = match.Groups[2].Value;
                wikiContent.Introduction.Content = match.Groups[2].Value;

            } else {
                wikiContent.Introduction.Content = "Intro paragraph could not be parsed with RegEx";
            }

            //Remove intro data from output
            pattern = @"(^{{.*?}}\\n\\n'''.*?\\n\\n)(==)";
            regex = new Regex(pattern);
            input = regex.Replace(input, "$2");
            wikiContent.RemainingContent = input;


            //Extract out header content into array
            
                pattern = @"(\n==\s?(\w|\d|\s)*?\s?==[^=](.|\n)*?)(?=\n\n==(\s|\w|\d))";
                string[] Content_Under_Headers = 
                    Regex.Matches(input, pattern)
                    .OfType<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToArray();



            
            int array_length = Content_Under_Header.Length;
            List<Header> headers = new List<Header>() { };

 
            foreach (var Var in Content_Under_Headers) {

                string sh_input = Var.ToString();
                string sh_pattern = @"(\n\n===.*?===\n(.|\n)*)[^===]";           
                string[] sh_array = Regex.Matches(sh_input, sh_pattern)
                    .OfType<Match>()
                    .Select(m => m.Groups[1].Value)
                    .ToArray();

                Regex sh_regex = new Regex(sh_pattern);
                string headers_without_subheaders = sh_regex.Replace(sh_input, "");

                List<Subheader> sh_content = new List<Subheader>() { };
                for (int j = 0; j < sh_array.Length; j++)
                    {
                    sh_content.Add(new Subheader() { Content = sh_array[j] });
                    }
              
               headers.Add(new Header() { Content = headers_without_subheaders, Subheaders = sh_content });
            }
            wikiContent.Headers = headers; //Store headers/subheaders within wikiContent object



      
            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            PassThruContent.PageTitle = wikiContent.PageTitle;
            //PassThruContent.PageContent = intro.datatable + intro.content + ParsedWikiPage.PageContent;

            foreach (Header header in headers)
            {
                PassThruContent.PageContent = String.Format("{0}{1}", PassThruContent.PageContent, header.Content);

                for (int j = 0; j < header.Subheaders.Count; j++)
                {
                    PassThruContent.PageContent = String.Format("{0}{1}", PassThruContent.PageContent, header.Subheaders[j].Content);
                }

            }

    */
            
            WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
            Introduction introduction = new Introduction();
            List<Header> headers = new List<Header>();
            List<Subheader> subheaders = new List<Subheader>();
            List<SubSubHeader> subsubheaders = new List<SubSubHeader>();

            string input;
            string pattern;
            string replace_with;
            string error_msg;
            int group_num;
            
            Rootobject data_AsRootObject = await wikiContent.GetPageContent(searchQuery.Text);
            string data_AsString = data_AsRootObject.query.pages[0].revisions[0].content;

            //Removes comments 
            pattern = @"<!--[^Staff](.|\n)*?-->";
            string data_AsStringCut = wikiContent.Regex_DeleteData(data_AsString, pattern, "");

            //TITLE
            wikiContent.PageTitle = wikiContent.GetPageTitle(data_AsRootObject);

            //INTRO - Content
            introduction.Content = wikiContent.GetIntroContent(data_AsStringCut);
            //INTRO - Data and tables
            introduction.DataTable = wikiContent.GetIntroTablesAndData(data_AsStringCut);
            //INTRO - Remove data from page
            data_AsStringCut = wikiContent.RemoveIntroContent(data_AsStringCut);

            //HEADERS, SUBHEADERS, and SUBSUBHEADERS
  
            error_msg = "There was a problem divvying up the headers";
            pattern = @"(\n==(?!=)(\s|\w)?.+?==(.|\n)*?\n)(!?==(\w|\s))";
            input = data_AsStringCut;
            group_num = 1;
            string[] headers_with_children = wikiContent.Regex_PutMatchesIntoArray(input, pattern, group_num, error_msg);



            List<Teststring> myteststringlist = new List<Teststring>();
            foreach (string header_with_children in headers_with_children)
            {
                pattern = @"(^==(\w|\d|\s).*==(.|\n)*\n\n)(==)";
                input = header_with_children;
                group_num = 1;
                error_msg = "There was an issue parsing the headers from subheaders and subsubheaders";

                //Adjust method HERE.
                //If no match found, then make sure it starts with ==.*== and delete that
                string header = wikiContent.Regex_SelectMatch(input, pattern, group_num, error_msg);

                pattern = @"(^==(\w|\d|\s).*==\n(.|\n)*)\n\n(==)";
                input = header_with_children;
                group_num = 1;
                string replacement = @"\n\n\n\n\n==";
                string input_without_header = wikiContent.Regex_ReplaceMatch(input, pattern, replacement);

                if (input_without_header != String.Empty && input_without_header != null)
                {
                    myteststringlist.Add(new Teststring() { teststringcontent = input_without_header });
                } else
                    myteststringlist.Add(new Teststring() { teststringcontent = "###############" + Environment.NewLine + "###########" });



            }

            Teststring teststring1 = new Teststring();
            foreach (var Var1 in myteststringlist)
            {
                teststring1.teststringcontent += Var1.teststringcontent;
                teststring1.teststringcontent += Environment.NewLine;
                teststring1.teststringcontent += "@@@@@@@@@@@@@@@@@@@";
                teststring1.teststringcontent += "@@@@@@@@@@@@@@@@@@@";
                teststring1.teststringcontent += Environment.NewLine;
                teststring1.teststringcontent += "@@@@@@@@@@@@@@@@@@@";
                teststring1.teststringcontent += "@@@@@@@@@@@@@@@@@@@";
                teststring1.teststringcontent += Environment.NewLine;
            }





            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            wikiContent.Introduction = introduction;
            PassThruContent.PageTitle = wikiContent.PageTitle;
            PassThruContent.PageContent = teststring1.teststringcontent;

            
            
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
    