namespace ConsoleUIUtilities2
{
    public static class ConsoleBufferSystem
    {
        private static List<BufferObject> BufferedObjects = new List<BufferObject>();

        public static void ClearBuffer()
        {
            BufferedObjects.Clear();
            Console.Clear();
        }

        public static void Write(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            BufferedObjects.Add(new BufferObject() { Text = text, TextColor = textColor, BackgroundColor = backgroundColor, StartTop = Console.CursorTop, StartLeft = Console.CursorLeft, WriteMethod = BufferLineWriteMethods.Write });
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
            Console.ResetColor(); 
        }

        public static void WriteLine(string text, ConsoleColor textColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            BufferedObjects.Add(new BufferObject() { Text = text, TextColor = textColor, BackgroundColor = backgroundColor, StartTop = Console.CursorTop, StartLeft = Console.CursorLeft, WriteMethod = BufferLineWriteMethods.WriteLine });
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor(); 
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
        }
    }
}