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

        /// <summary>
        /// Adds a menu item to the menu
        /// </summary>
        /// <param name="menuItem"></param>
        public void AddMenuItem(MenuItem menuItem)
        {
            MenuItems.Add(menuItem); 
        }

        /// <summary>
        /// Adds multiple MenuItems to the Menu. Accepted as MenuItem[]
        /// </summary>
        /// <param name="menuItems"></param>
        public void AddMenuItemRange(MenuItem[] menuItems)
        {
            foreach (MenuItem menuItem in menuItems)
            {
                MenuItems.Add(menuItem); 
            }
        }

        /// <summary>
        /// Activates and displays the menu and returns a console key that was pressed. 
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="justification"></param>
        /// <param name="menuHeaderText"></param>
        /// <param name="menuItemColor"></param>
        /// <returns></returns>
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

        // Takes the console key from the menu
        private ConsoleKey TakeMenuSelection(int startRow, int justification, ConsoleColor promptColor = ConsoleColor.White)
        {
            Console.SetCursorPosition(0, startRow);
            ConsoleBufferSystem.Write("".PadRight(Console.WindowWidth, ' '));
            Console.SetCursorPosition(justification, startRow);
            ConsoleBufferSystem.Write("Please enter your selection >> ", promptColor);
            return Console.ReadKey().Key;
        }

        /// <summary>
        /// Breaks execution of the menu loop externally. 
        /// </summary>
        public void CallMenuBreakExternal()
        {
            m_MenuBreakToken = true; 
        }

        /// <summary>
        /// Begins the menu loop process. 
        /// </summary>
        /// <param name="menuStartRow"></param>
        /// <param name="menuJustification"></param>
        /// <param name="menuHeaderText"></param>
        /// <param name="menuItemsColor"></param>
        /// <param name="callbackMessageColor"></param>
        /// <param name="callbackLineColor"></param>
        /// <param name="callbackPromptColor"></param>
        /// <param name="redrawAction"></param>
        /// <param name="invalidMenuSelectionCallback"></param>
        /// <param name="onMenuBreakCallRedraw"></param>
        public void BeginMenuLoop(int menuStartRow, 
            int menuJustification, 
            string menuHeaderText = "",
            ConsoleColor menuItemsColor = ConsoleColor.White, 
            ConsoleColor callbackMessageColor = ConsoleColor.White, 
            ConsoleColor callbackLineColor = ConsoleColor.White, 
            ConsoleColor callbackPromptColor = ConsoleColor.White,
            Action? redrawAction = null,
            Action? invalidMenuSelectionCallback = null, 
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
    }
}
