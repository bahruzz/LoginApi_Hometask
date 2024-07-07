using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Authors;
using Service.DTOs.Admin.Books;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BookService : IBookService
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IAuthorBookRepository _authorBookRepo;
        private readonly IMapper _mapper;

        public BookService(IAuthorRepository authorRepository,
                             IBookRepository bookRepository,
                             IMapper mapper,
                             IAuthorBookRepository authorBookRepository)
        {
            _authorRepo = authorRepository;
            _mapper = mapper;
            _bookRepo = bookRepository;
            _authorBookRepo = authorBookRepository;
        }

        public async Task CreateAsync(BookCreateDto model)
        {
            var data = _mapper.Map<Book>(model);
            await _bookRepo.CreateAsync(data);

            foreach (var id in model.AuthorIds)
            {
                await _authorBookRepo.CreateAsync(new AuthorBook
                {
                    BookId = data.Id,
                    AuthorId = id
                });
            }
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var book = await _bookRepo.GetById((int)id);

            if (book is null) throw new NotFoundException("Data was not found");

            await _bookRepo.DeleteAsync(book);
        }

        public async Task EditAsync(int? id, BookEditDto model)
        {
            ArgumentNullException.ThrowIfNull(nameof(id));
            ArgumentNullException.ThrowIfNull(nameof(model));

            var book = await _bookRepo.GetById((int)id) ?? throw new NotFoundException("Data not found");

            _mapper.Map(model, book);
            await _bookRepo.EditAsync(book);
        }

        public async Task<IEnumerable<BookDto>> GetAll()
        {
            var books = await _bookRepo.FindAllWithIncludes()
                 .Include(m => m.AuthorBooks)
                 .ThenInclude(m => m.Author)
                 .ToListAsync();
            var mappedBooks = _mapper.Map<List<BookDto>>(books);
            return mappedBooks;
        }

        public async Task<BookDto> GetById(int id)
        {
            var book = await _bookRepo.FindAllWithIncludes()
               .Where(m => m.Id == id)
               .Include(m => m.AuthorBooks)
               .ThenInclude(m => m.Author)
               .FirstOrDefaultAsync();

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> SearchByName(string name)
        {
            return _mapper.Map<IEnumerable<BookDto>>(await _bookRepo.FindAll(m => m.Name.Contains(name)));
        }
    }
}
