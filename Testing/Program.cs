using ConsoleUIUtilities2; 


namespace Testing
{
    public static class Program
    {
        public static void Main()
        {
            Header testHeader = new Header();
            testHeader.AddHeaderLine("2022 Test Application");
            testHeader.AddHeaderLine("Molecule Software Systems");
            testHeader.SetTopAndBottomLineChars('*'); 
            SelectionMenu<int> menu = new SelectionMenu<int>();


            for (int i = 0; i < 30; i++)
            {
                menu.AddSelectionItem(new SelectionItem<int>($"Test{i}", i)); 
            }


            menu.DrawSelectionScreen("SELECT AN ITEM", '/', ConsoleColor.Yellow, true, testHeader);
            Console.ReadLine(); 
        }
    }
}