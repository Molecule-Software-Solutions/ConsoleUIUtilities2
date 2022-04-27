namespace ConsoleUIUtilities2;

public class DisplayItem
{
    public string Label { get; private set; }
    public string Text { get; private set; }
    public int Justification { get; private set; }
    public int Row { get; private set; }
    public int InputValueStartPositionColumn { get; private set; }
    public int InputValueStartPositionRow { get; private set; }
    public string ValueMarker { get; private set; }
    public ConsoleColor LabelColor { get; private set; }
    public ConsoleColor ValueColor { get; private set; }

    public DisplayItem(string label,
        string text,
        string valueMarker = " >> ")
    {
        Label = label;
        Text = text;
        ValueMarker = valueMarker; 
    }


    /// <summary>
    /// Sets the position that the DisplayItem will be written to
    /// </summary>
    /// <param name="justification"></param>
    /// <param name="row"></param>
    public void SetInitialWritePosition(int justification, int row)
    {
        Justification = justification;
        Row = row; 
    }

    /// <summary>
    /// Sets the position of the values to be printed behind the label
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    public void SetValueStartPosition(int column, int row)
    {
        InputValueStartPositionColumn = column;
        InputValueStartPositionRow = row; 
    }

    /// <summary>
    /// Writes the display item to the console
    /// </summary>
    public void Write(ConsoleColor labelColor, ConsoleColor valueColor)
    {
        LabelColor = labelColor;
        ValueColor = valueColor; 
        JustifiedLines.WriteJustifiedLine(Label, Justification, Row, LabelColor);
        ConsoleBufferSystem.Write(ValueMarker, LabelColor); 
        JustifiedLines.WriteJustifiedLine(Text, InputValueStartPositionColumn, InputValueStartPositionRow, ValueColor); 
    }

}