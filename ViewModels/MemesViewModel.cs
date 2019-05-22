using System;
using System.Collections.Generic;
using System.Linq;
using memegeumApp.Models;

namespace memegeumApp.ViewModels
{
    public class MemesViewModel
    {
        public IEnumerable<Meme> Memes { get; set; }

        public IEnumerable<string> AllTags { get; set; }
        public List<string> WhiteListTags { get; set; }
        public string WhiteListTagsAsString { get; set; }
        public List<string> BlackListTags { get; set; }
        public string BlackListTagsAsString { get; set; }

        public IEnumerable<int> PageNumbers { get; set; }
        public int SelectedPage { get; set; }

        public MemesViewModel()
        {

        }


    }
}
