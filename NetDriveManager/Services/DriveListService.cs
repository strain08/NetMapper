using NetDriveManager.Interfaces;
using NetDriveManager.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetDriveManager.Services
{

    public class DriveListService : IListManager
    {

        public ObservableCollection<MappingModel> DriveList { get; set; } = new();

        private readonly IStorage _store;


        // CTOR
        public DriveListService(IStorage store)
        {
            _store = store;
            DriveList = new ObservableCollection<MappingModel>(_store.GetAll());

        }
        ~DriveListService()
        {
            _store.Update(new List<MappingModel>(DriveList));
        }

        public void AddDrive(MappingModel model)
        {
            DriveList.Add(model);
            _store.Update(new List<MappingModel>(DriveList));
        }

        public void RemoveDrive(MappingModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            _store.Update(new List<MappingModel>(DriveList));
        }

        public bool ContainsDriveLetter(char letter)
        {
            foreach (MappingModel d in DriveList)
            {
                if (d.DriveLetter.Equals(letter)) return true;
            }
            return false;
        }

        public void EditDrive(MappingModel oldModel, MappingModel newModel)
        {
            var i = DriveList.IndexOf(oldModel);
            DriveList.RemoveAt(i);
            DriveList.Insert(i, newModel);
            _store.Update(new List<MappingModel>(DriveList));
        }


    }


}

