namespace ConsoleUIUtilities2
{
    public static class ConsoleBufferSystem
    {
        private static List<BufferObject> BufferedObjects = new List<BufferObject>();
        private static int m_CursorLeft;
        private static int m_CursorTop; 

        public static void ClearBuffer()
        {
            BufferedObjects.Clear();
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            CursorStateCapture(); 
        }

        public static void SetCursorPosition(int cursorLeft, int cursorTop)
        {
            Console.SetCursorPosition(cursorLeft, cursorTop);
            CursorStateCapture(); 
        }

        private static void CursorStateCapture()
        {
            m_CursorLeft = Console.CursorLeft;
            m_CursorTop = Console.CursorTop; 
        }
        public static void Write(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            BufferedObjects.Add(new BufferObject() { Text = text, TextColor = textColor, BackgroundColor = backgroundColor, StartTop = Console.CursorTop, StartLeft = Console.CursorLeft, WriteMethod = BufferLineWriteMethods.Write });
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ResetColor();
            CursorStateCapture(); 
        }

        public static void WriteLine(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            BufferedObjects.Add(new BufferObject() { Text = text, TextColor = textColor, BackgroundColor = backgroundColor, StartTop = Console.CursorTop, StartLeft = Console.CursorLeft, WriteMethod = BufferLineWriteMethods.WriteLine });
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
            CursorStateCapture(); 
        }

        public static void WriteBuffer()
        {
            Console.Clear(); 
            foreach (BufferObject bo in BufferedObjects)
            {
                switch (bo.WriteMethod)
                {
                    case BufferLineWriteMethods.Write:
                        Console.SetCursorPosition(bo.StartLeft, bo.StartTop);
                        Console.ForegroundColor = bo.TextColor;
                        Console.BackgroundColor = bo.BackgroundColor;
                        Console.Write(bo.Text);
                        Console.ResetColor(); 
                        break;
                    case BufferLineWriteMethods.WriteLine:
                        Console.SetCursorPosition(bo.StartLeft, bo.StartTop);
                        Console.ForegroundColor = bo.TextColor;
                        Console.BackgroundColor = bo.BackgroundColor;
                        Console.WriteLine(bo.Text);
                        Console.ResetColor(); 
                        break;
                    default:
                        break;
                }
            }
            Console.SetCursorPosition(m_CursorLeft, m_CursorTop); 
        }
    }
}