using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Profiles
{
    public class CommandsProfile : Profile
    {
        public CommandsProfile()
        {
            //Source > Targe
            CreateMap<Command, CommandReadDto>(); //From Command to CommandReadDto
            CreateMap<CommandCreateDto, Command>(); //From CommandCreateDto to Command
            CreateMap<CommandUpdateDto, Command>(); //From CommandUpdate to Command
            CreateMap<Command, CommandUpdateDto>(); //From Command to CommandUpdate
        }
    }
}