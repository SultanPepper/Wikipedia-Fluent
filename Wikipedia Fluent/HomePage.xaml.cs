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
            string data_AsString = data_AsRootObject.parse.wikitext;



            //Defines page title
            wikiContent.PageTitle = wikiContent.GetPageTitle(data_AsRootObject);

            //Adds introduction information into variable
            Introduction introduction = new Introduction();
            introduction.DataTable = wikiContent.GetIntroTablesAndData(data_AsString); //Gets table data
            data_AsString = wikiContent.Regex_DeleteData(data_AsString, @"^{{(.|\n)*?\n}}(?=\n''')", ""); //Removes table data

            introduction.Content = wikiContent.GetIntroContent(data_AsString); //Gets intro paragraph
            data_AsString = wikiContent.Regex_DeleteData(data_AsString, @"(\\n|^)'''(.|\n)*?(?=\n==(\w|\d))", ""); //Removes intro paragraph


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

            //Remove image text
            //introduction.Content = wikiContent.Regex_DeleteData(wikiContent.GetIntroContent(data_AsStringCut),
            //    @"\n?(\[|^\[)\[?File:.*?\[?.*?(\]{2,4}\n|\]{2,4}$)(?=([^\n?\[\[\[?\[?]))", "");




            wikiContent.Introduction = introduction;


            //HEADERS, SUBHEADERS, and SUBSUBHEADERS            
            string[] n1s_and_derivatives = wikiContent.Regex_Put_N1_AndDerivativesToArray(data_AsString);

            List<Node_1> N1 = new List<Node_1>(); //NOTE: White space still needs to be formatted
            foreach (string n1_and_derivatives in n1s_and_derivatives)
            {
                Node_1 n1_iteration = new Node_1();
                string n1_ContentAndTitle = wikiContent.Regex_Get_N1_ContentAndTitle(n1_and_derivatives); ////// <============== N1 Content PLUS Title

                n1_iteration.Title = wikiContent.Regex_Get_N1_Title(n1_and_derivatives); /////////////// <============== N1 Title

                List<ParsedImageInfo> n1_imageinfo_list = new List<ParsedImageInfo>();
                string n1_iteration_content = wikiContent.Regex_Get_N1_Content(n1_and_derivatives);
                n1_imageinfo_list = GetImageInfoFromContent(n1_iteration_content); //Get image data

                //N1 images
                int n1_imageinfo_count = n1_imageinfo_list.Count;
                for (int i = 0; i < n1_imageinfo_count; i++)
                {
                    Rootobject rootobject_image = new Rootobject();
                    string parsedImageName = ParseImageName(n1_imageinfo_list[i].Filename);
                    string imageinfoURL = GetImageInfoURL(parsedImageName);
                    rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                    SetImageProperties(rootobject_image, n1_imageinfo_list, 0);
                    
                }

                n1_iteration_content = RemoveImageInfoFromContent(n1_and_derivatives); ////// <============== Header Content (with image data removed!)               

                n1_iteration.Content = wikiContent.Regex_Get_N1_Content(n1_and_derivatives); ////// <============== N1 Content
                //n1_iteration.Content = wikiContent.Regex_DeleteData(wikiContent.Regex_Get_N1_Content(n1_and_derivatives),
                //@"\n?(\[|^\[)\[?File:.*?\[?.*?(\]{2,4}\n|\]{2,4}$)(?=([^\n?\[\[\[?\[?]))", ""); 

                //Contains Subheaders and derivatives
                List<Node_2> N2 = new List<Node_2>();
                string[] n2s_and_derivatives = wikiContent.Regex_Put_N2_AndDerivativesToArray(n1_and_derivatives);
                foreach (string n2_and_derivatives in n2s_and_derivatives)
                {
                    Regex regex = new Regex(@"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                    Match match_n2 = regex.Match(n2_and_derivatives);
                    Node_2 n2_iteration = new Node_2();
                    if (match_n2.Success)
                    {
                        string n2_ContentAndTitle = wikiContent.Regex_Get_N2_ContentAndTitle(match_n2.Value); ////////// <============== N2 Content PLUS Title

                        List<ParsedImageInfo> n2_imageinfo_list = new List<ParsedImageInfo>();
                        n2_iteration.Title = wikiContent.Regex_Get_N2_Title(match_n2.Value); //////////////// <============== N2 Title


                        //n2_iteration.Content = wikiContent.Regex_DeleteData(wikiContent.Regex_Get_N2_Content(match_n2.Value),
                        //                                  @"\n?(\[|^\[)\[?File:.*?\[?.*?(\]{2,4}\n|\]{2,4}$)(?=([^\n?\[\[\[?\[?]))", "");
                        n2_iteration.Content = wikiContent.Regex_Get_N2_Content(match_n2.Value);  /////// <============== N2 Content

                        string n2_iteration_content = wikiContent.Regex_Get_N2_Content(match_n2.Value);
                        n2_imageinfo_list = GetImageInfoFromContent(n2_iteration_content); //Get image data

                        //N2 images
                        int n2_imageinfo_count = n2_imageinfo_list.Count;
                        for(int i = 0; i < n2_imageinfo_count; i++)
                        {
                            Rootobject rootobject_image = new Rootobject();
                            string parsedImageName = ParseImageName(n2_imageinfo_list[i].Filename);
                            string imageinfoURL = GetImageInfoURL(parsedImageName);
                            rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                            SetImageProperties(rootobject_image, n2_imageinfo_list, i);
                        }
                        

                        List<Node_3> N3 = new List<Node_3>();
                        string[] n3s_and_derivatives = wikiContent.Regex_Put_N3_AndDerivativesToArray(n2_and_derivatives);
                        foreach (string n3_and_derivatives in n3s_and_derivatives)
                        {
                            regex = new Regex(@"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,5}(\w|\d|\s)|\n={2,5}(\w|\d|\s)|$)))");
                            Match match_n3 = regex.Match(n3_and_derivatives);
                            Node_3 n3_iteration = new Node_3();
                            if (match_n3.Success)
                            {
                                string n3_ContentAndTitle = wikiContent.Regex_Get_N3_ContentAndTitle(match_n3.Value); //////////////////////// <============== N3 Content PLUS Title


                                List<ParsedImageInfo> n3_imageinfo_list = new List<ParsedImageInfo>();
                                n3_imageinfo_list = GetImageInfoFromContent(data_AsString);

                                //N3 images
                                int n3_imageinfo_count = intro_imageinfo_list.Count;
                                for(int i = 0; i < intro_imageinfo_count; i++)
                                {
                                    Rootobject rootobject_image = new Rootobject();
                                    string parsedImageName = ParseImageName(intro_imageinfo_list[i].Filename);
                                    string imageinfoURL = GetImageInfoURL(parsedImageName);
                                    rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                                    SetImageProperties(rootobject_image, n3_imageinfo_list, i);
                                }


                                n3_iteration.Title = wikiContent.Regex_Get_N3_Title(match_n3.Value); ////////////////////////// <============== N3 Title
                                //n3_iteration.Content = wikiContent.Regex_DeleteData(wikiContent.Regex_Get_N3_Content(match_n3.Value),
                                //                    @"\n?(\[|^\[)\[?File:.*?\[?.*?(\]{2,4}\n|\]{2,4}$)(?=([^\n?\[\[\[?\[?]))", "");
                                n3_iteration.Content = wikiContent.Regex_Get_N3_Content(match_n3.Value);        ////////////// <============== N3 Content

                                List<Node_4> N4 = new List<Node_4>();
                                string[] n4s_and_derivatives = wikiContent.Regex_Put_N4_AndDerivativesToArray(n3_and_derivatives);
                                foreach (string n4_and_derivatives in n4s_and_derivatives)
                                {
                                    //And current S3 header to list of S3 headers
                                    string n4_ContentAndTitle = wikiContent.Regex_Get_N4_ContentAndTitle(n4_and_derivatives); /////////////// <============== N4 COntent PLUS Title

                                    List<ParsedImageInfo> n4_imageinfo_list = new List<ParsedImageInfo>();
                                    n3_imageinfo_list = GetImageInfoFromContent(data_AsString);

                                    //N4 images
                                    int n4_imageinfo_count = intro_imageinfo_list.Count;
                                    for(int i = 0; i < intro_imageinfo_count; i++)
                                    {
                                        Rootobject rootobject_image = new Rootobject();
                                        string parsedImageName = ParseImageName(intro_imageinfo_list[i].Filename);
                                        string imageinfoURL = GetImageInfoURL(parsedImageName);
                                        rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                                        SetImageProperties(rootobject_image, n4_imageinfo_list, i);
                                    }


                                    string n4_Title = wikiContent.Regex_Get_N4_Title(n4_and_derivatives); //////////////////// <============== N4 Title

                                    //string n4_Content= wikiContent.Regex_DeleteData(wikiContent.Regex_Get_N4_Content(n4_and_derivatives),
                                    //                @"(\n\[|^\[)\[?File:.*?\[.*(\]\]{2,4}\n|\]\]{2,4}$)", "");
                                    string n4_Content = wikiContent.Regex_Get_N4_Content(n4_and_derivatives); ////////////// <============== N4 Content

                                    

                                    N4.Add(new Node_4() { Content = n4_Content, Title = n4_Title });
                                }

                                // Capital letter = List (e.g., N3) || Lowercase = Single instance of object (e.g., n3_iteration)
                                //Add list of fourth node as object in third node
                                n3_iteration.Node_4_list = N4;
                            }
                            //Add third node to list of third nodes
                            N3.Add(n3_iteration);
                        }
                        //Add list of current S2 headers to current S1 header
                        n2_iteration.Node_3_list = N3;
                        n2_iteration.ParsedImage_list = n2_imageinfo_list;
                    }
                    //Add current S1 header to list of S1 headers
                    N2.Add(n2_iteration);
                }
                //Add list of S1 headers to current header
                n1_iteration.Node_2_list = N2;
                n1_iteration.ParsedImage_list = n1_imageinfo_list;
                //Add current header to list of headers
                N1.Add(n1_iteration);
            }
            //Add list of headers to rootobject
            wikiContent.Node_1_list = N1;

            ForEachContentHolder teststring = new ForEachContentHolder();
            int header_length = wikiContent.Node_1_list.Count;

            //Teeing up info to pass to next page
            WikiPageContentsToPass PassThruContent = new WikiPageContentsToPass();
            PassThruContent.PageTitle = wikiContent.PageTitle;
            PassThruContent.PageContent = teststring.StringContent + data_AsString;




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
            
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
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
        public List<ParsedImageInfo> GetImageInfoFromContent(string Content)
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
                if (String.IsNullOrEmpty(imginfo.Filename))
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
        public string ParseImageName(string image_name)
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
            if (rootobj.query.pages[0].imageinfo == null)
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


        
    }
 
}
    