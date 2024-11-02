using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;

        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUnit",
                CommandLine = "dotnet test"
            };
        }

        public void Dispose()
        {
            testCommand = null;
        }

        [Fact]
        public void CanChangeHowTo()
        {
            //ARRAGE

            //ACT 
            testCommand.HowTo = "Execute Unit Tests";

            //ASSERT
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlataform()
        {
            //ARRAGE

            //ACT 
            testCommand.Platform = "Execute Unit Tests";

            //ASSERT
            Assert.Equal("Execute Unit Tests", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            //ARRAGE

            //ACT 
            testCommand.CommandLine = "Execute Unit Tests";

            //ASSERT
            Assert.Equal("Execute Unit Tests", testCommand.CommandLine);
        }
    }
}