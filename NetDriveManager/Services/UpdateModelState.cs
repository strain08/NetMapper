using NetMapper.Enums;
using NetMapper.Models;
using NetMapper.Services.Helpers;
using NetMapper.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMapper.Services
{
    public class UpdateModelState : IUpdateModelState
    {

        public void Update(MapModel m)
        {
            UpdateShareState(m);
            UpdateMappingState(m);
        }

        private void UpdateShareState(MapModel m)
        {
            m.ShareStateProp = Directory.Exists(m.NetworkPath) ?
                ShareState.Available : ShareState.Unavailable;
        }

        private void UpdateMappingState(MapModel m)
        {
            // if it is a network drive mapped to this path -> Mapped            
            if (Interop.GetActualPathForLetter(m.DriveLetter) == m.NetworkPath)
            {
                m.VolumeLabel = GetVolumeLabel(m);
                m.MappingStateProp = MappingState.Mapped;
                return;
            }
            // if letter available -> unmapped
            if (Interop.GetAvailableDriveLetters().Contains(m.DriveLetter))
            {
                m.MappingStateProp = MappingState.Unmapped;
            }
            else // ->unavailable
            {
                m.VolumeLabel = GetVolumeLabel(m);
                m.MappingStateProp = MappingState.LetterUnavailable;
            }
        }

        private string GetVolumeLabel(MapModel m)
        {
            DriveInfo drive = new(m.DriveLetterColon);
            return drive.IsReady ? drive.VolumeLabel : string.Empty;
        }
    }
}
