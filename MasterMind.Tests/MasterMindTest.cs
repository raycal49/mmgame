using static mastermind.MasterMind;
using static mastermind.Colors;

namespace mastermind.Tests;

[TestFixture]
public class MasterMindTests {
  [Test]
  public void TestCanary() {
    Assert.Pass();
  }

  [Test]
  public void AllColorsMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = selectedColors;

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 6 }, { Match.POSITION, 0 }, { Match.NONE, 0 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void NoMatches() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Pink, Pink, Pink, Pink, Pink, Pink };
    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 0 }, { Match.NONE, 6 } };
    var response = Guess(selectedColors, userProvidedColors);
    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void AllColorsOutOfPosition() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Blue, Green, Yellow, Purple, Orange, Red };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 6 }, { Match.NONE, 0 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void ExactMatchesFirstFour() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Red, Blue, Green, Yellow, Pink, Pink };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 4 }, { Match.POSITION, 0 }, { Match.NONE, 2 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void ExactMatchesLastFour() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Pink, Pink, Green, Yellow, Purple, Orange };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 4 }, { Match.POSITION, 0 }, { Match.NONE, 2 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void FirstThreeExactLastThreePosition() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Red, Blue, Green, Purple, Orange, Yellow };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 3 }, { Match.POSITION, 3 }, { Match.NONE, 0 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void FirstThirdNoneSecondExactOthersPosition() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Black, Blue, Silver, Purple, Orange, Yellow };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 1 }, { Match.POSITION, 3 }, { Match.NONE, 2 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void FirstColorRepeatedFiveTimes() {
    var selectedColors = new List<Colors> { Red, Orange, Pink, Blue, Maroon, Yellow };
    var userProvidedColors = new List<Colors> { Red, Red, Red, Red, Red, Red };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 1 }, { Match.POSITION, 0 }, { Match.NONE, 5 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void LastColorRepeatedFiveTimes() {
    var selectedColors = new List<Colors> { Red, Orange, Pink, Blue, Maroon, Yellow };
    var userProvidedColors = new List<Colors> { Yellow, Yellow, Yellow, Yellow, Yellow, Green };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 1 }, { Match.NONE, 5 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void FirstColorRepeatedFiveTimesAndSecondColorInFirstPosition() {
    var selectedColors = new List<Colors> { Red, Orange, Pink, Blue, Maroon, Yellow };
    var userProvidedColors = new List<Colors> { Orange, Red, Red, Red, Red, Red };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 2 }, { Match.NONE, 4 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void FirstColorRepeatedFiveTimesAndFirstPositionNoMatch() {
    var selectedColors = new List<Colors> { Red, Orange, Pink, Blue, Maroon, Yellow };
    var userProvidedColors = new List<Colors> { Purple, Red, Red, Red, Red, Red };

    var expectedResult = new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 1 }, { Match.NONE, 5 } };
    var response = Guess(selectedColors, userProvidedColors);

    CollectionAssert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayFirstAttemptExactMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = selectedColors;
    var number_of_attempts = 0;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 6 }, { Match.POSITION, 0 }, { Match.NONE, 0 } }, 1, GameStatus.WON);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayFirstAttemptNoMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Silver, Black, Pink, Red, Green, Maroon };
    var number_of_attempts = 0;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 2 }, { Match.NONE, 4 } }, 1, GameStatus.IN_PROGRESS);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayFirstAttemptSomeExactSomeNot() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Maroon, Blue, Pink, Yellow, Black, Orange };
    var number_of_attempts = 0;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 3 }, { Match.POSITION, 0 }, { Match.NONE, 3 } }, 1, GameStatus.IN_PROGRESS);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlaySecondAttemptWithExactMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var number_of_attempts = 1;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 6 }, { Match.POSITION, 0 }, { Match.NONE, 0 } }, 2, GameStatus.WON);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlaySecondAttemptWithNoMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Maroon, Silver, Pink, Red, Black, Blue };
    var number_of_attempts = 1;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 2 }, { Match.NONE, 4 } }, 2, GameStatus.IN_PROGRESS);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayTwentiethAttemptWithExactMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var number_of_attempts = 19;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 6 }, { Match.POSITION, 0 }, { Match.NONE, 0 } }, 20, GameStatus.WON);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayTwentiethAttemptWithNoMatch() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Maroon, Silver, Pink, Red, Black, Blue };
    var number_of_attempts = 19;

    var expectedResult = (new Dictionary<Match, int> { { Match.EXACT, 0 }, { Match.POSITION, 2 }, { Match.NONE, 4 } }, 20, GameStatus.LOST);
    var response = Play(selectedColors, userProvidedColors, number_of_attempts);

    Assert.AreEqual(expectedResult, response);
  }

  [Test]
  public void PlayTwentyFirstAttemptExactMatchThrowsException() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var number_of_attempts = 20;

    Assert.Throws<InvalidOperationException>(() =>
      Play(selectedColors, userProvidedColors, number_of_attempts)
    );
  }

  [Test]
  public void PlayTwentyFirstAttemptNoMatchThrowsException() {
    var selectedColors = new List<Colors> { Red, Blue, Green, Yellow, Purple, Orange };
    var userProvidedColors = new List<Colors> { Maroon, Maroon, Maroon, Maroon, Maroon, Maroon };
    var number_of_attempts = 20;

    Assert.Throws<InvalidOperationException>(() =>
      Play(selectedColors, userProvidedColors, number_of_attempts)
    );
  }

  [Test]
  public void SelectColorsPicksSixRandomColors() {
    var selectedColors = GenerateColors(10);

    int expectedSize = 6;
    var expectedResult = selectedColors.All(color => allColors.Contains(color));

    Assert.AreEqual(selectedColors.Count, expectedSize);
    Assert.IsTrue(expectedResult);
  }

  [Test]
  public void SelectColorsPicksRandomColorsTwice() {
    var selectedColors = GenerateColors(11);
    var otherColors = GenerateColors(12);

    var expectedResult = selectedColors.SequenceEqual(otherColors);

    Assert.IsFalse(expectedResult);
  }
}
