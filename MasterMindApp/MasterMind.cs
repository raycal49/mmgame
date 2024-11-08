using System.Text.RegularExpressions;

namespace mastermind {
  public class MasterMind {
    public static List<Colors> allColors = new List<Colors> {
        Colors.Red,
        Colors.Blue,
        Colors.Green,
        Colors.Yellow,
        Colors.Purple,
        Colors.Orange,
        Colors.Silver,
        Colors.Black,
        Colors.Pink,
        Colors.Maroon
      };

    public static List<Colors> GenerateColors(int seed) {

      Random random = new Random(seed);

      return allColors.OrderBy(color => random.Next()).Take(6).ToList();
    }

    public static Match MatchForPosition(int position, IEnumerable<Colors> selectedColors, IEnumerable<Colors> userProvidedColors) {
      var candidateColor = userProvidedColors.ElementAt(position);

      if (candidateColor == selectedColors.ElementAt(position)) {
        return Match.EXACT;
      }

      if (userProvidedColors.Take(position).Contains(candidateColor)) {
        return Match.NONE;
      }

      int index = selectedColors.ToList().IndexOf(candidateColor);

      if (index > -1 && selectedColors.ElementAt(index) != userProvidedColors.ElementAt(index)) {
        return Match.POSITION;
      }

      return Match.NONE;
    }

    public static Dictionary<Match, int> Guess(IEnumerable<Colors> selectedColors, IEnumerable<Colors> userProvidedColors) {
      Dictionary<Match, int> results = new Dictionary<Match, int> {
        { Match.EXACT, 0 },
        { Match.POSITION, 0 },
        { Match.NONE, 0 }
       };

      int MAX_COLORS = 6;

      var matches = Enumerable.Range(0, MAX_COLORS).Select(i => MatchForPosition(i, selectedColors, userProvidedColors));

      foreach (var match in matches) {
        results[match]++;
      }

      return results;
    }

    public static (Dictionary<Match, int> guess, int attempts, GameStatus status) Play(IEnumerable<Colors> selectedColors, IEnumerable<Colors> userProvidedColors, int attempts) {
      int MAX_ATTEMPTS = 20, MAX_COLORS = 6;
      int currentAttempts = attempts + 1;

      if (currentAttempts > MAX_ATTEMPTS) {
        throw new InvalidOperationException("Invalid operation. Game is already over.");
      }

      var guessResult = Guess(selectedColors, userProvidedColors);

      if (guessResult[Match.EXACT] == MAX_COLORS) {
        return (guessResult, currentAttempts, GameStatus.WON);
      }

      if (currentAttempts == MAX_ATTEMPTS) {
        return (guessResult, currentAttempts, GameStatus.LOST);
      }

      return (guessResult, currentAttempts, GameStatus.IN_PROGRESS);
    }
  }
}
