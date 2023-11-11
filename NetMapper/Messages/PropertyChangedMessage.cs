using CommunityToolkit.Mvvm.Messaging.Messages;
using NetMapper.Models;

namespace NetMapper.Messages;


public class PropChangedMessage : ValueChangedMessage<MapModel>
{
    public string PropertyName { get; private set; }
    public PropChangedMessage(MapModel value, string propertyName) : base(value)
    {
        PropertyName = propertyName;
    }
}
