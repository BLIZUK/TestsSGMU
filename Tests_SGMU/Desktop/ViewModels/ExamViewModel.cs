using System.Windows.Input;
using Tests_SGMU.Core;
using Tests_SGMU.Core.Models;


namespace Tests_SGMU.Desktop.ViewModels
{
    public class ExamViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand BackCommand { get; }

        public ExamViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            BackCommand = new RelayCommand(() => _mainViewModel.GoBackCommand.Execute(null));
        }
    }
}