namespace ConsoleUIUtilities2
{
    public static class CenteredLine
    {
        public static void PrintToConsole(string text, int row, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition((Console.WindowWidth - text.Length) / 2, row);
            ConsoleBufferSystem.Write(text, color); 
        }
    }
}
