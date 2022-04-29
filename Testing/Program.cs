using ConsoleUIUtilities2; 

namespace Testing;

public static class Program
{
    public static void Main()
    {
        Console.SetWindowSize(120, 28); 
        Console.SetBufferSize(120, 28);
        DisplayItemsTestPage page = new DisplayItemsTestPage(); 
        DisplayItemsContainer container = new DisplayItemsContainer(page);
        container.ShowPage();
    }
}
