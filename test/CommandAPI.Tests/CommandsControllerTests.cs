using System;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Profiles;
using CommandAPI.Dtos;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo>? mockRepo;
        CommandsProfile? realProfile;
        MapperConfiguration? configuration;
        IMapper? mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        //Test 1.1 - Check 200 OK HTTP Response
        [Fact]
        public void GetCommandsItems_ReturnsZeroItems_WhenDBIsEmpty()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetAllCommands())
                                                        .Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.GetAllCommands();

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Test 1.2 - Check Single Resource Returned
        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);
            Console.WriteLine($"controller: {controller}");

            //ACT
            var result = controller.GetAllCommands();
            Console.WriteLine($"Result: {result.Result}");
            //ASSERT
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }

        //Test 1.3 - Check 200 OK HTTP Response
        [Fact]
        public void GetAllCommands_Returns200Ok_WhenDBHasOneResource()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.GetAllCommands();

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Test 1.4 - Check the Correct object Type Returned
        [Fact]
        public void GetAllCommands_ReturnsCorrecType_WhenDBHasOneResource()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.GetAllCommands();

            //ASSERT
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        //Test 2.1 - Check 404 Not Found HTTP Response
        [Fact]
        public void GetCommandByID_Returns404NotFound_WhenNonExistentIDProvided()
        {
            //Arrange 
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetCommandById(1);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        //Test 2.2 - Check 200 OK HTTP Response
        [Fact]
        public void GetCommandById_ReturnsCorrecType_WhenValidIDProvided()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.GetCommandById(1);

            //ASSERT
            Assert.IsType<OkObjectResult>(result.Result);
        }

        //Test 2.3 - Check the Correct Object Type Returned
        [Fact]
        public void GetCommandById_Returns200OK_WhenValidIDProvided()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.GetCommandById(1);

            //ASSERT
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //Test 3.1 - Check if the Correct Object Type is Returned
        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmmitted()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.CreateCommand(new CommandCreateDto { });

            //ASSERT
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //Test 3.2 - Check 201 HTTP Response
        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            //ARREANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.CreateCommand(new CommandCreateDto { });

            //ASSERT
            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        //Test 4.1 - Check 204 HTTP Response
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            //ARRENGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.UpdateCommand(1, new CommandUpdateDto { });

            //ASSERT
            Assert.IsType<NoContentResult>(result);
        }

        //Test 4.2 - Check 404 HTTP Response
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIsSubmitted()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.UpdateCommand(0, new CommandUpdateDto { });

            //ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        //Test 5.1 - Check 404 HTTP Response
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.PartialCommandUpdate(0, new JsonPatchDocument<CommandUpdateDto> { });

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();

            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }

            return commands;
        }

        //Test 6.1 - Check for 204 No Content HTTP Response
        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidResourceIDSubmitted()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(GetCommandById(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.DeleteCommand(1);

            //ASSERT
            Assert.IsType<NoContentResult>(result);
        }

        //Test 6.2 - Check for 404 Not Found HTTP Response 
        [Fact]
        public void DeleteCommand_Return404NotFound_WhenNonExistenResourceIDSubmitted()
        {
            //ARRANGE
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            //ACT
            var result = controller.DeleteCommand(0);

            //ASSERT
            Assert.IsType<NotFoundResult>(result);
        }

        private Command GetCommandById(int num)
        {
            var command = new Command
            {
                Id = 0,
                HowTo = "mock",
                CommandLine = "Mock",
                Platform = "Mock"
            };

            if (num <= 0) return new Command();

            return command;
        }
    }
}