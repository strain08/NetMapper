using NetMapper.Interfaces;
using System.Collections.Generic;

namespace NetMapper.Models
{
    public class AppDataModel : IAppDataModel
    {
        public List<MapModel> Models { get; set; } = new();
    }
}
