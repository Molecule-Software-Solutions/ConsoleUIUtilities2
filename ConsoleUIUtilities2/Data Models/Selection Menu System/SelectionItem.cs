namespace ConsoleUIUtilities2;

public class SelectionItem<T>
{
    /// <summary>
    /// The item stored within the <see cref="SelectionItem{T}"/> with <see cref="T"/> being any type
    /// </summary>
    public T Item { get; private set; }

    /// <summary>
    /// The caption that will be printed on the <see cref="SelectionMenu{T}"/> so that users will kow what they are selecting
    /// </summary>
    public string Caption { get; private set; }

    /// <summary>
    /// The row that the item will be placed on
    /// </summary>
    public int ItemRow { get; private set; }

    /// <summary>
    /// The column that the item will be placed on
    /// </summary>
    public int ItemColumn { get; private set; }

    /// <summary>
    /// The page that the item will be placed on
    /// </summary>
    public int ItemPage { get; private set; }

    /// <summary>
    /// Returns the length of the caption
    /// </summary>
    public int CaptionLength => Caption.Length;

    /// <summary>
    /// The color of the item
    /// </summary>
    public ConsoleColor ItemColor { get; private set; }

    /// <summary>
    /// The color of the item when the selection highlight is activated
    /// </summary>
    public ConsoleColor SelectionForeColor { get; private set; }

    /// <summary>
    /// The color of the selection highlight
    /// </summary>
    public ConsoleColor SelectionBackColor { get; private set; }

    /// <summary>
    /// Returns the underlying item that is stored within the object
    /// </summary>
    /// <returns></returns>
    public T ReturnItem()
    {
        return Item;
    }

    /// <summary>
    /// Writes the item to the console in the position calculated by the <see cref="SelectionMenu{T}"/>
    /// </summary>
    /// <param name="rowOffset"></param>
    /// <param name="columnOffset"></param>
    /// <param name="selected"></param>
    public void WriteItem(int rowOffset, int columnOffset, bool selected = false)
    {
        Console.ResetColor(); 
        // If currently selected
        if(selected)
        {
            Console.ForegroundColor = SelectionForeColor;
            Console.BackgroundColor = SelectionBackColor;
            Console.SetCursorPosition(columnOffset * ItemColumn, rowOffset + ItemRow);
            Console.Write($"  {Caption}  ");
            Console.ResetColor();
            return; 
        }
        // Else if not currently selected
        Console.ForegroundColor = ItemColor;
        Console.SetCursorPosition(columnOffset * ItemColumn, rowOffset + ItemRow);
        Console.Write($"  {Caption}  ");
        Console.ResetColor();
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="selectionItem">Item that will be stored for selection</param>
    /// <param name="caption">Caption that will appear on the <see cref="SelectionMenu{T}"/></param>
    /// <param name="itemColor">Color of the item as it will appear on the <see cref="SelectionMenu{T}"/></param>
    /// <param name="selectionForeColor">Color of the item when it is highlighted by the selector</param>
    /// <param name="selectionBackColor">Color of the selector that will highlight the item when it is selected</param>
    public SelectionItem(T selectionItem, string caption, ConsoleColor itemColor = ConsoleColor.White, ConsoleColor selectionForeColor = ConsoleColor.Black, ConsoleColor selectionBackColor = ConsoleColor.DarkCyan)
    {
        Item = selectionItem;
        Caption = caption;
        ItemColor = itemColor;
        SelectionForeColor = selectionForeColor;
        SelectionBackColor = selectionBackColor;
    }

    /// <summary>
    /// Sets the item's position in the selection menu
    /// </summary>
    /// <param name="row">Row that the item will be placed on</param>
    /// <param name="column">Column that the item will be placed on</param>
    public void SetPosition(int row, int column, int page)
    {
        ItemRow = row;
        ItemColumn = column;
        ItemPage = page;
    }

    /// <summary>
    /// Begins monitoring the console key input for the item
    /// NOTE: This will send a navigation signal for the following input:
    /// UP_ARROW => Navigate Up, 
    /// DOWN_ARROW => Navigate Down, 
    /// LEFT_ARROW => Navigate Left, 
    /// RIGHT_ARROW => Navigate Right, 
    /// ESCAPE => Cancellation, 
    /// ENTER => Selection Made
    /// </summary>
    public void MonitorItem()
    {
        ConsoleKey key = new ConsoleKey();
        key = Console.ReadKey().Key;
        switch (key)
        {
            case ConsoleKey.UpArrow:
                OnSignalSent(NavigationSignalEventTypes.NAVIGATE_UP);
                break;
            case ConsoleKey.DownArrow:
                OnSignalSent(NavigationSignalEventTypes.NAVIGATE_DOWN);
                break;
            case ConsoleKey.LeftArrow:
                OnSignalSent(NavigationSignalEventTypes.NAVIGATE_LEFT);
                break;
            case ConsoleKey.RightArrow:
                OnSignalSent(NavigationSignalEventTypes.NAVIGATE_RIGHT);
                break;
            case ConsoleKey.Enter:
                OnSignalSent(NavigationSignalEventTypes.SELECTION_MADE);
                break;
            case ConsoleKey.Escape:
                OnSignalSent(NavigationSignalEventTypes.CANCELLATION);
                break;
            case ConsoleKey.F7:
                OnSignalSent(NavigationSignalEventTypes.PREV_PAGE);
                break;
            case ConsoleKey.F8:
                OnSignalSent(NavigationSignalEventTypes.NEXT_PAGE);
                break;
            default:
                break;
        }
    }

    // Sends a signal indication what navigation event has occurred
    private void OnSignalSent(NavigationSignalEventTypes eventType)
    {
        NavigationSignalSent.Invoke(this, new NavigationSignalEventArgs(eventType));
    }

    /// <summary>
    /// Provides a navigation signal to the selection menu
    /// </summary>
    public event EventHandler<NavigationSignalEventArgs> NavigationSignalSent = (sender, e) => { };
}