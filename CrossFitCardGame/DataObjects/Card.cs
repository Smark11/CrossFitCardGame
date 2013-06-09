using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossFitCardGame.DataObjects
{
    public enum CardColor
    {
        Red,
        Black
    }
    public class Card : System.ComponentModel.INotifyPropertyChanged
    {
        public Card(int numberOfReps, string imageSource, CardColor color, int cardKey)
        {
            NumberOfReps = numberOfReps;
            ImageSource = imageSource;
            Color = color;
            CardKey = cardKey;
        }

        public Card()
        {
        }

        private bool _completed;
        public bool Completed
        {
            get { return _completed; }
            set { _completed = value; RaisePropertyChanged("Completed"); }
        }


        private int _cardKey;
        public int CardKey
        {
            get { return _cardKey; }
            set { _cardKey = value; RaisePropertyChanged("CardKey"); }
        }

        private CardColor _color;
        public CardColor Color
        {
            get { return _color; }
            set { _color = value; RaisePropertyChanged("Color"); }
        }

        private int _numberOfReps;
        public int NumberOfReps
        {
            get { return _numberOfReps; }
            set { _numberOfReps = value; RaisePropertyChanged("NumberOfReps"); }
        }

        private string _imageSource;
        public string ImageSource
        {
            get { return _imageSource; }
            set { _imageSource = value; RaisePropertyChanged("ImageSource"); }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(property));
            }
        }

        public override bool Equals(object obj)
        {
            if ((obj != null) && (obj.GetType() == typeof(PanoramaItem)))
            {
                var thePanoItem = (PanoramaItem)obj;

                return base.Equals(thePanoItem.Header);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
