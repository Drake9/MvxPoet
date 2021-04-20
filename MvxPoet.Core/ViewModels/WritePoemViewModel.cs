using System.Collections.ObjectModel;
using MvvmCross.ViewModels;
using MvvmCross.Commands;
using MvxPoet.Core.Models;
using System;
using System.IO;
using System.Collections.Generic;

namespace MvxPoet.Core.ViewModels
{
    public class WritePoemViewModel : MvxViewModel
    {
        public WritePoemViewModel()
        {
            WriteToFileCommand = new MvxCommand(WriteToFile);
        }

        public IMvxCommand WriteToFileCommand { get; set; }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { 
                SetProperty(ref _title, value);
                RaisePropertyChanged(() => CanWriteToFile);
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { 
                SetProperty(ref _text, value);
                RaisePropertyChanged(() => TextLenght);

                SetLinesEndingsAndNumbersOfSyllables();
                SetRhymes();

                RaisePropertyChanged(() => LinesEndings);
                RaisePropertyChanged(() => NumbersOfSyllables);
                RaisePropertyChanged(() => Rhymes);
                RaisePropertyChanged(() => CanWriteToFile);
            }
        }

        public int TextLenght => Text.Length;

        public void WriteToFile()
        {
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, Title + ".txt")))
            {
                    outputFile.WriteLine(Title + "\n\n" + Text);
            }
        }

        public bool CanWriteToFile => Title?.Length > 0 && Text?.Length > 0;

        private ObservableCollection<string> _linesEndings = new ObservableCollection<string>();
        public ObservableCollection<string> LinesEndings
        {
            get { return _linesEndings; }
        }

        private void SetLinesEndingsAndNumbersOfSyllables()
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                var lines = Text.Split('\n');
                _linesEndings.Clear();
                _numbersOfSyllables.Clear();
                _rhymes.Clear();

                for (int i = 0; i < lines.Length; i++)
                {
                    _linesEndings.Add(VerseModel.GetLastSyllables(lines[i]));
                    _numbersOfSyllables.Add(VerseModel.NumberOfSyllables(lines[i]));
                }
            }
        }

        private ObservableCollection<int?> _numbersOfSyllables = new ObservableCollection<int?>();

        public ObservableCollection<int?> NumbersOfSyllables
        {
            get { return _numbersOfSyllables; }
        }


        private ObservableCollection<char> _rhymes = new ObservableCollection<char>();

        public ObservableCollection<char> Rhymes
        {
            get { return _rhymes; }
        }

        private void SetRhymes()
        {
            if (LinesEndings.Count == 0)
                return;

            char mark = 'A';

            int index = 0;

            List<int> observedLines = new List<int>();

            while (index < LinesEndings.Count)
            {
                if (_linesEndings[index] == "")
                {
                    observedLines.Clear();
                    _rhymes.Add(' ');
                    mark = 'A';
                    index++;
                    continue;
                }

                _rhymes.Add('?');

                foreach (int observedIndex in observedLines)
                {
                    if (VerseModel.DoesItRhyme(_linesEndings[index], _linesEndings[observedIndex]) == Rhyme.FEMININE)
                    {
                        if (_rhymes[observedIndex] != '?')
                        {
                            _rhymes[index] = _rhymes[observedIndex];
                        }
                        else
                        {
                            _rhymes[observedIndex] = mark;
                            _rhymes[index] = mark;
                            mark += (char)1;
                        }
                    }
                    else if (VerseModel.DoesItRhyme(_linesEndings[index], _linesEndings[observedIndex]) == Rhyme.MASCULINE)
                    {
                        if (_rhymes[observedIndex] != '?')
                        {
                            _rhymes[index] = _rhymes[observedIndex];
                        }
                        else
                        {
                            _rhymes[observedIndex] = char.ToLower(mark);
                            _rhymes[index] = char.ToLower(mark);
                            mark += (char)1;
                        }
                    }
                }

                observedLines.Add(index);
                index++;
            }
        }

    }
}
