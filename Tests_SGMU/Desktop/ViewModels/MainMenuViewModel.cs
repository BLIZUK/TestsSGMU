using System.Windows.Input;
using Tests_SGMU.Core;
using Tests_SGMU.Core.Models;


namespace Tests_SGMU.Desktop.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;

        public ICommand NavigateToTestSelectionCommand { get; }
        public ICommand NavigateToAllQuestionsCommand { get; }
        public ICommand NavigateToExamCommand { get; }

        public MainMenuViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            NavigateToTestSelectionCommand = new RelayCommand(() =>
                _mainViewModel.ShowTestSelectionCommand.Execute(null));

            NavigateToAllQuestionsCommand = new RelayCommand(() =>
                _mainViewModel.ShowAllQuestionsCommand.Execute(null));

            NavigateToExamCommand = new RelayCommand(() =>
                _mainViewModel.ShowExamCommand.Execute(null));
        }
    }
}