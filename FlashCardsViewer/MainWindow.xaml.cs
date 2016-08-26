//-------------------------------------------------------------------
// 2016-17 All rights reserved. 
// Created by:	Omar Chughtai
//-------------------------------------------------------------------

using Microsoft.VisualBasic.FileIO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace FlashCardsViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region private variables
        private const string flashCardConstant = "FlashCard #"; //represents a flashcard by what number it is
        private const string flashCardNotSelected = "No Flash Card Selected!!";
        private bool urduSide = true; //denotes whether the urdu side is showing or english side is
        private int handle = 0; //represents position inside collection
        private MediaPlayer mp;       
        private ObservableCollection<FlashCardSet> dict; //collection of key values
        private object currentItem;
        private ObservableCollection<FlashCardSet> dictCopy;
        private string defaultFilePath = @"C:\Users\" + Environment.UserName + @"\Desktop\urdu_to_english.csv";
        private QuizWindow qw;
        private SearchWindow sw;
        private AddWindow ad;
        private bool wasMaximized = false;
        #endregion
        

        public MainWindow()
        {
            SplashScreen sc = new SplashScreen(@"Resources\pakistan_splash2.jpg");
            sc.Show(false);
            sc.Close(TimeSpan.FromSeconds(3));
            System.Threading.Thread.Sleep(3000);
            sc = null;
            InitializeComponent();
            mp = new MediaPlayer();
            dict = new ObservableCollection<FlashCardSet>();
            dictCopy = new ObservableCollection<FlashCardSet>();
            listBoxFlashcards.DataContext = dict;
            if (string.IsNullOrEmpty(Properties.Settings.Default.filePath))
                Properties.Settings.Default.filePath = defaultFilePath;
            Properties.Settings.Default.Save();                     
        }

        ~MainWindow()
        {
            //WriteListToCSVFile();        is destructor needed?   
        }


        /// <summary>
        /// Writes each flashcard english and urdu value from collection into specified file.
        /// </summary>
        private void WriteListToCSVFile()
        {
            using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.filePath))
            {
                sw.WriteLine("Urdu, English");
                foreach (FlashCardSet kvp in dict)
                {
                    sw.WriteLine(kvp.Value.UrduPhrase + ", " + kvp.Value.EnglishPhrase);
                }
            }
        }

        private void GetFlashCardsFromFile()
        {
            TextFieldParser parser;
            try
            {
                parser = new TextFieldParser(Properties.Settings.Default.filePath);
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadFields(); //skips first line
                while (!parser.EndOfData)
                {
                    string[] phrases = parser.ReadFields();
                    FlashCard fc = new FlashCard();
                    fc.UrduPhrase = phrases[0];
                    fc.EnglishPhrase = phrases[1];
                    dict.Add(new FlashCardSet() { Key = flashCardConstant + ++handle, Value = fc });
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
     
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (!File.Exists(Properties.Settings.Default.filePath))
            {
                using (StreamWriter sw = new StreamWriter(Properties.Settings.Default.filePath))
                {
                    sw.WriteLine("Urdu, English");
                    sw.WriteLine("ab" + ", " + "now");
                    sw.WriteLine("in" + ", " + "those");
                    sw.WriteLine("aisa" + ", " + "such");
                    sw.WriteLine("amir" + ", " + "rich");
                    sw.WriteLine("aur" + ", " + "and");
                    sw.WriteLine("mera" + ", " + "my");
                    sw.WriteLine("muhjhe" + ", " + "me");
                    sw.WriteLine("eik" + ", " + "one");
                    sw.WriteLine("nam" + ", " + "name");
                    sw.WriteLine("batie" + ", " + "sit down");
                    sw.WriteLine("kahan" + ", " + "where");
                    sw.WriteLine("hamara" + ", " + "our");
                    sw.WriteLine("ham" + ", " + "we");
                    sw.WriteLine("voh" + ", " + "he/she/it");
                    sw.WriteLine("yih" + ", " + "he/she/it");
                    sw.WriteLine("hal" + ", " + "condition");
                    sw.WriteLine("hai" + ", " + "is");
                    sw.WriteLine("larka" + ", " + "boy");
                    sw.WriteLine("larkee" + ", " + "girl");
                    sw.WriteLine("bachay" + ", " + "children");
                    sw.WriteLine("bacha" + ", " + "child");
                    sw.WriteLine("tum" + ", " + "you(informal)");
                    sw.WriteLine("ap" + ", " + "you(formal)");
                    sw.WriteLine("kitna" + ", " + "how many");
                    sw.WriteLine("sar" + ", " + "head");
                    sw.WriteLine("sayhat" + ", " + "health");
                    sw.WriteLine("kab" + ", " + "when");
                    sw.WriteLine("koi" + ", " + "someone");
                    sw.WriteLine("sunna" + ", " + "hear");
                    sw.WriteLine("suna" + ", " + "heard");
                    sw.WriteLine("batana" + ", " + "tell");
                    sw.WriteLine("naachna" + ", " + "dance");
                    sw.WriteLine("bemar" + ", " + "sick");
                    sw.WriteLine("kuch" + ", " + "some/something");
                }             
            }

            GetFlashCardsFromFile();
        }

        private void listBoxFlashcards_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem ==null)
            {
                e.Handled = true;
                return;
            }      
            //string lbi = listBoxFlashcards.SelectedItem.ToString();
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            this.txtBlockCardData.Text = urduSide ? dict.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase : dict.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase;           
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;
            currentItem = listBoxFlashcards.SelectedItem;            
        }

        private void Button_ShowFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;            
                return;
            }
            //string lbi = listBoxFlashcards.SelectedItem.ToString();
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            this.txtBlockCardData.Text = dict.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase.ToString();
            this.txtBlockCardData.Visibility = Visibility.Visible;
            this.flashCardBorder.Visibility = Visibility.Visible;           
        }

        private void Button_FlipFlashCard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            //string lbi = listBoxFlashcards.SelectedItem.ToString();
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            if (urduSide)
            {
                this.txtBlockCardData.Text = dict.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase.ToString();
                urduSide = false;
            }
            else
            {
                this.txtBlockCardData.Text = dict.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase.ToString();
                urduSide = true;
            }
            this.listBoxFlashcards.Focus();
      
        }

        private void Button_AddFlashCard(object sender, RoutedEventArgs e)
        {
            ad = new AddWindow();
            ad.Show();
            
            ad.AddFlashCardEvent += ad_AddFlashCardEvent;
        }

        void ad_AddFlashCardEvent(string urduWord, string englishWord)
        {
            if (dict.Select(x=>x.Value.UrduPhrase.ToLower()).Contains(urduWord))
            {
                System.Windows.MessageBox.Show(urduWord + " already exists in flash card set!");        
                return;
            }
            FlashCard fc = new FlashCard();
            fc.UrduPhrase = urduWord;
            fc.EnglishPhrase = englishWord;
            dict.Add(new FlashCardSet() { Key = flashCardConstant + ++handle, Value = fc });
            ad.Close();   
        }

        private void Button_DeleteFlascard(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem==null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            //string key = listBoxFlashcards.SelectedItem.ToString();
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            dict.Remove(dict.First(x => x.Key == flashCardItem.Key));

            
            FlashCardSet[] dictCopy=new FlashCardSet[dict.Count()];
            dict.CopyTo(dictCopy,0);
            dict.Clear();
            handle = 0;
            foreach(FlashCardSet kvp in dictCopy)
            {
                dict.Add(new FlashCardSet() { Key = flashCardConstant + ++handle, Value = new FlashCard() { UrduPhrase = kvp.Value.UrduPhrase, EnglishPhrase = kvp.Value.EnglishPhrase } });
                
            }
            this.txtBlockCardData.Visibility = Visibility.Hidden;
            this.flashCardBorder.Visibility = Visibility.Hidden;           
        }

        private void Button_Edit(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            FlashCardSet kvp = listBoxFlashcards.SelectedItem as FlashCardSet;                        
            EditWindow ew = new EditWindow(kvp);
            ew.Show();

            ew.ApplyChangeEvent += ew_ApplyChangeEvent;
        }

        void ew_ApplyChangeEvent(string text1, string text2)
        {
            //string key = listBoxFlashcards.SelectedItem.ToString();
            FlashCardSet flashCardItem = (FlashCardSet)listBoxFlashcards.SelectedItem; 
            dict.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase = text1;
            dict.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase = text2;
            this.txtBlockCardData.Text = urduSide ? dict.Single(x => x.Key == flashCardItem.Key).Value.UrduPhrase : dict.Single(x => x.Key == flashCardItem.Key).Value.EnglishPhrase;
        }

        private void Button_Speak(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }

            SpeechSynthesizer speech = new SpeechSynthesizer();
            
            FlashCardSet kvp = listBoxFlashcards.SelectedItem as FlashCardSet;

            if (urduSide)
                speech.Speak(kvp.Value.UrduPhrase);
            else
                speech.Speak(kvp.Value.EnglishPhrase);
        }      

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double multiplier;
            double oldHeight;
            double oldWidth;          
            oldHeight=(double)e.PreviousSize.Height;
            oldWidth = (double)e.PreviousSize.Width;
            if (oldWidth == 0)
                return;
         
            if (oldWidth < e.NewSize.Width || oldHeight < e.NewSize.Height)
            {
               
                multiplier = FindMultiplier(oldHeight, oldWidth, e.NewSize.Height, e.NewSize.Width);
                txtBlockCardData.FontSize *= multiplier;

                if (txtBlockCardData.FontSize >= 140)
                {
                    txtBlockCardData.FontSize = 140;
                    return;
                }

            }
            else if (oldHeight > e.NewSize.Height || oldWidth > e.NewSize.Width)
            {
              
                multiplier = FindMultiplier(oldHeight, oldWidth, e.NewSize.Height, e.NewSize.Width);
                txtBlockCardData.FontSize *= multiplier;

                if (txtBlockCardData.FontSize <= 78)
                {
                    txtBlockCardData.FontSize = 78;
                    return;
                }          
            }
        } 

        private double FindMultiplier(double oldHeight, double oldWidth, double newHeight, double newWidth)
        {
          
            double oldArea = oldHeight * oldWidth;
            double newArea = newHeight * newWidth;

            double multiplier = newArea / oldArea;
           
            return multiplier;

        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }

            FlashCardSet item = listBoxFlashcards.SelectedItem as FlashCardSet;
            int currentPosition = dict.IndexOf(item);
            if (currentPosition == dict.Count()-1)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(0);
            }
            else
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(currentPosition + 1);
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxFlashcards.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show(flashCardNotSelected);
                e.Handled = true;
                return;
            }
            FlashCardSet item = listBoxFlashcards.SelectedItem as FlashCardSet;
            int currentPosition = dict.IndexOf(item);
            if (currentPosition == 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(dict.Count()-1);
            }
            else
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(currentPosition -1);
            }
        }

        private void RandomButton_Click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            if (dict != null)
            {
                 int randomnumber=0;
                 if (dict.Count() > 0)
                 {
                     randomnumber = r.Next(dict.Count());
                     listBoxFlashcards.SelectedItem = dict.ElementAt(randomnumber);
                 }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!!");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (dict != null)
            {
                
           DialogResult result=System.Windows.Forms.MessageBox.Show("Do you want to save this list?", "Save List",MessageBoxButtons.YesNo );
                
            if (result==System.Windows.Forms.DialogResult.Yes)
            {
                foreach (FlashCardSet k in dict)
                {
                    dictCopy.Add(k);
                }
            }
                dict.Clear();               
            }            
        }

        private void RetrieveButton_Click(object sender, RoutedEventArgs e)
        {
            if (dictCopy != null)
               foreach(FlashCardSet x in dictCopy)
               {
                   dict.Add(x);
               }
            else
            {
                System.Windows.Forms.MessageBox.Show("No list stored!");
            }
        }

       private void FirstButton_Click(object sender, RoutedEventArgs e)
       {
            if (dict !=null && dict.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(0);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void LastButton_Click(object sender, RoutedEventArgs e)
       {
            if (dict != null && dict.Count() > 0)
            {
                listBoxFlashcards.SelectedItem = dict.ElementAt(dict.Count() - 1);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No flashcards available!");
            }
       }

       private void TakeQuizButton_Click(object sender, RoutedEventArgs e)
       {
         
           if (dict != null)
           {
               if (dict.Count() >= 10)
               {
                   Dictionary<string, string> randomFlashCards = DrawRandomCards();
                   qw = new QuizWindow();
                   qw.Show();
                   qw.GenerateQuestions(randomFlashCards);
                   qw.GenerateRandomCardsEvent += qw_GenerateRandomCardsEvent;
               }
               else
               {
                   System.Windows.Forms.MessageBox.Show("Need at least 10 flashcards to take quiz!!");
               }
           }          
       }

       void qw_GenerateRandomCardsEvent()
       {
          Dictionary<string,string> randomCards= DrawRandomCards();
          qw.GenerateQuestions(randomCards);
       }
        
        private Dictionary<string,string> DrawRandomCards()
        {
            Random r = new Random();
            int[] randomNumbers = new int[10];
            for (int i = 0; i < 10; i++)
            {
                int temp = r.Next(dict.Count());
                while (randomNumbers.Contains(temp))
                {
                    temp = r.Next(dict.Count());
                }
                randomNumbers[i] = temp;
            }

            Dictionary<string, string> randomFlashCards = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                FlashCard fs = dict.ElementAt(randomNumbers[i]).Value;
                randomFlashCards.Add(fs.UrduPhrase, fs.EnglishPhrase);
            }

            return randomFlashCards;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {        
            sw = new SearchWindow();            
            sw.Show();
            sw.searchforcardevent += sw_searchforcardevent;
        }

        void sw_searchforcardevent(string englishphrase)
        {
            bool containsPhrase=dict.Select(x => x.Value.EnglishPhrase.ToLower()).Contains(englishphrase.ToLower());
            if (containsPhrase)
            {
                urduSide = false;
                listBoxFlashcards.SelectedItem = dict.Single(x => x.Value.EnglishPhrase.ToLower() == englishphrase.ToLower());
                sw.Close();
            }     
            else
            {
                System.Windows.MessageBox.Show("word/phrase not found!");
            }
        }

        private void SaveListButton_Click(object sender, RoutedEventArgs e)
        {
            WriteListToCSVFile();
            System.Windows.MessageBox.Show("List Saved!");
        }
    }

    /// <summary>
    /// Object represents a key representing Flashcard Number, and value representing Flashcard object(contains urdu and english phrase strings).
    /// </summary>
    public class FlashCardSet
    {
        public string Key
        {
            get;
            set;
        }

        public FlashCard Value
        {
            get;
            set;
        }

        //public override string ToString()
        //{

        //    {
        //        return Key;
        //    }
        //}
    }
}
