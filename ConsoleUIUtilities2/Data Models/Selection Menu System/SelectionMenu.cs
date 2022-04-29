namespace ConsoleUIUtilities2;

public class SelectionMenu<T> : IDisposable
{
    #region Private members 

    private int m_CurrentColumn = 0;
    private int m_CurrentRow = 0;
    private int m_CurrentPage = 0;
    private int m_CurrentSelectionRow = 0;
    private int m_CurrentSelectionColumn = 0;
    private int m_CurrentSelectionPage = 0;
    private int? m_ColumnOffset; 
    private ItemSelectionCallback? m_SelectionAction;
    private Action? m_CancellationAction;
    private Header? m_Header; 
    private bool m_BreakMenu = false;
    private bool m_PrintInstructions = false;

    #endregion

    #region Constructor

    public SelectionMenu(int startRow, int endRow, ItemSelectionCallback? selectionAction = null, Action? cancellationAction = null, bool printInstructions = false)
    {
        StartRow = startRow;
        EndRow = endRow;
        m_SelectionAction = selectionAction;
        m_CancellationAction = cancellationAction;
        m_PrintInstructions = printInstructions;
    }

    #endregion

    #region Properties 

    /// <summary>
    /// Items that are contained within the selection menu. NOTE: Set these items with <see cref="AddItem(T, string, ConsoleColor, ConsoleColor, ConsoleColor)"/>
    /// </summary>
    public List<SelectionItem<T>> Items { get; private set; } = new List<SelectionItem<T>>();

    /// <summary>
    /// The currently selected <see cref="SelectionItem{T}"/>
    /// </summary>
    public SelectionItem<T>? SelectedItem { get; private set; }

    /// <summary>
    /// Row that the list will start on
    /// </summary>
    public int StartRow { get; private set; } = 0;

    /// <summary>
    /// Row that the list will end on
    /// </summary>
    public int EndRow { get; private set; } = 0;

    /// <summary>
    /// Returns the appropraite number of columns that can be printed based on the length of the longest caption (based on console width)
    /// </summary>
    public int Columns
    {
        get
        {
            int maxWidth = Console.WindowWidth / GetMaxCaptionLength();
            if (maxWidth == 0)
            {
                return 0;
            }
            return maxWidth;
        }
    }

    /// <summary>
    /// Returns the number of items (in a column) that can be listed
    /// </summary>
    public int ItemsPerColumn
    {
        get
        {
            return (EndRow - StartRow) + 1;
        }
    }

    /// <summary>
    /// Since the row index starts at 0, this property returns the offset caused by starting the list on a lower line
    /// </summary>
    public int RowOffset
    {
        get
        {
            return StartRow;
        }
    }

    /// <summary>
    /// Similarly to <see cref="RowOffset"/>, this property determines how wide each column will be and provides that value for 
    /// further calculations during the console printing process. 
    /// </summary>
    public int ColumnOffset
    {
        get
        {
            if (m_ColumnOffset is null)
            {
                m_ColumnOffset = GetMaxCaptionLength();
            }
            return m_ColumnOffset ?? 0;
        }
    }

    /// <summary>
    /// Returns the maximum number of items that will fit on a page
    /// </summary>
    public int MaxItems
    {
        get
        {
            return Columns * ItemsPerColumn;
        }
    }

    /// <summary>
    /// Returns the necessary number of pages that are needed to contain all of the selection items
    /// </summary>
    public int Pages
    {
        get
        {
            if (Items.Count < MaxItems)
                return 1;
            if (Items.Count == MaxItems)
                return 1;
            int initial = Items.Count / MaxItems;
            int remainder = Items.Count % MaxItems;
            return remainder > 0 ? initial + 1 : initial;
        }
    }

    #endregion 

    #region Public Methods 

    /// <summary>
    /// Init will clear the console and print the first page of items (OPTIONAL): if a header was injected, that header can also be generated
    /// </summary>
    /// <param name="printHeader">Determines if the header will be printed</param>
    /// <param name="headerStartRow">Row where the header will start</param>
    /// <param name="headerLineColor">Color of the header lines (if a line char was set for the top and bottom of the header)</param>
    /// <param name="headerTextColor">Color of the header text</param>
    public void Init(bool printHeader = false, int headerStartRow = 0, ConsoleColor headerLineColor = ConsoleColor.White, ConsoleColor headerTextColor = ConsoleColor.White)
    {
        ConsoleBufferSystem.ClearBuffer();
        if(printHeader)
        {
            m_Header?.WriteHeader(headerStartRow, headerLineColor, headerTextColor);
        }
        PrintItemSelection();
    }

    /// <summary>
    /// Injects a header which can be printed by <see cref="Init(bool, int, ConsoleColor, ConsoleColor)"/>
    /// </summary>
    /// <param name="header"></param>
    public void InjectHeader(Header header)
    {
        m_Header = header;
    }

    /// <summary>
    /// Returns the currently selected item. It is preferred to return the item using the <see cref="ItemSelectionCallback"/> 
    /// however, if the developer wishes to use this method instead, a value will be returned from the <see cref="SelectionItem{T}"/>
    /// </summary>
    /// <returns></returns>
    public T? ReturnSelectedItem()
    {
        if (SelectedItem is not null)
        {
            return SelectedItem.Item;
        }
        else return default; 
    }

    /// <summary>
    /// Breaks the monitoring of further moves and commands and allows the menu to be exited. 
    /// </summary>
    public void ExternalBreak()
    {
        m_BreakMenu = true; 
    }

    /// <summary>
    /// Adds a new selection item to the menu. NOTE: This must be done before the menu is displayed or there may be unexpected behavior. 
    /// </summary>
    /// <param name="item">The item to be injected</param>
    /// <param name="caption">The caption that will appear on the menu</param>
    /// <param name="itemColor">The color of the item</param>
    /// <param name="selectionForeColor">The foreground of the item's caption when the selection highlight is behind it</param>
    /// <param name="selectionBackColor">The color of the selection highlight</param>
    public void AddItem(T item, string caption, ConsoleColor itemColor = ConsoleColor.White, ConsoleColor selectionForeColor = ConsoleColor.Black, ConsoleColor selectionBackColor = ConsoleColor.DarkCyan)
    {
        SelectionItem<T> newSelectioItem = new SelectionItem<T>(item, caption, itemColor, selectionForeColor, selectionBackColor);
        newSelectioItem.SetPosition(m_CurrentRow, m_CurrentColumn, m_CurrentPage);
        CalculateNextAvailableAddress();
        Items.Add(newSelectioItem);
    }

    /// <summary>
    /// Dispose properly unsubscribes from all monitored events and allows the menu to be collected by the garbage collector.
    /// NOTE: Failure to properly dispose of a menu may cause unexpected behaviors or memory leaks. (Especially if more than one menu is used within
    /// an application)
    /// </summary>
    public void Dispose()
    {
        foreach (var item in Items)
        {
            item.NavigationSignalSent -= SelectedItem_NavigationSignalSent;
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Subscribes to the NavigationSignalSent event for all items in the <see cref="Items"/> list
    /// </summary>
    private void ItemsSubscribe()
    {
        // Subscribe to each item's Navigation Signal Event
        foreach (var item in Items)
        {
            item.NavigationSignalSent += SelectedItem_NavigationSignalSent;
        }
    }

    /// <summary>
    /// Unsubscribes from the NavigationSignalSent event for all items in the <see cref="Items"/> list
    /// </summary>
    private void ItemsUnsubscribe()
    {
        // Unsubscribe to each item's Navigation Signal Event
        foreach (var item in Items)
        {
            item.NavigationSignalSent -= SelectedItem_NavigationSignalSent;
        }
    }

    /// <summary>
    /// Clears the rows populated by the navigation instructions
    /// </summary>
    private void ClearInstructionRows()
    {
        Console.SetCursorPosition(0, EndRow + 1);
        Console.Write("".PadRight(Console.WindowWidth, ' '));
        Console.SetCursorPosition(0, EndRow + 2);
        Console.Write("".PadRight(Console.WindowWidth, ' ')); 
    }

    /// <summary>
    /// Prints the navigation instructions one line after <see cref="EndRow"/>
    /// </summary>
    private void PrintInstructionRows()
    {
        string forwardArrow = "      "; 
        string reverseArrow = "      ";

        if(m_CurrentSelectionPage > 0)
        {
            reverseArrow = "( <- )";
        }

        if(m_CurrentSelectionPage != Pages - 1 && Pages > 1)
        {
            forwardArrow = "( -> )"; 
        }

        if(m_PrintInstructions)
        {
            CenteredLine.PrintToConsole($"{reverseArrow} USE ARROWS TO MOVE SELECTION. (F7) PAGE BACK (F8) PAGE FORWARD {forwardArrow}", EndRow + 1, ConsoleColor.Yellow);
            CenteredLine.PrintToConsole("(ENTER) ACCEPTS SELECTION          (ESC) CANCEL", EndRow + 2, ConsoleColor.Yellow); 
        }
    }

    /// <summary>
    /// Prints all items and begins monitoring the selected item for move/paging/selection/cancellations events
    /// </summary>
    private void PrintItemSelection()
    {
        // Clear the printable rows
        ClearPrintRows();
        ItemsUnsubscribe();
        ItemsSubscribe();

        // Write instructions for selection menu use
        ClearInstructionRows();
        PrintInstructionRows(); 

        List<SelectionItem<T>> items = new List<SelectionItem<T>>(Items.Where(i => i.ItemPage == m_CurrentSelectionPage)); 
        if(items.Count() > 0)
        {
            do
            {
                foreach (var item in items)
                {
                    item.WriteItem(RowOffset, ColumnOffset,
                        item.ItemRow == m_CurrentSelectionRow &&
                        item.ItemColumn == m_CurrentSelectionColumn &&
                        item.ItemPage == m_CurrentSelectionPage ? true : false);
                }
                SelectedItem = Items.Where(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage).FirstOrDefault();
                if (SelectedItem is not null)
                {
                    SelectedItem.MonitorItem();
                }
            } while (!m_BreakMenu); 
        }
    }

    /// <summary>
    /// Returns the length of the longest caption for calculation of the column widths
    /// </summary>
    /// <returns>Integer representing the max length of the captions in the <see cref="Items"/> list</returns>
    private int GetMaxCaptionLength()
    {
        int len = 0;
        foreach (SelectionItem<T> item in Items)
        {
            if (item.CaptionLength > len)
            {
                len = item.CaptionLength;
            }
        }
        return len + 4;  // Add 4 to give spacing around caption
    }

    /// <summary>
    /// Returns the next available "slot" where an item can be displayed on the menu
    /// </summary>
    private void CalculateNextAvailableAddress()
    {
        if (m_CurrentRow + 1 > ItemsPerColumn - 1)
        {
            if (m_CurrentColumn + 1 > Columns - 1)
            {
                m_CurrentRow = 0;
                m_CurrentColumn = 0;
                m_CurrentPage += 1;
                return;
            }
            m_CurrentRow = 0;
            m_CurrentColumn += 1;
            return;
        }
        m_CurrentRow += 1;
        return;
    }

    /// <summary>
    /// Clears the menu items only
    /// </summary>
    private void ClearPrintRows()
    {
        for (int i = RowOffset - 1; i < EndRow + 1; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
        }
    }

    /// <summary>
    /// Called when the NavigationSignalSent event is fired from a <see cref="SelectionItem{T}"/>
    /// NOTE: This item will be the currently selected item
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectedItem_NavigationSignalSent(object? sender, NavigationSignalEventArgs e)
    {
        var nav = e.NavigationType;
        switch (nav)
        {
            case NavigationSignalEventTypes.NAVIGATE_LEFT:
                Move(SelectionDirection.LEFT);
                break;
            case NavigationSignalEventTypes.NAVIGATE_RIGHT:
                Move(SelectionDirection.RIGHT);
                break;
            case NavigationSignalEventTypes.NAVIGATE_UP:
                Move(SelectionDirection.UP);
                break;
            case NavigationSignalEventTypes.NAVIGATE_DOWN:
                Move(SelectionDirection.DOWN);
                break;
            case NavigationSignalEventTypes.NEXT_PAGE:
                Move(SelectionDirection.PAGE_FORWARD);
                break;
            case NavigationSignalEventTypes.PREV_PAGE:
                Move(SelectionDirection.PAGE_BACK);
                break;
            case NavigationSignalEventTypes.SELECTION_MADE:
                if (m_SelectionAction is not null)
                {
                    if(SelectedItem is not null)
                    {
                        m_SelectionAction(SelectedItem.Item);
                        break; 
                    }
                    else
                    {
                        Dialog dlg = new Dialog("EXCEPTION", "THE SELECTED ITEM WAS NULL");
                        dlg.Show('*', ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Red);
                    }
                }
                break;
            case NavigationSignalEventTypes.CANCELLATION:
                if (m_CancellationAction is not null)
                {
                    m_CancellationAction();
                    m_BreakMenu = true;
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// This method accepts a move instruction and executes it
    /// </summary>
    /// <param name="direction"></param>
    private void Move(SelectionDirection direction)
    {
        switch (direction)
        {
            case SelectionDirection.DOWN:
                {
                    if (m_CurrentSelectionRow + 1 > ItemsPerColumn - 1)
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE DOWN", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    var testSelection = Items.Where(i => i.ItemRow == m_CurrentSelectionRow + 1 && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage).FirstOrDefault(); 
                    if(testSelection is null)
                    {
                        NotificationLine.WriteNotificationLine("NO ITEM THIS DIRECTION", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    NotificationLine.ClearNotificationLine(27);
                    m_CurrentSelectionRow += 1;
                    if(SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset);
                    }
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            case SelectionDirection.UP:
                {
                    if (m_CurrentSelectionRow - 1 < 0)
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE UP", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    NotificationLine.ClearNotificationLine();
                    m_CurrentSelectionRow -= 1;
                    if(SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset); 
                    }
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            case SelectionDirection.LEFT:
                {
                    if (m_CurrentSelectionColumn - 1 < 0)
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE LEFT", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    NotificationLine.ClearNotificationLine();
                    m_CurrentSelectionColumn -= 1;
                    if(SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset);
                    }
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            case SelectionDirection.RIGHT:
                {
                    if (m_CurrentSelectionColumn + 1 > Columns - 1)
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE RIGHT", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    var testSelection = Items.Where(i => i.ItemColumn == m_CurrentSelectionColumn + 1 && i.ItemRow == m_CurrentSelectionRow && i.ItemPage == m_CurrentSelectionPage).FirstOrDefault(); 
                    if(testSelection is null)
                    {
                        NotificationLine.WriteNotificationLine("NO ITEM THIS DIRECTION", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        break;
                    }
                    NotificationLine.ClearNotificationLine();
                    m_CurrentSelectionColumn += 1;
                    if(SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset);
                    }
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            case SelectionDirection.PAGE_FORWARD:
                {
                    if (m_CurrentSelectionPage + 1 > Pages - 1)
                    {
                        NotificationLine.WriteNotificationLine("NO MORE PAGES", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        return;
                    }
                    NotificationLine.ClearNotificationLine();
                    m_CurrentSelectionPage += 1;
                    m_CurrentSelectionColumn = 0;
                    m_CurrentSelectionRow = 0;
                    ClearPrintRows();
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    PrintItemSelection(); 
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            case SelectionDirection.PAGE_BACK:
                {
                    if (m_CurrentSelectionPage - 1 < 0)
                    {
                        NotificationLine.WriteNotificationLine("BEGINNING OF LIST", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        return;
                    }
                    NotificationLine.ClearNotificationLine();
                    m_CurrentSelectionPage -= 1;
                    m_CurrentSelectionColumn = 0;
                    m_CurrentSelectionRow = 0;
                    ClearPrintRows();
                    SelectedItem = Items.FirstOrDefault(i => i.ItemRow == m_CurrentSelectionRow && i.ItemColumn == m_CurrentSelectionColumn && i.ItemPage == m_CurrentSelectionPage);
                    PrintItemSelection();
                    if (SelectedItem is not null)
                    {
                        SelectedItem.WriteItem(RowOffset, ColumnOffset, true);
                    }
                    break;
                }
            default:
                break;
        }
    }

    #endregion

    #region Delegates

    /// <summary>
    /// Delegate method signature that allows for the return of the <see cref="T"/> item that is currently selected. 
    /// This is automatically called when a selection is made. If the developer chooses not to use this method, they may
    /// obtain the selected item with the <see cref="ReturnSelectedItem"/> method
    /// </summary>
    /// <param name="item"></param>
    public delegate void ItemSelectionCallback(T item);

    #endregion 
}