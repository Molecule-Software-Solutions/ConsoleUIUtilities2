namespace ConsoleUIUtilities2
{
    public class JustifiedLines
    {
        private List<string> m_ListOfStrings = new List<string>();

        /// <summary>
        /// Writes a line of text that is justified fromt he left side of the console by a specified amount
        /// </summary>
        /// <param name="text"></param>
        /// <param name="justification"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        public static void WriteJustifiedLine(string text, int justification, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write(text, color);
        }

        /// <summary>
        /// Writes multiple lines to the console that are justified from the left:
        /// NOTE: You must use the AddToStringList(string x) method. 
        /// </summary>
        /// <param name="justification"></param>
        /// <param name="startRow"></param>
        /// <param name="color"></param>
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

        /// <summary>
        /// Clears the string list for the justified lines
        /// </summary>
        public void ClearStringList()
        {
            m_ListOfStrings.Clear();
        }

        /// <summary>
        /// Adds a string to the justified lines to be written. 
        /// </summary>
        /// <param name="text"></param>
        public void AddToStringList(string text)
        {
            m_ListOfStrings.Add(text);
        }
    }
}
