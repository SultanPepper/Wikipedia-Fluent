using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wikipedia_Fluent.Models;

namespace Wikipedia_Fluent.Models
{

    public class ForEachContentHolder
    {
        public string StringContent { get; set; }
        public int IntContent { get; set; }
        public List<string[]> stringlist { get; set; }
    }

    public class Teststring
    {
        public string teststringcontent { get; set; }
    }

    public class WikiContent_Rootobject
    {
        public string PageTitle { get; set; }
        public string RemainingContent { get; set; }
        public Introduction introduction { get; set; }
        public List<Node_1> node_1 { get; set; }


        //Http methods
        public string GetURL(string SearchQuery)
        {
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

            string action = "parse";
            action = String.Format("?action={0}", action);
            sb_URL.Append(action);

            string page = Regex_ReplaceMatch(searchquery, @"\s", "%20");
            page = String.Format("&page={0}", page);
            sb_URL.Append(page);

            string prop = "revisions|sections|wikitext";
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
        public async Task<Rootobject> GetPageContent(string SearchQuery)
        {
            HttpClient http = new HttpClient();
            string searchquery = SearchQuery;
            string URL = GetURL(searchquery);

            var response = await http.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            return data;
        }


        //Parsing data methods
        public string GetPageTitle(Rootobject Data)
        {
            Rootobject data = Data;
            string PageTitle = data.parse.title;
            return PageTitle;
        }
        public string GetIntroTablesAndData(string Input)
        {
            string input = Input;
            string pattern = @"^{{(.|\n)*?\n}}(?=\n''')";
            string errorMsg = "There was difficulty extracting the data tables";
            int group = 1;
            string IntroTablesAndData = Regex_SelectMatch(input, pattern, group, errorMsg);
            return IntroTablesAndData;

        }
        public string GetIntroContent(string Input)
        {
            string input = Input;           
            string pattern = @"(\\n|^)'''(.|\n)*?(?=\n==(\w|\d))";
            string errorMsg = "There was difficulty extracting the data tables";
            int group = 0;

            string IntroContent = Regex_SelectMatch(input, pattern, group, errorMsg);
            return IntroContent;
        }
        public string[] GetArrayOfEach_N1(string Input)
        {
            string input = Input;
            int group_selected = 1;
            string pattern = @"(\n ==\s?(\w|\d|\s)*?\s?==[^=](.|\n)*?)(?=\n\n == (\s|\w|\d))";


            string[] headers = Regex_PutMatchesIntoArray(input, pattern, group_selected);
            return headers;
        }


        //Regex methods
        public string Regex_SelectMatch(string Input, string Pattern, int Group_Selected, string Error_Msg)
        {
            string pattern = Pattern;
            string input = Input;
            int group_selected = Group_Selected;
            string error_msg = Error_Msg;

            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success)
            {
                return match.Groups[group_selected].Value;
            } else
            {
                return error_msg;
            }
        }
        public string Regex_DeleteData(string Input, string Pattern, string Regex_Replacement)
        {
            string pattern = Pattern;
            string input = Input;
            string regex_replacement = Regex_Replacement;

            Regex regex = new Regex(pattern);
            string output = regex.Replace(input, regex_replacement);
            return output;

        }
        public string[] Regex_PutMatchesIntoArray(string Input, string Pattern, int Group_Selected)
        {
            string pattern = Pattern;
            string input = Input;
            int group_selected = Group_Selected;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_Put_N1_AndDerivativesToArray(string Input)
        {
            string input = Input;          
            string pattern = @"(^|\n)==(\s|\w|\d)+?==(\n|.)+?(?=\n==(\w|\d|\s)|$)";

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Value).ToArray();
            return output;
        }

        public string[] Regex_Put_N2_AndDerivativesToArray(string Input)
        {
            string input = Input;           
            string pattern = @"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,3}(\w|\d|\s)|\n={2,3}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_Put_N3_AndDerivativesToArray(string Input)
        {
            string input = Input;
            string pattern = @"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,3}(\w|\d|\s)|\n={2,4}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_Put_N4_AndDerivativesToArray(string Input)
        {
            string input = Input;
            string pattern = @"((^====|\n====)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={5}(\w|\d|\s)|\n={5}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_PutOnly_N2_ToArray(string Input)
        {
            string input = Input;

            


            string pattern = @"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={3,5}(\w|\d|\s)|\n={3,5}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();


            List<string> mystringlist = new List<string>();
            return output;
        }

        public string Regex_ReplaceMatch(string Input, string Pattern, string Replacement)
        {
            string pattern = Pattern;
            string input = Input;
            string replacement = Replacement;
            Regex regex = new Regex(pattern);

            string output = regex.Replace(input, replacement);
            return output;
        }
        public string Regex_ReplaceMatch(string Input, string Pattern, string Replacement, string Pattern_If_Fail, string Replacement_If_Fail)
        {
            string pattern = Pattern;
            string pattern_if_fail = Pattern_If_Fail;
            string input = Input;
            string output;
            string replacement = Replacement;
            string replacement_if_fail = Replacement_If_Fail;
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);

            if (match.Success)
            {
                output = regex.Replace(input, replacement);
            } else
            {
                regex = new Regex(pattern_if_fail);
                output = regex.Replace(input, replacement_if_fail);

            }

            return output;
        }

        public string Regex_GetFirstMatch(string Input)
        {
            string input = Input;

            string pattern_contentmatch = @"((^==|\n==)(\w|\d|\s).*?==(.|\n|\*)*?)";
            string pattern_lookaheadmatchA = @"(?=(^={2,5}=(\w|\d|\s)|\n{2,5}=(\w|\d|\s)";
            string pattern = String.Format("{0}{1}", pattern_contentmatch, pattern_lookaheadmatchA);

            string pattern_if_fail = @"((^=|\n=)=(\w|\d|\s).*?==(.|\n|\*)*)";
            string first_match;
            string error_msg = String.Format("There was an issue parsing the match collection{0}",
                                             "The problem was with Regex_GetFirstMatch()");


            Match m = Regex.Match(input, pattern);

            if (m.Success)
            {
                first_match = m.Value;
            }
            else
            {
                Match m_if_fail = Regex.Match(input, pattern_if_fail);
                if (m_if_fail.Success)
                {
                    first_match = m_if_fail.Value;
                }

                else
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("There was issue matching the header within the");
                    sb.Append("subheaders with children group.");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("The error was under section '//HEADERS, SUBHEADERS,");
                    sb.Append(" AND SUBSUBHEADERS' in HomePage.xaml.cs");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);
                    sb.Append("The method in error is wikiContent.Regex_GetFirstMatch()");
                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    first_match = sb.ToString();
                }
                  
            }

            return first_match;

        }
    
        ///Extract Header information
        public string Regex_Get_N1_ContentAndTitle(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"((^==|\n==)(\w|\d|\s).+?==(.|\n)*?)(?=(\n={3}(\w|\d|\s)|$))";

            string pattern_if_fail = @"((^=|\n=)=(\w|\d|\s).*?==(.|\n|\*)*)";
            string error_msg = String.Format("There was an issue parsing the match collection{0}",
                                                 "The problem was with Regex_Parse_Headers_And_Nodes()");
            Regex regex = new Regex(pattern); //To be redefined at each step

            //Get the header
            Match m = regex.Match(input);
            Match m_if_fail = Regex.Match(input, pattern_if_fail);

            if (m.Success)
            {
                //Creating the header
                output = m.Value;
                regex = new Regex(pattern);
            } 
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("There was issue matching the header within the");
                sb.Append("subheaders with children group.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("The error was under section '//HEADERS, SUBHEADERS,");
                sb.Append(" AND SUBSUBHEADERS' in HomePage.xaml.cs");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("The method in error is wikiContent.Regex_GetFirstMatch()");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                output = sb.ToString();
            }

            return output;

        }
        public string Regex_Get_N1_Content(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^==|\n==)(\w|\d|\s).+?==(((.|\n)*?)(?=(\n={3}(\w|\d|\s)|$)))";

            string pattern_if_fail = @"((^=|\n=)=(\w|\d|\s).*?==(.|\n|\*)*)";
            string error_msg = String.Format("There was an issue parsing the match collection{0}",
                                                 "The problem was with Regex_Parse_Headers_And_Nodes()");
            Regex regex = new Regex(pattern); //To be redefined at each step

            //Get the header
            Match m = regex.Match(input);
            Match m_if_fail = Regex.Match(input, pattern_if_fail);

            if (m.Success)
            {
                //Creating the header
                output = m.Groups[3].Value;
                regex = new Regex(pattern);
            } else
            {
                return "There was an issue getting the header without the title";
            }

            return output;

        }
        public string Regex_Get_N1_Title(string Input)
        {
            string input = Input;
            string output;
            string pattern = @"(^==|\n==)(\w|\d|\s).+?==";
            string error_msg = "There was an issue getting the header title";

            Regex regex = new Regex(pattern);
            Match m = regex.Match(input);

            if (m.Success)
            {
                output = m.Value;
            }
            else
            {
                output = error_msg;
            }

            return output;

        }

        ///Extract Sub-Header information
        public string Regex_Get_N2_ContentAndTitle(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"((^===|\n===)(\w|\d|\s).+?==(.|\n)*?)(?=(\n={4}(\w|\d|\s)|$))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            Match m = regex.Match(input);
           
            if (m.Success)
            {
                //Creating the header
                output = m.Value;
                regex = new Regex(pattern);
            }
            else
            {
                return "Issue getting subheader and title";
            }

            return output;

        }
        public string Regex_Get_N2_Content(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^===|\n===)(\w|\d|\s).*?==(((.|\n)*?)(?=(\n={4}(\w|\d|\s)|$)))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            //Get the header
            Match m = regex.Match(input);

            if (m.Success)
            {
                //Creating the header
                output = m.Groups[3].Value;
                regex = new Regex(pattern);
            } else
            {
                return "There was an issue getting the subheader without the title";
            }

            return output;

        }
        public string Regex_Get_N2_Title(string Input)
        {
            string input = Input;
            string output;
            string pattern = @"(^===|\n===)(\w|\d|\s).+?===";

            Regex regex = new Regex(pattern);
            Match m = regex.Match(input);

            if (m.Success)
            {
                output = m.Value;
            } else
            {
                return "There was an issue getting the subheader title" + Environment.NewLine + input;
            }

            return output;

        }
        public string Regex_GetSubheader(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^===|\n===)(\w|\d|\s).+?===(.|\n|\*)*?(?=(^==|\n==))";
            string pattern_if_fail = @"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*(!?(\n=|^=|$)))";
            string error_msg = String.Format("There was an issue parsing the match collection{0}",
                                                 "The problem was with Regex_Parse_Headers_And_Nodes()");
            Regex regex = new Regex(""); //To be redefined at each step

            //Get the header
            Match m = Regex.Match(input, pattern);
            Match m_if_fail = Regex.Match(input, pattern_if_fail);

            if (m.Success)
            {
                //Creating the header
                output = m.Value;
                regex = new Regex(pattern);
            } else if (m_if_fail.Success)
            {

                //Creating the header
                output = m_if_fail.Value;
                regex = new Regex(pattern_if_fail);
            } else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("There was issue matching the header within the");
                sb.Append("subheaders with children group.");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("The error was under section '//HEADERS, SUBHEADERS,");
                sb.Append(" AND SUBSUBHEADERS' in HomePage.xaml.cs");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append("The method in error is wikiContent.Regex_GetFirstMatch()");
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);

                output = sb.ToString();
            }

            return output;
        }

        ///Extract Sub-Sub-Header information
        public string Regex_Get_N3_ContentAndTitle(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"((^====|\n====)(\w|\d|\s).+?==(.|\n)*?)(?=(\n={4,5}(\w|\d|\s)|$))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            Match m = regex.Match(input);

            if (m.Success)
            {
                //Creating the header
                output = m.Value;
                regex = new Regex(pattern);
            } else
            {
                return "Issue getting SubSubheader and title";
            }

            return output;

        }
        public string Regex_Get_N3_Content(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^====|\n====)(\w|\d|\s).+?==(((.|\n)*?)(?=(\n={4}(\w|\d|\s)|$)))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            //Get the header
            Match m = regex.Match(input);

            if (m.Success)
            {
                //Creating the header
                output = m.Groups[3].Value;
                regex = new Regex(pattern);
            } else
            {
                return "There was an issue getting the subsubheader without the title";
            }

            return output;

        }
        public string Regex_Get_N3_Title(string Input)
        {
            string input = Input;
            string output;
            string pattern = @"(^====|\n====)(\w|\d|\s).+?====";

            Regex regex = new Regex(pattern);
            Match m = regex.Match(input);

            if (m.Success)
            {
                output = m.Value;
            } else
            {
                return "There was an issue getting the subsubheader title" + Environment.NewLine + input;
            }

            return output;

        }

        ///Extract Sub-Sub-Sub-Header information
        public string Regex_Get_N4_ContentAndTitle(string Input)
        {
            string input = Input;
            string output;
            
            string pattern = @"((^=====|\n=====)(\w|\d|\s).+?==(.|\n)*?)(?=(\n={5}(\w|\d|\s)|$))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            Match m = regex.Match(input);

            if (m.Success)
            {
                //Creating the header
                output = m.Value;
                regex = new Regex(pattern);
            } else
            {
                return "Issue getting SubSubSubheader and title";
            }

            return output;

        }
        public string Regex_Get_N4_Content(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^=====|\n=====)(\w|\d|\s).+?==(((.|\n)*?)(?=(\n={4}(\w|\d|\s)|$)))";

            Regex regex = new Regex(pattern); //To be redefined at each step

            //Get the header
            Match m = regex.Match(input);

            if (m.Success)
            {
                //Creating the header
                output = m.Groups[3].Value;
                regex = new Regex(pattern);
            } else
            {
                return "There was an issue getting the subsubsubheader without the title";
            }

            return output;

        }
        public string Regex_Get_N4_Title(string Input)
        {
            string input = Input;
            string output;
            string pattern = @"(^=====|\n=====)(\w|\d|\s).+?=====";

            Regex regex = new Regex(pattern);
            Match m = regex.Match(input);

            if (m.Success)
            {
                output = m.Value;
            } else
            {
                output = "There was an issue getting the subsubheader title" + Environment.NewLine + input;
            }

            return output;

        }

    }

        public class Introduction
        {     
            public string Content { get; set; }
            public string DataTable { get; set; }
            public List<String> References { get; set; }
            public List<String> Images { get; set; }
        }
        public class Node_1
    {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Tables { get; set; }
            public List<String> References { get; set; }
            public List<String> Images { get; set; }
            public List<Node_2> node_2 { get; set; }
        }
        public class Node_2
    {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Table { get; set; }
            public List<String> References { get; set; }
            public List<String> Images { get; set; }
            public List<Node_3> node_3 { get; set; }
        }
        public class Node_3
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Table { get; set; }
        public List<String> References { get; set; }
        public List<String> Images { get; set; }
        public List<Node_4> node_4 { get; set; }
        }

        public class Node_4
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Table { get; set; }
        public List<String> s3_References { get; set; }
        public List<String> s3_Images { get; set; }
        }
}





