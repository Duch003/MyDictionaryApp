using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using MyDictionaryApp.classes;

namespace MyDictionaryApp
{
    public partial class LogInWindowxaml : Window
    {
        private DatabaseManager _manager;
        public LogInWindowxaml()
        {
            InitializeComponent();
            _manager = new DatabaseManager("192.168.1.199", "Duch003", "", "5432", "Dictionary");
            try
            {
                DatabaseManager.Connect();
            }
            catch (Exception e)

            {
                MessageBox.Show(e.Message, "Connection error.", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(-1);
            }
        }
        internal bool Filter(string raw)
        {
            var forbidden = new[]
            {
                '\'',
                '-',
                ':',
                '\\',
                '/',
                '"',
                '!',
                '@',
                '#',
                '$',
                '%',
                '^',
                '&',
                '*',
                '(',
                ')',
                '+',
                '{',
                '}',
                '|'
            };
            return forbidden.All(z => !raw.Contains(z));
        }

        internal string HashString(string arg)
        {
            var bytes = Encoding.UTF8.GetBytes(arg);
            var encode = new SHA256Managed();
            var hash = encode.ComputeHash(bytes);

            return hash.Aggregate("", (current, z) => current + $"{z}");
        }

        internal DataSet SignIn(string nick, string password)
        {
            //Checking length of nickname and password
            if (string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtUser.Text))
            {
                MessageBox.Show("User or password field is empty. Please fill filed with Your nickname and password.", "Can't create new user.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            //Checking if nickname or password contains forbidden characters
            if (!Filter(txtUser.Text) || !Filter(txtPassword.Password))
            {
                MessageBox.Show("Your nickname or password contains forbidden characters. Please remove them and try again.", "Can't create new user.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            //Checking if user already exists
            var check = DatabaseManager.Query("user_name", "users", $"user_name = {txtUser.Text}");

            if (check.Tables[0].Rows.Count == 0)
            {
                //TODO dopisać wszystkie ustawienia
                var _password = HashString(password);
                DatabaseManager.Insert("users", $"default,'{txtUser.Text}, '{_password}'");
                return DatabaseManager.Query("*", "users", $"WHERE user_name = {txtUser.Text} AND user_password = {_password}");
            }

            MessageBox.Show("User already exists. Choose another nickname.", "Can't create new user.",
                MessageBoxButton.OK, MessageBoxImage.Information);
            return null;
        }

        internal DataSet SignUp(string nick, string password)
        {
            //Checking length of nickname and password
            if (string.IsNullOrEmpty(txtPassword.Password) || string.IsNullOrEmpty(txtUser.Text))
            {
                MessageBox.Show("User or password field is empty. Please fill filed with Your nickname and password.", "Can't create new user.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            //Checking if nickname or password contains forbidden characters
            if (!Filter(txtUser.Text) || !Filter(txtPassword.Password))
            {
                MessageBox.Show("Your nickname or password contains forbidden characters. Please remove them and try again.", "Can't create new user.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }
            //Checking if nickname and password are correct
            var check = DatabaseManager.Query("user_name, user_password", "users", $"user_name = {txtUser.Text} AND user_password = {HashString(txtPassword.Password)}");
            if (check.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("Wrong nickname or password.", "Wrong nickname or password.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return null;
            }

            var _password = HashString(password);
            DatabaseManager.Insert("users", $"default, '{txtUser.Text}', '{_password}', 0, false, 5000, false");
            return DatabaseManager.Query("*", "users", $"WHERE user_name = {txtUser.Text} AND user_password = {_password}");
        }

        //SINGING UP
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            var temp = SignUp(txtUser.Text, txtPassword.Password);
            if (temp == null) return;
            else
            {
                var userid = Convert.ToByte(temp.Tables[0].Rows[0].Field<string>("id"));
                var setting_RndVocabSize = Convert.ToByte(temp.Tables[0].Rows[0].Field<string>("RndVocabSize"));
                var setting_MuteSounds = Convert.ToBoolean(temp.Tables[0].Rows[0].Field<string>("MuteSounds"));
                var setting_Time = Convert.ToUInt32(temp.Tables[0].Rows[0].Field<string>("Time"));
                var wholeDictionary = Convert.ToBoolean(temp.Tables[0].Rows[0].Field<string>("AllowWholeVocab"));
                MainWindow window = new MainWindow(userid, setting_RndVocabSize, setting_MuteSounds, setting_Time,
                    wholeDictionary, ref _manager);
                Close();
            }
        }

        //SINGING IN
        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            var temp = SignIn(txtUser.Text, txtPassword.Password);
            if (temp == null) return;
            else
            {
                var userid = Convert.ToByte(temp.Tables[0].Rows[0].Field<string>("id"));
                var setting_RndVocabSize = Convert.ToByte(temp.Tables[0].Rows[0].Field<string>("RndVocabSize"));
                var setting_MuteSounds = Convert.ToBoolean(temp.Tables[0].Rows[0].Field<string>("MuteSounds"));
                var setting_Time = Convert.ToUInt32(temp.Tables[0].Rows[0].Field<string>("Time"));
                var wholeDictionary = Convert.ToBoolean(temp.Tables[0].Rows[0].Field<string>("AllowWholeVocab"));
                MainWindow window = new MainWindow(userid, setting_RndVocabSize, setting_MuteSounds, setting_Time,
                    wholeDictionary, ref _manager);
                Close();
            }
        }
    }
}
