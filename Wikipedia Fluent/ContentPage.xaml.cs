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

            //<------------------------------------------------------------------------------------------------------------------------------->//


            //////Add Article title
            RichTextBlock title_rtb0 = new RichTextBlock();
            Paragraph title_paragraph = new Paragraph();
            Run title_run = new Run();
            Add_PageTitle(parametersPassed, title_run, title_paragraph, title_rtb); //Smallest visual state
            title_rtb = title_rtb0;
            pageTitle.Text = parametersPassed.PageTitle; //Every other visual state



            //ContentsStackPanel.Children.Add(myRichTextBlock);

            //////Intro

            //^Add Intro Instantiations
            RichTextBlock intro_rtb = new RichTextBlock();
            Paragraph intro_paragraph = new Paragraph();
            Run intro_run_dataAndTables = new Run();
            Run intro_run_content = new Run();
            SetProps_IntroObjectProperties(intro_rtb, intro_run_dataAndTables, intro_run_content);

            //intro_run_dataAndTables.Text = parametersPassed.introduction.DataTable;
            //intro_paragraph.Inlines.Add(intro_run_dataAndTables);

            //^Add Intro Content
            Add_Content(parametersPassed.Introduction.Content, intro_run_content, intro_paragraph);

            //^Add N4 to RichTextBlock → Stackpanel
            Add_NodeToStackPanel(intro_paragraph, intro_rtb, ContentsStackPanel);

            //////Node #1
            int n1_count = parametersPassed.Node_1_list.Count;
            for(int i = 0; i < n1_count; i++)
            {
                //*Add N1 Instantiations
                RichTextBlock n1_rtb = new RichTextBlock();
                Paragraph n1_paragraph = new Paragraph();
                Run n1_run_title = new Run();
                Run n1_run_spacing = new Run();
                Run n1_run_content = new Run();
                SetProps_N1_ObjectProperties(n1_rtb, n1_run_title, n1_run_content);
                

                //*Add N1 Title
                title = parametersPassed.Node_1_list[i].Title;
                regexpattern = @"\s?={2,5}\s?";
                Add_Title(title, regexpattern, n1_run_title, n1_paragraph);

                //*Add each N1 image

                List<ParsedImageInfo> N1_Image_List = new List<ParsedImageInfo>();
                N1_Image_List = parametersPassed.Node_1_list[i].ParsedImage_list;

                int N1_ImageCount = N1_Image_List.Count;

                for (int q=0; q<N1_ImageCount; q++)
                {
                    if (String.IsNullOrEmpty(N1_Image_List[q].URL))
                    {
                        //Don't try to add the image
                    }
                    else
                    {
                        InlineUIContainer container = new InlineUIContainer();
                        //RichTextBlock img_rtb = new RichTextBlock();
                        Paragraph img_paragraph = new Paragraph();
                        Image img = new Image();
                        SvgImageSource svgimgsource = new SvgImageSource();
                        Run img_run = new Run();

                        string input = N1_Image_List[q].Filename;
                        string pattern = @"\.svg$";

                        string img_url = N1_Image_List[q].URL;
                        Regex regex_svg = new Regex(pattern);
                        Match m_svg = regex_svg.Match(input);
                        if (m_svg.Success)
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
                        sb.Append(N1_Image_List[q].CanonicalTitle);
                        sb.Append(" --- ");

                        string img_descr = N1_Image_List[q].ImageDescription;
                        Regex regex = new Regex(@"(\[|\])");
                        string withoutspecialchar = regex.Replace(img_descr, "");
                        sb.Append(withoutspecialchar);
                        sb.Append(Environment.NewLine);
                        sb.Append(N1_Image_List[q].ImageDescription);
                        sb.Append(Environment.NewLine);

                        img_run.Text = sb.ToString();
                        img_run.FontStyle = FontStyle.Italic;
                        img_run.FontSize = 14;
                        img_paragraph.Inlines.Add(img_run);
                        //img_rtb.Blocks.Add(img_paragraph);
                        n1_rtb.Blocks.Add(img_paragraph);
                    }

                }


                //*Add N1 Content
                content = parametersPassed.Node_1_list[i].Content;
                regexpattern = @"({{(.|\n)*?}}|<ref>(.|\n)*?<.ref>)";
                Add_Content(content, regexpattern, n1_run_content, n1_paragraph);

                //*Add N1 to RichTextBlock → Stackpanel
                Add_NodeToStackPanel(n1_paragraph, n1_rtb, ContentsStackPanel);

                //////Node #2
                int n2_count = parametersPassed.Node_1_list[i].Node_2_list.Count;
                for(int j = 0; j < n2_count; j++)
                {
                    //**Add N2 Instantiations
                    RichTextBlock n2_rtb = new RichTextBlock();
                    Paragraph n2_paragraph = new Paragraph();
                    Run n2_run_title = new Run();
                    Run n2_run_spacing = new Run();
                    Run n2_run_content = new Run();
                    SetProps_N2_AndOn_ObjectProperties(n2_rtb, n2_run_title, n2_run_content);

                    //**Add N2 Title
                    title = parametersPassed.Node_1_list[i].Node_2_list[j].Title;
                    regexpattern = @"\s?={2,5}\s?";
                    Add_Title(title, regexpattern, n2_run_title, n2_paragraph);

                    //**Add each N2 image

                    List<ParsedImageInfo> N2_Image_List = new List<ParsedImageInfo>();
                    N2_Image_List = parametersPassed.Node_1_list[i].Node_2_list[j].ParsedImage_list;

                    int N2_ImageCount = N2_Image_List.Count;

                    for(int q = 0; q < N2_ImageCount; q++)
                    {
                        if(String.IsNullOrEmpty(N2_Image_List[q].URL))
                        {
                            //Don't try to add the image
                        }
                        else
                        {
                            InlineUIContainer container = new InlineUIContainer();
                            //RichTextBlock img_rtb = new RichTextBlock();
                            Paragraph img_paragraph = new Paragraph();
                            Image img = new Image();
                            SvgImageSource svgimgsource = new SvgImageSource();
                            Run img_run = new Run();
                            string img_url = N2_Image_List[q].URL;


                            string input = N2_Image_List[q].Filename;
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
                            sb.Append(N2_Image_List[q].CanonicalTitle);
                            sb.Append(" --- ");

                            string img_descr = N2_Image_List[q].ImageDescription;
                            Regex regex = new Regex(@"(\[|\])");
                            string withoutspecialchar = regex.Replace(img_descr, "");
                            sb.Append(withoutspecialchar);
                            sb.Append(Environment.NewLine);
                            sb.Append(N2_Image_List[q].ImageDescription);
                            sb.Append(Environment.NewLine);

                            img_run.Text = sb.ToString();
                            img_run.FontStyle = FontStyle.Italic;
                            img_run.FontSize = 14;
                            img_paragraph.Inlines.Add(img_run);
                            //img_rtb.Blocks.Add(img_paragraph);
                            n1_rtb.Blocks.Add(img_paragraph);
                        }

                    }

                    //**Add N2 Content
                    content = parametersPassed.Node_1_list[i].Node_2_list[j].Content;
                    regexpattern = @"({{(.|\n)*?}}|<ref>(.|\n)*?<.ref>)";
                    Add_Content(content, regexpattern, n2_run_content, n2_paragraph);

                    //**Add N2 to RichTextBlock → Stackpanel
                    Add_NodeToStackPanel(n2_paragraph, n2_rtb, ContentsStackPanel);

                    //////Node #3
                    int n3_count = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                    .Node_3_list.Count;
                    for(int k = 0; k < n3_count; k++)
                    {
                        //***N3 Instantiations
                        RichTextBlock n3_rtb = new RichTextBlock();
                        Paragraph n3_paragraph = new Paragraph();
                        Run n3_run_title = new Run();
                        Run n3_run_spacing = new Run();
                        Run n3_run_content = new Run();
                        SetProps_N2_AndOn_ObjectProperties(n3_rtb, n3_run_title, n3_run_content);

                        //***Add N3 Title
                        title = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                 .Node_3_list[k].Title;
                        regexpattern = @"\s?={2,5}\s?";
                        Add_Title(title, regexpattern, n3_run_title, n3_paragraph);


                        //***Add N3 content
                        content = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                              .Node_3_list[k].Content;
                        regexpattern = @"({{(.|\n)*?}}|<ref>(.|\n)*?<.ref>)";
                        Add_Content(content, regexpattern, n3_run_content, n3_paragraph);

                        //***Add N4 to RichTextBlock → Stackpanel
                        Add_NodeToStackPanel(n3_paragraph, n3_rtb, ContentsStackPanel);

                        //////Node #4
                        int n4_count = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                        .Node_3_list[k].Node_4_list.Count;
                        for(int L = 0; L < n4_count; L++)
                        {
                            //****N4 Instantiations
                            RichTextBlock n4_rtb = new RichTextBlock();
                            Paragraph n4_paragraph = new Paragraph();
                            Run n4_run_title = new Run();
                            Run n4_run_spacing = new Run();
                            Run n4_run_content = new Run();      
                            SetProps_N2_AndOn_ObjectProperties(n4_rtb, n4_run_title, n4_run_content);

                            //****Add N4 Title
                            title = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                    .Node_3_list[k].Node_4_list[L]
                                                    .Title;
                            regexpattern = @"\s?={2,5}\s?";
                            Add_Title(title, regexpattern, n4_run_title, n4_paragraph);

                            //****Add N4 Content
                            content = parametersPassed.Node_1_list[i].Node_2_list[j]
                                                       .Node_3_list[k].Node_4_list[L]
                                                       .Content;
                            regexpattern = @"({{(.|\n)*?}}|<ref>(.|\n)*?<.ref>)";
                            Add_Content(content, regexpattern, n4_run_content, n4_paragraph);

                            //****Add N4 to RichTextBlock → Stackpanel
                            Add_NodeToStackPanel(n4_paragraph, n4_rtb, ContentsStackPanel);
                        }
                    }
                }
            }
            //pageContent.DataContext = parametersPassed.node_1[1].Content;
        }


        public static void Add_NodeToStackPanel(Paragraph paragraph, RichTextBlock rtb, StackPanel stackpanel)
        {
            rtb.Blocks.Add(paragraph);
            stackpanel.Children.Add(rtb);
        }
        public static void Add_Content(string Content, string RegexPattern, Run run, Paragraph paragraph)
        {
            string content = Content;
            string regexpattern = RegexPattern;
            string replacement = "";

            Regex regex_content = new Regex(regexpattern);

            //References removed
            run.Text = regex_content.Replace(content, replacement);
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
            string title = Title;
            string regexpattern = RegexPattern;
            string replacement = "";

            Regex regex_title = new Regex(regexpattern);
            run.Text = regex_title.Replace(title, replacement);
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
        }
        public static void SetProp_N1_RunTitle(Run run)
        {
            run.FontSize = 26;
            run.FontWeight = FontWeights.Bold;
            run.TextDecorations = TextDecorations.Underline;
        }
        public static void SetProp_N2andChildren_RunTitle(Run run)
        {
            run.FontSize = 24;
            run.FontWeight = FontWeights.Bold;
            run.TextDecorations = TextDecorations.Underline;
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
    }
}
