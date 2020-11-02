using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using GameManager.Data.Dtos;
using GameManager.Data.Models;

namespace GameManager.Data.Profiles
{
    /// <summary>
    /// Maps the game media objects and dtos
    /// </summary>
    public class  GameMediaProfile : Profile
    {
        public GameMediaProfile()
        {
            CreateMap<GameMedia, GameMediaResponseDto>()
                .ForMember(dst => dst.platform, src => src.MapFrom(z => z.Platform))
                .ForMember(dst => dst.media, src => src.MapFrom(z => z.MediaType))
                .ForMember(dst => dst.borrower, src => src.MapFrom(z => z.Borrower))
                .ForMember(dst => dst.title, src => src.MapFrom(z => z.Title))
                .ForMember(dst => dst.year, src => src.MapFrom(z => z.Year));

            CreateMap<GameMedia, BorrowedGameMediaResponseDto>()
                .ForMember(dst => dst.platform, src => src.MapFrom(z => z.Platform))
                .ForMember(dst => dst.media, src => src.MapFrom(z => z.MediaType))
                .ForMember(dst => dst.title, src => src.MapFrom(z => z.Title))
                .ForMember(dst => dst.year, src => src.MapFrom(z => z.Year));
        }
    }
}
