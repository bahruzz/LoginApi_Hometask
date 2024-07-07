using AutoMapper;
using Domain.Entities;
using Repository.Repositories.Interfaces;
using Service.DTOs.Admin.Authors;
using Service.Helpers.Exceptions;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository,
                             IBookRepository bookRepository,
                             IMapper mapper)
        {
            _authorRepo =authorRepository;

            _mapper =mapper;

            _bookRepo =bookRepository;
            
        }

        public async Task CreateAsync(AuthorCreateDto model)
        {

            ArgumentNullException.ThrowIfNull(nameof(model));

            if (await _authorRepo.ExistWithNameAsync(model.FullName))
            {
                throw new BadRequestException("Author with this name already exists");
            }


            await _authorRepo.CreateAsync(_mapper.Map<Author>(model));
        }

        public async Task DeleteAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var author = await _authorRepo.GetById((int)id);

            if (author is null) throw new NotFoundException("Data was not found");

            await _authorRepo.DeleteAsync(author);
        }

        public async Task EditAsync(int? id, AuthorEditDto model)
        {
            if (id is null) throw new ArgumentNullException();

            var author = await _authorRepo.GetById((int)id);

            if (author is null) throw new NotFoundException("Data was not found");

            await _authorRepo.EditAsync(_mapper.Map(model, author));
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            return _mapper.Map<IEnumerable<AuthorDto>>(await _authorRepo.GetAllAsync());
        }

        public async Task<AuthorDto> GetByIdAsync(int? id)
        {
            if (id is null) throw new ArgumentNullException();

            var author = await _authorRepo.GetById((int)id);

            if (author is null) throw new NotFoundException("Data was not found");

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<IEnumerable<AuthorDto>> SearchByNameAsync(string fullName)
        {
            return _mapper.Map<IEnumerable<AuthorDto>>(await _authorRepo.FindAll(m => m.FullName.Contains(fullName)));
        }

       
    }
}
