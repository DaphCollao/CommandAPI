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
    }
}