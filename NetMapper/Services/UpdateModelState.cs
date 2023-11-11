using System;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using NetMapper.Enums;
using NetMapper.Interfaces;
using NetMapper.Messages;
using NetMapper.Models;
using Splat;

namespace NetMapper.Services;

public class UpdateModelState : IUpdateModelState, IRecipient<PropChangedMessage>
{    
    private readonly IInterop interop;    
    public UpdateModelState(IInterop interop)
    {
        this.interop = interop;
        WeakReferenceMessenger.Default.Register(this);
    }

    public void Update(MapModel m)
    {
        UpdateShareState(m);
        UpdateMappingState(m);
    }

    /// <summary>
    ///     Updates model's ShareStateProp to System State
    /// </summary>
    /// <param name="m"></param>
    private void UpdateShareState(MapModel m)
    {
        m.ShareStateProp = Directory.Exists(m.NetworkPath) ? ShareState.Available : ShareState.Unavailable;
    }

    /// <summary>
    ///     Updates model's MappingStateProp to System State
    /// </summary>
    /// <param name="m"></param>
    private void UpdateMappingState(MapModel m)
    {
        
        // if a Network Drive is mapped to this path -> Mapped            
        if (interop.GetActualPathForLetter(m.DriveLetter) == m.NetworkPath)
        {
            m.MappingStateProp = MappingState.Mapped;
            return;
        }

        // if Letter Available -> drive is Unmapped
        if (interop.GetAvailableDriveLetters().Contains(m.DriveLetter))
            m.MappingStateProp = MappingState.Unmapped;
        else // Drive letter not available, is mapped to something else
            m.MappingStateProp = MappingState.LetterUnavailable;
    }

    public void Receive(PropChangedMessage message)
    {
        var s = nameof(MapModel.MappingStateProp);       

        if (message.PropertyName == s)
        {
            if ((message.Value.MappingStateProp == MappingState.Mapped) |
            (message.Value.MappingStateProp == MappingState.LetterUnavailable))
            {
                var interop = Locator.Current.GetService<IInterop>();
                message.Value.VolumeLabel = interop?.GetVolumeLabel(message.Value) ?? "volume label";
            }
            if (message.Value.MappingStateProp == MappingState.Unmapped) message.Value.VolumeLabel = "";
        }
    }
}