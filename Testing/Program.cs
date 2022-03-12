using ConsoleUIUtilities2; 

namespace Testing
{
    public static class Program
    {
        public static void Main()
        {
            Header header = new Header();
            header.SetTopAndBottomLineChars('-');
            header.AddHeaderLine("Molecule Software Systems, Inc.");
            header.AddHeaderLine("(C) 2022 - Console UI Utilities 2 test suite");
            header.WriteHeader(1, ConsoleColor.Cyan, ConsoleColor.Yellow);

            Input name = new Input("Name");
            Input address = new Input("Address");
            Input address2 = new Input("Address2");
            Input city = new Input("City");
            Input state = new Input("State");
            Input zip = new Input("Zip");

            InputFieldSet inputFieldSet = new InputFieldSet();
            inputFieldSet.AddInputRange(new Input[] { name, address, address2, city, state, zip });

            inputFieldSet.SetInputFieldMarker(">> "); 
            inputFieldSet.WriteMultipleInputs(6, 15, ConsoleColor.Yellow);
            inputFieldSet.TakeAllInputValues(ConsoleColor.Cyan, (cb) =>
            {
                Console.SetCursorPosition(0, 25);
                Console.Write("".PadRight(Console.WindowWidth, ' '));
                Console.SetCursorPosition(0, 25);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(cb);
            }, true);

            JustifiedLines.WriteJustifiedLine($"NAME: {name.InputValue}", 15, 13, ConsoleColor.Green);
            JustifiedLines.WriteJustifiedLine($"ADDRESS: {address.InputValue}", 15, 14, ConsoleColor.Green);
            JustifiedLines.WriteJustifiedLine($"ADDRESS 2: {address2.InputValue}", 15, 15, ConsoleColor.Green);
            JustifiedLines.WriteJustifiedLine($"CITY: {city.InputValue}", 15, 16, ConsoleColor.Green);
            JustifiedLines.WriteJustifiedLine($"STATE: {state.InputValue}", 15, 17, ConsoleColor.Green);
            JustifiedLines.WriteJustifiedLine($"ZIP: {zip.InputValue}", 15, 18, ConsoleColor.Green);



            Console.ReadLine(); 
        }
    }
}