using AutoMapper;
using BattleshipGame.API.Models.Game;
using BattleshipGame.Data.Entities;

namespace BattleshipGame.API.Profiles
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<FieldEntity, Field>()
           .ConstructUsing((src, context) => new Field(src.X, src.Y, src.Player.Name));
            CreateMap<Field, FieldEntity>();
        }
    }
}
