// See https://aka.ms/new-console-template for more information
// 06 Rock,paper,scissors,lizard,spock

using System;

enum GameMove
{
    Rock,
    Paper,
    Scissors,
    Lizard,
    Spock
}

enum GameResult
{
    PlayerWin,
    ComputerWin,
    Draw
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to Rock, Paper, Scissors, Lizard, Spock!");

        for (;;)
        {
            Console.WriteLine("Choose your move:");
            Console.WriteLine("1. Rock");
            Console.WriteLine("2. Paper");
            Console.WriteLine("3. Scissors");
            Console.WriteLine("4. Lizard");
            Console.WriteLine("5. Spock");
            Console.WriteLine("0. Quit");

            string input = Console.ReadLine();
            if (input == "0")
                break;

            if (!int.TryParse(input, out int playerMove) || playerMove < 1 || playerMove > 5)
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }

            GameMove playerChoice = (GameMove)(playerMove - 1);
            GameMove computerChoice = GetRandomComputerMove();

            Console.WriteLine($"Player chooses: {playerChoice}");
            Console.WriteLine($"Computer chooses: {computerChoice}");

            GameResult result = DetermineResult(playerChoice, computerChoice);

            Console.WriteLine("Result: " + result);
            Console.WriteLine();
        }

        Console.WriteLine("Thank you for playing! Goodbye.");
    }

    static GameMove GetRandomComputerMove()
    {
        Random random = new Random();
        return (GameMove)random.Next(0, 5);
    }

    static GameResult DetermineResult(GameMove playerMove, GameMove computerMove)
    {
        if (playerMove == computerMove)
            return GameResult.Draw;

        switch (playerMove)
        {
            case GameMove.Rock:
                return (computerMove == GameMove.Scissors || computerMove == GameMove.Lizard) ?
                    GameResult.PlayerWin : GameResult.ComputerWin;

            case GameMove.Paper:
                return (computerMove == GameMove.Rock || computerMove == GameMove.Spock) ?
                    GameResult.PlayerWin : GameResult.ComputerWin;

            case GameMove.Scissors:
                return (computerMove == GameMove.Paper || computerMove == GameMove.Lizard) ?
                    GameResult.PlayerWin : GameResult.ComputerWin;

            case GameMove.Lizard:
                return (computerMove == GameMove.Paper || computerMove == GameMove.Spock) ?
                    GameResult.PlayerWin : GameResult.ComputerWin;

            case GameMove.Spock:
                return (computerMove == GameMove.Rock || computerMove == GameMove.Scissors) ?
                    GameResult.PlayerWin : GameResult.ComputerWin;

            default:
                throw new ArgumentException("Invalid game move.");
        }
    }
}

