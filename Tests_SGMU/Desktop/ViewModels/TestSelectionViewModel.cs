using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Tests_SGMU.Core;
using Tests_SGMU.Core.Interface;
using Tests_SGMU.Core.Models;
using Tests_SGMU.Core.Services;


namespace Tests_SGMU.Desktop.ViewModels
{
    public class TestSelectionViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly IQuestionsService _questionsService;

        private List<Test> _availableTests;
        public List<Test> AvailableTests
        {
            get => _availableTests;
            set => SetProperty(ref _availableTests, value);
        }

        private Test _selectedTest;
        public Test SelectedTest
        {
            get => _selectedTest;
            set
            {
                SetProperty(ref _selectedTest, value);
                // Обновляем состояние команды при изменении выбора
                (StartTestCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand StartTestCommand { get; }
        public ICommand BackCommand { get; }

        public TestSelectionViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _questionsService = new QuestionsService();

            AvailableTests = _questionsService.LoadAllTests();

            StartTestCommand = new RelayCommand(StartTest, CanStartTest);
            BackCommand = new RelayCommand(Back);
        }

        private void StartTest()
        {
            if (SelectedTest != null)
            {
                // Здесь будет логика начала тестирования
                System.Windows.MessageBox.Show($"Начинаем тест: {SelectedTest.Name}\nВопросов: {SelectedTest.Questions.Count}");
            }
        }

        private bool CanStartTest()
        {
            return SelectedTest != null && SelectedTest.Questions.Count > 0;
        }

        private void Back()
        {
            _mainViewModel.GoBackCommand.Execute(null);
        }
    }
}