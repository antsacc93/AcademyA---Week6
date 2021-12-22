﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6.Linq.Demo.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PublishYear { get; set; }
        public string Genre { get; set; }
        public int Pages { get; set; }
        public int AuthorId { get; set; }
    }
}
