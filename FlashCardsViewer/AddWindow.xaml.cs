using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlashCardsViewer
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public delegate void AddFlashCard(string urduWord, string englishWord);
        public event AddFlashCard AddFlashCardEvent;

        public AddWindow()
        {
            InitializeComponent();
            this.KeyDown += AddWindow_KeyDown;         
        }

        void AddWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }



        private void Button_Add(object sender, RoutedEventArgs e)
        {            
            if (AddFlashCardEvent != null)
                AddFlashCardEvent(this.txtBoxUrduWord.Text, this.txtBoxEnglishWord.Text);
            
        }
    }
}
