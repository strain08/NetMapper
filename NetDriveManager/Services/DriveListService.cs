using NetMapper.Interfaces;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetMapper.Services
{

    public class DriveListService
    {

        public ObservableCollection<MappingModel> DriveList { get; set; }

        private readonly IStore<List<MappingModel>> store;

        // CTOR
        public DriveListService(IStore<List<MappingModel>> storeService)
        {
            store = storeService;
            DriveList = new ObservableCollection<MappingModel>(store.GetAll());

        }
        // DTOR
        ~DriveListService()
        {
            if (!store.Update(new List<MappingModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void AddDrive(MappingModel model)
        {
            DriveList.Add(model);
            if (!store.Update(new List<MappingModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void RemoveDrive(MappingModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            if (!store.Update(new List<MappingModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
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
            if (!store.Update(new List<MappingModel>(DriveList))) 
                throw new ApplicationException("Can not write settings file.");
        }


    }


}

