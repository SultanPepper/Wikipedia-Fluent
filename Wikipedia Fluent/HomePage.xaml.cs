using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private ObservableCollection<Prefixsearch> Suggestions;
        SuggestBox suggestbox = new SuggestBox();
        ForEachContentHolder imagenames = new ForEachContentHolder();
        List<string> imageurls = new List<string>();
        List<Imageinfo> imagelist = new List<Imageinfo>();

        WikiPageContentsToPass ParsedWikiPage = new WikiPageContentsToPass(); //Where PageTitle and PageContent will be saved



        string[] Content_Under_Header = new string[] { };



        public HomePage()
        {
            this.InitializeComponent();

            searchQuery.DataContext = Suggestions;
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


            //Adds introduction information into variable
            Introduction introduction = new Introduction();
            Rootobject intro = await GetPageContent(searchQuery.Text, "0");
            string data_AsString = intro.query.pages[0].revisions[0].content;

            introduction.Content = data_AsString; //Gets intro paragraph
            data_AsString = wikiContent.Regex_DeleteData(data_AsString, @"(\n|^)'''(.|\n)*?(?=\n==(\w|\d))", ""); //Removes intro paragraph

            introduction.DataTable = wikiContent.GetIntroTablesAndData(data_AsString); //Gets table data
            data_AsString = wikiContent.Regex_DeleteData(data_AsString, @"^{{(.|\n)*?\n}}(?=\n''')", ""); //Removes table data




            //Gets intro images
            string intro_tables_and_content = String.Format("{0}{1}", introduction.DataTable, introduction.Content);
            List<ParsedImageInfo> intro_imageinfo_list = new List<ParsedImageInfo>();
            intro_imageinfo_list = GetImageInfoFromContent(intro_tables_and_content);

            //Intro images
            int intro_imageinfo_count = intro_imageinfo_list.Count;
            for(int i = 0; i < intro_imageinfo_count; i++)
            {
                Rootobject rootobject_image = new Rootobject();
                string parsedImageName = ParseImageName(intro_imageinfo_list[i].Filename);
                string imageinfoURL = GetImageInfoURL(parsedImageName);
                rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                SetImageProperties(rootobject_image, intro_imageinfo_list, i);
            }

            wikiContent.Introduction = introduction;

            Rootobject rootobj = await GetPageSections(searchQuery.Text);


            wikiContent = await PutSectionsIntoNodes(rootobj);
            wikiContent.Introduction = introduction;
            wikiContent.PageTitle = data_AsRootObject.parse.title;

            ForEachContentHolder contentholder = new ForEachContentHolder();

            
            //Textblock1.Text = contentholder.StringContent;

            //Teeing up info to pass to next page
            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();

            wikiContent.RemainingContent = data_AsString;
            Frame.Navigate(typeof(ContentPage), wikiContent);
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

            if(e.Key == VirtualKey.Enter
                && searchtext != empty
                && searchtext != space)
            {
                SubmitButtonEvent();
            }
        }
        private void searchQuery_Loaded(object sender, RoutedEventArgs e)
        {
            searchQuery.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }


        private async void searchQuery_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {



        }
        private void searchQuery_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if(args.ChosenSuggestion != null)
            {
                //do smoething
            }

        }
        private void searchQuery_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {


                List<Prefixsearch> prefixsearches = new List<Prefixsearch>();
                Suggestions = new ObservableCollection<Prefixsearch>();

                var matchingresult = suggestbox.GetMatchingSuggestion(sender.Text);
                sender.ItemsSource = matchingresult.ToList();


                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = dataset;
            }
        }


        //Image methods
        public static List<ParsedImageInfo> GetImageInfoFromContent(string Content)
        {
            string input = Content;

            string pattern = @"\[\[File:.*?\]\]\n";
            string[] images = Regex.Matches(input, pattern)
                                .OfType<Match>()
                                .Select(m => m.Value)
                                .ToArray();

            List<ParsedImageInfo> imglist = new List<ParsedImageInfo>();

            foreach(var Var in images)
            {
                bool added_to_object = false;
                string pattern0 = @"\n?\[?\[(File:.*?)\|.*?(\]\]\n|\]\]$)";
                ParsedImageInfo imginfo = new ParsedImageInfo();
                Regex regex = new Regex("");

                regex = new Regex(pattern0);
                Match m_file = regex.Match(Var);

                //Get image name
                if(m_file.Success)
                {
                    imginfo.Filename = Var;

                    string replacedtext = regex.Replace(Var, @"$1");
                    imginfo.Filename = replacedtext;
                    added_to_object = true;
                }
                else { }
                pattern0 = @"thumb";
                regex = new Regex(pattern0);
                Match m_thumb = regex.Match(Var);

                //Determine if a thumbnail is requested
                if(m_thumb.Success)
                {
                    imginfo.Thumb = true;
                    added_to_object = true;
                }
                else
                {
                    imginfo.Thumb = false;
                }
                pattern0 = @"upright=((\d|\.){2,5})";
                regex = new Regex(pattern0);
                Match m_uprightequals = regex.Match(Var);

                //Get the upright='some float' value
                if(m_uprightequals.Success)
                {
                    pattern0 = @"\w";
                    regex = new Regex(pattern0);
                    Match has_letter = regex.Match(m_uprightequals.Groups[1].Value);

                    //If the extracted val has a letter in it, don't try to turn to float
                    if(has_letter.Success) { }
                    else
                    {
                        float val_as_float = float.Parse(m_uprightequals.Groups[1].Value);
                        added_to_object = true;
                    }
                }
                else { }

                //Get the file description (wikipage is usually part of this!)
                pattern0 = @"File:.*?\|([^\|]*?)(\]\]\n|\]\]$)";
                regex = new Regex(pattern0);
                Match m_imagedescription = regex.Match(Var);
                if(m_imagedescription.Success)
                {
                    imginfo.ImageDescription = m_imagedescription.Groups[1].Value;
                }
                else { }
                pattern0 = @"File:.*?\|.*?(\[\[(.*?)\]\]).*?(\]\]\n|\]\]$)";
                regex = new Regex(pattern0);
                Match m_associatedwikipage = regex.Match(Var);

                //Get the Wikipedia page associated with the image
                if(m_associatedwikipage.Success)
                {
                    imginfo.AssociatedWikiPageName = m_associatedwikipage.Groups[2].Value;
                    added_to_object = true;
                }
                else { }

                //Only if there's a file name, add to the list
                if(String.IsNullOrEmpty(imginfo.Filename))
                {

                }
                else
                {
                    if(added_to_object == true)
                    {
                        imglist.Add(imginfo);
                    }
                }

            }

            return imglist;
        }
        public static string ParseImageName(string image_name)
        {
            //Parsing the title
            string input_img = image_name;
            string pattern_img = " ";
            string replacement_img = "_";
            string input_parsed;


            if(String.IsNullOrEmpty(input_img))
            {
                input_parsed = "";
            }
            else
            {
                Regex regex = new Regex(pattern_img);
                input_parsed = regex.Replace(input_img, replacement_img);

                pattern_img = "&";
                replacement_img = "%26";
                regex = new Regex(pattern_img);
                input_parsed = regex.Replace(input_parsed, replacement_img);
            }

            return input_parsed;


        }
        public static string GetImageInfoURL(string file_name)
        {
            WikiContent_Rootobject wikiobj = new WikiContent_Rootobject();

            string scheme = "https://";
            StringBuilder sb_ImageInfoURL = new StringBuilder(scheme);

            string language = "en";
            language = String.Format("{0}.", language);
            sb_ImageInfoURL.Append(language);

            string authority = "wikipedia.org";
            sb_ImageInfoURL.Append(authority);

            string api_path = "/w/api.php";
            sb_ImageInfoURL.Append(api_path);

            string action = "query";
            action = String.Format("?action={0}", action);
            sb_ImageInfoURL.Append(action);

            string titles = file_name;
            action = String.Format("&titles={0}", titles);
            sb_ImageInfoURL.Append(action);

            string prop = "imageinfo";
            prop = String.Format("&prop={0}", prop);
            sb_ImageInfoURL.Append(prop);

            string iiprop = "url|size|mediatype|canonicaltitle|parsedcomment";
            iiprop = String.Format("&iiprop={0}", iiprop);
            sb_ImageInfoURL.Append(iiprop);

            string iiurlwidth = "100"; //Not used now, to be later customized to call resized images. Req to get thumburl
            iiurlwidth = String.Format("&iiurlwidth={0}", iiurlwidth);
            sb_ImageInfoURL.Append(iiurlwidth);

            string format = "json";
            format = String.Format("&format={0}", format);
            sb_ImageInfoURL.Append(format);

            string formatversion = "2";
            formatversion = String.Format("&formatversion={0}", formatversion);
            sb_ImageInfoURL.Append(formatversion);


            string URL = sb_ImageInfoURL.ToString();
            return URL;
        }
        public static void SetImageProperties(Rootobject rootobj, List<ParsedImageInfo> imageinfo_list, int i)
        {
            if(rootobj.query.pages[0].imageinfo == null)
            { }
            else
            {
                string image_DL_URL = rootobj.query.pages[0].imageinfo[0].url;
                int image_size = rootobj.query.pages[0].imageinfo[0].size;
                int image_height = rootobj.query.pages[0].imageinfo[0].height;
                int image_width = rootobj.query.pages[0].imageinfo[0].width;

                string image_description_URL = rootobj.query.pages[0].imageinfo[0].descriptionurl;
                string image_parsed_comment = rootobj.query.pages[0].imageinfo[0].parsedcomment;
                string media_type = rootobj.query.pages[0].imageinfo[0].mediatype;
                string canonical_title = rootobj.query.pages[0].imageinfo[0].canonicaltitle;

                string thumb_DL_URL = rootobj.query.pages[0].imageinfo[0].thumburl;
                int thumb_width = rootobj.query.pages[0].imageinfo[0].thumbwidth;
                int thumb_height = rootobj.query.pages[0].imageinfo[0].thumbheight;

                if(imageinfo_list[i] != null)
                {

                    imageinfo_list[i].URL = image_DL_URL;
                    imageinfo_list[i].Size = image_size;
                    imageinfo_list[i].Height = image_height;
                    imageinfo_list[i].Width = image_width;
                    imageinfo_list[i].DescriptionURL = image_description_URL;
                    imageinfo_list[i].ParsedComment = image_parsed_comment;
                    imageinfo_list[i].Mediatype = media_type;
                    imageinfo_list[i].CanonicalTitle = canonical_title;
                    imageinfo_list[i].ThumbURL = thumb_DL_URL;
                    imageinfo_list[i].ThumbWidth = thumb_width;
                    imageinfo_list[i].ThumbHeight = thumb_height;
                }

                else
                {
                    imageinfo_list.Add(new ParsedImageInfo
                    {
                        URL = image_DL_URL,
                        Size = image_size,
                        Height = image_height,
                        Width = image_width,
                        DescriptionURL = image_description_URL,
                        ParsedComment = image_parsed_comment,
                        Mediatype = media_type,
                        CanonicalTitle = canonical_title,
                        ThumbURL = thumb_DL_URL,
                        ThumbWidth = thumb_width,
                        ThumbHeight = thumb_height
                    });
                }
            }


        }
        public static async Task<Rootobject> GetImageInfoFromAPI(string imageURL)
        {
            Rootobject rootobject = new Rootobject();
            Imageinfo imginfo = new Imageinfo();



            HttpClient http = new HttpClient();
            var response = await http.GetAsync(imageURL);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            return data;


        }
        public string RemoveImageInfoFromContent(string Content)
        {
            string input = Content;
            string pattern = @"@""(^\[|\n\[)\[.*?]]]]";
            string replacement = "";
            Regex regex = new Regex(pattern);
            string replacedtext = regex.Replace(input, replacement);

            return replacedtext;
        }



        public bool Does_Intro_HaveImageInfo(WikiContent_Rootobject wikicontent_rootobject)
        {
            if(wikicontent_rootobject.Introduction.ParsedImage_list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        public bool Does_Node_HaveImageInfo(WikiContent_Rootobject wikicontent_rootobject, int N1_index)
        {
            if(wikicontent_rootobject.Node_1_list[N1_index].ParsedImage_list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Does_Node_HaveImageInfo(WikiContent_Rootobject wikicontent_rootobject, int N1_index, int N2_index)
        {
            if(wikicontent_rootobject.Node_1_list[N1_index].Node_2_list[N2_index]
                .ParsedImage_list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public bool Does_Node_HaveImageInfo(WikiContent_Rootobject wikicontent_rootobject, int N1_index, int N2_index, int N3_index)
        {
            if(wikicontent_rootobject.Node_1_list[N1_index].Node_2_list[N2_index]
                .Node_3_list[N3_index].ParsedImage_list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool Does_Node_HaveImageInfo(WikiContent_Rootobject wikicontent_rootobject, int N1_index, int N2_index, int N3_index, int N4_index)
        {
            if(wikicontent_rootobject.Node_1_list[N1_index].Node_2_list[N2_index]
                .Node_3_list[N3_index].Node_4_list[N4_index].ParsedImage_list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        //These cause problems
        public static void Set_N2_Regex(Regex n2_regex)
        {
            string n2_pattern = @"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))";
            n2_regex = new Regex(n2_pattern);
        }
        public static void Set_N3_Regex(Regex n3_regex)
        {
            string n3_pattern = @"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))";
            n3_regex = new Regex(n3_pattern);
        }

        public static string Regex_ReplaceMatch(string Input, string Pattern, string Replacement)
        {
            string pattern = Pattern;
            string input = Input;
            string replacement = Replacement;
            Regex regex = new Regex(pattern);

            string output = regex.Replace(input, replacement);
            return output;
        }
        public async Task<WikiContent_Rootobject> PutSectionsIntoNodes(Rootobject rootobject)
        {
            WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();
            List<Node_1> n1_list = new List<Node_1>();

            wikiContent.Node_1_list = n1_list;

            List<Node_2> n2_list = new List<Node_2>();
            List<Node_3> n3_list = new List<Node_3>();
            List<Node_4> n4_list = new List<Node_4>();

            int n1 = -1;
            int n2 = -1;
            int n3 = -1;
            int n4 = 0;

            foreach(Section section in rootobject.parse.sections)
            {
                string section_title = section.anchor;
                string section_index = section.index;
                string section_num = section.number;




                string input = section.number;

                //n1
                string pattern_n1 = @"^\d{1,3}$";
                Regex regex_n1 = new Regex(pattern_n1);
                Match m_n1 = regex_n1.Match(input);

                //n2
                string pattern_n2 = @"^\d{1,3}\.\d{1,3}$";
                Regex regex_n2 = new Regex(pattern_n2);
                Match m_n2 = regex_n2.Match(input);

                //n3
                string pattern_n3 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}$";
                Regex regex_n3 = new Regex(pattern_n3);
                Match m_n3 = regex_n3.Match(input);

                //n4
                string pattern_n4 = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
                Regex regex_n4 = new Regex(pattern_n4);
                Match m_n4 = regex_n1.Match(input);

                if(m_n1.Success)
                {
                    n2 = -1;
                    n3 = -1;
                    n4 = -1;
                    //Add everything added previously

                    Node_1 n1_iteration = new Node_1();
                    List<Node_2> n2list = new List<Node_2>();

                    n1_iteration.Node_2_list = n2list;

                    n1_iteration.SectionTitle = section_title;
                    n1_iteration.SectionNum = section_num;
                    n1_iteration.SectionIndex = section_index;

                    //string n1_and_derivative_content = rootobject.query.pages[0].revisions[0].content;
                    Rootobject n1_rootobject = await GetPageContent(searchQuery.Text, section_index);
                    string SectionContent = n1_rootobject.query.pages[0].revisions[0].content;

                    //And Saves value in N1
                    GetAllContent(SectionContent, n1_iteration);


                    wikiContent.Node_1_list.Add(n1_iteration);


                    //Now empty the node so we add the current val into it
                    Node_1 n1_empty = new Node_1();
                    n1_iteration = n1_empty;

                    n1++;
                }

                else
                { }

                
            }
            return wikiContent;
        }
        public async static void GetAllContent(string SectionContent, Node_1 n1_iteration)
        {
            string n1_andderivatives = SectionContent;

            //Get N1 title and content
            string n1_pattern = @"(^=|\n=)=([^=]?.+?)==((.|\n)*?)(?=($|\n===))";
            Regex regex_select_n1 = new Regex(n1_pattern);
            Match m_n1 = regex_select_n1.Match(SectionContent);
            if(m_n1.Success)
            {
                n1_iteration.Title = m_n1.Groups[2].Value;

                string n1_content = m_n1.Groups[3].Value;
                n1_iteration.Content = n1_content;

            }



            string n2_pattern_and_deriv = @"(^=|\n=)==([^=]?.+?)===((\n|.)*?)(?=($|\n={3}(\w|\d|\s)))";

            Regex regex_select_n2_and_deriv = new Regex(n2_pattern_and_deriv);
            MatchCollection n2_match_collection = regex_select_n2_and_deriv.Matches(SectionContent);
            int n_2_match_collection_count = n2_match_collection.Count;

            if(n_2_match_collection_count > 0)
            {
                List<Node_2> n2_list = new List<Node_2>();

                foreach(Match match_n2 in n2_match_collection)
                {
                    Node_2 node_2_interation = new Node_2();


                    string n2_only = @"(^=|\n=)==([^=]?.+?)===((.|\n)*?)(?=($|\n={3,5}(\w|\d|\s)))";
                    Regex regex_select_n2 = new Regex(n2_only);
                    Match m_n2 = regex_select_n2.Match(match_n2.Value);

                    node_2_interation.Title = m_n2.Groups[2].Value;

                    string n2_content = m_n2.Groups[3].Value;
                    node_2_interation.Content = n2_content;


                    string n3_pattern_and_deriv = @"(^=|\n=)===([^=].+?)====((\n|.)*?)(?=($|\n={4}(\w|\d|\s)))";
                    Regex regex_select_n3_and_deriv = new Regex(n3_pattern_and_deriv);
                    MatchCollection n3_match_collection = regex_select_n3_and_deriv.Matches(m_n2.Value);

                    int n_3_match_collection_count = n3_match_collection.Count;

                    if(n_3_match_collection_count > 0)
                    {
                        List<Node_3> n3_list = new List<Node_3>();

                        foreach(Match m_n3 in n3_match_collection)
                        {
                            Node_3 node_3_iteration = new Node_3();

                            node_3_iteration.Title = m_n3.Groups[2].Value;

                            string n3_content = m_n2.Groups[3].Value;

                            node_3_iteration.Content = n3_content;

                            string n4_pattern_and_deriv = @"(^=|\n=)====([^=].+?)=====((\n|.)*?)(?=($|\n={5}(\w|\d|\s)))";
                            Regex regex_select_n4_and_deriv = new Regex(n4_pattern_and_deriv);
                            MatchCollection n4_match_collection = regex_select_n4_and_deriv.Matches(m_n3.Value);

                            int n_4_match_collection_count = n4_match_collection.Count;

                            if(n_4_match_collection_count > 0)
                            {
                                List<Node_4> n4_list = new List<Node_4>();

                                foreach(Match m_n4 in n4_match_collection)
                                {
                                    Node_4 node_4_iteration = new Node_4();

                                    node_4_iteration.Title = m_n4.Groups[2].Value;

                                    string n4_content = m_n4.Groups[3].Value;
                                    node_4_iteration.Content = n4_content;

                                    //Add n4 iteration to list
                                    n4_list.Add(node_4_iteration);
                                }
                                //Add n4 list to n3 iteration
                                node_3_iteration.Node_4_list = n4_list;
                            }

                            //Add n3 iteration to n3 list
                            n3_list.Add(node_3_iteration);

                        }

                        //Add n3 list to n2 iteration
                        node_2_interation.Node_3_list = n3_list;

                    }

                    //Add n2 iteration to n2 list
                    n2_list.Add(node_2_interation);
                }
                //add n2 list to n1 iteration
                n1_iteration.Node_2_list = n2_list;
            }
        }
        public static void FixWhiteSpace(string Input)
        {
            string pattern = @"^\n{1,5}";
            Regex removeintrospaces = new Regex(pattern);
            Input = removeintrospaces.Replace(Input, "");
            Input = Environment.NewLine + Input;

            pattern = @"\n{1,6}$";
            Regex removeendspace = new Regex(pattern);
            Input = removeendspace.Replace(Input, "");
            Input = Input + Environment.NewLine + Environment.NewLine;


        }
        public string GetContentURL(string SearchQuery, string Section)
        {
            //https://en.wikipedia.org/w/api.php?action=parse&page=New_York&format=json&prop=wikitext&section=4
            //prop=revisions&titles=california&rvprop=content&rvsection=2&formatversion=2

            string searchquery = SearchQuery;

            string scheme = "https://";
            StringBuilder sb_URL = new StringBuilder(scheme);

            string language = "en";
            language = String.Format("{0}.", language);
            sb_URL.Append(language);

            string authority = "wikipedia.org";
            sb_URL.Append(authority);

            string api_path = "/w/api.php";
            sb_URL.Append(api_path);

            string action = "query";
            action = String.Format("?action={0}", action);
            sb_URL.Append(action);

            string titles = Regex_ReplaceMatch(searchquery, @"\s", "%20");
            titles = String.Format("&titles={0}", titles);
            sb_URL.Append(titles);

            string prop = "revisions";
            prop = String.Format("&prop={0}", prop);
            sb_URL.Append(prop);

            string rvprop = "content";
            rvprop = String.Format("&rvprop={0}", rvprop);
            sb_URL.Append(rvprop);

            string format = "json";
            format = String.Format("&format={0}", format);
            sb_URL.Append(format);

            string formatversion = "2";
            formatversion = String.Format("&formatversion={0}", formatversion);
            sb_URL.Append(formatversion);

            string andRedirects = "&redirects";
            sb_URL.Append(andRedirects);

            string rvsection = Section;
            rvsection = String.Format("&rvsection={0}", rvsection);
            sb_URL.Append(rvsection);

            string URL = sb_URL.ToString();

            return URL;

        }
        public static string GetSectionsURL(string String)
        {
            //string searchquery = SearchQuery;

            string scheme = "https://";
            StringBuilder sb_URL = new StringBuilder(scheme);

            string language = "en";
            language = String.Format("{0}.", language);
            sb_URL.Append(language);

            string authority = "wikipedia.org";
            sb_URL.Append(authority);

            string api_path = "/w/api.php";
            sb_URL.Append(api_path);

            string action = "parse";
            action = String.Format("?action={0}", action);
            sb_URL.Append(action);

            string page = Regex_ReplaceMatch(String, @"\s", "%20");
            page = String.Format("&page={0}", String);
            sb_URL.Append(page);

            string prop = "sections";
            prop = String.Format("&prop={0}", prop);
            sb_URL.Append(prop);

            string format = "json";
            format = String.Format("&format={0}", format);
            sb_URL.Append(format);

            string formatversion = "2";
            formatversion = String.Format("&formatversion={0}", formatversion);
            sb_URL.Append(formatversion);

            string andRedirects = "&redirects";
            sb_URL.Append(andRedirects);

            string URL = sb_URL.ToString();

            return URL;

        }
        public async Task<Rootobject> GetPageContent(string SearchQuery, string SectionNum)
        {
            HttpClient http = new HttpClient();
            string searchquery = SearchQuery;

            string URL = GetContentURL(searchquery, SectionNum);

            var response = await http.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            return data;
        }

        public static async Task<Rootobject> GetPageSections(string String)
        {
            HttpClient http = new HttpClient();
            //string searchquery = SearchQuery;
            string URL = GetSectionsURL(String);

            var response = await http.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            return data;
        }
    }
 
}
    