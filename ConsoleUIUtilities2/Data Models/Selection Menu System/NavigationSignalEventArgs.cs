namespace ConsoleUIUtilities2;

public class NavigationSignalEventArgs : EventArgs
{
    public NavigationSignalEventTypes NavigationType { get; set; }

    public NavigationSignalEventArgs(NavigationSignalEventTypes navigationType)
    {
        NavigationType = navigationType;
    }
}