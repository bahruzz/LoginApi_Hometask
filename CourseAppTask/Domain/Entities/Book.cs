using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book:BaseEntity
    {
        public string Name { get; set; }
        public int PageCount { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
