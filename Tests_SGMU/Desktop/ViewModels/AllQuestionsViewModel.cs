using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Tests_SGMU.Core;
using Tests_SGMU.Core.Models;



namespace Tests_SGMU.Desktop.ViewModels
{
    public class AllQuestionsViewModel : BaseViewModel
    {
        private readonly MainViewModel _mainViewModel;
        private readonly Test _test;

        private int _currentQuestionIndex;
        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                SetProperty(ref _currentQuestionIndex, value);
                UpdateNavigationButtons();
                ClearResult();
            }
        }

        public Question CurrentQuestion => _test.Questions.ElementAtOrDefault(CurrentQuestionIndex);

        public string TestTitle => _test.Name;
        public int TotalQuestions => _test.Questions.Count;
        public string ProgressText => $"Вопрос {CurrentQuestionIndex + 1} из {TotalQuestions}";

        private string _selectedAnswer;
        public string SelectedAnswer
        {
            get => _selectedAnswer;
            set => SetProperty(ref _selectedAnswer, value);
        }

        private bool _isAnswerConfirmed;
        public bool IsAnswerConfirmed
        {
            get => _isAnswerConfirmed;
            set => SetProperty(ref _isAnswerConfirmed, value);
        }

        private string _resultMessage;
        public string ResultMessage
        {
            get => _resultMessage;
            set => SetProperty(ref _resultMessage, value);
        }

        private string _resultColor;
        public string ResultColor
        {
            get => _resultColor;
            set => SetProperty(ref _resultColor, value);
        }

        public bool CanGoBack => CurrentQuestionIndex > 0;
        public bool CanGoForward => CurrentQuestionIndex < TotalQuestions - 1;
        public bool CanConfirm => !string.IsNullOrEmpty(SelectedAnswer) && !IsAnswerConfirmed;

        public ICommand BackCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }

        public AllQuestionsViewModel(MainViewModel mainViewModel, Test test)
        {
            _mainViewModel = mainViewModel;
            _test = test;
            CurrentQuestionIndex = 0;

            BackCommand = new RelayCommand(() => _mainViewModel.GoBackCommand.Execute(null));
            ConfirmCommand = new RelayCommand(ConfirmAnswer, () => CanConfirm);
            NextCommand = new RelayCommand(NextQuestion, () => CanGoForward);
            PreviousCommand = new RelayCommand(PreviousQuestion, () => CanGoBack);

            // Восстанавливаем выбранный ответ если он был
            if (CurrentQuestion != null && !string.IsNullOrEmpty(CurrentQuestion.UserAnswer))
            {
                SelectedAnswer = CurrentQuestion.UserAnswer;
            }
        }

        private void ConfirmAnswer()
        {
            if (string.IsNullOrEmpty(SelectedAnswer)) return;

            // Сохраняем ответ пользователя
            CurrentQuestion.UserAnswer = SelectedAnswer;
            IsAnswerConfirmed = true;

            // Проверяем правильность ответа
            bool isCorrect = CurrentQuestion.IsCorrect;

            ResultMessage = isCorrect ? "✓ Правильно!" : "✗ Неправильно!";
            ResultColor = isCorrect ? "#4CAF50" : "#F44336"; // Зеленый для правильно, красный для неправильно

            // Обновляем состояние команд
            (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void NextQuestion()
        {
            if (CanGoForward)
            {
                CurrentQuestionIndex++;
                // Восстанавливаем выбранный ответ если он был
                if (CurrentQuestion != null && !string.IsNullOrEmpty(CurrentQuestion.UserAnswer))
                {
                    SelectedAnswer = CurrentQuestion.UserAnswer;
                    IsAnswerConfirmed = true;
                }
                else
                {
                    SelectedAnswer = null;
                    IsAnswerConfirmed = false;
                }
            }
        }

        private void PreviousQuestion()
        {
            if (CanGoBack)
            {
                CurrentQuestionIndex--;
                // Восстанавливаем выбранный ответ если он был
                if (CurrentQuestion != null && !string.IsNullOrEmpty(CurrentQuestion.UserAnswer))
                {
                    SelectedAnswer = CurrentQuestion.UserAnswer;
                    IsAnswerConfirmed = true;
                }
                else
                {
                    SelectedAnswer = null;
                    IsAnswerConfirmed = false;
                }
            }
        }

        private void UpdateNavigationButtons()
        {
            OnPropertyChanged(nameof(CanGoBack));
            OnPropertyChanged(nameof(CanGoForward));
            OnPropertyChanged(nameof(CanConfirm));
            OnPropertyChanged(nameof(CurrentQuestion));
            OnPropertyChanged(nameof(ProgressText));

            (ConfirmCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (NextCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (PreviousCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void ClearResult()
        {
            ResultMessage = string.Empty;
            ResultColor = "Transparent";
        }
    }
}