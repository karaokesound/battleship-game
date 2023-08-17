using AutoMapper;
using BattleshipGame.API.Models.Player;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<PlayerEntity, PlayerDto>();
            CreateMap<PlayerForCreationDto, PlayerEntity>();
            CreateMap<PlayerForCreationDto, PlayerDto>();
            CreateMap<PlayerForUpdateDto, PlayerEntity>();
            CreateMap<PlayerEntity, PlayerForUpdateDto>();
        }
    }
}
