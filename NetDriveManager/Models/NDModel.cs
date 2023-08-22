using Avalonia.Controls.Chrome;
using CommunityToolkit.Mvvm.ComponentModel;

namespace NetDriveManager.Models
{
    public partial class NDModel : ObservableObject
    {
        public NDModel(NDModel copyModel)
        {
            Provider = copyModel.Provider;
            DriveLetter = copyModel.DriveLetter;
        }
        public NDModel() { }

        public string Provider { get; set; }
        //[ObservableProperty]
        //string? provider;

        [ObservableProperty]
        string? driveLetter;

        //[ObservableProperty]
        //string? name;

        //[ObservableProperty]
        //string? caption;



        //[ObservableProperty]
        //string? volumeName;



        //[ObservableProperty]
        //string? deviceID;

        //[ObservableProperty]
        //string? fileSystem;

        //[ObservableProperty]
        //string? freeSpace;

        //[ObservableProperty]
        //string? size;

        //[ObservableProperty]
        //string? volumeSerialNumber;

        private string? _hostName;        

    }
}
