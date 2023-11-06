using NetMapper.Models;
using NetMapper.Services;
using NetMapper.Services.Stores;
using NSubstitute;

namespace NetMapper.Tests
{
    public class DriveListServiceTest
    {
        private readonly IDriveListService _sut;
        private readonly IDataStore<List<MapModel>> _db = Substitute.For<IDataStore<List<MapModel>>>();

        public DriveListServiceTest()
        {
            _db.GetData().Returns(new List<MapModel>());
            _sut = new DriveListService(_db);

        }

        [Fact]
        public void AddDrive_ShouldAddDriveToList()
        {
            // Arrange
            var mapModel = new MapModel();
            _sut.DriveList.Clear();
            // Act
            _sut.AddDrive(mapModel);
            // Assert
            Assert.Contains(mapModel, _sut.DriveList);
        }

        [Fact]
        public void RemoveDrive_ShouldRemoveDriveFromList()
        {
            // Arrange
            var mapModel = new MapModel();
            _sut.DriveList.Clear();
            _sut.AddDrive(mapModel);
            // Act
            _sut.RemoveDrive(mapModel);
            // Assert
            Assert.DoesNotContain(mapModel, _sut.DriveList);
            Assert.Empty(_sut.DriveList);
        }
        
        [Fact]
        public void RemoveDrive_ShoulThrowRemovingNonexistingModel()
        {
            // Arrange
            var oldMapModel = new MapModel();
            var newMapModel = new MapModel();
            _sut.DriveList.Clear();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => { _sut.EditDrive(oldMapModel, newMapModel); });
        }
        
        [Fact]
        public void EditDrive_ShouldReplaceOldModelWithNewModel()
        {
            // Arrange
            var oldMapModel = new MapModel();
            var newMapModel = new MapModel();
            _sut.DriveList.Clear();
            _sut.AddDrive(oldMapModel);

            // Act
            _sut.EditDrive(oldMapModel, newMapModel);

            // Assert
            Assert.Contains(newMapModel, _sut.DriveList);
            Assert.DoesNotContain(oldMapModel, _sut.DriveList);
        }

        [Fact]
        public void EditDrive_ShoulThrowReplaceNonexistingOldModel()
        {
            // Arrange
            var oldMapModel = new MapModel();
            var newMapModel = new MapModel();
            _sut.DriveList.Clear();

            // Act & Assert
            Assert.Throws<KeyNotFoundException>(() => { _sut.EditDrive(oldMapModel, newMapModel); });
        }
    }
}