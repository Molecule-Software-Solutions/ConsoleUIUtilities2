using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUIUtilities2
{
    public class SelectionMenu<T>
    {
        public List<SelectionItem<T>>? Items { get; set; }
        public int TopOfSelectionScreen { get; init; }
        public int BottomOfSelectionScreen { get; init; }
        public int TopOfSelectionItems => TopOfSelectionScreen + 1;
        public int BottomOfSelectionItems => BottomOfSelectionScreen - 1;
        public ConsoleColor ItemForegroundColor { get; private set; }
        public ConsoleColor SelectionBackgroundColor { get; private set; }
        public bool Finalized { get; private set; }

        public SelectionMenu(int topOfSelectionScreen = 6, int bottomOfSelectionScreen = 28, ConsoleColor itemForeground = ConsoleColor.White, ConsoleColor selectionBackgroundColor = ConsoleColor.DarkBlue)
        {
            Items = new List<SelectionItem<T>>();
            TopOfSelectionScreen = topOfSelectionScreen; 
            BottomOfSelectionScreen = bottomOfSelectionScreen;
            ItemForegroundColor = itemForeground; 
            SelectionBackgroundColor = selectionBackgroundColor;
        }

        private void BeginMonitor()
        {
            WriteOptions();     
        }

        /// <summary>
        /// Will write options to the screen and will finalize the item selection idnex
        /// </summary>
        private void WriteOptions()
        {
            if(!Finalized)
            {
                // Guard
                if(Items is null)
                {
                    return; 
                }

                // Clears the writing area
                ClearWritingArea(); 

                // Finalize all items
                ItemsFinalize();

                Items[0].ChangeSelection(true); 

                int itemsThatFit = BottomOfSelectionItems - TopOfSelectionItems;
                foreach (var item in Items)
                {
                    if(item.SelectionItemIndex >= 0 && item.SelectionItemIndex <= itemsThatFit)
                    {
                        Console.SetCursorPosition(5, TopOfSelectionItems + item.SelectionItemIndex);
                        Console.ForegroundColor = ItemForegroundColor; 
                        if(item.IsSelected)
                        {
                            Console.BackgroundColor = SelectionBackgroundColor; 
                        }
                        Console.Write(item.DisplayText);
                        Console.ResetColor(); 
                    }
                }
                CenteredLine.PrintToConsole("Press ENTER to select, Press UP ARROW and DOWN ARROW to navigate. Press ESC to exit", BottomOfSelectionScreen + 1, ConsoleColor.Yellow); 
            }
        }

        private void ItemsFinalize()
        {
            // Guard 
            if(Items is null)
            {
                return; 
            }

            int index = 0;
            foreach (var item in Items)
            {
                item.SetFinalizingIndex(index);
                index += 1; 
            }
        }

        private void PopItems()
        {
            // Guard
            if(Items is null)
            {
                return; 
            }

            foreach (var item in Items)
            {
                item.Popped(); 
            }
        }

        private void PushItems()
        {
            // Guard
            if(Items is null)
            {
                return; 
            }

            foreach (var item in Items)
            {
                item.Pushed(); 
            }
        }

        private void ClearWritingArea()
        {
            for (int i = TopOfSelectionItems; i < BottomOfSelectionItems; i++)
            {
                HorizontalLine.WriteHorizontalLine(' ', i, ConsoleColor.Black); 
            }
        }

        public void DrawSelectionScreen(string headerText, char boundaryChar = '-', ConsoleColor boundaryLineColor = ConsoleColor.White, bool printHeader = false, Header? header = null)
        {
            // Clear the console window
            ConsoleBufferSystem.ClearBuffer();

            // Prints header if one is supplied
            if(printHeader)
            {
                header?.WriteHeader(0, boundaryLineColor, ConsoleColor.White); 
            }

            // Header inserted
            CenteredLine.PrintToConsole(headerText, TopOfSelectionScreen - 1, boundaryLineColor); 

            // Draw horizontal boundaries
            HorizontalLine.WriteHorizontalLine(boundaryChar, TopOfSelectionScreen, boundaryLineColor); 
            HorizontalLine.WriteHorizontalLine(boundaryChar, BottomOfSelectionScreen, boundaryLineColor);

            // Begin Monitoring
            BeginMonitor(); 
            Console.ReadLine(); 
        }

        public void AddSelectionItem(SelectionItem<T> item)
        {
            if (!Finalized)
            {
                Items?.Add(item);
            }
            else throw new Exception("You cannot add items after the indexing is finalized. Use Refresh instead"); 
        }

        public void AddSelectionItemRange(SelectionItem<T>[] items)
        {
            if(!Finalized)
            {
                Items?.AddRange(items); 
            }
            else throw new Exception("You cannot add items after the indexing is finalized. Use Refresh instead");
        }

        public T? GetSelectedItem()
        {
            var value = Items?.Where(i => i.IsSelected).FirstOrDefault();
            if (value is not null)
            {
                return value.Value;
            }
            else return default(T); 
        }

        public void RemoveSelectionItem(string identifier)
        {
            if (!Finalized)
            {
                var value = Items?.Where(i => i.Identifier == identifier).FirstOrDefault();
                if (value is not null)
                {
                    Items?.Remove(value);
                }
            }
            else throw new Exception("You cannot remove items after the indexing is finalized."); 
        }

        public void SelectItem(string identifier)
        {
            if(Items is not null)
            {
                foreach (SelectionItem<T> item in Items)
                {
                    item.ChangeSelection(false); 
                }

                var value = Items.Where(i => i.Identifier != identifier).FirstOrDefault();
                if(value is not null)
                {
                    value.ChangeSelection(true); 
                }
            }
        }
    }
}
