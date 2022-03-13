using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public class Menu
    {
        bool m_MenuBreakToken = false;

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public void AddMenuItem(MenuItem menuItem)
        {
            MenuItems.Add(menuItem); 
        }

        public void AddMenuItemRange(MenuItem[] menuItems)
        {
            foreach (MenuItem menuItem in menuItems)
            {
                MenuItems.Add(menuItem); 
            }
        }

        private ConsoleKey ActivateMenu(int startRow, int justification, ConsoleColor menuItemColor = ConsoleColor.White)
        {
            int currentRowPosition = startRow;
            foreach (MenuItem item in MenuItems)
            {
                item.PrintMenuItem(currentRowPosition, justification, menuItemColor);
                currentRowPosition += 1; 
            }
            currentRowPosition += 1;
            return TakeMenuSelection(currentRowPosition, justification, menuItemColor); 
        }

        private ConsoleKey TakeMenuSelection(int startRow, int justification, ConsoleColor promptColor = ConsoleColor.White)
        {
            int currentRowPosition = startRow; 
            Console.SetCursorPosition(0, currentRowPosition);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(0, currentRowPosition);
            Console.ForegroundColor = promptColor;
            Console.Write("Please enter your selection >> ");
            return Console.ReadKey().Key;
        }

        public void BeginMenuLoop(int menuStartRow, 
            int menuJustification, 
            ConsoleColor menuItemsColor = ConsoleColor.White, 
            ConsoleColor callbackMessageColor = ConsoleColor.White, 
            ConsoleColor callbackLineColor = ConsoleColor.White, 
            ConsoleColor callbackPromptColor = ConsoleColor.White)
        {
            while (!m_MenuBreakToken)
            {
                var keyPressed = ActivateMenu(menuStartRow, menuJustification, menuItemsColor); 
                var selectedMenuItem = MenuItems.Where(m => m.MenuItemTriggerKey == keyPressed).FirstOrDefault();
                if(selectedMenuItem is not null)
                {
                    selectedMenuItem.ActivateMenuItemCommand((cb) =>
                    {
                        NotificationLine.WriteNotificationLine(cb, callbackMessageColor, callbackLineColor, callbackPromptColor); 
                    }); 
                }
            }
        }
    }
}
