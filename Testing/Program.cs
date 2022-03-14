using ConsoleUIUtilities2; 


namespace Testing
{
    public static class Program
    {
        public static void Main()
        {
            // Application Main Page
            ApplicationMainPage applicationMainPage = new ApplicationMainPage();
            applicationMainPage.ShowAndLoadMenu(0, 6, 15, "MAIN MENU", ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.Blue, ConsoleColor.White, false); 
            Console.ReadLine(); 
        }
    }
}