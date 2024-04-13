using NetMapper.Models;
using System.Collections.Generic;

namespace NetMapper.Interfaces
{
    internal interface IAppDataModel
    {
        List<MapModel> Models { get; set; }
    }
}