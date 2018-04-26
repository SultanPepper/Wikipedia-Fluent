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
        protected override void OnNavigatedTo(NavigationEventArgs e)
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
                SetProps_N1_ObjectProperties(n1_rtb, n1_run_title, n1_run_content);
                

                //*Add N1 Title
                title = parametersPassed.Node_1_list[a].Title;
                regexpattern = @"\s?={2,5}\s?";
                Add_Title(title, regexpattern, n1_run_title, n1_paragraph);

                //*Add each N1 image
                AddImages(parametersPassed.Node_1_list[a].ParsedImage_list, n1_rtb);

                //*Add N1 Content
                content = parametersPassed.Node_1_list[a].Content;
                Add_Content_MinimalMarkdown(content, n1_run_content, n1_paragraph);

                //*Add N1 to RichTextBlock → Stackpanel
                Add_NodeToStackPanel(n1_paragraph, n1_rtb, ContentsStackPanel);

                //////Node #2
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
                    AddImages(parametersPassed.Node_1_list[a].Node_2_list[b].ParsedImage_list, n2_rtb);

                    //**Add N2 Content
                    content = parametersPassed.Node_1_list[a].Node_2_list[b].Content;
                    Add_Content_MinimalMarkdown(content, n2_run_content, n2_paragraph);

                    //**Add N2 to RichTextBlock → Stackpanel
                    Add_NodeToStackPanel(n2_paragraph, n2_rtb, ContentsStackPanel);

                    //////Node #3
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
                        AddImages(parametersPassed.Node_1_list[a].Node_2_list[b].Node_3_list[c].ParsedImage_list, n3_rtb);


                        //List<ParsedImageInfo> N3_Image_List = new List<ParsedImageInfo>();
                        //N2_Image_List = parametersPassed.Node_1_list[a].Node_2_list[b].ParsedImage_list;

                        //int N3_ImageCount = N3_Image_List.Count;

                        //for(int q = 0; q < N3_ImageCount; q++)
                        //{
                        //    if(String.IsNullOrEmpty(N3_Image_List[q].URL) || N3_Image_List == null)
                        //    {
                        //        //Don't try to add the image
                        //    }
                        //    else
                        //    {
                        //        InlineUIContainer container = new InlineUIContainer();
                        //        //RichTextBlock img_rtb = new RichTextBlock();
                        //        Paragraph img_paragraph = new Paragraph();
                        //        Image img = new Image();
                        //        SvgImageSource svgimgsource = new SvgImageSource();
                        //        Run img_run = new Run();
                        //        string img_url = N3_Image_List[q].URL;


                        //        string input = N3_Image_List[q].Filename;
                        //        string pattern = @"\.svg$";
                        //        Regex regex_svg = new Regex(pattern);
                        //        Match m_svg = regex_svg.Match(input);
                        //        if(m_svg.Success)
                        //        {
                        //            img.Source = new SvgImageSource(new Uri(img_url, UriKind.Absolute));
                        //        }

                        //        else
                        //        {
                        //            img.Source = new BitmapImage(new Uri(img_url, UriKind.Absolute));
                        //        }

                        //        container.Child = img;
                        //        img_paragraph.Inlines.Add(container);

                        //        StringBuilder sb = new StringBuilder();
                        //        sb.Append(Environment.NewLine);
                        //        sb.Append(N3_Image_List[q].CanonicalTitle);
                        //        sb.Append(" --- ");

                        //        string img_descr = N3_Image_List[q].ImageDescription;
                        //        Regex regex = new Regex(@"(\[|\])");
                        //        string withoutspecialchar = regex.Replace(img_descr, "");
                        //        sb.Append(withoutspecialchar);
                        //        sb.Append(Environment.NewLine);
                        //        sb.Append(N3_Image_List[q].ImageDescription);
                        //        sb.Append(Environment.NewLine);

                        //        img_run.Text = sb.ToString();
                        //        img_run.FontStyle = FontStyle.Italic;
                        //        img_run.FontSize = 12;
                        //        img_paragraph.Inlines.Add(img_run);
                        //        //img_rtb.Blocks.Add(img_paragraph);
                        //        n3_rtb.Blocks.Add(img_paragraph);
                        //    }

                        //}


                        //***Add N3 content
                        content = parametersPassed.Node_1_list[a].Node_2_list[b]
                                                              .Node_3_list[c].Content;
                        Add_Content_MinimalMarkdown(content, n3_run_content, n3_paragraph);

                        //***Add N4 to RichTextBlock → Stackpanel
                        Add_NodeToStackPanel(n3_paragraph, n3_rtb, ContentsStackPanel);

                        //////Node #4
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
                            AddImages(parametersPassed.Node_1_list[a].Node_2_list[b].Node_3_list[c]
                                .Node_4_list[d].ParsedImage_list, n4_rtb);


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
            //pageContent.DataContext = parametersPassed.node_1[1].Content;
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
                {
                    //Do nothing
                }

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
                    {
                        img.Source = new SvgImageSource(new Uri(img_url, UriKind.Absolute));
                    }

                    else
                    {
                        img.Source = new BitmapImage(new Uri(img_url, UriKind.Absolute));
                    }

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
                    sb.Append(parsedimagelist[i].ImageDescription);
                    sb.Append(Environment.NewLine);

                    img_run.Text = sb.ToString();
                    img_run.FontStyle = FontStyle.Italic;
                    img_run.FontSize = 12;
                    img_paragraph.Inlines.Add(img_run);
                    //img_rtb.Blocks.Add(img_paragraph);
                    rtb.Blocks.Add(img_paragraph);
                }
            }
            }

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
            string replacement = "";
            string pattern;
            Regex regex = new Regex("");

            //Citations removed
            pattern = @"<ref>.*?</ref>";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            pattern = @"<ref[^>]{0,75}>{{.*?}}</ref>";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            pattern = @"<ref\sname[^\]]+?>.*?</ref>";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            pattern = @"<ref\s[^>]+?/>";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);


            //File info removed
            pattern = @"\[\[File:.*?\]\]\n";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Editor comments removed
            pattern = @"<!--.*?-->";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Remove multiple image tables at the beginning of section
            pattern = @"{{multiple image(.|\n)*?\n}}";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Removes table information
            pattern = @"{\|\sclass=(.|\n)+?\n\|}";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Remove starting whitespace
            pattern = @"^\n{0,5}";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);

            //Remove trailing whitespace
            pattern = @"\n{0,5}$";
            regex = new Regex(pattern);
            content = regex.Replace(content, replacement);


            //Format to start with \n and end with \n\n
            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append(content);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            //Final output
            run.Text = sb.ToString();
            paragraph.Inlines.Add(run);

        }
        public static void Add_Content(string Content, Run run, Paragraph paragraph)
        {
            string content = Content;
            //References removed
            run.Text = content;
            paragraph.Inlines.Add(run);

        }
        public static void Add_Title (string Title, string RegexPattern, Run run, Paragraph paragraph)
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
            rtb.HorizontalAlignment = HorizontalAlignment.Center;
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
            rtb.HorizontalAlignment = HorizontalAlignment.Center;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;

        }

        public static void Set_BodyRTB(RichTextBlock rtb)
        {
            rtb.HorizontalAlignment = HorizontalAlignment.Center;
            rtb.VerticalAlignment = VerticalAlignment.Top;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.TextWrapping = TextWrapping.Wrap;
            rtb.TextTrimming = TextTrimming.None;
            rtb.TextLineBounds = TextLineBounds.Full;
            rtb.LineStackingStrategy = LineStackingStrategy.MaxHeight;
            rtb.FontWeight = FontWeights.Normal;
            rtb.FontSize = 15;

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
