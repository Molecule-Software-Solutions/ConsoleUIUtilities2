namespace ConsoleUIUtilities2
{
    public class Dialog
    {
        private string m_Message;
        private string m_Title;
        private readonly int m_StartHeight = (Console.WindowWidth / 2) - 10; 
        private readonly int m_EndHeight = (Console.WindowWidth / 2) + 10; 
        public Dialog(string title, string message)
        {
            m_Message = message;
            m_Title = title; 
        }

        public void Show()
        {

        }

        public void Close(Action closeAction)
        {
            Console.Clear(); 
            closeAction(); 
        }
    }
}