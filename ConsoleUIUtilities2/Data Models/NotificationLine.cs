﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public static class NotificationLine
    {
        /// <summary>
        /// Writes a notification message to the bottom of the console screen
        /// NOTE: This line is separated from the console content by a horizontal line
        /// and an optioanl label component
        /// </summary>
        /// <param name="line"></param>
        /// <param name="color"></param>
        /// <param name="notificationLineColor"></param>
        /// <param name="notificationLineMessageColor"></param>
        /// <param name="notificationLineText"></param>
        public static void WriteNotificationLine(string line = "",
            ConsoleColor color = ConsoleColor.White,
            ConsoleColor notificationLineColor =
            ConsoleColor.White,
            ConsoleColor notificationLineMessageColor = ConsoleColor.White,
            string notificationLineText = "",
            int notificationLineStartPosition = 27)
        {
            ClearNotificationLine(27);
            Console.SetCursorPosition(0, notificationLineStartPosition);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, '-'), notificationLineColor);
            Console.SetCursorPosition(5, notificationLineStartPosition);
            ConsoleBufferSystem.Write($"[ {notificationLineText} ]", notificationLineMessageColor);
            Console.SetCursorPosition(0, notificationLineStartPosition + 1);
            ConsoleBufferSystem.Write(line, color); 
        }

        /// <summary>
        /// Clears the notification line
        /// </summary>
        public static void ClearNotificationLine(int notificationLineStartPosition = 27)
        {
            Console.SetCursorPosition(0, notificationLineStartPosition);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(0, notificationLineStartPosition + 1);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, ' '));
        }
    }
}
