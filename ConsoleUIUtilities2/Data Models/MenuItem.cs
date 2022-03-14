namespace ConsoleUIUtilities2
{
    public class MenuItem
    {
        public string MenuItemText { get; set; } = string.Empty;
        public ConsoleKey[] MenuItemTriggerKeys { get; set; }
        public Action? MenuItemCommand { get; set; } = null;

        public MenuItem() : this("", Array.Empty<ConsoleKey>(), () => { }) { }

        public MenuItem(string menuItemText, ConsoleKey[] menuItemTriggerKey, Action menuItemCommand)
        {
            MenuItemText = menuItemText;
            MenuItemTriggerKeys = menuItemTriggerKey;
            MenuItemCommand = menuItemCommand;
        }

        public void SetMenuTriggerKeys(ConsoleKey[] keys)
        {
            MenuItemTriggerKeys = keys;
        }

        public void PrintMenuItem(int row, int justification = 0, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth - justification, ' '));
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write(MenuItemText, color);
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
