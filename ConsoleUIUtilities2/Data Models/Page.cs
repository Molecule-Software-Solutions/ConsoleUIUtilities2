﻿namespace ConsoleUIUtilities2
{
    public abstract class Page
    {
        private bool m_DisplayItemsLoopBreak;
        protected object? m_DisplayItemData; 

        public string? Title { get; private set; }
        public Header? Header { get; private set; }
        public Menu? Menu { get; private set; }
        public DisplayItemSet? DisplayItemSet { get; private set; }
        public InputFieldSet? InputFieldSet { get; private set; }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetHeader(Header? header = null)
        {
            if (header is not null)
            {
                Header = header;
                return;
            }
            Header = new Header();
            Header.AddHeaderLine(Title ?? "NO TITLE SET");
            Header.SetTopAndBottomLineChars('-');
        }

        public void SetMenu(Menu menu)
        {
            Menu = menu;
        }

        public void SetInputFieldSet(InputFieldSet inputFieldSet)
        {
            InputFieldSet = inputFieldSet;
        }

        public void SetDisplayItemSet(DisplayItemSet displayItemSet)
        {
            DisplayItemSet = displayItemSet;
        }

        public Page(string title = "", Header? header = null, InputFieldSet? inputFieldSet = null, Menu? menu = null, DisplayItemSet? displayItemSet = null)
        {
            Title = title;
            Header = header;
            InputFieldSet = inputFieldSet;
            Menu = menu;
            DisplayItemSet = displayItemSet;
            InitComponent();
        }

        /// <summary>
        /// Shows the page only. 
        /// NOTE: This method calls the <see cref="ShowPostInitItems"/> method which will display items that cannot be added during
        /// the <see cref="BuildComponent"/> call during page init. 
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="headerLineColor"></param>
        /// <param name="headerTextColor"></param>
        public void ShowPageOnly(int headerRow = 0, ConsoleColor headerLineColor = ConsoleColor.White, ConsoleColor headerTextColor = ConsoleColor.White)
        {
            ConsoleBufferSystem.ClearBuffer();
            if (Header is not null)
            {
                Header.WriteHeader(headerRow, headerLineColor, headerTextColor);
            }
            else
            {
                Header = new Header();
                Header.SetTopAndBottomLineChars('-');
                Header.AddHeaderLine(Title ?? string.Empty);
                Header.WriteHeader(headerRow, headerLineColor, headerTextColor);
            }
            ShowPostInitItems();
        }

        /// <summary>
        /// Inits the page and shows the page's menu
        /// NOTE: The <see cref="Menu"/> should be added in the <see cref="BuildComponent"/> method 
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="menuStartRow"></param>
        /// <param name="menuJustification"></param>
        /// <param name="menuHeaderText"></param>
        /// <param name="menuItemColor"></param>
        /// <param name="headerLineColor"></param>
        /// <param name="headerTextColor"></param>
        /// <param name="callbackMessageColor"></param>
        /// <param name="callbackLineColor"></param>
        /// <param name="callbackPromptColor"></param>
        /// <param name="onMenuBreakCallPageRedraw"></param>
        public void ShowAndLoadMenu(int headerRow = 0,
            int menuStartRow = 6,
            int menuJustification = 15,
            string menuHeaderText = "",
            ConsoleColor menuItemColor = ConsoleColor.White,
            ConsoleColor headerLineColor = ConsoleColor.White,
            ConsoleColor headerTextColor = ConsoleColor.White,
            ConsoleColor callbackMessageColor = ConsoleColor.White,
            ConsoleColor callbackLineColor = ConsoleColor.White,
            ConsoleColor callbackPromptColor = ConsoleColor.White,
            bool onMenuBreakCallPageRedraw = false)
        {
            ConsoleBufferSystem.ClearBuffer();
            Header?.WriteHeader(headerRow, headerLineColor, headerTextColor);
            ShowPostInitItems();
            Menu?.BeginMenuLoop(
                menuStartRow,
                menuJustification,
                menuHeaderText,
                menuItemColor,
                callbackMessageColor,
                callbackLineColor,
                callbackPromptColor, () =>
            {
                ConsoleBufferSystem.ClearBuffer();
                Header?.WriteHeader(headerRow, headerLineColor, headerTextColor);
            },
            InvalidMenuSelectionNotice,
            onMenuBreakCallPageRedraw);
        }

        /// <summary>
        /// Displays the page and displays all display items that are in the display item set:
        /// NOTE: If you wish to hold this page in a loop you may do so by passing true in the
        /// holdPageInLoop parameter. You may request a console key entry to break this loop using
        /// the DisplayPostInitItems virtual method or you can call BreakDisplayItemsLoopExternal
        /// </summary>
        /// <param name="data"></param>
        /// <param name="headerRow"></param>
        /// <param name="headerLineColor"></param>
        /// <param name="headerTextColor"></param>
        /// <param name="holdPageInLoop"></param>
        public void ShowAndLoadDisplayItems(object? data = null, int headerRow = 0,
            ConsoleColor headerLineColor = ConsoleColor.White,
            ConsoleColor headerTextColor = ConsoleColor.White,
            bool holdPageInLoop = false)
        {
            if(data is not null)
            {
                m_DisplayItemData = data; 
            }

            // Build display item components after data is set
            BuildDisplayItemComponents(); 

            if (holdPageInLoop)
            {
                while (!m_DisplayItemsLoopBreak)
                {
                    ConsoleBufferSystem.ClearBuffer();
                    Header?.WriteHeader(headerRow, headerLineColor, headerTextColor);
                    DisplayItemSet?.WriteAllDisplayItems();
                    ShowPostInitItems();
                }
                return;
            }
            ConsoleBufferSystem.ClearBuffer();
            Header?.WriteHeader(headerRow, headerLineColor, headerTextColor);
            DisplayItemSet?.WriteAllDisplayItems();
            ShowPostInitItems();
        }

        public void BreakDisplayItemLoopExternal()
        {
            m_DisplayItemsLoopBreak = true;
        }

        /// <summary>
        /// Inits the page and shows the page's inputs
        /// NOTE: The <see cref="InputFieldSet"/> should be added in the <see cref="BuildComponent"/> method
        /// </summary>
        /// <param name="headerRow"></param>
        /// <param name="inputStartRow"></param>
        /// <param name="inputJustification"></param>
        /// <param name="headerLineColor"></param>
        /// <param name="headerTextColor"></param>
        /// <param name="inputPromptTextColor"></param>
        /// <param name="inputValueTextColor"></param>
        /// <param name="callbackMessageColor"></param>
        /// <param name="callbackLineColor"></param>
        /// <param name="callbackPromptColor"></param>
        /// <param name="collectValuesAsAllUppercase"></param>
        public void ShowAndLoadInputs(int headerRow = 0,
            int inputStartRow = 5,
            int inputJustification = 15,
            ConsoleColor headerLineColor = ConsoleColor.White,
            ConsoleColor headerTextColor = ConsoleColor.White,
            ConsoleColor inputPromptTextColor = ConsoleColor.White,
            ConsoleColor inputValueTextColor = ConsoleColor.White,
            ConsoleColor callbackMessageColor = ConsoleColor.White,
            ConsoleColor callbackLineColor = ConsoleColor.White,
            ConsoleColor callbackPromptColor = ConsoleColor.White,
            bool collectValuesAsAllUppercase = false)
        {
            ConsoleBufferSystem.ClearBuffer();
            Header?.WriteHeader(headerRow, headerLineColor, headerTextColor);
            ShowPostInitItems();
            InputFieldSet?.WriteMultipleInputs(inputStartRow, inputJustification, inputPromptTextColor);
            InputFieldSet?.TakeAllInputValues(inputValueTextColor, (cb) =>
            {
                NotificationLine.WriteNotificationLine(cb, callbackMessageColor, callbackLineColor, callbackPromptColor, "INPUT ALERT");
            }, collectValuesAsAllUppercase);
        }

        /// <summary>
        /// Calls a callback method of the developer's choosing on a page close
        /// </summary>
        /// <param name="closeCallback"></param>
        public void Close(Action closeCallback)
        {
            closeCallback();
        }

        private void InvalidMenuSelectionNotice()
        {
            NotificationLine.WriteNotificationLine("INVALID KEY PRESSED", ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.White, "NOTICE");
        }

        /// <summary>
        /// Inits the page. The default action is to call <see cref="BuildComponent"/> which adds components to the page
        /// </summary>
        public virtual void InitComponent()
        {
            BuildComponent();
        }

        /// <summary>
        /// Allows the developer to set the components for the page. NOTE: If you wish to display items after init is called you must use the
        /// <see cref="ShowPostInitItems"/> method. 
        /// </summary>
        protected virtual void BuildComponent() { }

        /// <summary>
        /// Builds display items specifically once data is passed into the ShowAndLoadDisplayItems
        /// method. This works similarly to BuildComponent, except the build occurs after the page
        /// initiation, allowing data to be passed into the page after creation as opposed to being
        /// restricted to the build phase
        /// </summary>
        protected virtual void BuildDisplayItemComponents() { }

        /// <summary>
        /// Returns an input value that matches the identifier ID
        /// IF: No ID is located within the inputs then an empty string will be returned. 
        /// </summary>
        /// <param name="identifierID"></param>
        /// <returns></returns>
        public virtual string GetInputValue(string identifierID)
        {
            return InputFieldSet?.GetValue(identifierID) ?? string.Empty;
        }

        /// <summary>
        /// Allows the developer to display items after the initial build has completed
        /// </summary>
        public virtual void ShowPostInitItems() { }

        /// <summary>
        /// Allows the developer to pass data into the page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public virtual void TakeInData<T>(T data) { }

        /// <summary>
        /// Allows the developer to pass secondary data into the page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public virtual void TakeInSecondaryData<T>(T data) { }
    }
}
