using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace pisanka
{
    public partial class MainWindow : Window
    {
        const int SIZE = 10;

        Border[,] cells = new Border[SIZE, SIZE];
        CellType[,] board = new CellType[SIZE, SIZE];

        Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
            InitGrid();
            GenerateBoard();
            Render();
        }

        enum CellType
        {
            Empty,
            Player,
            Egg,
            Blocked
        }

        void InitGrid()
        {
            for (int i = 0; i < SIZE; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    Border b = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1),
                        Background = Brushes.White
                    };

                    Grid.SetRow(b, y);
                    Grid.SetColumn(b, x);
                    GameGrid.Children.Add(b);

                    cells[x, y] = b;
                }
            }
        }

        void GenerateBoard()
        {
            PlaceRandom(CellType.Egg, 10);
            PlaceRandom(CellType.Blocked, 40);
            PlaceRandom(CellType.Player, 1);
        }

        void PlaceRandom(CellType type, int count)
        {
            int placed = 0;

            while (placed < count)
            {
                int x = rand.Next(SIZE);
                int y = rand.Next(SIZE);

                if (board[x, y] == CellType.Empty)
                {
                    board[x, y] = type;
                    placed++;
                }
            }
        }

        (int, int) FindPlayer()
        {
            for (int y = 0; y < SIZE; y++)
                for (int x = 0; x < SIZE; x++)
                    if (board[x, y] == CellType.Player)
                        return (x, y);

            return (-1, -1);
        }

        void Render()
        {
            for (int y = 0; y < SIZE; y++)
            {
                for (int x = 0; x < SIZE; x++)
                {
                    switch (board[x, y])
                    {
                        case CellType.Empty:
                            cells[x, y].Background = Brushes.White;
                            break;
                        case CellType.Blocked:
                            cells[x, y].Background = Brushes.Black;
                            break;
                        case CellType.Player:
                            cells[x, y].Background = Brushes.Gold;
                            break;
                        case CellType.Egg:
                            cells[x, y].Background = Brushes.LightGreen;
                            break;
                    }
                }
            }
        }
    }
}