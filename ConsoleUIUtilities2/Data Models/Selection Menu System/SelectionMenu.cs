namespace ConsoleUIUtilities2;

public class SelectionMenu<T>
{
    private int m_CurrentColumn = 0;
    private int m_CurrentRow = 0;
    private int m_CurrentPage = 0;
    private int m_CurrentSelectionRow = 0;
    private int m_CurrentSelectionColumn = 0;
    private int m_CurrentSelectionPage = 0;
    private int? m_ColumnOffset; 
    private Action? m_SelectionAction;
    private Action? m_CancellationAction;
    private bool m_BreakMenu = false; 
    public SelectionMenu(int startRow, int endRow, Action? selectionAction = null, Action? cancellationAction = null)
    {
        StartRow = startRow;
        EndRow = endRow;
        m_SelectionAction = selectionAction;
        m_CancellationAction = cancellationAction;
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

    public void ExternalBreak()
    {
        m_BreakMenu = true; 
    }

    public void PrintItemSelection()
    {
        // Clear the printable rows
        ClearPrintRows();

        // Subscribe to each item's Navigation Signal Event
        foreach (var item in Items)
        {
            item.NavigationSignalSent += SelectedItem_NavigationSignalSent; 
        }

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

    public void AddItem(T item, string caption, ConsoleColor itemColor = ConsoleColor.White, ConsoleColor selectionColor = ConsoleColor.DarkCyan)
    {
        SelectionItem<T> newSelectioItem = new SelectionItem<T>(item, caption, itemColor, selectionColor);
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
        for (int i = StartRow; i < EndRow; i++)
        {
            Console.SetCursorPosition(0, StartRow);
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
                    m_SelectionAction();
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
                    if (m_CurrentSelectionRow + 1 > (RowOffset + ItemsPerColumn))
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE DOWN", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        return;
                    }
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
                    if (m_CurrentSelectionRow - 1 > (RowOffset + ItemsPerColumn))
                    {
                        NotificationLine.WriteNotificationLine("YOU CANNOT MOVE UP", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        return;
                    }
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
                        return;
                    }
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
                        return;
                    }
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
                    if (m_CurrentSelectionPage + 1 > Pages)
                    {
                        NotificationLine.WriteNotificationLine("NO MORE PAGES", ConsoleColor.Red, ConsoleColor.Red, ConsoleColor.Red, "INVALID MOVE");
                        return;
                    }
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
                return;
        }
    }
}