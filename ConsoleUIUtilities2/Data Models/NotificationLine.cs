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
            Console.SetCursorPosition(0, writePosition - 1);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, '-'), notificationLineColor);
            Console.SetCursorPosition(5, writePosition - 1);
            ConsoleBufferSystem.Write($"[ {notificationLineText} ]", notificationLineMessageColor);
            Console.SetCursorPosition(0, writePosition);
            ConsoleBufferSystem.Write(line, color); 
        }

        public static void ClearNotificationLine()
        {
            int writePosition = Console.WindowHeight - 1;
            Console.SetCursorPosition(0, writePosition - 1);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(0, writePosition);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, ' '));
        }
    }
}
