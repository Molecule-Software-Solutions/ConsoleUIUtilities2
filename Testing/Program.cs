namespace Testing;

public static class Program
{
    public static void Main()
    {
        DisplayItemsTestPage page = new DisplayItemsTestPage(); 
        DisplayItemsContainer container = new DisplayItemsContainer(page);
        container.ShowPage();
    }
}
