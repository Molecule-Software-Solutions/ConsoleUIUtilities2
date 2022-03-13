using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public static class NotificationLine
    {
        public static void WriteNotificationLine(string line = "",
            ConsoleColor color = ConsoleColor.White,
            ConsoleColor notificationLineColor =
            ConsoleColor.White,
            ConsoleColor notificationLineMessageColor = ConsoleColor.White,
            string notificationLineText = "")
        {
            int writePosition = Console.WindowHeight - 1;
            ClearNotificationLine();
            Console.ForegroundColor = notificationLineColor; 
            Console.SetCursorPosition(0, writePosition - 1);
            Console.Write("".PadRight(Console.WindowWidth, '-'));
            Console.ForegroundColor = notificationLineMessageColor; 
            Console.SetCursorPosition(5, writePosition - 1);
            Console.Write($"[ {notificationLineText} ]");
            Console.ForegroundColor = color;
            Console.SetCursorPosition(0, writePosition);
            Console.Write(line); 
        }

        public static void ClearNotificationLine()
        {
            int writePosition = Console.WindowHeight - 1;
            Console.SetCursorPosition(0, writePosition - 1);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(0, writePosition);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
        }
    }
}
