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
using Windows.UI.Text;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class ContentPage : Page
    {
        WikiContent_Rootobject wikiContent = new WikiContent_Rootobject();

        public ContentPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            string title;
            string content;
            string regexpattern;

            Run run = new Run();
            Paragraph paragraph = new Paragraph();
            RichTextBlock rtb = new RichTextBlock();
            StackPanel stackpanel = new StackPanel();

            WikiContent_Rootobject parametersPassed = (WikiContent_Rootobject)e.Parameter;


            //////Add Article title
            RichTextBlock title_rtb0 = new RichTextBlock();
            Paragraph title_paragraph = new Paragraph();
            Run title_run = new Run();
            SetProp_PageTitle_RTB(title_rtb);
            Add_PageTitle(parametersPassed, title_run, title_paragraph, title_rtb); //Smallest visual state
            title_rtb = title_rtb0;
            pageTitle.Text = parametersPassed.PageTitle; //Every other visual state


            //////Intro

            //^Add Intro Instantiations
            RichTextBlock intro_rtb = new RichTextBlock();
            Paragraph intro_paragraph = new Paragraph();
            Run intro_run_dataAndTables = new Run();
            Run intro_run_content = new Run();
            SetProps_IntroObjectProperties(intro_rtb, intro_run_dataAndTables, intro_run_content);

            //intro_run_dataAndTables.Text = parametersPassed.introduction.DataTable;
            //intro_paragraph.Inlines.Add(intro_run_dataAndTables);

            //^Add each intro image
            List<ParsedImageInfo> intro_imageinfo_list = new List<ParsedImageInfo>();
            intro_imageinfo_list = GetImageInfoFromContent(parametersPassed.Introduction.Content);

            int intro_imageinfo_count = intro_imageinfo_list.Count;
            for(int i = 0; i < intro_imageinfo_count; i++)
            {
                Rootobject rootobject_image = new Rootobject();
                string parsedImageName = ParseImageName(intro_imageinfo_list[i].Filename);
                string imageinfoURL = GetImageInfoURL(parsedImageName);
                rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                SetImageProperties(rootobject_image, intro_imageinfo_list, 0);

                string imgurl = rootobject_image.query.pages[0].imageinfo[0].url;
                string imgname = rootobject_image.query.pages[0].imageinfo[0].canonicaltitle;
                string imgdescription = rootobject_image.query.pages[0].imageinfo[0].descriptionurl;

                AddSingleImage(imgurl, imgdescription, imgname, intro_rtb);
            }

            AddImages(parametersPassed.Introduction.ParsedImage_list, intro_rtb);

            //^Add Intro Content
            Add_Content_MinimalMarkdown(parametersPassed.Introduction.Content, intro_run_content, intro_paragraph);

            //^Add intro to RichTextBlock → Stackpanel
            Add_NodeToStackPanel(intro_paragraph, intro_rtb, ContentsStackPanel);

            //////Node #1
            int n1_count = parametersPassed.Node_1_list.Count;
            for(int a = 0; a < n1_count; a++)
            {
                //*Add N1 Instantiations
                RichTextBlock n1_rtb = new RichTextBlock();
                Paragraph n1_paragraph = new Paragraph();
                Run n1_run_title = new Run();
                Run n1_run_spacing = new Run();
                Run n1_run_content = new Run();
                Run n1_run_table = new Run();
                SetProps_N1_ObjectProperties(n1_rtb, n1_run_title, n1_run_content);
                

                //*Add N1 Title
                title = parametersPassed.Node_1_list[a].Title;
                regexpattern = @"\s?={2,5}\s?";
                Add_Title(title, regexpattern, n1_run_title, n1_paragraph);

                //*Add each N1 image
                List<ParsedImageInfo> n1_imageinfo_list = new List<ParsedImageInfo>();
                n1_imageinfo_list = GetImageInfoFromContent(parametersPassed.Node_1_list[a].Content);

                int n1_imageinfo_count = n1_imageinfo_list.Count;
                for (int i = 0; i <n1_imageinfo_count; i++)
                {
                    Rootobject rootobject_image = new Rootobject();
                    string parsedImageName = ParseImageName(n1_imageinfo_list[i].Filename);
                    string imageinfoURL = GetImageInfoURL(parsedImageName);
                    rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                    SetImageProperties(rootobject_image, n1_imageinfo_list, 0);

                    string imgurl = rootobject_image.query.pages[0].imageinfo[0].url;
                    string imgname = rootobject_image.query.pages[0].imageinfo[0].canonicaltitle;
                    string imgdescription = rootobject_image.query.pages[0].imageinfo[0].descriptionurl;

                    AddSingleImage(imgurl, imgdescription, imgname, n1_rtb);
                }

                //*Add N1 Content
                content = parametersPassed.Node_1_list[a].Content;
                Add_Content_MinimalMarkdown(content, n1_run_content, n1_paragraph);

                //*Add N1 tables
                PutTablesIn(parametersPassed.Node_1_list[a].Content, contentgrid, n1_rtb);

                //*Add N1 to RichTextBlock → Stackpanel
                Add_NodeToStackPanel(n1_paragraph, n1_rtb, ContentsStackPanel);



                //////Node #2
                if (parametersPassed.Node_1_list[a].Node_2_list != null)
                { 
                int n2_count = parametersPassed.Node_1_list[a].Node_2_list.Count;
                for(int b = 0; b < n2_count; b++)
                {
                    //**Add N2 Instantiations
                    RichTextBlock n2_rtb = new RichTextBlock();
                    Paragraph n2_paragraph = new Paragraph();
                    Run n2_run_title = new Run();
                    Run n2_run_spacing = new Run();
                    Run n2_run_content = new Run();
                    SetProps_N2_AndOn_ObjectProperties(n2_rtb, n2_run_title, n2_run_content);

                    //**Add N2 Title
                    title = parametersPassed.Node_1_list[a].Node_2_list[b].Title;
                    regexpattern = @"\s?={2,5}\s?";
                    Add_Title(title, regexpattern, n2_run_title, n2_paragraph);

                        //**Add each N2 image
                        List<ParsedImageInfo> n2_imageinfo_list = new List<ParsedImageInfo>();
                        n2_imageinfo_list = GetImageInfoFromContent(parametersPassed.Node_1_list[a].Node_2_list[b].Content);

                        int n2_imageinfo_count = n2_imageinfo_list.Count;
                        for(int i = 0; i < n2_imageinfo_count; i++)
                        {
                            Rootobject rootobject_image = new Rootobject();
                            string parsedImageName = ParseImageName(n2_imageinfo_list[i].Filename);
                            string imageinfoURL = GetImageInfoURL(parsedImageName);
                            rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                            SetImageProperties(rootobject_image, n2_imageinfo_list, 0);

                            string imgurl = rootobject_image.query.pages[0].imageinfo[0].url;
                            string imgname = rootobject_image.query.pages[0].imageinfo[0].canonicaltitle;
                            string imgdescription = rootobject_image.query.pages[0].imageinfo[0].descriptionurl;

                            AddSingleImage(imgurl, imgdescription, imgname, n2_rtb);
                        }

                        //**Add N2 Content
                        content = parametersPassed.Node_1_list[a].Node_2_list[b].Content;
                    Add_Content_MinimalMarkdown(content, n2_run_content, n2_paragraph);

                        //**Add N2 tables
                        PutTablesIn(parametersPassed.Node_1_list[a].Node_2_list[b].Content, contentgrid, n2_rtb);

                        //**Add N2 to RichTextBlock → Stackpanel
                        Add_NodeToStackPanel(n2_paragraph, n2_rtb, ContentsStackPanel);

                    //////Node #3
                    if (parametersPassed.Node_1_list[a].Node_2_list[b]
                                                    .Node_3_list != null)
                    { 
                    int n3_count = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                    .Node_3_list.Count;
                    for(int c = 0; c < n3_count; c++)
                    {
                        //***N3 Instantiations
                        RichTextBlock n3_rtb = new RichTextBlock();
                        Paragraph n3_paragraph = new Paragraph();
                        Run n3_run_title = new Run();
                        Run n3_run_spacing = new Run();
                        Run n3_run_content = new Run();
                        SetProps_N2_AndOn_ObjectProperties(n3_rtb, n3_run_title, n3_run_content);

                        //***Add N3 Title
                        title = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                 .Node_3_list[c].Title;
                        regexpattern = @"\s?={2,5}\s?";
                        Add_Title(title, regexpattern, n3_run_title, n3_paragraph);

                                //**Add each N3 image
                                List<ParsedImageInfo> n3_imageinfo_list = new List<ParsedImageInfo>();
                                n2_imageinfo_list = GetImageInfoFromContent(parametersPassed.Node_1_list[a].Node_2_list[b].Node_3_list[c].Content);

                                int n3_imageinfo_count = n3_imageinfo_list.Count;
                                for(int i = 0; i < n3_imageinfo_count; i++)
                                {
                                    Rootobject rootobject_image = new Rootobject();
                                    string parsedImageName = ParseImageName(n2_imageinfo_list[i].Filename);
                                    string imageinfoURL = GetImageInfoURL(parsedImageName);
                                    rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                                    SetImageProperties(rootobject_image, n2_imageinfo_list, 0);

                                    string imgurl = rootobject_image.query.pages[0].imageinfo[0].url;
                                    string imgname = rootobject_image.query.pages[0].imageinfo[0].canonicaltitle;
                                    string imgdescription = rootobject_image.query.pages[0].imageinfo[0].descriptionurl;

                                    AddSingleImage(imgurl, imgdescription, imgname, n3_rtb);
                                }

                                //***Add N3 content
                                content = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                              .Node_3_list[c].Content;
                        Add_Content_MinimalMarkdown(content, n3_run_content, n3_paragraph);

                        //***Add N4 to RichTextBlock → Stackpanel
                        Add_NodeToStackPanel(n3_paragraph, n3_rtb, ContentsStackPanel);

                        //////Node #4
                        if (parametersPassed.Node_1_list[a].Node_2_list[b]
                                                        .Node_3_list[c].Node_4_list != null)
                        { 
                        int n4_count = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                        .Node_3_list[c].Node_4_list.Count;
                        for(int d = 0; d < n4_count; d++)
                        {
                            //****N4 Instantiations
                            RichTextBlock n4_rtb = new RichTextBlock();
                            Paragraph n4_paragraph = new Paragraph();
                            Run n4_run_title = new Run();
                            Run n4_run_spacing = new Run();
                            Run n4_run_content = new Run();      
                            SetProps_N2_AndOn_ObjectProperties(n4_rtb, n4_run_title, n4_run_content);

                            //****Add N4 Title
                            title = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                    .Node_3_list[c].Node_4_list[d]
                                                    .Title;
                            regexpattern = @"\s?={2,5}\s?";
                            Add_Title(title, regexpattern, n4_run_title, n4_paragraph);

                                        //*****Add each N4 image
                                        List<ParsedImageInfo> n4_imageinfo_list = new List<ParsedImageInfo>();
                                        n4_imageinfo_list = GetImageInfoFromContent(parametersPassed.Node_1_list[a].Node_2_list[b].Node_3_list[c].Node_4_list[d].Content);

                                        int n4_imageinfo_count = n4_imageinfo_list.Count;
                                        for(int i = 0; i < n3_imageinfo_count; i++)
                                        {
                                            Rootobject rootobject_image = new Rootobject();
                                            string parsedImageName = ParseImageName(n4_imageinfo_list[i].Filename);
                                            string imageinfoURL = GetImageInfoURL(parsedImageName);
                                            rootobject_image = await GetImageInfoFromAPI(imageinfoURL);
                                            SetImageProperties(rootobject_image, n4_imageinfo_list, 0);

                                            string imgurl = rootobject_image.query.pages[0].imageinfo[0].url;
                                            string imgname = rootobject_image.query.pages[0].imageinfo[0].canonicaltitle;
                                            string imgdescription = rootobject_image.query.pages[0].imageinfo[0].descriptionurl;

                                            AddSingleImage(imgurl, imgdescription, imgname, n3_rtb);
                                        }


                                        //****Add N4 Content
                                        content = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                       .Node_3_list[c].Node_4_list[d]
                                                       .Content;
                            Add_Content_MinimalMarkdown(content, n4_run_content, n4_paragraph);

                            //****Add N4 to RichTextBlock → Stackpanel
                            Add_NodeToStackPanel(n4_paragraph, n4_rtb, ContentsStackPanel);
                        }
                        }
                    }
                    }
                }
                }
            }
            //pageContent.DataContext = parametersPassed.node_1[1].Content;
        }
        //Add tables
        public static void PutTablesIn(string SectionContent, Grid Parentgrid, RichTextBlock rtb)
        {

            ScrollViewer scrollviewer = new ScrollViewer();
            scrollviewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollviewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollviewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;

            Grid contentgrid = new Grid();

            //Add newly created grid as a child
            contentgrid.ColumnSpacing = 15;
            contentgrid.RowSpacing = 5;
            contentgrid.Margin = new Thickness(20, 20, 20, 20);

            //Extract the ENTIRE table
            string input = SectionContent;
            string pattern = @"{\|.*(.|\n)*?\|}";
            Regex regex = new Regex(pattern);
            MatchCollection EntireTables = Regex.Matches(input, pattern);
            Match TablePresent = regex.Match(input);
            if (TablePresent.Success)
            { 

            //Add each column result to that row
            pattern = @"(?<=(^\||\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\||$))";
            regex = new Regex(pattern);
            MatchCollection columns_count = Regex.Matches(EntireTables[0].Value, pattern);
            int column_counted = columns_count.Count;

            for (int i = 0; i < column_counted; i++)
            {
                //Create the new column
                ColumnDefinition coldef = new ColumnDefinition();
                //coldef.Width = new GridLength(1, GridUnitType.Star);
                coldef.Width = new GridLength(0, GridUnitType.Auto);
                contentgrid.ColumnDefinitions.Add(coldef);
            }

            //Get list of entire tables
            List<string> EntireTables_list = new List<string>();
            foreach (Match match_entiretable in EntireTables)
            {
                EntireTables_list.Add(match_entiretable.Value);
            }

            //Parse out the tables
            foreach (string Table in EntireTables_list)
            {
                InlineUIContainer tablecontainer = new InlineUIContainer();
                Paragraph tableparagraph = new Paragraph();
                int current_row = 0;

                //Get each row
                pattern = @"(?<=(\|-.*\n))(\n|.)*?(?=\n(\|-|\|}))";
                regex = new Regex(pattern);
                MatchCollection rows = Regex.Matches(Table, pattern);

                pattern = @"(?<=(^!|\n!|!!)).*?(?=(!!|\n!|\n\|))";
                regex = new Regex(pattern);
                MatchCollection headers = Regex.Matches(Table, pattern);
                Match m_header = regex.Match(Table);
                int current_column = 0;
                if (m_header.Success)
                {

                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = new GridLength(0, GridUnitType.Auto);
                    contentgrid.RowDefinitions.Add(rowdef);

                    foreach (Match header in headers)
                    {
                        TextBlock textblock = new TextBlock();
                        textblock.Text = header.Value;
                        textblock.FontSize = 16;
                        textblock.FontWeight = FontWeights.Bold;
                        textblock.HorizontalAlignment = HorizontalAlignment.Center;
                        textblock.VerticalAlignment = VerticalAlignment.Center;

                        contentgrid.Children.Add(textblock);

                        Grid.SetColumn(textblock, current_column);
                        Grid.SetRow(textblock, current_row);
                        current_column++;
                    }

                    current_row++;
                }

                foreach (Match m_row in rows)
                {
                    current_column = 0;

                    //Add a new row
                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = new GridLength(0, GridUnitType.Auto);
                    contentgrid.RowDefinitions.Add(rowdef);

                    //Add each column result to that row
                    pattern = @"(?<=(^\||\n\||\|\|))[^(\-|\+)].*?(?=(\|\||\n\||$))";
                    regex = new Regex(pattern);
                    MatchCollection columns = Regex.Matches(m_row.Value, pattern);



                    foreach (Match m_col in columns)
                    {


                        //Content we will be adding too
                        TextBlock textblock = new TextBlock();
                        textblock.Text = m_col.Value;
                        textblock.FontSize = 14;
                        textblock.FontWeight = FontWeights.SemiLight;
                        textblock.HorizontalAlignment = HorizontalAlignment.Left;
                        textblock.VerticalAlignment = VerticalAlignment.Center;

                        //Add block to grid
                        contentgrid.Children.Add(textblock);

                        //Set the row
                        Grid.SetColumn(textblock, current_column);
                        Grid.SetRow(textblock, current_row);

                        current_column++;
                    }
                    current_row++;


                    }

                /*
                    //Create container and add to it
                    tablecontainer.Child = contentgrid;
                    tableparagraph.Inlines.Add(tablecontainer);
                    rtb.Blocks.Add(tableparagraph);
                */

                }

        }
            else { }
        }



        //Image methods
        public static List<ParsedImageInfo> GetImageInfoFromContent(string Content)
        {
            string input = Content;

            string pattern = @"\[\[(File|Image):.*?\]\]\n";
            string[] images = Regex.Matches(input, pattern)
                                .OfType<Match>()
                                .Select(m => m.Value)
                                .ToArray();

            List<ParsedImageInfo> imglist = new List<ParsedImageInfo>();

            foreach(var Var in images)
            {
                bool added_to_object = false;
                string pattern0 = @"\n?\[?\[((File|Image):.*?)\|.*?(\]\]\n|\]\]$)";
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
                pattern0 = @"(File|Image):.*?\|([^\|]*?)(\]\]\n|\]\]$)";
                regex = new Regex(pattern0);
                Match m_imagedescription = regex.Match(Var);
                if(m_imagedescription.Success)
                {
                    imginfo.ImageDescription = m_imagedescription.Groups[1].Value;
                }
                else { }
                pattern0 = @"(File|Image):.*?\|.*?(\[\[(.*?)\]\]).*?(\]\]\n|\]\]$)";
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


            string pattern = @"(\s|\+)Cleaned";
            string input = URL;
            string replacement = "";
            Regex regex = new Regex(pattern);
            Match url = regex.Match(input);

            if (url.Success)
            {
                URL = regex.Replace(input, replacement);
            }

            else
            {

            }


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
        public static void AddImages(List<ParsedImageInfo> imageinfolist, RichTextBlock rtb)
        {
            if (imageinfolist == null)
            { }
            else
            { 
            List<ParsedImageInfo> parsedimagelist = new List<ParsedImageInfo>();
            parsedimagelist = imageinfolist;

            int imagecount = parsedimagelist.Count;
            for (int i = 0; i < imagecount; i++)
            {
                if (parsedimagelist == null || String.IsNullOrEmpty(parsedimagelist[i].URL))
                    {/*Do nothing*/}
                else
                {
                    InlineUIContainer container = new InlineUIContainer();
                    //RichTextBlock img_rtb = new RichTextBlock();
                    Paragraph img_paragraph = new Paragraph();
                    Image img = new Image();
                    SvgImageSource svgimgsource = new SvgImageSource();
                    Run img_run = new Run();
                    string img_url = parsedimagelist[i].URL;

                    string input = parsedimagelist[i].Filename;
                    string pattern = @"\.svg$";
                    Regex regex_svg = new Regex(pattern);
                    Match m_svg = regex_svg.Match(input);
                    if(m_svg.Success)
                        img.Source = new SvgImageSource(new Uri(img_url, UriKind.Absolute));
                    
                    else
                        img.Source = new BitmapImage(new Uri(img_url, UriKind.Absolute));

                    img.MaxWidth = 1800;
                    img.MaxHeight = 1200;

                    container.Child = img;
                    img_paragraph.Inlines.Add(container);

                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append(parsedimagelist[i].CanonicalTitle);
                    sb.Append(" --- ");

                    string img_descr = parsedimagelist[i].ImageDescription;
                    Regex regex = new Regex(@"(\[|\])");
                    string withoutspecialchar = regex.Replace(img_descr, "");
                    sb.Append(withoutspecialchar);
                    sb.Append(Environment.NewLine);

                    img_run.Text = sb.ToString();
                    img_run.FontStyle = FontStyle.Italic;
                    img_run.FontSize = 13;
                    img_run.FontWeight = FontWeights.Light;
                    img_paragraph.Inlines.Add(img_run);
                    rtb.Blocks.Add(img_paragraph);
                }
            }
            }
        }
        public static void AddSingleImage(string ImgURL, string ImgDescription, string imgName, RichTextBlock rtb)
        {

                        InlineUIContainer container = new InlineUIContainer();
                        //RichTextBlock img_rtb = new RichTextBlock();
                        Paragraph img_paragraph = new Paragraph();
                        Image img = new Image();
                        img.Stretch = Stretch.Uniform;
                        SvgImageSource svgimgsource = new SvgImageSource();
                        //svgimgsource.RasterizePixelHeight = 1000;
                        
                        Run img_run = new Run();
                        string img_url = ImgURL;

                        string input = imgName;
                        string pattern = @"\.svg$";
                        Regex regex_svg = new Regex(pattern);
                        Match m_svg = regex_svg.Match(input);
                        if(m_svg.Success)
                            img.Source = new SvgImageSource(new Uri(img_url, UriKind.Absolute));

                        else
                            img.Source = new BitmapImage(new Uri(img_url, UriKind.Absolute));

                        img.MaxWidth = 1800;
                        img.MaxHeight = 1200;

                        container.Child = img;
                        img_paragraph.Inlines.Add(container);

                        StringBuilder sb = new StringBuilder();
                        sb.Append(Environment.NewLine);
                        sb.Append(imgName);
                        sb.Append(" --- ");

                        string img_descr = ImgDescription;
                        Regex regex = new Regex(@"(\[|\])");
                        string withoutspecialchar = regex.Replace(img_descr, "");
                        sb.Append(withoutspecialchar);
                        sb.Append(Environment.NewLine);

                        img_run.Text = sb.ToString();
                        img_run.FontStyle = FontStyle.Italic;
                        img_run.FontSize = 13;
                        img_run.FontWeight = FontWeights.Light;
                        img_paragraph.Inlines.Add(img_run);
                        rtb.Blocks.Add(img_paragraph);

            
        }

        public static void FixStartAndEnd_WhiteSpace(string Input)
        {
            string pattern = @"^\n{0,5}";
            string replacement = "";
            Regex regex = new Regex(pattern);

            //Remove starting white space
            Input = regex.Replace(Input, replacement);

            pattern = @"\n{0,5}$";
            regex = new Regex(pattern);

            //Remove trailing white space
            Input = regex.Replace(Input, replacement);

            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append(Input);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            Input = sb.ToString();

        }
        public static void Add_NodeToStackPanel(Paragraph paragraph, RichTextBlock rtb, StackPanel stackpanel)
        {
            rtb.Blocks.Add(paragraph);
            stackpanel.Children.Add(rtb);
        }
        public static void Add_Content_MinimalMarkdown(string Content, Run run, Paragraph paragraph)
        {
            string content = Content;
            string input;
            string output;
            string replacement = "";
            string pattern;
            Regex regex = new Regex("");
            input = content;

            //Add tabs for *s
            string tab_pattern = @"(^\*\*\*|\n\*\*\*)([^\*]{0,5})";
            string tab_replacement = "\n\t\t";
            regex = new Regex(tab_pattern);
            Content = regex.Replace(Content, "\n\t\t\t$2");

            tab_pattern = @"(^\*\*|\n\*\*)([^\*]{0,5})";
            tab_replacement = "\n\t";
            regex = new Regex(tab_pattern);
            Content = regex.Replace(Content, "\n\t\t$2");

            tab_pattern = @"(^\*|\n\*)([^\*]{0,5})";
            tab_replacement = "\n\t";
            regex = new Regex(tab_pattern);
            Content = regex.Replace(Content, "\n$2");







            //Citations removed
            
            pattern = @"{{(R|r)efbegin(.|\n)*?{{(R|r)efend}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"<(r|R)ef.*?>[{{]?(.|\n)*?[}}]?</(r|R)ef>";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"<(r|R)ef\s.+?>.*?</(r|R)ef>";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"<(r|R)ef\s.+?/>";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            //Remove {{citation\sneeding...}}
            pattern = @"{{(C|c)itation\sneed.*?}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            //Remove {{sfn|...}}
            pattern = @"{{.fn\|.*?}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

             //File info removed
            pattern = @"\[\[(File|Image):.*?\]\]\n";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);
            
            //Editor comments removed
 
            pattern = @"<!--(.|\n)+?-->";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);
            
            //Remove multiple image tables at the beginning of section
            //input = output;
            pattern = @"{{(m|M)ultiple\simage(.|\n)*?}}\n";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);
            
            //Removes table information
            //pattern = @"{\|\sclass=(.|\n)+?\n\|}";
            //regex = new Regex(pattern);
            //Content = regex.Replace(Content, replacement);
            
            //Remove clarify note
            pattern = @"{{(C|c)larify[^}]+?}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            //Remove tables
            pattern = @"{\|.*(.|\n)*\n\|}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"{{col-begin}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"{{col-break}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            pattern = @"{{col-end}}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);


            //Remove starting whitespace
            //input = output;
            pattern = @"^\n{0,5}";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);

            //Remove trailing whitespace
            //input = output;
            pattern = @"\n{0,5}$";
            regex = new Regex(pattern);
            Content = regex.Replace(Content, replacement);
            

  
            //Format to start with \n and end with \n\n
            StringBuilder sb = new StringBuilder();       

            sb.Append(Environment.NewLine);
            sb.Append(Content);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            Content = sb.ToString();
            //Final output
            run.Text = Content;
            paragraph.Inlines.Add(run);
        }
        public static void Add_Content(string Content, Run run, Paragraph paragraph)
        {
            string content = Content;
            //References removed
            run.Text = content;
            paragraph.Inlines.Add(run);

        }
        public static void Add_Title(string Title, string RegexPattern, Run run, Paragraph paragraph)
        {
            if(String.IsNullOrEmpty(Title))
            { }
            else
            { 
            //Remove the '=='
            string content = Title;
            string pattern = RegexPattern;
            string replacement = "";

            Regex regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Clean whitespace
            pattern = @"^\n{0,5}";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Remove trailing whitespace
            pattern = @"\n{0,5}$";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            run.Text = content;
            paragraph.Inlines.Add(run);
            }
        }
        public static void Add_PageTitle(WikiContent_Rootobject ParametersPassed, Run run, Paragraph paragraph, RichTextBlock rtb)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(ParametersPassed.PageTitle);
            sb.Append(Environment.NewLine);

            run.Text = sb.ToString();
            paragraph.Inlines.Add(run);
            rtb.Blocks.Add(paragraph);
        }
        public static void SetProps_N2_AndOn_ObjectProperties (RichTextBlock rtb, Run title, Run content)
        {
            SetProp_RichTextBlockContent(rtb);
            SetProp_N2andChildren_RunTitle(title);
            SetProp_Content_Run(content);
        }
        public static void SetProps_N1_ObjectProperties (RichTextBlock rtb, Run title, Run content)
        {
            SetProp_RichTextBlockContent(rtb);
            SetProp_N1_RunTitle(title);
            SetProp_Content_Run(content);
        }
        public static void SetProps_IntroObjectProperties(RichTextBlock rtb, Run run_DataAndTables, Run run_Content)
        {
            SetProp_RichTextBlockContent(rtb);
            SetProp_DataAndTables_Run(run_DataAndTables);
            SetProp_Content_Run(run_Content);

        }
        public static void SetProp_RichTextBlockContent(RichTextBlock rtb)
        {
            rtb.TextWrapping = TextWrapping.WrapWholeWords;
            rtb.IsTextScaleFactorEnabled = true;
            rtb.IsTextSelectionEnabled = true;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;
            rtb.FontWeight = FontWeights.Normal;
            rtb.FontSize = 15;
        }
        public static void SetProp_N1_RunTitle(Run run)
        {
            run.FontSize = 46;
            run.FontWeight = FontWeights.Light;
        }
        public static void SetProp_N2andChildren_RunTitle(Run run)
        {
            run.FontSize = 34;
            run.FontWeight = FontWeights.Light;
        }       
        public static void SetProp_Content_Run(Run run)
        {
            run.FontSize = 16;
        }
        public static void SetProp_DataAndTables_Run(Run run)
        {
            run.FontSize = 16;
        }
        public static void SetProp_PageTitle_RTB(RichTextBlock rtb)
        {
            rtb.HorizontalAlignment = HorizontalAlignment.Center;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.FontSize = 44;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.FontWeight = FontWeights.SemiBold;
            rtb.FontFamily = FontFamily.XamlAutoFontFamily;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
        }          
        public static void Set_OtherRTB (RichTextBlock rtb)
        {
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;

        }
        public static void Set_BodyRTB(RichTextBlock rtb)
        {
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;
            rtb.FontWeight = FontWeights.Normal;
            rtb.FontSize = 16;

        }
        public static void Set_CaptionRTB(RichTextBlock rtb)
        {
            rtb.HorizontalAlignment = HorizontalAlignment.Center;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;
            rtb.FontSize = 12;
            rtb.FontWeight = FontWeights.Normal;

        }
        public static void Set_BaseTextBlockStyle (Run run)
        {
            run.FontFamily = FontFamily.XamlAutoFontFamily;
            run.FontWeight = FontWeights.SemiBold;
            run.FontSize = 15;
        }
        public static void Set_HeaderTextBlockStyle(Run run)
        {
            Set_BaseTextBlockStyle(run);
            run.FontSize = 46;
            run.FontWeight = FontWeights.Light;

        }
        public static void Set_SubheaderTextBlockStyle(Run run)
        {
            Set_BaseTextBlockStyle(run);
        }
        public static void Set_BodyTextBlockStyle(Run run)
        {
            Set_BaseTextBlockStyle(run);
        }
        public static void Set_CaptionTextBlockStyle (Run run)
        {
            Set_BaseTextBlockStyle(run);
        }
    }
}
