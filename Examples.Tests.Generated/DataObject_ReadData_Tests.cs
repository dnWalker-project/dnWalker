using Examples.Demonstrations.AbstractData;

using Moq;

using Xunit;

namespace Examples.Tests.Demonstrations.AbstractData
{
    
    [Trait("dnWalkerGenerated", "DataObject::ReadData")]
    [Trait("ExplorationStrategy", "SmartAllPathsCoverage")]
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
                .Returns(54629);
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
        [Fact]
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "17")]
        public void ReadDataExceptionSchema_20()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[1] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "18")]
        public void ReadDataChangedObjectSchema_21()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "18")]
        public void ReadDataReturnValueSchema_22()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "19")]
        public void ReadDataChangedObjectSchema_23()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "19")]
        public void ReadDataReturnValueSchema_24()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
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
        [Trait("Iteration", "20")]
        public void ReadDataChangedObjectSchema_25()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "20")]
        public void ReadDataReturnValueSchema_26()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
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
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "21")]
        public void ReadDataChangedObjectSchema_27()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "Author";
            dataRecord2.StrValue = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "21")]
        public void ReadDataReturnValueSchema_28()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "Author";
            dataRecord2.StrValue = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "22")]
        public void ReadDataChangedObjectSchema_29()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "A";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            Assert.Equal("A", dataObject1.Author);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "22")]
        public void ReadDataReturnValueSchema_30()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "A";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "23")]
        public void ReadDataChangedObjectSchema_31()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "23")]
        public void ReadDataReturnValueSchema_32()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(false);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "24")]
        public void ReadDataChangedObjectSchema_33()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "Created";
            dataRecord2.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "24")]
        public void ReadDataReturnValueSchema_34()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "Created";
            dataRecord2.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "25")]
        public void ReadDataChangedObjectSchema_35()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.IntValue = 1;
            dataRecord2.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "25")]
        public void ReadDataReturnValueSchema_36()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.IntValue = 1;
            dataRecord2.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "26")]
        public void ReadDataChangedObjectSchema_37()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.IntValue = 0;
            dataRecord2.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = -1;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.Created);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "26")]
        public void ReadDataReturnValueSchema_38()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.IntValue = 0;
            dataRecord2.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = -1;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "27")]
        public void ReadDataChangedObjectSchema_39()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "LastAccess";
            dataRecord2.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "27")]
        public void ReadDataReturnValueSchema_40()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = "LastAccess";
            dataRecord2.IntValue = 0;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(false, result);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "28")]
        public void ReadDataChangedObjectSchema_41()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "28")]
        public void ReadDataReturnValueSchema_42()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
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
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "29")]
        public void ReadDataExceptionSchema_43()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "A";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[3];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[2] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "30")]
        public void ReadDataChangedObjectSchema_44()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "A";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            Assert.Equal("A", dataObject1.Author);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "30")]
        public void ReadDataReturnValueSchema_45()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = "A";
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
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
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "31")]
        public void ReadDataChangedObjectSchema_46()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = null;
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "31")]
        public void ReadDataReturnValueSchema_47()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.StrValue = null;
            dataRecord2.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[1] = dataRecord2;
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
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
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
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "32")]
        public void ReadDataExceptionSchema_48()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord dataRecord2 = new DataRecord();
            dataRecord2.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[3];
            dataRecordArr1[1] = dataRecord2;
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[2] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "33")]
        public void ReadDataChangedObjectSchema_49()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.LastAccess);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "33")]
        public void ReadDataReturnValueSchema_50()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "LastAccess";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
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
        [Trait("Iteration", "34")]
        public void ReadDataExceptionSchema_51()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[1] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.LastAccess = 0;
            dataObject1.Created = -1;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "35")]
        public void ReadDataChangedObjectSchema_52()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.LastAccess = 0;
            dataObject1.Created = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal(0, dataObject1.Created);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "35")]
        public void ReadDataReturnValueSchema_53()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.IntValue = 0;
            dataRecord1.Name = "Created";
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.LastAccess = 0;
            dataObject1.Created = -1;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
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
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "36")]
        public void ReadDataReturnValueSchema_54()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = "";
            dataRecord1.Name = "Author";
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
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "37")]
        public void ReadDataExceptionSchema_55()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = "A";
            dataRecord1.Name = "Author";
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[1] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ChangedObjectSchema")]
        [Trait("Iteration", "38")]
        public void ReadDataChangedObjectSchema_56()
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
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            @this.ReadData(database);
            Assert.Equal("A", dataObject1.Author);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "38")]
        public void ReadDataReturnValueSchema_57()
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
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
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
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "39")]
        public void ReadDataReturnValueSchema_58()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.StrValue = null;
            dataRecord1.Name = "Author";
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
        [Trait("TestSchema", "ExceptionSchema")]
        [Trait("Iteration", "40")]
        public void ReadDataExceptionSchema_59()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[2];
            dataRecordArr1[0] = dataRecord1;
            dataRecordArr1[1] = null;
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            Action readData = () => @this.ReadData(database);
            Assert.Throws<NullReferenceException>(readData);
            
        }
        [Fact]
        [Trait("TestSchema", "ReturnValueSchema")]
        [Trait("Iteration", "41")]
        public void ReadDataReturnValueSchema_60()
        {
            // Arrange input model heap
            DataRecord dataRecord1 = new DataRecord();
            dataRecord1.Name = null;
            DataRecord[] dataRecordArr1 = new DataRecord[1];
            dataRecordArr1[0] = dataRecord1;
            Mock<DataObject> dataObject1_mock = new Mock<DataObject>();
            DataObject dataObject1 = dataObject1_mock.Object;
            dataObject1.Id = 0;
            dataObject1.Created = 0;
            dataObject1.LastAccess = 0;
            dataObject1_mock
                .Setup(o => o.SetValue(It.IsAny<DataRecord>()))
                .Returns(true);
            Mock<IDatabase> iDatabase1_mock = new Mock<IDatabase>();
            IDatabase iDatabase1 = iDatabase1_mock.Object;
            iDatabase1_mock
                .Setup(o => o.GetCheckSum(It.IsAny<int>()))
                .Returns(54629);
            iDatabase1_mock
                .Setup(o => o.GetRecords(It.IsAny<int>()))
                .Returns(dataRecordArr1);
            
            // Arrange method arguments
            DataObject @this = dataObject1;
            IDatabase database = iDatabase1;
            
            bool result = @this.ReadData(database);
            Assert.Equal(true, result);
            
        }
    }
}
