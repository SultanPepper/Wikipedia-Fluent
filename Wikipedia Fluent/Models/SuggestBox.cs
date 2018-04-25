using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Wikipedia_Fluent.Models
{
    class SuggestBox
    {
        List<Prefixsearch> list = new List<Prefixsearch>();
        Rootobject rootobject = new Rootobject();

        public async Task<Rootobject> GetPrefixsearches(string String)
        {
            string pattern = " ";
            string input = String;
            string replacement = @"%20";

            Regex regex = new Regex(pattern);
            string output = regex.Replace(input, replacement);

            StringBuilder sb = new StringBuilder();
            sb.Append("https://en.wikipedia.org/w/api.php?action=query&list=prefixsearch&format=json");
            sb.Append("&pssearch=");
            sb.Append(input);
            output = sb.ToString();

            HttpClient http = new HttpClient();
            var response = await http.GetAsync(output);
            var result = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Rootobject>(result);

            List<Prefixsearch> prefixsearch = new List<Prefixsearch>();


            prefixsearch = data.query.prefixsearch;




            return data;
        }

        public List<Prefixsearch> GetResults(string query)
        {
            return list;
        }

        public async void SetResults(string query)
        {


            list.Clear();

            rootobject = await GetPrefixsearches(query);
            list = rootobject.query.prefixsearch;

        }

        public IEnumerable<Prefixsearch> GetMatchingSuggestion(string query)
        {
            return list
                .Where(c => c.title.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1)
                .OrderByDescending(c => c.title.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }

        public string textsuggestion;

        public override string ToString()
        {
            return string.Format("{0}", textsuggestion);
        }
    }
}
