using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WPFThread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public record Person(string Name, string LastName, string SecondName);

        private void OpenFileMenu_Click(object Sender, RoutedEventArgs E)
        {
            var OpenDialog = new OpenFileDialog
            {
                Filter = "CSV (*.csv)|*.csv|Все файлы (*.*)|*.*",
                InitialDirectory = Environment.CurrentDirectory,
                Title = "Выбор файла для чтения"
            };

            if (OpenDialog.ShowDialog() != true) return;

            var FileName = OpenDialog.FileName;

            if (!File.Exists(FileName)) return;

            new Thread(() => LoadData(FileName)).Start();
        }

        private void LoadData(string FileName)
        {
            SaveToTXT(GetPersons(FileName));
            Application.Current.Dispatcher.Invoke(() => TextResult.Text = "Готово!");
        }

        private static IEnumerable<Person> GetPersons(string FileName)
        {
            var persons = new List<Person>();
            
            using(StreamReader reader = new StreamReader(FileName))
            {
                while (!reader.EndOfStream)
                {
                    var value = reader.ReadLine().Split(";");
                    persons.Add(new Person(value[0], value[1], value[2]));
                }
            }
            return persons;
        }

        private static void SaveToTXT(IEnumerable<Person> persons)
        {
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Test.txt", false, Encoding.UTF8))
            {
                foreach (var person in persons)
                    writer.WriteLine($"{person.Name} {person.LastName} {person.SecondName}");
            }
        }
    }
}
