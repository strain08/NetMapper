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
        /// <summary>
        /// Updates model's ShareStateProp to System State
        /// </summary>
        /// <param name="m"></param>
        private void UpdateShareState(MapModel m)
        {
            m.ShareStateProp = Directory.Exists(m.NetworkPath) ?
                ShareState.Available : ShareState.Unavailable;
        }
        /// <summary>
        /// Updates model's MappingStateProp to System State
        /// </summary>
        /// <param name="m"></param>
        private void UpdateMappingState(MapModel m)
        {
            // if a Network Drive is mapped to this path -> Mapped            
            if (Interop.GetActualPathForLetter(m.DriveLetter) == m.NetworkPath)
            {   
                m.MappingStateProp =  MappingState.Mapped;                
                return;
            }
            // if Letter Available -> drive is Unmapped
            if (Interop.GetAvailableDriveLetters().Contains(m.DriveLetter))
            {               
                m.MappingStateProp = MappingState.Unmapped;
            }
            else // Drive letter not available, is mapped to something else
            {
                m.MappingStateProp = MappingState.LetterUnavailable;
            }
        }

        
    }
}
