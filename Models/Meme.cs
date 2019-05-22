using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace memegeumApp.Models
{
    public class Meme
    {
        public enum SourcePage { JBZD, KWEJK, MEMYPL }
        [NotMapped]
        public List<string> Tags { get; set; }

        [Key]
        public int Id { get; set; }
        public SourcePage Page { get; set; }
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string TagsAsString 
        {
            get { return string.Join(',', Tags); } 
            set { Tags = value.Split(',').ToList(); }
         }

       
    }
}
