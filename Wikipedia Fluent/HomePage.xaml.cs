using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            progressRing.IsActive = false;
            base.OnNavigatedFrom(e);
        }

        public async void SubmitButtonEvent()
        {

            progressRing.IsActive = true;

            /////////////////////////////////////////////////////////////////////
            WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
            /////////////////////////////////////////////////////////////////////


            Rootobject data_AsRootObject = await wikiContent.GetPageContent(searchQuery.Text);
            string data_AsString = data_AsRootObject.parse.wikitext;

            //Removes comments 
            string pattern = @"<!--[^Staff](.|\n)*?-->";
            string data_AsStringCut = wikiContent.Regex_DeleteData(data_AsString, pattern, "");

            //Defines page title
            wikiContent.PageTitle = wikiContent.GetPageTitle(data_AsRootObject);

            //Adds introduction information into variable
            Introduction introduction = new Introduction();
            introduction.Content = wikiContent.GetIntroContent(data_AsStringCut);
            introduction.DataTable = wikiContent.GetIntroTablesAndData(data_AsStringCut);
            wikiContent.introduction = introduction;

            //INTRO - Remove data from page
            data_AsStringCut = wikiContent.RemoveIntroContent(data_AsStringCut);

            //HEADERS, SUBHEADERS, and SUBSUBHEADERS            
            string[] header_and_derivative_content = wikiContent.Regex_PutHeadersAndDerivativesToArray(data_AsStringCut);
            
            List<Header> headers = new List<Header>();
            foreach (string section in header_and_derivative_content)
            {
                Header currentheader = new Header();
                currentheader.Content = wikiContent.Regex_GetHeader(section); ///////////<==============Header


                //Contains Subheaders and derivatives
                List<S1_Header> subheaders = new List<S1_Header>();
                string[] subheaders_and_children = wikiContent.Regex_PutSubheadersAndDerivativesToArray(section);
                foreach (string indiv_subheader_and_children in subheaders_and_children)
                {
                    Regex regex = new Regex(@"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                    Match m_sh = regex.Match(indiv_subheader_and_children);
                    S1_Header currentsubheader = new S1_Header();
                    if (m_sh.Success)
                    {
                        currentsubheader.s1_Content = m_sh.Value; ///////////<==============Subheader
                        List<S2_Header> subsubheaders = new List<S2_Header>();
                        string[] subsubheaders_and_children = wikiContent.Regex_Put_Sub_SubheadersAndDerivativesToArray(indiv_subheader_and_children);
                        foreach (string indiv_subsubheader_and_children in subsubheaders_and_children)
                        {
                            regex = new Regex(@"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                            Match m_sh_sh = regex.Match(indiv_subsubheader_and_children);
                            S2_Header currentsubsubheader = new S2_Header();
                            if (m_sh_sh.Success)
                            {
                                currentsubsubheader.s2_Content = m_sh_sh.Value;///////////<==============SubSUBheader

                                List<S3_Header> subsubsubheaders = new List<S3_Header>();
                                string[] subSUBsubheaders_and_children = wikiContent.Regex_Put_Sub_Sub_SubheadersAndDerivativesToArray(indiv_subsubheader_and_children);
                                foreach (string indivSubSubSubheader in subSUBsubheaders_and_children)
                                {
                                    //And current S3 header to list of S3 headers
                                    subsubsubheaders.Add(new S3_Header() { s3_Content = indivSubSubSubheader });
                                }
                                //Add list of S3Headers into the current S2 header
                                currentsubsubheader.s3_Headers = subsubsubheaders;
                            }
                            //Add current S2 header to list of S2 headers
                            subsubheaders.Add(currentsubsubheader);
                        }
                        //Add list of current S2 headers to current S1 header
                        currentsubheader.s2_headers = subsubheaders;
                    }
                    //Add current S1 header to list of S1 headers
                    subheaders.Add(currentsubheader);
                }
                //Add list of S1 headers to current header
                currentheader.s1_headers = subheaders;
                //Add current header to list of headers
                headers.Add(currentheader);
            }
            //Add list of headers to rootobject
            wikiContent.headers = headers;

           

            Teststring subheaderListAsString = new Teststring();

            int headers_count = wikiContent.headers.Count;

            for (int i=0; i < headers_count; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                sb.Append("!!!!!!!!NEW HEADER!!!!!!!!!!!!!!!!!!!!!!");
                sb.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                sb.Append(Environment.NewLine);

                sb.Append(wikiContent.headers[i].Content);


                foreach (S1_Header Var in wikiContent.headers[i].s1_headers)
                 {
                    sb.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    sb.Append("!!!!!!!!NEW SUB-HEADER!!!!!!!!!!!!!!!!!!!");
                    sb.Append("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append(Var.s1_Content);
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    }

                subheaderListAsString.teststringcontent += sb.ToString();

            }









            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            wikiContent.introduction = introduction;
            PassThruContent.PageTitle = wikiContent.PageTitle;

            PassThruContent.PageContent = subheaderListAsString.teststringcontent;
            
            
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
        private void searchQuery_Loaded(object sender, RoutedEventArgs e)
        {
            searchQuery.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }
    }
 
}
    