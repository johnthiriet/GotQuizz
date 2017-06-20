using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GotQuizz.Models;
using GotQuizz.Services;

namespace GotQuizz.ViewModels
{
    public class GotQuizzViewModel : ViewModelBase
    {
        private QuizzState _quizz;
		private QuizzQuestion _currentQuestion;
		private IReadOnlyList<string> _locations;
		private IReadOnlyList<CharactersLocation> _charactersLocations;
        private readonly QuestionGenerator _generator;
        private readonly GotApiService _api;

		public string CurrentQuestionLabel => _currentQuestion?.Question;
		public string CorrectAnswersLabel => $"{_quizz?.CorrectAnswers ?? 0} correct answers";

		public async void Start()
		{
		    try
		    {
		        await StartAsync();
		    }
		    catch (Exception ex)
		    {
		        Debug.WriteLine(ex);
		    }
		}

		private async Task StartAsync()
		{
			var result = await _api.GetCharactersLocationAsync();

		    _charactersLocations = result;
		    _locations = _charactersLocations
		                .SelectMany(x => x.locations)
		                .Distinct()
		                .ToList();

		    StartQuizz();
		}

		private void StartQuizz()
		{
			_currentQuestion = null;
		    _quizz = new QuizzState();

			NextQuestion();
		}

		private void NextQuestion()
		{
		    _currentQuestion = _generator.GenerateQuestion(_locations, _charactersLocations);
			RaisePropertyChanged(nameof(CurrentQuestionLabel));
			RaisePropertyChanged(nameof(CorrectAnswersLabel));
		}

		public RelayCommand Yes => _yes;
		public RelayCommand No => _no;

		private RelayCommand _yes;
		private RelayCommand _no;

		public GotQuizzViewModel()
		{
			_yes = new RelayCommand(() => AnswerQuestion(true));
			_no = new RelayCommand(() => AnswerQuestion(false));
            _generator = new QuestionGenerator();
            _api = new GotApiService();
		}

        private void AnswerQuestion(bool answer)
        {
            if (answer == _currentQuestion.Answer)
                _quizz.CorrectAnswers++;

            NextQuestion();
        }	
    }
}
