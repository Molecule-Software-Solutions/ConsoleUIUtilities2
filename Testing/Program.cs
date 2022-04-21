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
        }
    }
}