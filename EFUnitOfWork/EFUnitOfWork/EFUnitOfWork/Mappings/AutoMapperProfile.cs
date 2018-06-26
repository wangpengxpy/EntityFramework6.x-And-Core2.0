using AutoMapper;
using EF.Core.Data;
using EFUnitOfWork.Models;

namespace EFUnitOfWork.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
        }
    }
}