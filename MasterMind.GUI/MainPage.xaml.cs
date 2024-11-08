using mastermind;
using static mastermind.Colors;
using Colors = Microsoft.Maui.Graphics.Colors;

namespace MasterMind.GUI {
  public partial class MainPage : ContentPage {
    int count;
    GameStatus gameStatus;
    private static Random? rand;
    private List<mastermind.Colors> mastermindColors;

    //private List<mastermind.Colors> mastermindColors = new List<mastermind.Colors> { Red, Blue, Green, Yellow, Purple, Orange };

    private List<mastermind.Colors> colorSelection = new List<mastermind.Colors>{
       Red,
       Blue,
       Green,
       Yellow,
       Purple,
       Orange,
       Silver,
       Black,
       Pink,
       Maroon
      };

    private Dictionary<mastermind.Colors, Color> colorMapping = new Dictionary<mastermind.Colors, Color> {
        { mastermind.Colors.Red, Microsoft.Maui.Graphics.Colors.Red },
        { mastermind.Colors.Blue, Microsoft.Maui.Graphics.Colors.Blue },
        { mastermind.Colors.Green, Microsoft.Maui.Graphics.Colors.Green },
        { mastermind.Colors.Yellow, Microsoft.Maui.Graphics.Colors.Yellow },
        { mastermind.Colors.Purple, Microsoft.Maui.Graphics.Colors.Purple },
        { mastermind.Colors.Orange, Microsoft.Maui.Graphics.Colors.Orange },
        { mastermind.Colors.Silver, Microsoft.Maui.Graphics.Colors.Silver },
        { mastermind.Colors.Black, Microsoft.Maui.Graphics.Colors.Black },
        { mastermind.Colors.Pink, Microsoft.Maui.Graphics.Colors.Pink },
        { mastermind.Colors.Maroon, Microsoft.Maui.Graphics.Colors.Maroon }
      };

    private Dictionary<Button, int> buttonColorIndex = new Dictionary<Button, int>();

    public MainPage() {
      InitializeComponent();

      count = 0;
      gameStatus = GameStatus.IN_PROGRESS;
      rand = new Random();
      mastermindColors = mastermind.MasterMind.GenerateColors(rand.Next());

      // Initialize color indices for each button
      foreach (var button in new[] { Btn1, Btn2, Btn3, Btn4, Btn5, Btn6 }) {
        buttonColorIndex[button] = 0;
      }

      label.BindingContext = this;
      label.SetBinding(Label.TextProperty, "count");


    }

    private void OnColorButtonClicked(object sender, EventArgs e) {
      if (sender is Button button && buttonColorIndex.ContainsKey(button)) {
        // Increment color index
        buttonColorIndex[button] = (buttonColorIndex[button] + 1) % colorSelection.Count;

        // Update button color
        var color = colorSelection[(int)buttonColorIndex[button]];
        button.BackgroundColor = colorMapping[color];
      }
    }

    private void ShowGameOverScreen() {
      foreach (var secretColor in mastermindColors) {
        SecretColorsStackLoss.Children.Add(new BoxView {
          Color = colorMapping[secretColor],
          WidthRequest = 50,
          HeightRequest = 50,
          CornerRadius = 50
        });
      }

      GameOverOverlay.IsVisible = true;
    }

    private async void OnGiveUpButtonClicked(object sender, EventArgs e) {
      if (gameStatus != GameStatus.IN_PROGRESS)
        return;

      var answer = await DisplayAlert("Give Up?", "Are you sure you want to give up?", "Yes", "No");
      if (answer) {
        gameStatus = GameStatus.LOST;
        ShowGameOverScreen();
      }

    }

    private void OnGuessButtonClicked(object sender, EventArgs e) {
      if (gameStatus != GameStatus.IN_PROGRESS)
        return;

      var userColors = new List<mastermind.Colors>();

      foreach (var b in buttonColorIndex) {
        userColors.Add((mastermind.Colors)b.Value);
      }

      // Create a dynamic row for the left side with the user's color selections
      var colorRow = new StackLayout {
        Orientation = StackOrientation.Horizontal,
        Spacing = 5,
        HorizontalOptions = LayoutOptions.Start,
        VerticalOptions = LayoutOptions.Start
      };

      // Populate dynamic row with userColors
      foreach (mastermind.Colors color in userColors) {
        var colorCircle = new BoxView {
          Color = colorMapping[color],
          WidthRequest = 50,
          HeightRequest = 50,
          CornerRadius = 50
        };
        colorRow.Children.Add(colorCircle);
      }

      PreviousGuessStack.Children.Insert(0, colorRow);

      // Process the guess
      var results = mastermind.MasterMind.Guess(mastermindColors, userColors);
      var playResult = mastermind.MasterMind.Play(mastermindColors, userColors, count);
      count = playResult.attempts;
      gameStatus = playResult.status;

      if (gameStatus == GameStatus.LOST) {
        ShowGameOverScreen();
      }

      else if (gameStatus == GameStatus.WON) {

        foreach (var secretColor in mastermindColors) {
          SecretColorsStackWon.Children.Add(new BoxView {
            Color = colorMapping[secretColor],
            WidthRequest = 50,
            HeightRequest = 50,
            CornerRadius = 50
          });
        }

        GameWinOverlay.IsVisible = true;
      }

      // Create a feedback row with black and silver pegs
      var feedbackRow = new StackLayout {
        Orientation = StackOrientation.Horizontal,
        Spacing = 5,
        HorizontalOptions = LayoutOptions.End,
      };

      // Add Black Pegs for EXACT matches
      for (int i = 0; i < results[mastermind.Match.EXACT]; i++) {
        var blackPeg = new BoxView {
          Color = Microsoft.Maui.Graphics.Colors.Black,
          WidthRequest = 50,
          HeightRequest = 50,
          CornerRadius = 50
        };
        feedbackRow.Children.Add(blackPeg);
      }

      // Add Silver Pegs for POSITION matches
      for (int i = 0; i < results[mastermind.Match.POSITION]; i++) {
        var silverPeg = new BoxView {
          Color = Microsoft.Maui.Graphics.Colors.Silver,
          WidthRequest = 50,
          HeightRequest = 50,
          CornerRadius = 50
        };
        feedbackRow.Children.Add(silverPeg);
      }

      FeedbackStack.Children.Insert(0, feedbackRow);
    }
  }
}
