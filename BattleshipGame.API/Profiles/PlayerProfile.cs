using AutoMapper;
using BattleshipGame.API.Models;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerForCreationDto, Player>();
            CreateMap<PlayerForCreationDto, PlayerDto>();
            CreateMap<PlayerForUpdateDto, Player>();
            CreateMap<Player, PlayerForUpdateDto>();
        }
    }
}
