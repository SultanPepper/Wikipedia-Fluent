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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Wikipedia_Fluent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void simpleWikitogglebtn_Toggled(object sender, RoutedEventArgs e)
        {

        }
    }
}
//public static void GetN1Content(Rootobject rootobject, Node_1 n1_iteration)
//{
//    string n1_andderivatives = rootobject.query.pages[0].revisions[0].content;

//    //Get N1 title and content
//    string n1_pattern = @"^==(.+)?==\n(.|\n)+?(?=($|\n\n=))";
//    Regex regex_select_n1 = new Regex(n1_pattern);
//    Match m_n1 = regex_select_n1.Match(n1_andderivatives);
//    if(m_n1.Success)
//    {
//        n1_iteration.Title = m_n1.Groups[1].Value;
//        n1_iteration.Content = m_n1.Groups[2].Value;
//    }

//    //Remove n1 title and content, leaving N2+
//    string n2_andderivatives = regex_select_n1.Replace(n1_andderivatives, "");

//    //Remove starting spaces
//    n2_andderivatives = RemoveStartingSpaces(n2_andderivatives);







//    //Removes the '==...==' from content
//    string n2_and_deriv = Regex.Replace(n1_andderivatives, n1_pattern, @"$2");


//    string n2_pattern_and_deriv = @"(^=|\n=)==(.+?)==\n(.+?)(?=($|\n={3}(\w|\d|\s)))";

//    Regex regex_select_n2_and_deriv = new Regex(n2_pattern_and_deriv);
//    MatchCollection n2_match_collection = regex_select_n2_and_deriv.Matches(n2_and_deriv);
//    int n_2_match_collection_count = n2_match_collection.Count;

//    if(n_2_match_collection_count > 0)
//    {
//        List<Node_2> n2_list = new List<Node_2>();

//        foreach(Match match_n2 in n2_match_collection)
//        {
//            Node_2 node_2_interation = new Node_2();
//            node_2_interation.Title = match_n2.Groups[1].Value;


//            string n2_only = @"(^=|\n=)==.+?==\n(.+?)(?=($|\n={3,5}(\w|\d|\s)))";
//            Regex regex_select_n2 = new Regex(n2_only);
//            Match m_n2 = regex_select_n2.Match(match_n2.Value);
//            if(m_n2.Success)
//            {
//                node_2_interation.Content = match_n2.Groups[1].Value;
//            }



//            n2_list.Add(node_2_interation);

//        }

//    }


//}