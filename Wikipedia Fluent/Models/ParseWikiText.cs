using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wikipedia_Fluent.Models;

namespace Wikipedia_Fluent.Models
{
    public class WikiContent_Rootobject {

            public Introduction introduction { get; set; }
            public List<Headers> headers { get; set; }
            public string pagetitle { get; set; }
            public string remainingcontent { get; set; }
        }

        public class Introduction {
            public string content { get; set; }
            public string datatables { get; set; }
            public List<String> references { get; set; }
            public List<String> images { get; set; }
        }

        public class Headers {
            public string content { get; set; }
            public string datatables { get; set; }
            public string[] subheaders { get; set; }
            public List<String> references { get; set; }
            public List<String> images { get; set; }
            public string headertitle { get; set; }
        }

        public class Subheaders {
            public string content { get; set; }
            public string datatables { get; set; }
            public List<String> references { get; set; }
            public List<String> images { get; set; }
            public string subheadertitle { get; set; }
        }

 }
        
    

