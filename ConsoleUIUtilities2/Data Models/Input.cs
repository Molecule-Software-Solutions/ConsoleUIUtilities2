namespace ConsoleUIUtilities2
{
    public class Input
    {
        public string IdentifierID { get; set; } = string.Empty;
        public string InputLabel { get; private set; } = string.Empty;
        public string InputValue { get; private set; } = string.Empty;
        public int InputValueStartPositionRow { get; private set; } = 0; 
        public int InputValueStartPositionColumn { get; private set; } = 0;

        /// <summary>
        /// Input will be added to InputFieldSet and represents an input structure
        /// </summary>
        /// <param name="identifierID">ID that represents the input field during queries</param>
        public Input(string identifierID)
        {
            IdentifierID = identifierID;
            InputLabel = IdentifierID.ToUpper();
        }

        /// <summary>
        /// Sets the input label
        /// </summary>
        /// <param name="label"></param>
        public void SetInputLabel(string label)
        {
            InputLabel = label;
        }

        /// <summary>
        /// Sets the input value
        /// </summary>
        /// <param name="value"></param>
        public void SetInputValue(string value)
        {
            InputValue = value; 
        }

        /// <summary>
        /// Sets the value input position. This is the area where the cursor will fall to represent entry into a field
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        public void SetValueInputPosition(int left, int top)
        {
            InputValueStartPositionColumn = left;
            InputValueStartPositionRow = top; 
        }
    }
}
