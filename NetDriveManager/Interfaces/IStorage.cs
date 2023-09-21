using NetMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Interfaces
{
    public interface IStorage
    {
        public List<MappingModel> GetAll();
        public bool Load();
        public bool Save();
        public bool Update(List<MappingModel> updatedList);
        
    }
}
