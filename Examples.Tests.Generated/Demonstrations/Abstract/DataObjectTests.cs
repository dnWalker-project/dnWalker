#nullable disable
using Examples.Demonstrations.Abstract;

using Moq;

using System.Xml.Linq;

using Xunit;

namespace Examples.Demonstrations.Abstract.Tests
{
    [Trait("dnWalkerGenerated", "SUT name")]
    public class DataObject_ReadDataTests
    {

        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        public void Test_ReadData_1()
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
        public void Test_ReadData_2()
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
        public void Test_ReadData_3()
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
        public void Test_ReadData_4()
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
        public void Test_ReadData_5()
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
        public void Test_ReadData_6()
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
        public void Test_ReadData_7()
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
        [Trait("TestSchema", "ChangedObjectSchema")]
        public void Test_ReadData_8()
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

            @this.ReadData(database);
            Assert.Null(dataObject1.Author);

        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        public void Test_ReadData_9()
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
        public void Test_ReadData_10()
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
        public void Test_ReadData_11()
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
        public void Test_ReadData_12()
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
        public void Test_ReadData_13()
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
        public void Test_ReadData_14()
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
        public void Test_ReadData_15()
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
        public void Test_ReadData_16()
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
        public void Test_ReadData_17()
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
