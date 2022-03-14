namespace ConsoleUIUtilities2
{
    public abstract class Page
    {
        public string? Title { get; private set; }
        public Header? Header { get; private set; }
        public Menu? Menu { get; private set; }
        public InputFieldSet? InputFieldSet { get; private set; }

        public void SetTitle(string title)
        {
            Title = title; 
        }

        public void SetHeader(Header? header = null)
        {
            if(header is not null)
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

        public Page(string title = "", Header? header = null, InputFieldSet? inputFieldSet = null, Menu? menu = null)
        {
            Title = title;
            Header = header;
            InputFieldSet = inputFieldSet;
            Menu = menu;
        }

        public void ShowPageOnly(int headerRow = 0, ConsoleColor headerLineColor = ConsoleColor.White, ConsoleColor headerTextColor = ConsoleColor.White)
        {
            ConsoleBufferSystem.ClearBuffer(); 
            if(Header is not null)
            {
                Header.WriteHeader(headerRow, headerLineColor, headerTextColor); 
            }
            else
            {
                Header = new Header();
                Header.SetTopAndBottomLineChars('-');
                Header.AddHeaderLine(Title?? string.Empty);
                Header.WriteHeader(headerRow, headerLineColor, headerTextColor); 
            }
        }

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
            }, InvalidMenuSelectionNotice, onMenuBreakCallPageRedraw); 
        }

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
            InputFieldSet?.WriteMultipleInputs(inputStartRow, inputJustification, inputPromptTextColor);
            InputFieldSet?.TakeAllInputValues(inputValueTextColor, (cb) =>
            {
                NotificationLine.WriteNotificationLine(cb, callbackMessageColor, callbackLineColor, callbackPromptColor, "INPUT ALERT"); 
            }, collectValuesAsAllUppercase); 
        }

        public void Close(Action closeCallback)
        {
            closeCallback(); 
        }

        private void InvalidMenuSelectionNotice()
        {
            NotificationLine.WriteNotificationLine("INVALID KEY PRESSED", ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.White, "NOTICE");
        }

        public abstract void InitComponent();
        protected abstract void BuildComponent(); 
    }
}
