using AutoMapper;
using MimicAPI.Models;
using MimicAPI.Models.DTO;


namespace MimicAPI.Helpers
{
    public class DTOMapperProfile : Profile
    {
        public DTOMapperProfile()
        {
            CreateMap<Palavras, PalavraDTO>();
            CreateMap<PaginacaoList<Palavras>, PaginacaoList<PalavraDTO>>();
        }
    }
}
