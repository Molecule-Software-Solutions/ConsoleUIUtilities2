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
            Console.ReadLine();

            List<int> testList = new List<int>(); 
            for (int i = 0; i < 140; i++)
            {
                testList.Add(i); 
            }
            SelectionMenu<int> menu = new SelectionMenu<int>(6, 25, () =>
            {

            }, () =>
            {

            }); 

            foreach (int item in testList)
            {
                menu.AddItem(item, $"THE QUICK BROWN FOX JUMPED OVER THE LAZY DOG {item}");
            }

            menu.PrintItemSelection();
        }
    }
}