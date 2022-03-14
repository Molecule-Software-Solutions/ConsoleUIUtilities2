using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public class Header
    {
        private List<string> m_HeaderLines { get; set; } = new List<string>();
        private char m_TopLineChar = ' ';
        private char m_BottomLineChar = ' ';

        public void AddHeaderLine(string line)
        {
            m_HeaderLines.Add(line);
        }

        public void SetTopLineChar(char lineChar)
        {
            m_TopLineChar = lineChar;
        }

        public void SetBottomLineChar(char lineChar)
        {
            m_BottomLineChar = lineChar;
        }

        public void SetTopAndBottomLineChars(char lineChar)
        {
            m_TopLineChar = lineChar;
            m_BottomLineChar = lineChar;
        }

        public void WriteHeader(int row, ConsoleColor lineColor = ConsoleColor.White, ConsoleColor textColor = ConsoleColor.White)
        {
            int startRow = row;
            Console.SetCursorPosition(0, startRow);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, m_TopLineChar), lineColor); 
            startRow += 1;
            foreach (string line in m_HeaderLines)
            {
                CenteredLine.PrintToConsole(line, startRow, textColor);
                startRow += 1;
            }
            Console.SetCursorPosition(0, startRow);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, m_BottomLineChar), lineColor); 
        }
    }
}
