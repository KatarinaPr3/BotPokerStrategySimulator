using System.ComponentModel;
using System.Windows;

namespace PokerTable.View
{
    public class ViewModelBase2 : INotifyPropertyChanged
    {
        public ViewModelBase2()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnFirstPropertyChanged(
   DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            if (h != null)
            {
                h(sender, new PropertyChangedEventArgs("Second"));
            }
        }
    }

    }