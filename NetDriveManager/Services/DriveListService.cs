using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetDriveManager.Services
{

    public class DriveListService : IListManager
    {

        public ObservableCollection<DriveModel> DriveList { get; set; } = new();

        private readonly IStorage _store;


        // CTOR
        public DriveListService(IStorage store)
        {
            _store = store;
            DriveList = new ObservableCollection<DriveModel>(_store.GetAll());

        }
        ~DriveListService()
        {
            _store.Update(new List<DriveModel>(DriveList));
        }

        public void AddDrive(DriveModel model)
        {
            DriveList.Add(model);
            _store.Update(new List<DriveModel>(DriveList));
        }

        public void RemoveDrive(DriveModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            _store.Update(new List<DriveModel>(DriveList));
        }

        public bool ContainsDriveLetter(char letter)
        {
            DriveModel? d = null;
            try
            {
                d = DriveList.First((m) => (m.DriveLetter == letter));
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void EditDrive(DriveModel oldModel, DriveModel newModel)
        {
            var i = DriveList.IndexOf(oldModel);
            DriveList.RemoveAt(i);
            DriveList.Insert(i, newModel);
            _store.Update(new List<DriveModel>(DriveList));
        }


    }


}

