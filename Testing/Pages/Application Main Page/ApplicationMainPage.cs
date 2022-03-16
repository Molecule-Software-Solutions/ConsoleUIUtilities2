using ConsoleUIUtilities2; 
using System.Windows; 
using System; 

namespace Testing
{
    public class ApplicationMainPage : Page
    {
        /// <summary>
        /// The constructor for the page should call the base constructor.
        /// Note that the base constructor can take in optional parameters to
        /// set up the page without extensive use of the builder. 
        /// The method signature for Page's constructor is as follows ::: 
        /// Page(string title = "", Header? header = null, InputFieldSet? inputFieldSet = null, Menu? menu = null)
        /// </summary>
        public ApplicationMainPage() : base()
        {
            InitComponent(); 
        }

        /// <summary>
        /// The BuildComponent method can be used to construct the page.
        /// The default behavior for the builder is { } and overriding this method
        /// is optional, but the InitComponent method automatically calls the builder
        /// </summary>
        protected override void BuildComponent()
        {
            // Create Header
            Header header = new Header();
            header.SetTopAndBottomLineChars('=');

            // Add header lines
            header.AddHeaderLine("MOLECULE SOFTWARE SOLUTIONS");
            header.AddHeaderLine("(C) 2022 - CONSOLE UTILITIES 2 TEST APPLICATION");
            header.AddHeaderLine("V. 1.0.0.0");

            // Add Headerto page
            SetHeader(header); 

            // Create a Menu
            Menu applicationMenu = new Menu();

            // Add Menu Items
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
                // Clear and buffer enables a user to clear the screen while preserving the state since the last clear
                // while this feature can become slow if many elements have been loaded or changed, it will allow the 
                // previous content to be displayed easily. 

                // Clears the screen content and resets the buffer
                ConsoleBufferSystem.ClearBuffer();

                // Write some line to the console window
                JustifiedLines.WriteJustifiedLine("First menu item selected", 15, 2, ConsoleColor.Yellow);
                JustifiedLines.WriteJustifiedLine("Press ENTER to clear the screen", 15, 3, ConsoleColor.Cyan);
                Console.ReadLine();

                /*
                 * NOTE: When the console cursor is moved into a position prior to clearing manually, recalling the
                 * previous buffered state will reset the cursor into the previously held position.
                 * This can only be done by writing with one of the ConsoleUIUtilities2 objects or by calling
                 * ConsoleBufferSystem.SetCursorPosition(0, 0) method. 
                 */

                // To clear the screen without a buffer reset, call Console.Clear() as normal
                Console.Clear();

                // To write to the console, call normal write commands
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Press ENTER to reset the page"); 
                Console.ReadLine();

                // Once enter is pressed then the console buffer should be redrawn
                ConsoleBufferSystem.WriteBuffer();

                // We can now add lines from where we left off... notice the optional text formatting available 
                // this new function begins writing from the last point the cursor was located before the standard console clear
                // so we have added special formatting to highlight the changes. 
                ConsoleBufferSystem.WriteLine("The buffer has now been redrawn and this line was added", ConsoleColor.Magenta, ConsoleColor.DarkBlue);

                // Here is another line with one of the optional text formatting options modified. 
                // Note that writeline works much like Console.WriteLine and will position the cursor at 0,0
                ConsoleBufferSystem.WriteLine("Press ENTER to continue", ConsoleColor.Yellow);

                // To write a justified line that is the same, use the JustifiedLine class.
                JustifiedLines.WriteJustifiedLine("Press ENTER to continue (now this is justified properly)", 15, 10, ConsoleColor.Cyan); 

                // Read a line to hold the screen
                Console.ReadLine(); 
            });
            // Set the trigger keys that will call this menu option
            // NOTE: If a duplicate trigger key is set, the default behavior is 
            // that the first menu item with that trigger key will be called.
            // AVOID duplicating triggers to prevent unwanted behavior
            firstChoice.SetMenuTriggerKeys(new ConsoleKey[] { ConsoleKey.D1, ConsoleKey.NumPad1 });

            // Continue setting up menu items
            secondChoice.SetMenuItemText("2) SHOW INPUTS"); 

            // This menu item demonstrates calling the input fields into action
            secondChoice.SetMenuItemCommand(() =>
            {
                ShowAndLoadInputs(0, 6, 15, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.White, true);
                // Normally you would perform some sort of validation or 
                // verification action with the inputs, then deal with them in some way
                // here. 
                // Sample to get values: InputFieldSet.GetValue("2) SHOW INPUTS"); 
                // NOTE: The default behavior, unless you manually set the identifierID, is
                // for the system to accept the field's text as the identifier. 
                // FURTHER NOTE: You may also directly access the field's value by accessing the
                // field's InputValue property if this is easier. 
                Console.ReadLine(); 
            });
            // Again adding trigger keys
            secondChoice.SetMenuTriggerKeys(new ConsoleKey[] { ConsoleKey.D2, ConsoleKey.NumPad2 });

            // This shows an example of how to properly call a dialog.
            thirdChoice.SetMenuItemText("3) SHOW DIALOG"); 
            thirdChoice.SetMenuItemCommand(() =>
            {
                // NOTE: the test application contains a class called Lorem which
                // creates sample text to fill the dialog
                Dialog dialog = new Dialog("Test Title", Lorem.LOREM_LONG_STRING);
                // set up and show the dialog
                dialog.Show('=', ConsoleColor.Yellow, ConsoleColor.Cyan, ConsoleColor.White);
                // Close the dialog and handle the close call with a callback anan method. 
                dialog.Close(() =>
                {
                    ConsoleBufferSystem.WriteBuffer();
                });
            }); 
            thirdChoice.SetMenuTriggerKeys(new ConsoleKey[] {ConsoleKey.D3, ConsoleKey.NumPad3 });

            // This is an example of how to close an application using the page's 
            // close callback anon method. 
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
            InputFieldSet applicationInputs = new InputFieldSet();

            // Create inputs
            Input personName = new Input("personName");
            Input personAddress = new Input("personAddress");
            Input personCity = new Input("personCity");

            // Setup Inputs
            personName.SetInputLabel("NAME");
            personAddress.SetInputLabel("ADDRESS");
            personCity.SetInputLabel("CITY");

            // Set the input field marker * This can be any string to denote the beginning of the field
            // it is suggested to add a space at the end of the marker
            applicationInputs.SetInputFieldMarker(">> "); 

            // Add inputs to input field set
            applicationInputs.AddInputRange(new Input[] {personName, personAddress, personCity});

            // Add inputs to page
            SetInputFieldSet(applicationInputs); 

            // Show the page
            // NOTE: Most of the parameters are optioanl here but assist with the visual
            // setup of the page. Inline documentation can assist with this setup, but it
            // mostly consists of item placements and colorations
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

        // NOTE: This override is not necessary to perform a simple Init
        // but it demonstrates how additional code can be added to the init command. 
        // Just make sure to call the BuildComponent method at some point if the InitComponent
        // method is overridden. 
        public override void InitComponent()
        {
            // Fill any models or perform any other non-GUI work here
            // <  >

            // Call builder last
            BuildComponent();
        }
    }
}