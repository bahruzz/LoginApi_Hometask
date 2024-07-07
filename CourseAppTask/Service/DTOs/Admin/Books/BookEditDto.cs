using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Admin.Books
{
    public class BookEditDto
    {
        public string Name { get; set; }
        public int PageCount { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
    }
}
