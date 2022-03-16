using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public static class HorizontalLine
    {
        /// <summary>
        /// Writes a horizontal line which extends the full width of the console window
        /// </summary>
        /// <param name="lineCharacter">Character that will be used to represent the horizontal line</param>
        /// <param name="row">starting row</param>
        /// <param name="color">line color</param>
        public static void WriteHorizontalLine(char lineCharacter, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(0, row);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, lineCharacter), color);
        }
    }
}
