using dnWalker.Interface;
using dnWalker.Interface.Commands;

using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Interface.Commands
{
    public class LoadAssemblyCommandTests
    {
        [Fact]
        public void LoadSucceeds()
        {
            Mock<IAppModel> modelMock = new Mock<IAppModel>();
            modelMock.Setup(m => m.LoadAssembly(It.IsAny<string>())).Returns(true);


            LoadAssemblyCommand cmd = new LoadAssemblyCommand("file1", "file2");

            cmd.Execute(modelMock.Object).Should().Be(CommandResult.Success);

            modelMock.Verify(m => m.LoadAssembly("file1"), Times.Once());
            modelMock.Verify(m => m.LoadAssembly("file2"), Times.Once());
        }

        [Fact]
        public void LoadFails()
        {
            Mock<IAppModel> modelMock = new Mock<IAppModel>();
            modelMock.Setup(m => m.LoadAssembly(It.IsAny<string>())).Returns(false);


            LoadAssemblyCommand cmd = new LoadAssemblyCommand("file1", "file2");

            cmd.Execute(modelMock.Object).Should().Be(CommandResult.BreakFail(-1));

            modelMock.Verify(m => m.LoadAssembly("file1"), Times.Once());
            modelMock.Verify(m => m.LoadAssembly("file2"), Times.Never());
        }
    }
}
