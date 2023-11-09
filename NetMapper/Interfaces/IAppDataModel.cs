using System.Collections.Generic;
using NetMapper.Models;

namespace NetMapper.Interfaces
{
    internal interface IAppDataModel
    {
        List<MapModel> Models { get; set; }
    }
}