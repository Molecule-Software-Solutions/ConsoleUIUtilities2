using ConsoleUIUtilities2; 
using System.Windows; 
using System; 

namespace Testing
{
    public class ApplicationMainPage : Page
    {
        public ApplicationMainPage() : base()
        {
            InitComponent(); 
        }

        protected override void BuildComponent()
        {
            // Header
            Header header = new Header();
            header.SetTopAndBottomLineChars('=');
            header.AddHeaderLine("MOLECULE SOFTWARE SOLUTIONS");
            header.AddHeaderLine("(C) 2022 - CONSOLE UTILITIES 2 TEST APPLICATION");
            header.AddHeaderLine("V. 1.0.0.0");

            // Add Header
            SetHeader(header); 

            // Application Menu
            Menu applicationMenu = new Menu();

            // Application Menu Items
            MenuItem firstChoice = new MenuItem();
            MenuItem secondChoice = new MenuItem();
            MenuItem thirdChoice = new MenuItem();
            MenuItem fourthChoice = new MenuItem(); 

            // Add menu to page
            SetMenu(applicationMenu); 

            // Setup Menu Items
            firstChoice.SetMenuItemText("1) FIRST CHOICE");
            firstChoice.SetMenuItemCommand(() =>
            {
                Console.Clear();
                Console.WriteLine("First Choice Selected");
                Console.WriteLine("Press ENTER to contimue");
                Console.ReadLine(); 
            });
            firstChoice.SetMenuTriggerKeys(new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.NumPad1 });

            secondChoice.SetMenuItemText("2) SHOW INPUTS"); 
            secondChoice.SetMenuItemCommand(async () =>
            {
                ShowAndLoadInputs(0, 6, 15, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.White, true);
                Console.ReadLine(); 
            });
            secondChoice.SetMenuTriggerKeys(new ConsoleKey[] { ConsoleKey.D2, ConsoleKey.NumPad2 });

            thirdChoice.SetMenuItemText("3) SHOW DIALOG"); 
            thirdChoice.SetMenuItemCommand(() =>
            {
                Dialog dialog = new Dialog("Test Title", Lorem.LOREM_LONG_STRING);
                dialog.Show('=', ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White);
                dialog.Close(() =>
                {
                    ConsoleBufferSystem.WriteBuffer();
                });
            }); 
            thirdChoice.SetMenuTriggerKeys(new ConsoleKey[] {ConsoleKey.D3, ConsoleKey.NumPad3 });

            fourthChoice.SetMenuItemText("4) EXIT APPLICATION"); 
            fourthChoice.SetMenuItemCommand(() =>
            {
                Close(() =>
                {
                    Environment.Exit(0); 
                }); 
            });
            fourthChoice.SetMenuTriggerKeys(new ConsoleKey[] { ConsoleKey.D4, ConsoleKey.NumPad4 });

            // Fill Menu
            applicationMenu.AddMenuItemRange(new MenuItem[] {firstChoice, secondChoice, thirdChoice, fourthChoice});

            // Input Items
            InputFieldSet applicaitonInputs = new InputFieldSet();

            // Create inputs
            Input personName = new Input("personName");
            Input personAddress = new Input("personAddress");
            Input personCity = new Input("personCity");

            // Setup Inputs
            personName.SetInputLabel("NAME");
            personAddress.SetInputLabel("ADDRESS");
            personCity.SetInputLabel("CITY");

            // Add inputs to page
            SetInputFieldSet(applicaitonInputs); 

            // Show the page
            ShowAndLoadMenu(
                0,
                6,
                15,
                "MAIN MENU",
                ConsoleColor.Yellow,
                ConsoleColor.Cyan,
                ConsoleColor.Yellow,
                ConsoleColor.Blue,
                ConsoleColor.Blue,
                ConsoleColor.Yellow); 
        }

        public override void InitComponent()
        {
            // Call builder
            BuildComponent();

            // Call any additional code to modify the page here
            // {}
        }
    }
}