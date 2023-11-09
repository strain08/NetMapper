using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMapper.Interfaces;

namespace NetMapper.Models
{
    public class AppDataModel : IAppDataModel
    {
        public List<MapModel> Models { get; set; } = new();
    }
}
