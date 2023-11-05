using System.Text.Json.Serialization;
using Avalonia;

namespace NetMapper.Models;

public class AppSettingsModel
{
    [JsonIgnore] public bool WindowIsOpened = false;

    public bool SetLoadAtStartup { get; set; }
    public bool SetRemoveUnmanaged { get; set; }
    public bool SetMinimizeToTaskbar { get; set; }
    public bool SetInfoNotify { get; set; }

    public double WindowHeight { get; set; } = 400;
    public double WindowWidth { get; set; } = 225;
    public int WinX { get; set; }
    public int WinY { get; set; }

    [JsonIgnore]
    public PixelPoint WindowPosition
    {
        get => new PixelPoint(WinX, WinY);
        set
        {
            WinX = value.X;
            WinY = value.Y;
        }
    }

    public bool PositionOK()
    {
        if (WinY > 0 && WinY > 0) return true;
        return false;
    }

    public AppSettingsModel Clone()
    {
        return (AppSettingsModel)MemberwiseClone();
    }
}