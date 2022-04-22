namespace ConsoleUIUtilities2;

public class NavigationSignalEventArgs : EventArgs
{
    /// <summary>
    /// The navigation event that will be passed as an argument to the consumer
    /// </summary>
    public NavigationSignalEventTypes NavigationType { get; set; }

    public NavigationSignalEventArgs(NavigationSignalEventTypes navigationType)
    {
        NavigationType = navigationType;
    }
}