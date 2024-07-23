using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase{

        private readonly ICommandAPIRepo _repo;
        public CommandsController(ICommandAPIRepo repo){
            _repo = repo;
        }

        // [HttpGet]
        // public ActionResult<IEnumerable<string>> Get(){
        //     return new string[] {"this","is","hard","code"};
        // }

        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {
            var commandItems = _repo.GetAllCommands();
            return Ok(commandItems);
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = _repo.GetCommandById(id);
            if (command == null)
                return NotFound();
            return Ok(command);
        }
    }
}