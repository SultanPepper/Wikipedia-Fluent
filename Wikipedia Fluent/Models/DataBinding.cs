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
        private string pageTitleText;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };


        public DataBinding()
        {
            this.PageTitleText = "Text";
        }
        
        public string PageTitleText
        {
            get { return this.pageTitleText; }
            set
            {
                this.pageTitleText = value;
                this.OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}