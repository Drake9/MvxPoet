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

            string content = File.ReadAllText(selectedFilePath);

            Title.Text = content.Substring(0, content.IndexOf("\n\n"));
            Text.Text = content.Substring(content.IndexOf("\n\n") + 2);
        }
    }
}
