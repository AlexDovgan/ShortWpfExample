using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ShoqBitsSampleApplication
{

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }  

    public class BitViewModel
    {

        public ICommand BitPressedCmd { get; set; }
        public int BitNumber { get; set; }
        public SolidColorBrush BitColor { get; set; }
        void BitPressed()
        {

        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection <BitViewModel> Bits {get;protected set;}
        
        public MainWindow()
        {
            Bits = new ObservableCollection<BitViewModel>();
            DataContext = this;
            InitializeComponent();
            
        }
        protected override void OnInitialized(EventArgs e)
        {
            Enumerable.Range(0, 32).Select(
                    i => new BitViewModel() 
                    { 
                        BitNumber = i, 
                        BitColor = i % 2==0 ? Brushes.Gray : Brushes.Yellow,
                        BitPressedCmd = new RelayCommand(ExecutedBitPressed)
                        }).ToList().ForEach(b=>Bits.Add(b));

            base.OnInitialized(e);
        }
        private void ExecutedBitPressed(object sender)
        {
            var bit = (BitViewModel)sender;
            MessageBox.Show(String.Format("Bit Pressed {0}",bit.BitNumber));

        }

    }
}
