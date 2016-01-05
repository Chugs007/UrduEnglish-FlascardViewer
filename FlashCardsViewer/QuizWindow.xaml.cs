using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for QuizWindow.xaml
    /// </summary>
    public partial class QuizWindow : Window
    {
        public delegate void GenerateRandomCards();

        public event GenerateRandomCards GenerateRandomCardsEvent;

        private Dictionary<string, string> flashCards;
        private SolidColorBrush correctAnswerBrush = new SolidColorBrush(Colors.Transparent);
        private SolidColorBrush wrongAnswerBrush = new SolidColorBrush(Colors.Red);
        private int numberCorrect = 0;

        public QuizWindow()
        {
            InitializeComponent();
        }


        public void GenerateQuestions(Dictionary<string,string> randomCards)
        {
            flashCards = randomCards;
            textBlockQuestion1.Text = randomCards.ElementAt(0).Key;
            textBlockQuestion2.Text = randomCards.ElementAt(1).Key;
            textBlockQuestion3.Text = randomCards.ElementAt(2).Key;
            textBlockQuestion4.Text = randomCards.ElementAt(3).Key;
            textBlockQuestion5.Text = randomCards.ElementAt(4).Key;
            textBlockQuestion6.Text = randomCards.ElementAt(5).Key;
            textBlockQuestion7.Text = randomCards.ElementAt(6).Key;
            textBlockQuestion8.Text = randomCards.ElementAt(7).Key;
            textBlockQuestion9.Text = randomCards.ElementAt(8).Key;
            textBlockQuestion10.Text = randomCards.ElementAt(9).Key;
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (textboxAnswer1.Text != flashCards.ElementAt(0).Value)
            {
                textboxAnswer1.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer1.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer2.Text != flashCards.ElementAt(1).Value)
            {
                textboxAnswer2.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer2.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer3.Text != flashCards.ElementAt(2).Value)
            {
                textboxAnswer3.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer3.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer4.Text != flashCards.ElementAt(3).Value)
            {
                textboxAnswer4.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer4.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer5.Text != flashCards.ElementAt(4).Value)
            {
                textboxAnswer5.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer5.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer6.Text != flashCards.ElementAt(5).Value)
            {
                textboxAnswer6.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer6.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer7.Text != flashCards.ElementAt(6).Value)
            {
                textboxAnswer7.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer7.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer8.Text != flashCards.ElementAt(7).Value)
            {
                textboxAnswer8.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer8.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer9.Text != flashCards.ElementAt(8).Value)
            {
                textboxAnswer9.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer9.Background = correctAnswerBrush;
                numberCorrect++;
            }

            if (textboxAnswer10.Text != flashCards.ElementAt(9).Value)
            {
                textboxAnswer10.Background = wrongAnswerBrush;
            }
            else
            {
                textboxAnswer10.Background = correctAnswerBrush;
                numberCorrect++;
            }
             System.Windows.Forms.MessageBox.Show("You got " + numberCorrect + " out of 10 correct!");

             numberCorrect = 0;
        }

        private void buttonGenerateQuiz_Click(object sender, RoutedEventArgs e)
        {
          if (GenerateRandomCardsEvent !=null)
          {
              GenerateRandomCardsEvent();
          }
        }
    }
}
