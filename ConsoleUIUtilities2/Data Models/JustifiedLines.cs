namespace ConsoleUIUtilities2
{
    public class JustifiedLines
    {
        private List<string> m_ListOfStrings = new List<string>();

        public static void WriteJustifiedLine(string text, int justification, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        public void WriteMultipleJustifiedLines(int justification, int startRow, ConsoleColor color = ConsoleColor.White)
        {
            int rowPosition = startRow; 

            foreach (string line in m_ListOfStrings)
            {
                Console.SetCursorPosition(justification, rowPosition);
                Console.ForegroundColor = color;
                Console.Write(line);
                Console.ResetColor(); 
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
