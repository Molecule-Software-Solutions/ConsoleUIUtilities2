using ConsoleUIUtilities2; 


namespace Testing;

public static class Program
{
    private static SelectionMenu<int> menu = new SelectionMenu<int>(5, 24, (selection) =>
    {
        NotificationLine.WriteNotificationLine($"Congratulations {selection} was selected!", ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.White, "HURRAH!");
    }, CloseAndDispose, printInstructions: true);

    public static void Main()
    {
        Header testHeader = new Header();
        testHeader.AddHeaderLine("2022 Test Application");
        testHeader.AddHeaderLine("Molecule Software Systems");
        testHeader.SetTopAndBottomLineChars('*'); 
        Console.ReadLine();

        List<int> testList = new List<int>(); 
        for (int i = 0; i < 152; i++)
        {
            testList.Add(i); 
        }


        foreach (int item in testList)
        {
            menu.AddItem(item, $"The quick brown fox jumped over the lazy dog {item} times");
        }

        menu.InjectHeader(testHeader);
        menu.Init(true, 0, ConsoleColor.Cyan, ConsoleColor.Yellow); 
    }

    private static void CloseAndDispose()
    {
        menu.ExternalBreak();
        menu.Dispose();

        ConsoleBufferSystem.ClearBuffer();
        CenteredLine.PrintToConsole("CLOSED", 10, ConsoleColor.Yellow);
    }
}
