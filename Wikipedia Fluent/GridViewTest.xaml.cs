using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
    public sealed partial class GridViewTest : Page
    {
        public GridViewTest()
        {
            this.InitializeComponent();

            //Reference:https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.Grid

            //Reference: https://stackoverflow.com/questions/9301163/how-to-create-dynamic-columndefinitions-with-relative-width-values


            TextBox txbox = new TextBox();


            string column = @"{| class=""wikitable sortable"" style=""margin:auto;""
|+Average temperatures and precipitation for selected communities in California
|-
!Location
! August<br>(°F)
! August<br>(°C)
! January<br>(°F)
! January<br>(°C)
! Annual<br>Precipitation<br>(mm/in)
|-
|[[Los Angeles]] || 83/64 || 29/18 || 66/48 || 20/8 || 377/15
|-
|[[Los Angeles International Airport|LAX/LA Beaches]] || 75/64 || 23/18 || 65/49 || 18/9 || 326/13
|-
|[[San Diego, California|San Diego]] || 76/67 || 24/19 || 65/49 || 18/9 || 262/10
|-
|[[San Jose, California|San Jose]] || 82/58 || 27/14 || 58/42 || 14/5 || 401/16
|-
|[[San Francisco, California|San Francisco]] || 67/54 || 20/12 || 56/46 || 14/8 || 538/21
|-
|[[Fresno, California|Fresno]] || 97/66 || 34/19 || 55/38 || 12/3 || 292/11
|-
|[[Sacramento, California|Sacramento]] || 91/58 || 33/14 || 54/39 || 12/3 || 469/18
|-
|[[Oakland, California|Oakland]] || 73/58 || 23/14 || 58/44 || 14/7 || 588/23
|-
|[[Bakersfield, California|Bakersfield]] || 96/69 || 36/21 || 56/39 || 13/3 || 165/7
|-
|[[Riverside, California|Riverside]] || 94/60 || 35/18 || 67/39  || 19/4 || 260/10
|-
|[[Eureka, California|Eureka]] || 62/53 || 16/11 || 54/41 || 12/5 || 960/38
|-
|[[Death Valley, California|Death Valley]] || 113/84 || 45/29 || 64/37 || 18/3 ||&nbsp; 53/2
|-
|[[Mammoth Lakes, California|Mammoth Lakes]] || 77/45 || 25/7 || 40/15 || 4/ -9 || 583/23
|}";


            string input = column;
            string pattern = @"(<?!)\s?(.*)";
            Regex regex = new Regex(pattern);

            

            MatchCollection columns = Regex.Matches(input, pattern);
            
            int i = 0;
            RowDefinition r_Def = new RowDefinition();
            r_Def.Height = GridLength.Auto;
            GridLayout.RowDefinitions.Add(r_Def);
            foreach(Match match in columns)
            {
                
                StackPanel stackpanel = new StackPanel();
                stackpanel.Margin = new Thickness(15);
                ColumnDefinition cz = new ColumnDefinition();
                cz.Width = GridLength.Auto;
                GridLayout.ColumnDefinitions.Add(cz);
                Grid.SetColumn(stackpanel, i);
                

                TextBlock textblock = new TextBlock();
                textblock.Text = match.Groups[2].Value;
                stackpanel.Children.Add(textblock);
                GridLayout.Children.Add(stackpanel);
                i++;

            }

            i = 0;
            int current_row = 1;
            

            input = column;
            pattern = @"(<?\|-\n)(.*)(?=\n\|)";
            regex = new Regex(pattern);
            MatchCollection rows = Regex.Matches(input, pattern);
            foreach (Match match in rows)
            {
                RowDefinition rz = new RowDefinition();
                rz.Height = GridLength.Auto;
                GridLayout.RowDefinitions.Add(rz);

                input = match.Value;
                pattern = @"(?<=(\|\|\s?|\n\|))[^\-].*?(?=\s\|\||\n|$)";
                MatchCollection columnsinrows = Regex.Matches(match.Value, pattern);

                int current_col = 0;
                foreach (Match col in columnsinrows)
                {
                    StackPanel stackpanel = new StackPanel();


                    Grid.SetRow(stackpanel, current_row);
                    Grid.SetColumn(stackpanel, current_col);

                    TextBlock textblk = new TextBlock();
                    textblk.Margin = new Thickness(3);
                    textblk.Text = col.Value;
                    stackpanel.Children.Add(textblk);
                    GridLayout.Children.Add(stackpanel);

                    current_col++;
                     
                }

                current_row++;
            }

            /*
            StackPanel myStackpanel = new StackPanel();
            myStackpanel.Margin = new Thickness(10);


            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(1, GridUnitType.Star);
            GridLayout.ColumnDefinitions.Add(c1);


            Grid.SetColumn(myStackpanel, 0);
            Grid.SetRow(myStackpanel, 0);

            
            TextBlock textblk = new TextBlock();
            textblk.Text = "Heading A";

            ListBox ItemsList = new ListBox();
            ItemsList.Items.Add("Number 2");
            ItemsList.Items.Add("Number 3");
            ItemsList.Items.Add("Number 4");

            myStackpanel.Children.Add(textblk);
            myStackpanel.Children.Add(ItemsList);

            GridLayout.Children.Add(myStackpanel);

            StackPanel sp2 = new StackPanel();
            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(1, GridUnitType.Star);
            GridLayout.ColumnDefinitions.Add(c2);

            Grid.SetColumn(sp2, 1);
            Grid.SetRow(sp2, 0);

            TextBlock textblk2 = new TextBlock();
            textblk2.Text = "Heading B";

            ListBox ItemsList2 = new ListBox();
            ItemsList2.Items.Add("Number 5");
            ItemsList2.Items.Add("Number 6");
            ItemsList2.Items.Add("Number 7");

            sp2.Children.Add(textblk2);
            sp2.Children.Add(ItemsList2);
            GridLayout.Children.Add(sp2);


            Grid mygrid = new Grid();
            */




        }
        //public async Task<Rootobject> GetPageContent(string SearchQuery, string SectionNum)
        //{
        //    HttpClient http = new HttpClient();
        //    string searchquery = SearchQuery;

        //    string URL = GetContentURL(searchquery, SectionNum);

        //    var response = await http.GetAsync(URL);
        //    var result = await response.Content.ReadAsStringAsync();
        //    var data = JsonConvert.DeserializeObject<Rootobject>(result);

        //    return data;
        //}

        //public static async Task<Rootobject> GetPageSections(string String)
        //{
        //    HttpClient http = new HttpClient();
        //    //string searchquery = SearchQuery;
        //    string URL = GetSectionsURL(String);

        //    var response = await http.GetAsync(URL);
        //    var result = await response.Content.ReadAsStringAsync();
        //    var data = JsonConvert.DeserializeObject<Rootobject>(result);

        //    return data;
        //}

        //public async Task<WikiContent_Rootobject> PutSectionsIntoNodes(Rootobject rootobject)
        //{
        //    WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
        //    List<Node_1> n1_list = new List<Node_1>();

        //    wikiContent.Node_1_list = n1_list;
            
        //    List<Node_2> n2_list = new List<Node_2>();           
        //    List<Node_3> n3_list = new List<Node_3>();            
        //    List<Node_4> n4_list = new List<Node_4>();

        //    int n1 = -1;
        //    int n2 = -1;
        //    int n3 = -1;
        //    int n4 = 0;

        //    foreach (Section section in rootobject.parse.sections)
        //    {
        //        string section_title = section.anchor;
        //        string section_index = section.index;
        //        string section_num = section.number;




        //        string input = section.number;

        //        //n1
        //        string pattern_n1 = @"^\d{1,3}$";
        //        Regex regex_n1 = new Regex(pattern_n1);
        //        Match m_n1 = regex_n1.Match(input);

        //        //n2
        //        string pattern_n2 = @"^\d{1,3}\.\d{1,3}$";
        //        Regex regex_n2 = new Regex(pattern_n2);
        //        Match m_n2 = regex_n2.Match(input);

        //        //n3
        //        string pattern_n3 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}$";
        //        Regex regex_n3 = new Regex(pattern_n3);
        //        Match m_n3 = regex_n3.Match(input);

        //        //n4
        //        string pattern_n4 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
        //        Regex regex_n4 = new Regex(pattern_n4);
        //        Match m_n4 = regex_n1.Match(input);

        //        if(m_n1.Success)
        //        {
        //            n2 = -1;
        //            n3 = -1;
        //            n4 = -1;
        //            //Add everything added previously

        //            Node_1 n1_iteration = new Node_1();
        //            List<Node_2> n2list = new List<Node_2>();

        //            n1_iteration.Node_2_list = n2list;

        //            n1_iteration.SectionTitle = section_title;
        //            n1_iteration.SectionNum = section_num;
        //            n1_iteration.SectionIndex = section_index;

        //            //string n1_and_derivative_content = rootobject.query.pages[0].revisions[0].content;
        //            Rootobject n1_rootobject = await GetPageContent(searchQuery.Text, section_index);
        //            string SectionContent = n1_rootobject.query.pages[0].revisions[0].content;

        //            //And Saves value in N1
        //            GetN1Content(SectionContent, n1_iteration);


        //            wikiContent.Node_1_list.Add(n1_iteration);


        //            //Now empty the node so we add the current val into it
        //            Node_1 n1_empty = new Node_1();
        //            n1_iteration = n1_empty;

        //            n1++;
        //        }




        //    }

        //    return wikiContent;
        //}
        //public static void GetN1Content(string SectionContent, Node_1 n1_iteration)
        //{
        //    string n1_andderivatives = SectionContent;

        //    //Get N1 title and content
        //    string n1_pattern = @"(^=|\n=)=([^=]?.+?)==((.|\n)*?)(?=($|\n===))";
        //    Regex regex_select_n1 = new Regex(n1_pattern);
        //    Match m_n1 = regex_select_n1.Match(SectionContent);
        //    if(m_n1.Success)
        //    {
        //        n1_iteration.Title = m_n1.Groups[2].Value;

        //        string n1_content = m_n1.Groups[3].Value;
        //        FixWhiteSpace(n1_content);
        //        n1_iteration.Content = n1_content;
        //    }



        //    string n2_pattern_and_deriv = @"(^=|\n=)==([^=]?.+?)===((\n|.)*?)(?=($|\n={3}(\w|\d|\s)))";

        //    Regex regex_select_n2_and_deriv = new Regex(n2_pattern_and_deriv);
        //    MatchCollection n2_match_collection = regex_select_n2_and_deriv.Matches(SectionContent);
        //    int n_2_match_collection_count = n2_match_collection.Count;
            
        //    if (n_2_match_collection_count > 0)
        //    {
        //        List<Node_2> n2_list = new List<Node_2>();

        //        foreach (Match match_n2 in n2_match_collection)
        //        {
        //            Node_2 node_2_interation = new Node_2();


        //            string n2_only = @"(^=|\n=)==([^=]?.+?)===((.|\n)*?)(?=($|\n={3,5}(\w|\d|\s)))";
        //            Regex regex_select_n2 = new Regex(n2_only);
        //            Match m_n2 = regex_select_n2.Match(match_n2.Value);

        //                node_2_interation.Title = m_n2.Groups[2].Value;

        //            string n2_content = m_n2.Groups[3].Value;
        //            FixWhiteSpace(n2_content);
        //            node_2_interation.Content = n2_content;
                    

        //                string n3_pattern_and_deriv = @"(^=|\n=)===([^=].+?)====((\n|.)*?)(?=($|\n={4}(\w|\d|\s)))";
        //                Regex regex_select_n3_and_deriv = new Regex(n3_pattern_and_deriv);
        //                MatchCollection n3_match_collection = regex_select_n3_and_deriv.Matches(m_n2.Value);

        //                int n_3_match_collection_count = n3_match_collection.Count;

        //                if (n_3_match_collection_count > 0)
        //                {
        //                    List<Node_3> n3_list = new List<Node_3>();

        //                    foreach (Match m_n3 in n3_match_collection)
        //                    {
        //                        Node_3 node_3_iteration = new Node_3();

        //                        node_3_iteration.Title = m_n3.Groups[2].Value;

        //                        string n3_content = m_n2.Groups[3].Value;
        //                        FixWhiteSpace(n3_content);

        //                        node_3_iteration.Content = n3_content;

        //                        string n4_pattern_and_deriv = @"(^=|\n=)====([^=].+?)=====((\n|.)*?)(?=($|\n={5}(\w|\d|\s)))";
        //                        Regex regex_select_n4_and_deriv = new Regex(n4_pattern_and_deriv);
        //                        MatchCollection n4_match_collection = regex_select_n4_and_deriv.Matches(m_n3.Value);

        //                        int n_4_match_collection_count = n4_match_collection.Count;

        //                        if(n_4_match_collection_count > 0)
        //                        {
        //                            List<Node_4> n4_list = new List<Node_4>();

        //                            foreach(Match m_n4 in n4_match_collection)
        //                            {
        //                                Node_4 node_4_iteration = new Node_4();

        //                                node_4_iteration.Title = m_n4.Groups[2].Value;

        //                                string n4_content = m_n4.Groups[3].Value;
        //                                FixWhiteSpace(n4_content);
        //                                node_4_iteration.Content = n4_content;

        //                                //Add n4 iteration to list
        //                                n4_list.Add(node_4_iteration);
        //                            }
        //                            //Add n4 list to n3 iteration
        //                            node_3_iteration.Node_4_list = n4_list;
        //                        }

        //                        //Add n3 iteration to n3 list
        //                        n3_list.Add(node_3_iteration);
                                
        //                    }

        //                    //Add n3 list to n2 iteration
        //                    node_2_interation.Node_3_list = n3_list;
                        
        //            }

        //            //Add n2 iteration to n2 list
        //            n2_list.Add(node_2_interation);
        //        }
        //        //add n2 list to n1 iteration
        //        n1_iteration.Node_2_list = n2_list;
        //    }
        //}
        //public static void FixWhiteSpace(string Input)
        //{
        //    string pattern = @"^\n{1,5}";
        //    Regex removeintrospaces = new Regex(pattern);
        //    Input = removeintrospaces.Replace(Input, "");
        //    Input = Environment.NewLine + Input;

        //    pattern = @"\n{1,6}$";
        //    Regex removeendspace = new Regex(pattern);
        //    Input = removeendspace.Replace(Input, "");
        //    Input = Input + Environment.NewLine + Environment.NewLine;


        //}
        //public string GetContentURL(string SearchQuery, string Section)
        //{
        //    //https://en.wikipedia.org/w/api.php?action=parse&page=New_York&format=json&prop=wikitext&section=4
        //    //prop=revisions&titles=california&rvprop=content&rvsection=2&formatversion=2

        //    string searchquery = SearchQuery;

        //    string scheme = "https://";
        //    StringBuilder sb_URL = new StringBuilder(scheme);

        //    string language = "en";
        //    language = String.Format("{0}.", language);
        //    sb_URL.Append(language);

        //    string authority = "wikipedia.org";
        //    sb_URL.Append(authority);

        //    string api_path = "/w/api.php";
        //    sb_URL.Append(api_path);

        //    string action = "query";
        //    action = String.Format("?action={0}", action);
        //    sb_URL.Append(action);

        //    string titles = Regex_ReplaceMatch(searchquery, @"\s", "%20");
        //    titles = String.Format("&titles={0}", titles);
        //    sb_URL.Append(titles);

        //    string prop = "revisions";
        //    prop = String.Format("&prop={0}", prop);
        //    sb_URL.Append(prop);

        //    string rvprop = "content";
        //    rvprop = String.Format("&rvprop={0}", rvprop);
        //    sb_URL.Append(rvprop);

        //    string format = "json";
        //    format = String.Format("&format={0}", format);
        //    sb_URL.Append(format);

        //    string formatversion = "2";
        //    formatversion = String.Format("&formatversion={0}", formatversion);
        //    sb_URL.Append(formatversion);

        //    string andRedirects = "&redirects";
        //    sb_URL.Append(andRedirects);

        //    string rvsection = Section;
        //    rvsection = String.Format("&rvsection={0}", rvsection);
        //    sb_URL.Append(rvsection);

        //    string URL = sb_URL.ToString();

        //    return URL;

        //}
        //public static string GetSectionsURL(string String)
        //{
        //    //string searchquery = SearchQuery;

        //    string scheme = "https://";
        //    StringBuilder sb_URL = new StringBuilder(scheme);

        //    string language = "en";
        //    language = String.Format("{0}.", language);
        //    sb_URL.Append(language);

        //    string authority = "wikipedia.org";
        //    sb_URL.Append(authority);

        //    string api_path = "/w/api.php";
        //    sb_URL.Append(api_path);

        //    string action = "parse";
        //    action = String.Format("?action={0}", action);
        //    sb_URL.Append(action);

        //    string page = Regex_ReplaceMatch(String, @"\s", "%20");
        //    page = String.Format("&page={0}", String);
        //    sb_URL.Append(page);

        //    string prop = "sections";
        //    prop = String.Format("&prop={0}", prop);
        //    sb_URL.Append(prop);

        //    string format = "json";
        //    format = String.Format("&format={0}", format);
        //    sb_URL.Append(format);

        //    string formatversion = "2";
        //    formatversion = String.Format("&formatversion={0}", formatversion);
        //    sb_URL.Append(formatversion);

        //    string andRedirects = "&redirects";
        //    sb_URL.Append(andRedirects);

        //    string URL = sb_URL.ToString();

        //    return URL;

        //}
        //public static string Regex_ReplaceMatch(string Input, string Pattern, string Replacement)
        //{
        //    string pattern = Pattern;
        //    string input = Input;
        //    string replacement = Replacement;
        //    Regex regex = new Regex(pattern);

        //    string output = regex.Replace(input, replacement);
        //    return output;
        //}

        //public class sections
        //{
        //    public string Section_Title { get; set; }
        //    public string Section_Index { get; set; }
        //    public string Section_Num { get; set; }
        //}

        //private async void Submit_Click(object sender, RoutedEventArgs e)
        //{
        //    Rootobject rootobj = await GetPageSections(searchQuery.Text);

        //    List<Node_1> n1_list = new List<Node_1>();
        //    List<Node_2> n2_list = new List<Node_2>();
        //    List<Node_3> n3_list = new List<Node_3>();
        //    List<Node_4> n4_list = new List<Node_4>();

        //    WikiContent_Rootobject WikiContent_Rootobj = await PutSectionsIntoNodes(rootobj);

        //    ForEachContentHolder contentholder = new ForEachContentHolder();
        //    foreach (Node_1 n1 in WikiContent_Rootobj.Node_1_list)
        //    {
        //        contentholder.StringContent += Environment.NewLine;
        //        contentholder.StringContent += Environment.NewLine;
        //        contentholder.StringContent += "-- N1 -- " + n1.Title;
        //        contentholder.StringContent += Environment.NewLine;
        //        contentholder.StringContent += n1.Content;
        //        contentholder.StringContent += Environment.NewLine;

        //        if (n1.Node_2_list == null)
        //        {}
        //        else
        //        { 
        //            foreach (Node_2 n2 in n1.Node_2_list)
        //            { 
        //            contentholder.StringContent += "-- N2 -- " + n2.Title;
        //            contentholder.StringContent += Environment.NewLine;
        //            contentholder.StringContent += n2.Content;
        //            contentholder.StringContent += Environment.NewLine;

        //                if (n2.Node_3_list == null)
        //                { }
        //            else
        //                { 
        //                foreach (Node_3 n3 in n2.Node_3_list)
        //                    {
        //                    contentholder.StringContent += "N3 -- " + n3.Title;
        //                    contentholder.StringContent += Environment.NewLine;
        //                    contentholder.StringContent += n3.Content;
        //                    contentholder.StringContent += Environment.NewLine;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    Textblock1.Text = contentholder.StringContent;
        //}
    }

}

