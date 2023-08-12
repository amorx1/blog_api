using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogAPI
{
    public enum Tag {
        Tech,
        Lifestyle,
        Health,
        Food
    }
    public class BlogPost
    {
        public int id { get; set; }
        public String? Title { get; set; }

        public String? Content { get; set; }

        public Tag[]? Tags { get; set; }
    }
}