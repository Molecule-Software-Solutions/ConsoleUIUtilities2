namespace ConsoleUIUtilities2; 

public class DisplayItemSet
{
    private List<DisplayItem> m_DisplayItems = new List<DisplayItem>();
    private int m_FinalValueStartPosColumn = 0; 
    public int Justification { get; private set; }
    public int StartRow { get; private set; }
    public ConsoleColor LabelColor { get; private set; }
    public ConsoleColor ValueColor { get; private set; }

    public DisplayItemSet(int justification, int startRow, ConsoleColor labelColor = ConsoleColor.White, ConsoleColor valueColor = ConsoleColor.White)
    {
        Justification = justification;
        StartRow = startRow;
        LabelColor = labelColor;
        ValueColor = valueColor; 
    }

    public void AddDisplayItem(DisplayItem item)
    {
        m_DisplayItems.Add(item);
        m_FinalValueStartPosColumn = CalculateValueJustification();
    }

    private int CalculateValueJustification()
    {
        int justification = 0; 
        foreach (DisplayItem item in m_DisplayItems)
        {
            if((item.Label.Length + 5 + Justification) > justification)
            {
                justification = item.Label.Length + 5 + Justification; 
            }
        }
        return justification; 
    }

    public void WriteAllDisplayItems()
    {
        int rowCounter = StartRow; 
        foreach (DisplayItem item in m_DisplayItems)
        {
            item.SetInitialWritePosition(Justification, rowCounter);
            item.SetValueStartPosition(m_FinalValueStartPosColumn, rowCounter); 
            item.Write(LabelColor, ValueColor);
            rowCounter += 1; 
        }
    }
}