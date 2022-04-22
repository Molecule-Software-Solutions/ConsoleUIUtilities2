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

        /// <summary>
        /// Add a new line to the header content
        /// </summary>
        /// <param name="line">line of text that will be printed within the header</param>
        public void AddHeaderLine(string line)
        {
            m_HeaderLines.Add(line);
        }

        /// <summary>
        /// Set the character that will represent the top line of the header only
        /// NOTE: You must call SetBottomLineChar(char x) to set the bottom line character
        /// </summary>
        /// <param name="lineChar"></param>
        public void SetTopLineChar(char lineChar)
        {
            m_TopLineChar = lineChar;
        }

        /// <summary>
        /// Set the character that will represent the bottom line of the header only
        /// NOTE: You must call SetTopLineChar(char x) to set the top line character
        /// </summary>
        /// <param name="lineChar"></param>
        public void SetBottomLineChar(char lineChar)
        {
            m_BottomLineChar = lineChar;
        }

        /// <summary>
        /// Set the character that will represent the top and bottom line of the header
        /// </summary>
        /// <param name="lineChar"></param>
        public void SetTopAndBottomLineChars(char lineChar)
        {
            m_TopLineChar = lineChar;
            m_BottomLineChar = lineChar;
        }

        /// <summary>
        /// Writes the header to the console at the specified location
        /// </summary>
        /// <param name="row">starting row where the header will be drawn</param>
        /// <param name="lineColor">line color above and below the header content.</param>
        /// <param name="textColor">text color within the header</param>
        public void WriteHeader(int row, ConsoleColor lineColor = ConsoleColor.White, ConsoleColor textColor = ConsoleColor.White)
        {
            int startRow = row; 
            HorizontalLine.WriteHorizontalLine(m_TopLineChar, startRow, lineColor);
            startRow += 1;
            foreach (string line in m_HeaderLines)
            {
                CenteredLine.PrintToConsole(line, startRow, textColor);
                startRow += 1;
            }
            HorizontalLine.WriteHorizontalLine(m_BottomLineChar, startRow, lineColor);
        }
    }
}
