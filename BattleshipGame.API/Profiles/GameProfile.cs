using AutoMapper;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Profiles
{
    public class GameProfile : Profile
    {
        public GameProfile()
        {
            CreateMap<PlayerEntity, GameEntity>();
        }
    }
}
