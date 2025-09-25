using System.Collections.Generic;
using System.Windows.Input;
using Tests_SGMU.Core;
using Tests_SGMU.Core.Models;
using Tests_SGMU.Desktop.Views;


namespace Tests_SGMU.Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        private Stack<object> _navigationStack;

        public object CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public ICommand ShowTestSelectionCommand { get; }
        public ICommand ShowAllQuestionsCommand { get; }
        public ICommand ShowExamCommand { get; }
        public ICommand GoBackCommand { get; }

        public bool CanGoBack => _navigationStack.Count > 0;

        public MainViewModel()
        {
            _navigationStack = new Stack<object>();

            ShowTestSelectionCommand = new RelayCommand(ShowTestSelection);
            ShowAllQuestionsCommand = new RelayCommand(ShowAllQuestions);
            ShowExamCommand = new RelayCommand(ShowExam);
            GoBackCommand = new RelayCommand(GoBack, () => CanGoBack);

            // По умолчанию показываем главное меню
            NavigateTo(new MainMenuViewModel(this));
        }

        private void ShowTestSelection()
        {
            NavigateTo(new TestSelectionViewModel(this));
        }

        private void ShowAllQuestions()
        {
            NavigateTo(new AllQuestionsViewModel(this));
        }

        private void ShowExam()
        {
            NavigateTo(new ExamViewModel(this));
        }

        private void GoBack()
        {
            if (_navigationStack.Count > 0)
            {
                CurrentView = _navigationStack.Pop();
                OnPropertyChanged(nameof(CanGoBack));
            }
        }

        public void NavigateTo(object viewModel)
        {
            if (CurrentView != null)
            {
                _navigationStack.Push(CurrentView);
            }

            CurrentView = viewModel;
            OnPropertyChanged(nameof(CanGoBack));
        }

        public void NavigateToMainMenu()
        {
            _navigationStack.Clear();
            CurrentView = new MainMenuViewModel(this);
            OnPropertyChanged(nameof(CanGoBack));
        }
    }
}