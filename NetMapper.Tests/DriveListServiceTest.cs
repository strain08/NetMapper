using NetMapper.Interfaces;
using NetMapper.Models;
using NetMapper.Services;
using NSubstitute;

namespace NetMapper.Tests
{
    public class DriveListServiceTest
    {
        private readonly IDriveListService _sut;
        private readonly IDataStore<AppDataModel> _db = Substitute.For<IDataStore<AppDataModel>>();

        public DriveListServiceTest()
        {
            _db.GetData().Returns(new AppDataModel());
            _sut = new DriveListService(_db);
        }

        [Fact]
        public void AddDrive_ShouldAddDriveToList()
        {
            // Arrange
            var mapModel = new MapModel();
            _sut.DriveCollection.Clear();
            // Act
            _sut.AddDrive(mapModel);
            // Assert
            Assert.Contains(mapModel, _sut.DriveCollection);
        }

        [Fact]
        public void RemoveDrive_ShouldRemoveItemFromList()
        {
            // Arrange
            var mapModel = new MapModel();
            _sut.DriveCollection.Clear();
            _sut.AddDrive(mapModel);
            // Act
            _sut.RemoveDrive(mapModel);
            // Assert
            Assert.DoesNotContain(mapModel, _sut.DriveCollection);
            Assert.Empty(_sut.DriveCollection);
        }
        
        [Fact]
        public void RemoveDrive_ShoulThrowRemovingNonexistingItem()
        {
            // Arrange
            var newMapModel = new MapModel();
            _sut.DriveCollection.Clear();

            // Act & Assert
            _ = Assert.Throws<KeyNotFoundException>(() => 
            { 
                _sut.RemoveDrive(newMapModel); 
            });
        }
        
        [Fact]
        public void EditDrive_ShouldReplaceOldItemWithNewItem()
        {
            // Arrange
            var oldMapModel = new MapModel();
            var newMapModel = new MapModel();
            _sut.DriveCollection.Clear();
            _sut.AddDrive(oldMapModel);

            // Act
            _sut.EditDrive(oldMapModel, newMapModel);

            // Assert
            Assert.Contains(newMapModel, _sut.DriveCollection);
            Assert.DoesNotContain(oldMapModel, _sut.DriveCollection);
        }

        [Fact]
        public void EditDrive_ShouldThrowReplaceNonexistingOldModel()
        {
            // Arrange
            var oldMapModel = new MapModel();
            var newMapModel = new MapModel();
            _sut.DriveCollection.Clear();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => 
            { 
                _sut.EditDrive(oldMapModel, newMapModel); 
            });
        }
        [Fact]
        public void SaveAll_ShouldCallStoreUpdate()
        {
            // Arrange
            _sut.DriveCollection.Clear();
            var mapModel = new MapModel();
            _sut.AddDrive(mapModel);

            // Act
            _sut.SaveAll();
            // Assert
            _db.Received(1).Update(Arg.Any<AppDataModel>());
        }
    }
}