namespace ConsoleUIUtilities2
{
    /// <summary>
    /// Container that will hold a particular page type, control the display of the page, subscribe and unsubscribe from page events, 
    /// accept data validation, and other tasks associated with the consumption and disposition of data (if required)
    /// </summary>
    public class PageContainer<T> : IDisposable where T : Page
    {
        /// <summary>
        /// The page which is contained within the container
        /// </summary>
        protected T Page { get; private set; }

        /// <summary>
        /// Page break token. When set to false a page loop will continue, when set to true the page loop will break 
        /// </summary>
        protected bool PageBreak { get; set; } = false; 

        public PageContainer(T page)
        {
            Page = page;
            EventSubscriber(); 
        }

        /// <summary>
        /// Shows a page. NOTE: The default behavior is for the <see cref="ShowPage"/> method to show the page only and load all post init items. 
        /// Override this method to chnage that behavior
        /// </summary>
        public virtual void ShowPage()
        {
            Page.ShowPageOnly();
            Page.ShowPostInitItems(); 
        }

        /// <summary>
        /// Displays a page and runs that page in a loop until <see cref="PageBreak"/> is set to <see cref="true"/>. Page break can be called by the
        /// external method <see cref="BreakPageLoop"/>
        /// NOTE: The default behavior for this method is to show a page and then load all post init items
        /// </summary>
        public virtual void ShowPageInLoop()
        {
            while(!PageBreak)
            {
                Page.ShowPageOnly();
                Page.ShowPostInitItems(); 
            }
        }

        /// <summary>
        /// Breaks the page loop
        /// </summary>
        protected void BreakPage()
        {
            PageBreak = true; 
        }

        /// <summary>
        /// Displays a page and awaits an enter keypress to continue.
        /// NOTE: The default behavior for this method is to show a page and then load all post init items. 
        /// Override this method to change that behavior. 
        /// </summary>
        public virtual void ShowPageAndHoldForEnter()
        {
            Page.ShowPageOnly();
            Page.ShowPostInitItems();
            Console.ReadLine(); 
        }

        /// <summary>
        /// Subscription to page events. You will need to override this method to add necessary event subscribers.
        /// NOTE: Subscriber is called by the constructor
        /// </summary>
        protected virtual void EventSubscriber() { }

        /// <summary>
        /// Unsubscribes from page events. You will need to override this method to unsubscribe. 
        /// NOTE: Unsubscribe is also called during <see cref="Dispose"/>
        /// </summary>
        protected virtual void EventUnsubscriber() { }

        /// <summary>
        /// Disposes of all subscribed events
        /// </summary>
        public void Dispose()
        {
            EventUnsubscriber(); 
        }
    }
}
