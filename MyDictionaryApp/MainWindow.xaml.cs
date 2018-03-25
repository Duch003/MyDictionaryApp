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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyDictionaryApp.classes;
using Npgsql.PostgresTypes;

namespace MyDictionaryApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO Blokada edycji nie swoich słów
        //TODO Wszystkie ustawienia przypisane do użytkownika
        //TODO Dodać hasło dla użytkowników
        //SETTINGS
        private bool _muteAllSounds;                        //State of sounds
        private byte _timeForTimeChallange;                 //Time for time challange [ms]
        private uint _rndVocabSize;                        //Number of words in challange
        private bool _allowWholeDictionaryWithoutRegard;    //Switch -> true if use all vocabulary
        private byte _myId;                                 //Id of logged user

        public MainWindow(byte userid, byte rndVocabSize, bool soundState, uint time, bool wholeDictionary, ref DatabaseManager manager)
        {
            InitializeComponent();

            //Zapytanie o użytkownika

            //Załadowanie ustawień

            //Oczekiwanie na akcję
        }

        #region WhileLoadingMethods
        #endregion

        #region OnLoadMethods

        #endregion

        #region Events

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new Settings();
            settingsWindow.Show();
        }

        private void RandomVocabSet_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new MainGameBoard();
            gameWindow.Show();
        }

        private void ParticularCategory_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new MainGameBoard();
            gameWindow.Show();
        }

        private void TimeChallange_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new MainGameBoard();
            gameWindow.Show();
        }

        private void WholeDictionary_Click(object sender, RoutedEventArgs e)
        {
            var gameWindow = new MainGameBoard();
            gameWindow.Show();
        }

        private void EditDictionary_Click(object sender, RoutedEventArgs e)
        {
            var dictionaryManager = new DictionaryManager();
            dictionaryManager.Show();
        }

        private void EditCategories_Click(object sender, RoutedEventArgs e)
        {
            var categoryManager = new CategoryManager();
            categoryManager.Show();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new About();
            aboutWindow.Show();
        }

        #endregion
    }
}
