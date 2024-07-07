using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author : BaseEntity
    {
        public string FullName { get; set; }
        public string Country { get; set; }
        public int Age { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
