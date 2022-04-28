using ConsoleUIUtilities2; 

namespace Testing;

public class DisplayItemsContainer : PageContainer<DisplayItemsTestPage>
{
    public DisplayItemsContainer(DisplayItemsTestPage page) : base(page) {}

    public override void ShowPage()
    {
        Page.ShowAndLoadDisplayItems(headerLineColor: ConsoleColor.Cyan, headerTextColor: ConsoleColor.Yellow, holdPageInLoop: true); 
    }
}