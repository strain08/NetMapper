using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NetMapper.Messages;

public class PropertyChangedMessage : ValueChangedMessage<string>
{
    public PropertyChangedMessage(string propertyName, object propertyValue) : base(propertyName)
    {
        this.PropertyValue = propertyValue;
    }

    public object PropertyValue { get; }
}