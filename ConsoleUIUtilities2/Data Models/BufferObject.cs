namespace ConsoleUIUtilities2
{
    public class BufferObject
    {
        public BufferLineWriteMethods WriteMethod { get; set; } = BufferLineWriteMethods.Write; 
        public string Text { get; set; } = string.Empty;
        public int StartLeft { get; set; } = 0;
        public int StartTop { get; set; } = 0; 
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black; 
    }
}
