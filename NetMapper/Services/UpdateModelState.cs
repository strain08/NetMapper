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
            m.MappingStateProp = MapState.Mapped;
            return;
        }

        // if Letter Available -> drive is Unmapped
        if (interop.GetAvailableDriveLetters().Contains(m.DriveLetter))
            m.MappingStateProp = MapState.Unmapped;
        else // Drive letter not available, is mapped to something else
            m.MappingStateProp = MapState.LetterUnavailable;
    }

    public void Receive(PropChangedMessage message)
    {
        string s = nameof(MapModel.MappingStateProp);       
        
        if (message.PropertyName == s)
        {
            if (message.m.MappingStateProp == MapState.Mapped ||
            message.m.MappingStateProp == MapState.LetterUnavailable)
            {
                message.m.VolumeLabel = interop.GetVolumeLabel(message.m) ?? "volume label";
            }
            if (message.m.MappingStateProp == MapState.Unmapped) message.m.VolumeLabel = "";
        }
    }
}