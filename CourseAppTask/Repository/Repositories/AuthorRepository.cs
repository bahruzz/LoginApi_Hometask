using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistWithNameAsync(string name)
        {
            return await _context.Books.AnyAsync(m => m.Name == name);
        }

        public async Task<bool> ExistWithNameExceptIdAsync(int id, string name)
        {
            return await _context.Books.AnyAsync(m => m.Name == name && m.Id != id);
        }
    }
}
