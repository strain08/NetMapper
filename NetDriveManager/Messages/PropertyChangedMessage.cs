using CommunityToolkit.Mvvm.Messaging.Messages;

namespace NetMapper.Messages;

public class PropertyChangedMessage : ValueChangedMessage<string>
{
    public PropertyChangedMessage(string value) : base(value) {  }
}

