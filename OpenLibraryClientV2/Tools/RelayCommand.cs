using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenLibraryClientV2.Tools
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Predicate<object> CanExecuteDelegate
        {
            get;
            set;
        }

        public Action<object> ExecuteDelegate
        {
            get;
            set;
        }

        public RelayCommand(Action<object> action)
        {
            ExecuteDelegate = action;
        }

        bool ICommand.CanExecute(object parameter)
        {
            if (CanExecuteDelegate != null)
            {
                return CanExecuteDelegate(parameter);
            }

            return true;
        }

        void ICommand.Execute(object parameter)
        {
            if (ExecuteDelegate != null)
            {
                ExecuteDelegate(parameter);
            }
        }
    }
}
