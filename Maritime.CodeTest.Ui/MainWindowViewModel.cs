using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using Maritime.CodeTest.Ui.Annotations;

namespace Maritime.CodeTest.Ui
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Create a MainWindowViewModel
        /// </summary>
        public MainWindowViewModel()
        {
            SetWorkingFileCommand = new RelayCommand<object>(
                (param) => SelectWorkingFile(), 
                (param) => true);
            OpenWorkingFileCommand = new RelayCommand<string>(
                (param) => OpenWorkingFile(param),
                (param) => !string.IsNullOrEmpty(FilePath));
            CalculateStatisticsCommand = new RelayCommand<FileReadResult<decimal>>(
                (param) => CalculateStatistics(param),
                (param) => param != null && param.ReadResult == ReadResult.FileReadOk);
        }

        #region Relay Commands
        
        private RelayCommand<object> _setWorkingFileCommand;
        private RelayCommand<string> _openWorkingFileCommand;
        private RelayCommand<FileReadResult<decimal>> _calculateStatisticsCommand;

        public RelayCommand<object> SetWorkingFileCommand
        {
            get { return _setWorkingFileCommand; }
            set
            {
                _setWorkingFileCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<string> OpenWorkingFileCommand
        {
            get { return _openWorkingFileCommand; }
            set
            {
                _openWorkingFileCommand = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<FileReadResult<decimal>> CalculateStatisticsCommand
        {
            get { return _calculateStatisticsCommand; }
            set
            {
                _calculateStatisticsCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion
        
        #region Properties

        private string _filePath;
        private FileReadResult<decimal> _fileReadResult;
        private decimal _arithmeticMean;
        private decimal _standardDeviation;
        private List<Bin> _frequency;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }

        public FileReadResult<decimal> FileReadResult
        {
            get { return _fileReadResult; }
            set
            {
                _fileReadResult = value;
                OnPropertyChanged();
            }
        }

        public decimal ArithmeticMean
        {
            get { return _arithmeticMean; }
            set
            {
                _arithmeticMean = value;
                OnPropertyChanged();
            }
        }

        public decimal StandardDeviation
        {
            get { return _standardDeviation; }
            set
            {
                _standardDeviation = value;
                OnPropertyChanged();
            }
        }

        public List<Bin> Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Set the working file
        /// </summary>
        public void SelectWorkingFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            var result = ofd.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrEmpty(ofd.SafeFileName))
            {
                FilePath = ofd.FileName;
            }
        }

        /// <summary>
        /// Open the working file and set the file read result
        /// </summary>
        /// <param name="path"></param>
        public void OpenWorkingFile(string path)
        {
            var reader = new FileReader();
            FileReadResult = reader.ReadAllLines(path);
        }

        /// <summary>
        /// Calculate all statistics from a file read result
        /// </summary>
        /// <param name="readResult"></param>
        public void CalculateStatistics(FileReadResult<decimal> readResult)
        {
            if (readResult.ReadResult != ReadResult.FileReadOk)
                return;
            
            ArithmeticMean = Calculator.ArithmeticMean(readResult.ReadLines);
            StandardDeviation = Calculator.StandardDeviation(readResult.ReadLines);
            Frequency = Calculator.Frequency(GetBins(readResult), readResult.ReadLines);
        }

        /// <summary>
        /// Get a list of valid bins to calculate frequencies
        /// </summary>
        /// <param name="readResult"></param>
        /// <returns></returns>
        private List<Bin> GetBins(FileReadResult<decimal> readResult)
        {
            var allBins = new List<Bin>();
            var min = readResult.ReadLines.Min();
            var max = readResult.ReadLines.Max();

            min = min - min % 10m;
            max = max - max % 10m + 10m;

            while (min < max)
            {
                allBins.Add(new Bin
                {
                    LowerValue = min,
                    UpperValue = min + 10m
                });

                min += 10m;
            }

            return allBins;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
