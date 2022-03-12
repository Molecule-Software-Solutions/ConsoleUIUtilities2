namespace ConsoleUIUtilities2
{
    public class Input
    {
        public string IdentifierID { get; set; } = string.Empty;
        public string InputLabel { get; private set; } = string.Empty;
        public string InputValue { get; private set; } = string.Empty;
        public int InputValueStartPositionRow { get; private set; } = 0; 
        public int InputValueStartPositionColumn { get; private set; } = 0;

        public Input(string identifierID)
        {
            IdentifierID = identifierID;
        }

        public void SetInputLabel(string label)
        {
            InputLabel = label;
        }

        public void SetInputValue(string value)
        {
            InputValue = value; 
        }

        public void SetValueInputPosition(int left, int top)
        {
            InputValueStartPositionColumn = left;
            InputValueStartPositionRow = top; 
        }
    }
}
