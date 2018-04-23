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
        public Query2 query { get; set; }
        [DataMember]
        public Parse parse { get; set; }
    }


    [DataContract]
    public class Parse
    {
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public int pageid { get; set; }
        [DataMember]
        public string wikitext { get; set; }
        [DataMember]
        public Text text { get; set; }
        [DataMember]
        public List<Langlink> langlinks { get; set; }
        [DataMember]
        public int revid { get; set; }
        [DataMember]
        public List<object> categories { get; set; }
        [DataMember]
        public List<Link> links { get; set; }
        [DataMember]
        public List<Template> templates { get; set; }
        [DataMember]
        public string[] images { get; set; }
        [DataMember]
        public string[] externallinks { get; set; }
        [DataMember]
        public Section[] sections { get; set; }
        [DataMember]
        public object[] parsewarnings { get; set; }
        [DataMember]
        public string displaytitle { get; set; }
        [DataMember]
        public List<object> iwlinks { get; set; }
        [DataMember]
        public List<Property> properties { get; set; }
    }

            [DataContract]
            public class Text
    {
        [DataMember]
        public string WikiText { get; set; }
    }

            [DataContract]
            public class Langlink
    {
        [DataMember]
        public string lang { get; set; }
        [DataMember]
        public string url { get; set; }
        [DataMember]
        public string langname { get; set; }
        [DataMember]
        public string autonym { get; set; }
        [DataMember]
        public string invalidname_dunnosignif { get; set; }
    }

            [DataContract]
            public class Link
    {
        [DataMember]
        public int ns { get; set; }
        [DataMember]
        public string exists { get; set; }
        [DataMember]
        public string invalidname_dunnosignif { get; set; }
    }

            [DataContract]
            public class Template
    {
        [DataMember]
        public int ns { get; set; }
        [DataMember]
        public string exists { get; set; }
        [DataMember]
        public string invalidname_dunnosignif { get; set; }
    }

            [DataContract]
            public class Section
    {
        [DataMember]
        public int toclevel { get; set; }
        [DataMember]
        public string level { get; set; }
        [DataMember]
        public string line { get; set; }
        [DataMember]
        public string number { get; set; }
        [DataMember]
        public string index { get; set; }
        [DataMember]
        public string fromtitle { get; set; }
        [DataMember]
        public int byteoffset { get; set; }
        [DataMember]
        public string anchor { get; set; }
    }

            [DataContract]
            public class Property
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string invalidname_dunnosignif { get; set; }
    }




    [DataContract]
    public class Query2
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
        public List<Image> images { get; set; }
        [DataMember]
        public List<Revision> revisions { get; set; }
    }

            [DataContract]
            public class Image
    {
        [DataMember]
        public int ns { get; set; }
        [DataMember]
        public string title { get; set; }
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
        public List<WikiPage> images { get; set; }
        public string ImageTitles { get; set; }
    }

}






