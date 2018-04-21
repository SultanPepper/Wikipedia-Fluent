using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wikipedia_Fluent.Models
{
    public class DataBinding : INotifyPropertyChanged
    {

        public string PageTitleText
        {
            get { return _pageTitleText; }
            set { Set(ref _pageTitleText, value); }
        }

        private string _pageTitleText;

        public DataBinding()
        {
            PageTitleText = "pagetitle";
        }

        public static DataBinding CreateTitleText(string titletext)
        {
            return new DataBinding { PageTitleText = titletext };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            //? mark here \/ represents to invoke this method if not null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }



    }
}