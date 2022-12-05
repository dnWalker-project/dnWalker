using dnWalker.Interface;

using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dnWalker.Tests.Interface
{
    public class BatchModeRunnerTests : IDisposable
    {
        private readonly string _scriptFile;

        public BatchModeRunnerTests()
        {
            _scriptFile = $"{Random.Shared.Next()}.dn";
        }

        public void Dispose()
        {
            File.Delete(_scriptFile);
        }

        private void WriteScript(params string[] lines)
        {
            File.WriteAllLines(_scriptFile, lines);
        }

        [Fact]
        public void TestBatchModeRunner()
        {
            const string Script =
            """
            ; comments should be skipped
            load assembly path/to/assembly
            load models path/to/models

            explore my_method
            explore other_method output.xml

            exit
            """;

            WriteScript(Script);

            BatchModeRunner runner = new BatchModeRunner(new Options() { Script = _scriptFile });

            Mock<IAppModel> appModelMock = new Mock<IAppModel>();
            appModelMock.Setup(m => m.LoadAssembly(It.IsAny<string>())).Returns(true);
            appModelMock.Setup(m => m.LoadModels(It.IsAny<string>())).Returns(true);
            appModelMock.Setup(m => m.Explore(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            runner.Run(appModelMock.Object).Should().Be(0);

            appModelMock.Verify(a => a.LoadAssembly("path/to/assembly"), Times.Once);
            appModelMock.Verify(a => a.LoadModels("path/to/models"), Times.Once);

            appModelMock.Verify(a => a.Explore("my_method", null), Times.Once);
            appModelMock.Verify(a => a.Explore("other_method", "output.xml"), Times.Once);
        }
    }
}
