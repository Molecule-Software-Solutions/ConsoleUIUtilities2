namespace ConsoleUIUtilities2
{
    public class MenuItem
    {
        public string MenuItemText { get; set; } = string.Empty;
        public ConsoleKey[] MenuItemTriggerKey { get; set; }
        public Action? MenuItemCommand { get; set; } = null;

        public MenuItem() : this("", Array.Empty<ConsoleKey>(), () => { }) { }

        public MenuItem(string menuItemText, ConsoleKey[] menuItemTriggerKey, Action menuItemCommand)
        {
            MenuItemText = menuItemText;
            MenuItemTriggerKey = menuItemTriggerKey;
            MenuItemCommand = menuItemCommand;
        }

        public void PrintMenuItem(int row, int justification = 0, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            Console.Write("".PadRight(Console.WindowWidth - justification, ' '));
            Console.SetCursorPosition(justification, row);
            Console.ForegroundColor = color;
            Console.Write(MenuItemText);
            Console.ResetColor(); 
        }

        public void SetMenuItemText(string text)
        {
            MenuItemText = text;
        }

        public void SetMenuItemCommand(Action command)
        {
            MenuItemCommand = command; 
        }

        public void ActivateMenuItemCommand(StatusCallback? callbackMessage = null)
        {
            if (MenuItemCommand is not null)
            {
                MenuItemCommand();
                callbackMessage?.Invoke("COMMAND EXECUTED SUCCESSFULLY");
            }
            else
                callbackMessage?.Invoke("ER101: COMMAND DOES NOT EXIST"); 
        }

        public delegate void StatusCallback(string message); 
    }
}
