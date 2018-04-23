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
        public List<Header> headers { get; set; }


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
        public string[] GetArrayOfHeaders(string Input)
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
        public string[] Regex_PutHeadersAndDerivativesToArray(string Input)
        {
            string input = Input;
            string pattern = @"(^|\n)==(\s|\w|\d)+?==(\n|.)+?(?=(\n==(\w|\d|\s)|$))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_PutSubheadersAndDerivativesToArray(string Input)
        {
            string input = Input;           
            string pattern = @"((^==|\n==)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,3}(\w|\d|\s)|\n={2,3}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_Put_Sub_SubheadersAndDerivativesToArray(string Input)
        {
            string input = Input;
            string pattern = @"((^===|\n===)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={2,3}(\w|\d|\s)|\n={2,4}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_Put_Sub_Sub_SubheadersAndDerivativesToArray(string Input)
        {
            string input = Input;
            string pattern = @"((^====|\n====)=(\w|\d|\s).*?==(.|\n|\*)*?(?=(^={5}(\w|\d|\s)|\n={5}(\w|\d|\s)|$)))";
            int group_selected = 0;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
            return output;
        }
        public string[] Regex_PutOnlySubheadersToArray(string Input)
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
        public string Regex_GetHeader(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"((^==|\n==)(\w|\d|\s).*?==(.|\n)*?)(?=(\n={3}(\w|\d|\s)|$))";

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
        public string Regex_GetSubheader(string Input)
        {
            string input = Input;
            string output;

            string pattern = @"(^===|\n===)(\w|\d|\s).*?===(.|\n|\*)*?(?=(^==|\n==))";
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
    }

        public class Introduction
        {     
            public string Content { get; set; }
            public string DataTable { get; set; }
            public List<String> References { get; set; }
            public List<String> Images { get; set; }
        }
        public class Header
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public string Tables { get; set; }
            public List<String> References { get; set; }
            public List<String> Images { get; set; }
            public List<S1_Header> s1_headers { get; set; }
        }
        public class S1_Header
        {
            public string s1_Title { get; set; }
            public string s1_Content { get; set; }
            public string s1_Table { get; set; }
            public List<String> s1_References { get; set; }
            public List<String> s1_Images { get; set; }
            public List<S2_Header> s2_headers { get; set; }
        }
        public class S2_Header
        {
        public string s2_Title { get; set; }
        public string s2_Content { get; set; }
        public string s2_Table { get; set; }
        public List<String> s2_References { get; set; }
        public List<String> s2_Images { get; set; }
        public List<S3_Header> s3_Headers { get; set; }
        }

        public class S3_Header
        {
        public string s3_Title { get; set; }
        public string s3_Content { get; set; }
        public string s3_Table { get; set; }
        public List<String> s3_References { get; set; }
        public List<String> s3_Images { get; set; }
        }
}





