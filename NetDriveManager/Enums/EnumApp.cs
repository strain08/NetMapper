namespace NetMapper.Enums
{
    public enum ShareState
    {
        Undefined,        
        Available,
        Unavailable 
    }
    public enum MappingState
    {
        Undefined,
        Mapped,
        Unmapped,
        LetterUnavailable
    }

    public enum DisconnectDriveAnswer
    {
        Force,
        Retry,
        ShowWindow
    }
    public enum AddRemoveAnswer
    {
        ShowWindow
    }
}
