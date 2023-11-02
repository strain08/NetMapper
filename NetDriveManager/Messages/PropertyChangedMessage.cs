using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NetMapper.Messages;

public class PropertyChangedMessage : ValueChangedMessage<string>
{
    public object PropertyValue => propertyValue;
    readonly object propertyValue;
    public PropertyChangedMessage(string propertyName, object propertyValue) : base(propertyName) 
    {  
        this.propertyValue = propertyValue;
    }
    
}

