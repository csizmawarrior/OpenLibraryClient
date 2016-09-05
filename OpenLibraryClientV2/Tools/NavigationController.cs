using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;


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
                
            }
        }

        public void NavigateTo(Type t, object vm)
        {
            
        }
    }
}
