using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Wikipedia_Fluent.Models
{
    //For some reason the program wants these definitions at the top
    [DataContract]
    public class Rootobject
    {
        [DataMember]
        public bool batchcomplete { get; set; }
        [DataMember]
        public Query query { get; set; }
    }
    [DataContract]
    public class Query
    {
        [DataMember]
        public List<Normalized> normalized { get; set; }
        [DataMember]
        public List<WikiPage> pages { get; set; }
    }
    [DataContract]
    public class Normalized
    {
        [DataMember]
        public bool fromencoded { get; set; }
        [DataMember]
        public string from { get; set; }
        [DataMember]
        public string to { get; set; }
    }
    [DataContract]
    public class WikiPage
    {
        [DataMember]
        public int pageid { get; set; }
        [DataMember]
        public int ns { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public List<Revision> revisions { get; set; }
    }
    [DataContract]
    public class Revision
    {
        [DataMember]
        public string contentformat { get; set; }
        [DataMember]
        public string contentmodel { get; set; }
        [DataMember]
        public string content { get; set; }
    }

    public class WikiPageContentsToPass
    {
        public string PageContent { get; set; }

        public string PageTitle { get; set; }
    }

    public class WikiAPINavigator
    {

        //Eventually replace the action, prop, revision, content,
        //format, etc with variables for functionality
        public static string ParseQueryForWiki(string String)
        {
            string parseSpacesInURL = String.Replace(" ", "%20");
            string url_To_Navigate_To = string.Format("https://en.wikipedia.org/w/api.php?action=extracts&titles&titles=" + String + "&redirects=true");
            return url_To_Navigate_To;
        }

        //Not guaranteed to return value, but we hope it'll return a Rootobject
        //Therefore, Task<Rootobject>



            

    }




}






