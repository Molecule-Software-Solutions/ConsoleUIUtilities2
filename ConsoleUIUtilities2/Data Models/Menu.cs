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

        private ConsoleKey ActivateMenu(int startRow, int justification, string menuHeaderText = "", ConsoleColor menuItemColor = ConsoleColor.White)
        {
            int currentRowPosition = startRow;
            if(!string.IsNullOrWhiteSpace(menuHeaderText))
            {
                JustifiedLines.WriteJustifiedLine(menuHeaderText, justification, currentRowPosition, menuItemColor);
                currentRowPosition += 2; 
            }
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
            Console.SetCursorPosition(0, startRow);
            Console.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(justification, startRow);
            Console.ForegroundColor = promptColor;
            Console.Write("Please enter your selection >> ");
            return Console.ReadKey().Key;
        }

        public void CallMenuBreakExternal()
        {
            m_MenuBreakToken = true; 
        }

        public void BeginMenuLoop(int menuStartRow, 
            int menuJustification, 
            string menuHeaderText = "",
            ConsoleColor menuItemsColor = ConsoleColor.White, 
            ConsoleColor callbackMessageColor = ConsoleColor.White, 
            ConsoleColor callbackLineColor = ConsoleColor.White, 
            ConsoleColor callbackPromptColor = ConsoleColor.White,
            Action? redrawAction = null,
            InvalidMenuSelectionCallback? invalidMenuSelectionCallback = null, 
            bool onMenuBreakCallRedraw = false)
        {
            while (!m_MenuBreakToken)
            {
                var keyPressed = ActivateMenu(menuStartRow, menuJustification, menuHeaderText, menuItemsColor); 
                var selectedMenuItem = MenuItems.Where(m => m.MenuItemTriggerKeys.Contains(keyPressed)).FirstOrDefault();
                if(selectedMenuItem is not null)
                {
                    selectedMenuItem.ActivateMenuItemCommand((cb) =>
                    {
                        if(!m_MenuBreakToken)
                        {
                            NotificationLine.WriteNotificationLine(cb, callbackMessageColor, callbackLineColor, callbackPromptColor); 
                        }
                    }); 
                }
                if(redrawAction is not null)
                {
                    if(m_MenuBreakToken && onMenuBreakCallRedraw)
                    {
                        redrawAction(); 
                    }
                    else if(!m_MenuBreakToken)
                    {
                        redrawAction(); 
                    }
                }
                if(selectedMenuItem is null && invalidMenuSelectionCallback is not null)
                {
                    invalidMenuSelectionCallback();
                }
            }
        }

        public delegate void InvalidMenuSelectionCallback(); 
    }
}
