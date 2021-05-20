using MvvmCross.Platforms.Wpf.Views;
using Microsoft.Win32;
using System;
using System.IO;

namespace MvxPoet.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy WritePoemView.xaml
    /// </summary>
    public partial class WritePoemView : MvxWpfView
    {
        public WritePoemView()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowDialog();
            string selectedFilePath = dialog.FileName;

            Title.Clear();
            Text.Clear();

            try {
                string content = File.ReadAllText(selectedFilePath);

                Title.Text = content.Substring(0, content.IndexOf("\n\n"));
                Text.Text = content.Substring(content.IndexOf("\n\n") + 2);
            } catch
            {

            }
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.FileName = Title.Text;
            dialog.Title = "Zapisz wiersz do pliku:";
            dialog.Filter = "Dokument tekstowy(.txt)| *.txt";
            dialog.DefaultExt = ".txt";

            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(dialog.FileName)))
                {
                    outputFile.Write(Title.Text + "\n\n" + Text.Text);
                }
            }
        }
    }
}
