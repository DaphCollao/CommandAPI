using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase{

        private readonly ICommandAPIRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandAPIRepo repo, IMapper mapper){
            _repo = repo;
            _mapper = mapper;
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<string>> Get(){
        //     return new string[] {"this","is","hard","code"};
        // }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repo.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<Command>>(commandItems).OrderBy(o => o.Id));
        }

        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var command = _repo.GetCommandById(id);
            if (command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto createDto)
        {
            var commandModel = _mapper.Map<Command>(createDto); //Se mapea de CommandCreateDto a Command (modelo en BD)
            _repo.CreateCommand(commandModel); //Se crea Command como CommandItem en BD
            _repo.SaveChanges(); //Se guarda nuevo Command en BD

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel); //Se mapea de Command a CommandReadDto 

            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto); //Se crea un recurso, devuelve una respuesta 201 y proporciona la URL donde se puede acceder al nuevo recurso
            
            //Explicacion del Return
            //CreatedAtRoute indica que el recurso ha sido creado y proporciona la ubicación del nuevo recurso.
            //nameof(GetCommandById) especifica el nombre de la ruta que se usará para obtener el recurso recién creado (en este caso, un método llamado GetCommandById).
            //new {Id = commandReadDto.Id} es un objeto anónimo que contiene los valores necesarios para generar la URL de la ruta, en este caso, el ID del recurso recién creado.
            //commandReadDto es el contenido del recurso que se devuelve en la respuesta.
        }

        [HttpPut("{id}")]
        public ActionResult<CommandReadDto> UpdateCommand(int id, [FromBody] CommandUpdateDto updateDto)
        {
            var commandModelFromRepo = _repo.GetCommandById(id); //Get Command Object

            if (commandModelFromRepo == null) 
                return NotFound();

            _mapper.Map(updateDto,commandModelFromRepo); //Map from CommandUpdateDto to Command Object

            _repo.UpdateCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }
    }
}