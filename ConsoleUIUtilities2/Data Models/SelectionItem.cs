namespace ConsoleUIUtilities2
{
    public class SelectionItem<T>
    {
        public string? Identifier { get; private set; }
        public string? DisplayText { get; private set; }
        public bool IsSelected { get; private set; }
        public T? Value { get; private set; }
        public uint BeginWriteTop { get; private set; }
        public uint BeginWriteLeft { get; private set; }
        public uint EndWriteTop { get; private set; }
        public uint EndWriteLeft { get; private set; }
        public int  SelectionItemIndex { get; private set; }
        public bool Finalized { get; private set; }

        public SelectionItem(string displayText, T value)
        {
            DisplayText = displayText;
            Value = value;
        }

        public void Popped()
        {
            SelectionItemIndex -= 1; 
        }

        public void Pushed()
        {
            SelectionItemIndex += 1; 
        }

        public void SetFinalizingIndex(int index)
        {
            if(!Finalized)
            {
                SelectionItemIndex = index;
                Finalized = true; 
            }
        }

        public void SetBeginWrite(uint left, uint top)
        {
            BeginWriteLeft = left; 
            BeginWriteTop = top;
        }

        public void SetEndWrite(uint left, uint top)
        {
            EndWriteLeft = left;
            EndWriteTop = top;
        }

        public T? GetValue()
        {
            return Value; 
        }

        public void ChangeSelection(bool selection)
        {
            IsSelected = selection; 
        }
    }
}
