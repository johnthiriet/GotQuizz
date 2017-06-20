using System;
using System.Collections.Generic;
using System.Linq;
using GotQuizz.Models;

namespace GotQuizz.Services
{
	public class QuestionGenerator
	{
		private Random _random = new Random();

		public QuizzQuestion GenerateQuestion(
			IReadOnlyList<string> locations,
			IReadOnlyList<CharactersLocation> charactersLocations)
		{
			var character = GetRandomCharacter(charactersLocations);
			var location = GetRandomLocation(character, locations);

			return new QuizzQuestion
			{
				Question = $"Has {character.name} ever been to {location} ?",
				Answer = character.locations.Contains(location)
			};
		}

		private CharactersLocation GetRandomCharacter(IReadOnlyList<CharactersLocation> charactersLocations)
		{
			return charactersLocations[_random.Next(0, charactersLocations.Count - 1)];
		}

		private string GetRandomLocation(CharactersLocation character, IReadOnlyList<string> locations)
		{
			var characterLocations = character.locations;

			var locationsNotInCharacterLocations = locations.Except(character.locations)
															 .Take(character.locations.Count);

			var locationSet = character.locations.Concat(locationsNotInCharacterLocations)
									   .OrderBy(x => Guid.NewGuid())
									   .ToList();

			var location = locationSet[_random.Next(0, locationSet.Count - 1)];

			return location;
		}
	}
}
