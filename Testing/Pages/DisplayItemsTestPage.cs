using ConsoleUIUtilities2;

namespace Testing; 

public class DisplayItemsTestPage : Page
{
    protected override void BuildComponent()
    {
        // Create a header
        Header hdr = new Header();
        hdr.AddHeaderLine("TESTING APPLICATION - DISPLAY ITEMS");
        hdr.SetTopAndBottomLineChars('=');
        SetHeader(hdr); 

        DisplayItemSet dis = new DisplayItemSet(15, 6, labelColor: ConsoleColor.Yellow, valueColor: ConsoleColor.Cyan);
        dis.AddDisplayItem(new DisplayItem("Test Label 1", "Value"));
        dis.AddDisplayItem(new DisplayItem("Test Label a bit longer 2", "Second Value"));

        SetDisplayItemSet(dis); 
    }

    public override void ShowPostInitItems()
    {
        // Demonstrates how to break a display item page with no other controls
        CenteredLine.PrintToConsole("TO BREAK PRESS ENTER", 15, ConsoleColor.Blue);
        if(Console.ReadKey().Key == ConsoleKey.Enter)
        {
            BreakDisplayItemLoopExternal(); 
        }
    }

}