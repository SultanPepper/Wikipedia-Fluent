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

    public class Teststring
    {
        public string teststringcontent { get; set; }
    }

    public class WikiContent_Rootobject
        {
            public string PageTitle { get; set; }
            public string RemainingContent { get; set; }
            public Introduction Introduction { get; set; }
            public List<Header> Headers { get; set; }



        //Http methods
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
        private string GetURL(string SearchQuery)
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

            string URL = sb_URL.ToString();
            return URL;

        }


        //Parsing data methods
        public string GetPageTitle(Rootobject Data)
        {
            Rootobject data = Data;
            string PageTitle = data.query.pages[0].title;
            return PageTitle;
        }
        public string GetIntroTablesAndData(string Input)
        {
            string input = Input;
            string pattern = @"({{(.|\n)*?\n}}\n\n)'";
            string errorMsg = "There was difficulty extracting the data tables";
            int group = 1;
            string IntroTablesAndData = Regex_SelectMatch(input, pattern, group, errorMsg);
            return IntroTablesAndData;

        }
        public string GetIntroContent(string Input)
        {
            string input = Input;
            string pattern = @"(\n}}\n\n)('''(.|\s|\d)*?)(\n\n==\s?(\w|\d))";
            string errorMsg = "There was difficulty extracting the data tables";
            int group = 2;

            string IntroContent = Regex_SelectMatch(input, pattern, group, errorMsg);
            return IntroContent;
        }
        public string RemoveIntroContent(string Input)
        {
            string input = Input;
            string pattern = @"^{{(.|\n)*?(?=(==\s?(\w|\s|\d)+?)==)";
            string output = Regex_DeleteData(input, pattern, "");
            return output;
        }
        public string[] GetArrayOfHeaders (string Input)
        {
            string input = Input;
            int group_selected = 1;
            string pattern = @"(\n ==\s?(\w|\d|\s)*?\s?==[^=](.|\n)*?)(?=\n\n == (\s|\w|\d))";
            string error_msg = "There was a problem extracting the headers into the array!";


            string[] headers = Regex_PutMatchesIntoArray(input, pattern, group_selected, error_msg);
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
            }

            else
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
        public string[] Regex_PutMatchesIntoArray(string Input, string Pattern, int Group_Selected, string Error_Msg)
        {
            string pattern = Pattern;
            string input = Input;
            int group_selected = Group_Selected;
            string error_msg = Error_Msg;

            string[] output = Regex.Matches(input, pattern).OfType<Match>().Select(m => m.Groups[group_selected].Value).ToArray();
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
        

    }

        public class Introduction
        {
            public string Content { get; set; }
            public string DataTable { get; set; }
        }

        public class IntroductionContent : Introduction
    {
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
            public List<Subheader> Subheaders { get; set; }
        }

        public class Subheader
    {
            public string sbhdr_Title { get; set; }
            public string sbhdr_Content { get; set; }
            public string sbhdr_Table { get; set; }
            public List<String> sbhdr_References { get; set; }
            public List<String> sbhdr_Images { get; set; }
            public List<SubSubHeader> SubSubHeaders { get; set; }
        }

        public class SubSubHeader
    {
        public string sb_sbhdr_Title { get; set; }
        public string sb_sbhdr_Content { get; set; }
        public string sb_sbhdr_Table { get; set; }
        public List<String> sb_sbhdr_References { get; set; }
        public List<String> sb_sbhdr_Images { get; set; }
    }






}





