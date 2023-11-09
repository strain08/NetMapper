using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NetMapper.Messages;

public class PropertyChangedMessage : ValueChangedMessage<string>
{
    public object PropertyValue { get; }
    public PropertyChangedMessage(string propertyName, object propertyValue) : base(propertyName)
    {
        PropertyValue = propertyValue;
    }    
}