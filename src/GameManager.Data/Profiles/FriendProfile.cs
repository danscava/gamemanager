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
    /// Maps the friend objects and DTOs
    /// </summary>
    public class FriendProfile : Profile
    {
        public FriendProfile()
        {
            CreateMap<Friend, FriendResponseDto>()
                .ForMember(dst => dst.email, src => src.MapFrom(z => z.Email))
                .ForMember(dst => dst.id, src => src.MapFrom(z => z.Id))
                .ForMember(dst => dst.name, src => src.MapFrom(z => z.Name))
                .ForMember(dst => dst.telephone, src => src.MapFrom(z => z.Telephone))
                .ForMember(dst => dst.games, src => src.MapFrom(z => z.BorrowedGames));

            CreateMap<Friend, BorrowerResponseDto>()
                .ForMember(dst => dst.email, src => src.MapFrom(z => z.Email))
                .ForMember(dst => dst.id, src => src.MapFrom(z => z.Id))
                .ForMember(dst => dst.name, src => src.MapFrom(z => z.Name))
                .ForMember(dst => dst.telephone, src => src.MapFrom(z => z.Telephone));
        }
    }
}
