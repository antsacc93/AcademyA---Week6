using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6.Linq.Demo.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int Vote { get; set; }
        public string Description { get; set; }

    }
}
