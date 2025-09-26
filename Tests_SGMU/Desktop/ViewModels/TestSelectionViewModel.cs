using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        private ObservableCollection<Test> _availableTests;
        public ObservableCollection<Test> AvailableTests
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
                (StartTestCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand StartTestCommand { get; }
        public ICommand SelectFileCommand { get; }

        public TestSelectionViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _questionsService = new QuestionsService();

            // Загружаем тесты из папки Data
            var tests = _questionsService.LoadAllTests();
            AvailableTests = new ObservableCollection<Test>(tests);

            StartTestCommand = new RelayCommand(StartTest, CanStartTest);
            SelectFileCommand = new RelayCommand(SelectFile);
        }

        private void StartTest()
        {
            if (SelectedTest != null)
            {
                _mainViewModel.StartAllQuestions(SelectedTest);
                // Здесь будет логика начала тестирования
                MessageBox.Show($"Начинаем тест: {SelectedTest.Name}\nВопросов: {SelectedTest.Questions.Count}");
            }
        }

        private bool CanStartTest()
        {
            return SelectedTest != null && SelectedTest.Questions.Count > 0;
        }

        private void SelectFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Выберите файл с тестом",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var test = _questionsService.ParseTestFromFile(openFileDialog.FileName);

                    // Проверяем, нет ли уже такого теста в списке
                    if (!AvailableTests.Any(t => t.FilePath == test.FilePath))
                    {
                        AvailableTests.Add(test);
                        SelectedTest = test;

                        MessageBox.Show($"Тест '{test.Name}' успешно загружен!\nВопросов: {test.Questions.Count}",
                                      "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Этот тест уже загружен", "Информация",
                                      MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}