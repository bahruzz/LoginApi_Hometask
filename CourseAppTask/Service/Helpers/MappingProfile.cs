using AutoMapper;
using Domain.Entities;
using Service.DTOs.Account;
using Service.DTOs.Admin.Authors;
using Service.DTOs.Admin.Books;


namespace Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<AuthorCreateDto, Author>();
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorEditDto, Author>();

            CreateMap<BookCreateDto, Book>();
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(m => m.AuthorBooks.Select(m => m.Author.FullName)));

            CreateMap<BookEditDto, Book>();

           

           
            CreateMap<RegisterDto, AppUser>();
            CreateMap<AppUser, UserDto>();




        }
    }
}
