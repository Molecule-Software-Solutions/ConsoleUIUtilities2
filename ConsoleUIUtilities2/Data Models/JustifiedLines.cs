namespace ConsoleUIUtilities2
{
    public class JustifiedLines
    {
        private List<string> m_ListOfStrings = new List<string>();

        public static void WriteJustifiedLine(string text, int justification, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write(text, color);
        }

        public void WriteMultipleJustifiedLines(int justification, int startRow, ConsoleColor color = ConsoleColor.White)
        {
            int rowPosition = startRow; 

            foreach (string line in m_ListOfStrings)
            {
                Console.SetCursorPosition(justification, rowPosition);
                ConsoleBufferSystem.Write(line, color);
                rowPosition++;
            }
        }

        public void ClearStringList()
        {
            m_ListOfStrings.Clear();
        }

        public void AddToStringList(string text)
        {
            m_ListOfStrings.Add(text);
        }
    }
}
