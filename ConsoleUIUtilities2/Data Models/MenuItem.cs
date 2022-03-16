namespace ConsoleUIUtilities2
{
    public class MenuItem
    {
        public string MenuItemText { get; set; } = string.Empty;
        public ConsoleKey[] MenuItemTriggerKeys { get; set; }
        public Action? MenuItemCommand { get; set; } = null;

        /// <summary>
        /// MenuItem that is added to a Menu. 
        /// </summary>
        public MenuItem() : this("", Array.Empty<ConsoleKey>(), () => { }) { }

        /// <summary>
        /// MenuItem that is added to a Menu. 
        /// </summary>
        public MenuItem(string menuItemText, ConsoleKey[] menuItemTriggerKey, Action menuItemCommand)
        {
            MenuItemText = menuItemText;
            MenuItemTriggerKeys = menuItemTriggerKey;
            MenuItemCommand = menuItemCommand;
        }

        /// <summary>
        /// Adds an array of ConsoleKey that will activate the menu item's action
        /// </summary>
        /// <param name="keys"></param>
        public void SetMenuTriggerKeys(ConsoleKey[] keys)
        {
            MenuItemTriggerKeys = keys;
        }

        /// <summary>
        /// Prints the menu item to the console
        /// </summary>
        /// <param name="row"></param>
        /// <param name="justification"></param>
        /// <param name="color"></param>
        public void PrintMenuItem(int row, int justification = 0, ConsoleColor color = ConsoleColor.White)
        {
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth - justification, ' '));
            Console.SetCursorPosition(justification, row);
            ConsoleBufferSystem.Write(MenuItemText, color);
        }

        /// <summary>
        /// Sets the text of the menu item
        /// </summary>
        /// <param name="text"></param>
        public void SetMenuItemText(string text)
        {
            MenuItemText = text;
        }

        /// <summary>
        /// Sets the action to be executed
        /// </summary>
        /// <param name="command"></param>
        public void SetMenuItemCommand(Action command)
        {
            MenuItemCommand = command;
        }

        /// <summary>
        /// Activates the menu item command
        /// </summary>
        /// <param name="callbackMessage"></param>
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

        /// <summary>
        /// Provides a callback message for a menu item that has been executed or that has encountered an error
        /// </summary>
        /// <param name="message"></param>
        public delegate void StatusCallback(string message);
    }
}
