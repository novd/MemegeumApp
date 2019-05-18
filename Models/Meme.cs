using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace memegeumApp.Models
{
    public class Meme
    {
        public enum SourcePage { JBZD, KWEJK, MEMYPL }

        public SourcePage Page { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public List<string> Tags { get; set; }

       
    }
}
