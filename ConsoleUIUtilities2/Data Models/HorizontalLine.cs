using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public static class HorizontalLine
    {
        public static void WriteHorizontalLine(char lineCharacter, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(0, row);
            Console.ForegroundColor = color; 
            Console.Write("".PadRight(Console.WindowWidth, lineCharacter));
            Console.ResetColor(); 
        }
    }
}
