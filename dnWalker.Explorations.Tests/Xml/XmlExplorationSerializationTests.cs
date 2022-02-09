using dnWalker.Explorations.Xml;
using dnWalker.Parameters;
using dnWalker.Parameters.Serialization.Xml;

using FluentAssertions;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Xunit;

namespace dnWalker.Explorations.Tests.Xml
{
    public class XmlExplorationSerializationTests
    {
        [Fact]
        public void Test_DeSerialization()
        {
            Mock<IReadOnlyParameterSet> setMock = new Mock<IReadOnlyParameterSet>();
            setMock.SetupGet(s => s.Parameters)
                .Returns(new Dictionary<ParameterRef, IParameter>());

            ConcolicExploration.Builder explBuilder = new ConcolicExploration.Builder();
            explBuilder.Start = new DateTime(2022, 10, 31, 15, 35, 48);
            explBuilder.End = new DateTime(2022, 10, 31, 15, 35, 49);

            explBuilder.MethodSignature = "SomeKindOfMethod";
            explBuilder.Solver = "SomeKindOfSolver";
            explBuilder.AssemblyFileName = "Assembly.dll";
            explBuilder.AssemblyName = "Assembly";
            explBuilder.Failed = true;

            // 1. iteration
            ConcolicExplorationIteration.Builder iterBuilder = new ConcolicExplorationIteration.Builder();
            iterBuilder.Start = explBuilder.Start;
            iterBuilder.End = explBuilder.End;

            iterBuilder.IterationNumber = 1;
            iterBuilder.PathConstraint = "(x < 0)";
            iterBuilder.BaseParameterSet = XmlParameterSetInfo.FromSet(setMock.Object);
            iterBuilder.ExecutionParameterSet = XmlParameterSetInfo.FromSet(setMock.Object);


            explBuilder.Iterations.Add(iterBuilder);


            XmlExplorationSerializer serializer = new XmlExplorationSerializer();

            XElement xml = serializer.ToXml(explBuilder.Build());

            XmlExplorationDeserializer deserializer = new XmlExplorationDeserializer();

            ConcolicExploration exploration = deserializer.GetExploration(xml);

            exploration.Start.Should().Be(explBuilder.Start);
            exploration.End.Should().Be(explBuilder.End);
            exploration.MethodSignature.Should().Be(explBuilder.MethodSignature);
            exploration.Solver.Should().Be(explBuilder.Solver);
            exploration.AssemblyFileName.Should().Be(explBuilder.AssemblyFileName);
            exploration.AssemblyName.Should().Be(explBuilder.AssemblyName);
            exploration.Failed.Should().Be(explBuilder.Failed);

            exploration.Iterations.Should().HaveCount(1);
            ConcolicExplorationIteration iteration = exploration.Iterations[0];
            iteration.Start.Should().Be(iterBuilder.Start);
            iteration.End.Should().Be(iterBuilder.End);
            
            iteration.IterationNumber.Should().Be(iterBuilder.IterationNumber);
            iteration.PathConstraint.Should().Be(iterBuilder.PathConstraint);

            iteration.BaseParameterSet.Should().BeOfType<XmlParameterSetInfo>();
            iteration.ExecutionParameterSet.Should().BeOfType<XmlParameterSetInfo>();

            XmlParameterSetInfo baseSet = (XmlParameterSetInfo)iteration.BaseParameterSet;
            XmlParameterSetInfo execSet = (XmlParameterSetInfo)iteration.ExecutionParameterSet;

            baseSet.Xml.Name.ToString().Should().Be("Set");
            baseSet.Xml.Elements().Should().BeEmpty();

            execSet.Xml.Name.ToString().Should().Be("Set");
            execSet.Xml.Elements().Should().BeEmpty();
        }
    }
}
