using CommunityToolkit.Mvvm.Messaging.Messages;
using NetMapper.Models;

namespace NetMapper.Messages;
public record PropChangedMessage(MapModel m, string PropertyName);