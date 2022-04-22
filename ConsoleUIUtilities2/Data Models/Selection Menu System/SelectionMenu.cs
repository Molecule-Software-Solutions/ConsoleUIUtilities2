namespace ConsoleUIUtilities2;

public class SelectionMenu<T> : IDisposable
{
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

    public SelectionMenu(int startRow, int endRow, ItemSelectionCallback? selectionAction = null, Action? cancellationAction = null, bool printInstructions = false)
    {
        StartRow = startRow;
        EndRow = endRow;
        m_SelectionAction = selectionAction;
        m_CancellationAction = cancellationAction;
        m_PrintInstructions = printInstructions;
    }

    public void InjectHeader(Header header)
    {
        m_Header = header;
    }

    public void Init(bool printHeader = false, int headerStartRow = 0, ConsoleColor headerLineColor = ConsoleColor.White, ConsoleColor headerTextColor = ConsoleColor.White)
    {
        ConsoleBufferSystem.ClearBuffer();
        if(printHeader)
        {
            m_Header?.WriteHeader(headerStartRow, headerLineColor, headerTextColor);
        }
        PrintItemSelection();
    }

    public List<SelectionItem<T>> Items { get; set; } = new List<SelectionItem<T>>();
    public SelectionItem<T>? SelectedItem { get; private set; }

    /// <summary>
    /// Row that the list will start on
    /// </summary>
    public int StartRow { get; private set; } = 0;

    /// <summary>
    /// Row that the list will end on
    /// </summary>
    public int EndRow { get; private set; } = 0;

    public int Columns
    {
        get
        {
            int maxWidth = Console.WindowWidth / GetMaxCaptionLength();
            if(maxWidth == 0)
            {
                return 0;
            }
            return maxWidth; 
        }
    }

    public int ItemsPerColumn
    {
        get
        {
            return (EndRow - StartRow) + 1;
        }
    }

    public int RowOffset
    {
        get
        {
            return StartRow;
        }
    }

    public int ColumnOffset
    {
        get
        {
            if(m_ColumnOffset is null)
            {
                m_ColumnOffset = GetMaxCaptionLength(); 
            }
            return m_ColumnOffset ?? 0; 
        }
    }

    public int MaxItems
    {
        get
        {
            return Columns * ItemsPerColumn;
        }
    }

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

    public T? ReturnSelectedItem()
    {
        if (SelectedItem is not null)
        {
            return SelectedItem.Item;
        }
        else return default; 
    }

    public void ExternalBreak()
    {
        m_BreakMenu = true; 
    }

    private void ItemsSubscribe()
    {
        // Subscribe to each item's Navigation Signal Event
        foreach (var item in Items)
        {
            item.NavigationSignalSent += SelectedItem_NavigationSignalSent;
        }
    }

    private void ItemsUnsubscribe()
    {
        // Unsubscribe to each item's Navigation Signal Event
        foreach (var item in Items)
        {
            item.NavigationSignalSent -= SelectedItem_NavigationSignalSent;
        }
    }

    private void ClearInstructionRows()
    {
        Console.SetCursorPosition(0, EndRow + 1);
        Console.Write("".PadRight(Console.WindowWidth, ' '));
        Console.SetCursorPosition(0, EndRow + 2);
        Console.Write("".PadRight(Console.WindowWidth, ' ')); 
    }

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

    public void AddItem(T item, string caption, ConsoleColor itemColor = ConsoleColor.White, ConsoleColor selectionForeColor = ConsoleColor.Black, ConsoleColor selectionBackColor = ConsoleColor.DarkCyan)
    {
        SelectionItem<T> newSelectioItem = new SelectionItem<T>(item, caption, itemColor, selectionForeColor, selectionBackColor);
        newSelectioItem.SetPosition(m_CurrentRow, m_CurrentColumn, m_CurrentPage);
        CalculateNextAvailableAddress();
        Items.Add(newSelectioItem);
    }

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

    private void ClearPrintRows()
    {
        for (int i = RowOffset - 1; i < EndRow + 1; i++)
        {
            Console.SetCursorPosition(0, i);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
        }
    }

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
                    NotificationLine.ClearNotificationLine();
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

    public void Dispose()
    {
        foreach(var item in Items)
        {
            item.NavigationSignalSent -= SelectedItem_NavigationSignalSent; 
        }
    }

    public delegate void ItemSelectionCallback(T item); 
}