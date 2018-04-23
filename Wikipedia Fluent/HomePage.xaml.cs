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

        WikiPageContentsToPass ParsedWikiPage = new WikiPageContentsToPass(); //Where PageTitle and PageContent will be saved



        string[] Content_Under_Header = new string[] { };



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

            WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
            Rootobject data_AsRootObject = await wikiContent.GetPageContent(searchQuery.Text);
            string data_AsString = data_AsRootObject.parse.wikitext;

           
            //Removes comments 
            string pattern = @"<!--[^Staff](.|\n)*?-->";
            string data_AsStringCut = wikiContent.Regex_DeleteData(data_AsString, pattern, "");

            //Defines page title
            wikiContent.PageTitle = wikiContent.GetPageTitle(data_AsRootObject);

            //Adds introduction information into variable
            Introduction introduction = new Introduction();
            introduction.DataTable = wikiContent.GetIntroTablesAndData(data_AsStringCut); //Gets table data
            pattern = @"^{{(.|\n)*?\n}}(?=\n''')";
            data_AsStringCut = wikiContent.Regex_DeleteData(data_AsStringCut, pattern, ""); //Removes table data

            introduction.Content = wikiContent.GetIntroContent(data_AsStringCut); //Gets intro paragraph
            pattern = @"(\\n|^)'''(.|\n)*?(?=\n==(\w|\d))";
            data_AsStringCut = wikiContent.Regex_DeleteData(data_AsStringCut, pattern, ""); //Removes intro paragraph


            wikiContent.introduction = introduction;
            

           //HEADERS, SUBHEADERS, and SUBSUBHEADERS            
           string[] n1_and_derivatives = wikiContent.Regex_Put_N1_AndDerivativesToArray(data_AsStringCut);

           List<Node_1> N1 = new List<Node_1>(); //NOTE: White space still needs to be formatted
           foreach (string section in n1_and_derivatives)
           {
               Node_1 n1_iteration = new Node_1();
               string n1_ContentAndTitle = wikiContent.Regex_Get_N1_ContentAndTitle(section); ////// <============== Header + Title

                n1_iteration.Title = wikiContent.Regex_Get_N1_Title(section); /////////////// <============== Header Title
                n1_iteration.Content = wikiContent.Regex_Get_N1_Content(section); ////// <============== Header Content

               //Contains Subheaders and derivatives
               List<Node_2> N2 = new List<Node_2>();
               string[] n2_and_derivatives = wikiContent.Regex_Put_N2_AndDerivativesToArray(section);
               foreach (string indiv_subheader_and_children in n2_and_derivatives)
               {
                   Regex regex = new Regex(@"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                   Match m_sh = regex.Match(indiv_subheader_and_children);
                   Node_2 n2_iteration = new Node_2();
                   if (m_sh.Success)
                   {
                       string n2_ContentAndTitle = wikiContent.Regex_Get_N2_ContentAndTitle(m_sh.Value); ////////// <============== Subheader + Title

                       n2_iteration.Title = wikiContent.Regex_Get_N2_Title(m_sh.Value); //////////////// <============== Subheader Title
                       n2_iteration.Content = wikiContent.Regex_Get_N2_Content(m_sh.Value); /////// <============== Subheader Content

                       List<Node_3> N3 = new List<Node_3>();
                       string[] n3_and_derivatives = wikiContent.Regex_Put_N3_AndDerivativesToArray(indiv_subheader_and_children);
                       foreach (string indiv_subsubheader_and_children in n3_and_derivatives)
                       {
                           regex = new Regex(@"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                           Match m_sh_sh = regex.Match(indiv_subsubheader_and_children);
                           Node_3 n3_iteration = new Node_3();
                           if (m_sh_sh.Success)
                           {
                               string n3_ContentAndTitle = wikiContent.Regex_Get_N3_ContentAndTitle(m_sh_sh.Value); //////////////////////// <============== SubSubheader + Title

                                n3_iteration.Title = wikiContent.Regex_Get_N3_Title(m_sh_sh.Value); ////////////////////////// <============== SubSubheader Title
                                n3_iteration.Content = wikiContent.Regex_Get_N3_Content(m_sh_sh.Value); ////////////// <============== SubSubheader Content

                               List<Node_4> N4 = new List<Node_4>();
                               string[] n4_and_derivatives = wikiContent.Regex_Put_N4_AndDerivativesToArray(indiv_subsubheader_and_children);
                               foreach (string indivSubSubSubheader in n4_and_derivatives)
                               {
                                    //And current S3 header to list of S3 headers
                                    string n4_ContentAndTitle = wikiContent.Regex_Get_N4_ContentAndTitle(indivSubSubSubheader); /////////////// <============== SubSubSubheader + Title

                                    string n4_Title = wikiContent.Regex_Get_N4_Title(indivSubSubSubheader); //////////////////// <============== SubSubSubheader Title
                                    string n4_Content = wikiContent.Regex_Get_N4_Content(indivSubSubSubheader); // <============== SubSubSubheader Content

                                    N4.Add(new Node_4() { Content = n4_Content, Title = n4_Title });
                               }

                               // Capital letter = List (e.g., N3) || Lowercase = Single instance of object (e.g., n3_iteration)
                               //Add list of fourth node as object in third node
                               n3_iteration.node_4 = N4;
                           }
                           //Add third node to list of third nodes
                           N3.Add(n3_iteration);
                       }
                       //Add list of current S2 headers to current S1 header
                       n2_iteration.node_3 = N3;
                   }
                   //Add current S1 header to list of S1 headers
                   N2.Add(n2_iteration);
               }
                //Add list of S1 headers to current header
                n1_iteration.node_2 = N2;
               //Add current header to list of headers
                N1.Add(n1_iteration);
           }
           //Add list of headers to rootobject
           wikiContent.node_1 = N1;

            ForEachContentHolder teststring = new ForEachContentHolder();
            int header_length = wikiContent.node_1.Count;

        
            for (int j=0; j<header_length;j++)
            {
                int subheader_length = wikiContent.node_1[j].node_2.Count;
                teststring.StringContent += Environment.NewLine;
                teststring.StringContent += "------------------------------------------------------------------------------------------------------------------------------------------";
                teststring.StringContent += Environment.NewLine;           
                teststring.StringContent += wikiContent.node_1[j].Title;
                teststring.StringContent += Environment.NewLine;
                teststring.StringContent += Environment.NewLine;
                teststring.StringContent += wikiContent.node_1[j].Content;
                teststring.StringContent += Environment.NewLine;
                teststring.StringContent += Environment.NewLine;

                if (subheader_length > 0)
                { 
                teststring.StringContent += "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ";
                teststring.StringContent += Environment.NewLine;
                    }

                for (int k=0; k<subheader_length;k++)
                {          
                teststring.StringContent += wikiContent.node_1[j].node_2[k].Title;
                teststring.StringContent += Environment.NewLine;               
                }
            }
           

            //Teeing up info to pass to next page
            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            PassThruContent.PageTitle = wikiContent.PageTitle;
            PassThruContent.PageContent = teststring.StringContent + data_AsString;
            
            
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
    