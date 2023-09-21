using NetMapper.Interfaces;
using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NetMapper.Services
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
            if (!_store.Update(new List<MappingModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void AddDrive(MappingModel model)
        {
            DriveList.Add(model);
            if (!_store.Update(new List<MappingModel>(DriveList)))
                throw new ApplicationException("Can not write settings file.");
        }

        public void RemoveDrive(MappingModel model)
        {
            var i = DriveList.IndexOf(model);
            DriveList.RemoveAt(i);
            if (!_store.Update(new List<MappingModel>(DriveList)))
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
            if (!_store.Update(new List<MappingModel>(DriveList))) 
                throw new ApplicationException("Can not write settings file.");
        }


    }


}

