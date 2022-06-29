using AutoMapper;
using RespawnCoreApiExample.Domain.Models.Dto;
using RespawnCoreApiExample.Domain.Models.Entities;

namespace RespawnCoreApiExample.Api.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Author, AuthorDto>().ReverseMap();
        }
    }
}