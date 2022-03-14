namespace ConsoleUIUtilities2
{
    public class Dialog
    {
        private string m_Message;
        private string m_Title;
        private readonly int m_StartHeight = (Console.WindowHeight / 2) - 10;
        private readonly int m_EndHeight = (Console.WindowHeight / 2) + 10;
        public Dialog(string title, string message)
        {
            m_Message = message;
            m_Title = title;
        }

        public void Show(char frameLineChar = '-', ConsoleColor dialogFrameColor = ConsoleColor.White, ConsoleColor consoleTitleColor = ConsoleColor.White, ConsoleColor consoleMessageColor = ConsoleColor.White)
        {
            // Clear the dialog area
            for (int i = m_StartHeight; i < m_EndHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("".PadRight(Console.WindowWidth, ' ')); 
            }

            // Draw Frame
            Console.SetCursorPosition(0, m_StartHeight);
            Console.ForegroundColor = dialogFrameColor;
            Console.Write("".PadRight(Console.WindowWidth, frameLineChar));
            Console.SetCursorPosition(0, m_StartHeight + 2);
            Console.Write("".PadRight(Console.WindowWidth, frameLineChar));
            Console.SetCursorPosition(0, m_EndHeight);
            Console.Write("".PadRight(Console.WindowWidth, frameLineChar));

            // Draw Title
            Console.ForegroundColor = consoleTitleColor;
            Console.SetCursorPosition((Console.WindowWidth - m_Title.Length) / 2, m_StartHeight + 1);
            Console.Write(m_Title); 

            // Get message enumerator
            var messageEnum = m_Message.GetEnumerator();
            int maxConosoleWidth = Console.WindowWidth;
            int currentCharCount = 0;

            Console.SetCursorPosition(0, m_StartHeight + 3);

            while (messageEnum.MoveNext())
            {
                if (currentCharCount != maxConosoleWidth)
                {
                    Console.ForegroundColor = consoleMessageColor;
                    Console.Write(messageEnum.Current);
                    currentCharCount += 1;
                    Console.ResetColor();
                }
                else
                {
                    currentCharCount = 0;
                    int currentTop = Console.CursorTop;
                    Console.ForegroundColor = consoleMessageColor; 
                    Console.SetCursorPosition(0, currentTop + 1);
                    Console.Write(messageEnum.Current);
                    currentCharCount += 1;
                    Console.ResetColor(); 
                }
            }
            int finalConsoleLine = Console.CursorTop;
            Console.SetCursorPosition(0, finalConsoleLine + 2);
            Console.ForegroundColor = consoleTitleColor; 
            Console.Write("Press ENTER to continue");
            Console.ResetColor(); 
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
        }

        public void Close(Action closeAction)
        {
            Console.Clear();
            closeAction();
        }
    }
}