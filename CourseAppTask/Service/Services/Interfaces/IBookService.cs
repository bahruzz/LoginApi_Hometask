using Service.DTOs.Admin.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IBookService
    {
        Task CreateAsync(BookCreateDto model);
        Task EditAsync(int? id, BookEditDto model);
        Task DeleteAsync(int? id);
        Task<BookDto> GetById(int id);
        Task<IEnumerable<BookDto>> GetAll();
        Task<IEnumerable<BookDto>> SearchByName(string name);
    }
}
