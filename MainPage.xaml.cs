    using Microsoft.Maui.Controls;

    namespace DominicanTicTacToe
    {
        public partial class MainPage : ContentPage
        {
            private bool isPlayer1Turn = true; // Tracks whose turn it is
            private string[,] board = new string[3, 3]; // Represents the game board

            // Image sources for the players' symbols
            private readonly ImageSource player1Image = ImageSource.FromFile("bandera.jpg");
            private readonly ImageSource player2Image = ImageSource.FromFile("mangu.jpg");
            public MainPage()
            {
                InitializeComponent();
            ResetGame();
        }
        private async void OnButtonClicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender; // Cast to ImageButton
            var frame = (Frame)button.Parent; // Get the parent Frame
            int row = Grid.GetRow(frame); // Get the row of the parent Frame
            int col = Grid.GetColumn(frame); // Get the column of the parent Frame

            // Check if the button is already occupied
            if (!string.IsNullOrEmpty(board[row, col]))
                return;

            // Click animation
            await frame.ScaleTo(0.9, 100); // Scale down
            await frame.ScaleTo(1, 100); // Scale back up

            // Update the board and button image
            if (isPlayer1Turn)
            {
                board[row, col] = "🌴";
                button.Source = player1Image;
            }
            else
            {
                board[row, col] = "🚬";
                button.Source = player2Image;
            }

            // Symbol placement animation
            button.Opacity = 0;
            await button.FadeTo(1, 500); // Fade in the symbol

            // Check for a winner
            if (CheckForWinner())
            {
                await HighlightWinningCells();
                DisplayWinner();
                return;
            }

            // Check for a tie
            if (CheckForTie())
            {
                DisplayAlert("Tie!", "The game is a tie!", "OK");
                ResetGame();
                return;
            }

            // Switch turns
            isPlayer1Turn = !isPlayer1Turn;
            TurnLabel.Text = isPlayer1Turn ? "Player 1's Turn (🌴)" : "Player 2's Turn (🚬)";
        }

        private bool CheckForWinner()
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (!string.IsNullOrEmpty(board[row, 0]) &&
                    board[row, 0] == board[row, 1] &&
                    board[row, 1] == board[row, 2])
                {
                    return true;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (!string.IsNullOrEmpty(board[0, col]) &&
                    board[0, col] == board[1, col] &&
                    board[1, col] == board[2, col])
                {
                    return true;
                }
            }

            // Check diagonals
            if (!string.IsNullOrEmpty(board[0, 0]) &&
                board[0, 0] == board[1, 1] &&
                board[1, 1] == board[2, 2])
            {
                return true;
            }

            if (!string.IsNullOrEmpty(board[0, 2]) &&
                board[0, 2] == board[1, 1] &&
                board[1, 1] == board[2, 0])
            {
                return true;
            }

            // If no winning condition is met, return false
            return false;
        }

        private bool CheckForTie()
        {
            // Check if all cells are filled and no winner
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    if (string.IsNullOrEmpty(board[row, col]))
                        return false; // If any cell is empty, it's not a tie
                }
            }
            return true; // All cells are filled, and no winner (tie)
        }

        private async Task HighlightWinningCells()
        {
            // Example: Highlight the first row for demonstration
            await A1Frame.FadeTo(0.5, 500); // Fade out
            await A1Frame.FadeTo(1, 500); // Fade in
            await A2Frame.FadeTo(0.5, 500);
            await A2Frame.FadeTo(1, 500);
            await A3Frame.FadeTo(0.5, 500);
            await A3Frame.FadeTo(1, 500);
        }

        private void DisplayWinner()
        {
            string winner = isPlayer1Turn ? "Player 1 (🌴)" : "Player 2 (🚬)";
            DisplayAlert("Winner!", $"{winner} wins!", "OK");
            ResetGame(); // Reset the game after declaring a winner
        }

        private void OnResetClicked(object sender, EventArgs e)
        {
            ResetGame();
        }

        private void ResetGame()
        {
            // Clear the board
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = string.Empty;
                }
            }

            // Clear the buttons
            foreach (var child in GameGrid.Children)
            {
                if (child is Frame frame && frame.Content is ImageButton button)
                {
                    button.Source = null; // Clear the image
                }
            }

            // Reset turn
            isPlayer1Turn = true;
            TurnLabel.Text = "Player 1's Turn (🌴)";
        }
    }

    }
