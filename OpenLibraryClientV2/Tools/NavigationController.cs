using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace OpenLibraryClientV2.Tools
{
    class NavigationController
    {
        private static volatile NavigationController instance;
        private static object syncRoot = new object();

        private NavigationController() { }
        
        public static NavigationController GetInstance()
        {
            if (instance == null)
            {
                lock(syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new NavigationController();
                    }
                }
            }

            return instance;
        }

        private Frame _frame;
        public Frame Frame
        {
            get
            {
                return _frame;
            }

            set
            {
                if (_frame != null)
                {
                    _frame.Navigated -= _navService_Navigated;
                }

                _frame = value;
                _frame.Navigated += _navService_Navigated;
            }
        }

        public bool TryGoBack()
        {
            if (_frame.CanGoBack)
            {
                Frame.GoBack();
                return true;
            }

            return false;
        }

        public void Navigate(Type page, object context)
        {
            if (_frame == null || page == null)
            {
                return;
            }
            
            _frame.Navigate(page, context);
        }

        private void _navService_Navigated(object sender, NavigationEventArgs args)
        {
            var page = args.Content as Page;

            if (page == null)
            {
                return;
            }

            if (args.Parameter.GetType() != typeof(string))
            {
                page.DataContext = args.Parameter;
            }
        }
    }
}
