#nullable disable
using Examples.Demonstrations.Abstract;

using Moq;

using System.Xml.Linq;

using Xunit;

namespace Examples.Tests.Generated.Demonstrations.Abstract
{
    [Trait("dnWalkerGenerated", "DataObject::ReadData")]
    [Trait("ExplorationStrategy", "AllEdgesCoverage")]
    public class DataObject_ReadData_Tests
    {

        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "1")]
        public void ReadDataExceptionSchema_1()
        {
            // Arrange input model heap
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = null;

            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);

        }
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "2")]
        public void ReadDataExceptionSchema_2()
        {
            // Arrange input model heap
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns((DataRecord[])null);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "3")]
        public void ReadDataReturnValueSchema_3()
        {
            // Arrange input model heap
            DataRecord[] dataRecordArr1 = new DataRecord[0];
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "4")]
        public void ReadDataReturnValueSchema_4()
        {
            // Arrange input model heap
            DataRecord[] dataRecordArr1 = new DataRecord[0];
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(1671941);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(true, result);

        }
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "5")]
        public void ReadDataExceptionSchema_5()
        {
            // Arrange input model heap
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = null;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "6")]
        public void ReadDataReturnValueSchema_6()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "7")]
        public void ReadDataReturnValueSchema_7()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "8")]
        public void ReadDataReturnValueSchema_8()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = "Author";
            dataRecord1.StrValue = null;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "9")]
        public void ReadDataChangedObjectSchema_9()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = "A";
            dataRecord1.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            dataObject1.Author = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            @this.ReadData(database);
            Assert.Equal("A", dataObject1.Author);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "9")]
        public void ReadDataReturnValueSchema_10()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = "A";
            dataRecord1.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            dataObject1.Author = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "10")]
        public void ReadDataReturnValueSchema_11()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = "";
            dataRecord1.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "11")]
        public void ReadDataReturnValueSchema_12()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = "Created";
            dataRecord1.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "12")]
        public void ReadDataReturnValueSchema_13()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 1;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.Id = 0;
            dataObject1.LastAccess = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "13")]
        public void ReadDataChangedObjectSchema_14()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.LastAccess = 0;
            dataObject1.Created = -1;
            dataObject1.Id = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            @this.ReadData(database);
            Assert.Equal(0, dataObject1.Created);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "13")]
        public void ReadDataReturnValueSchema_15()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.LastAccess = 0;
            dataObject1.Created = -1;
            dataObject1.Id = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "14")]
        public void ReadDataReturnValueSchema_16()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = "LastAccess";
            dataRecord1.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.LastAccess = 0;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "15")]
        public void ReadDataChangedObjectSchema_17()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 1;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.LastAccess = 0;
            dataObject1.Id = 0;
            dataObject1.Created = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            @this.ReadData(database);
            Assert.Equal(1, dataObject1.LastAccess);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "15")]
        public void ReadDataReturnValueSchema_18()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 1;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(0);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.LastAccess = 0;
            dataObject1.Id = 0;
            dataObject1.Created = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "16")]
        public void ReadDataReturnValueSchema_19()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 1;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;

            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;

            bool result = @this.ReadData(database);
            Assert.Equal(false, result);

        }
    }
}
