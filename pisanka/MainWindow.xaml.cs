using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            do
            {
                board = new CellType[SIZE, SIZE];

                PlaceRandom(CellType.Egg, 10);
                PlaceRandom(CellType.Blocked, 40);
                PlaceRandom(CellType.Player, 1);

            } while (!IsBoardSolvable());
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
                            cells[x, y].Background = new ImageBrush(new BitmapImage(new Uri("https://png.pngtree.com/png-clipart/20250601/original/pngtree-adorable-brown-hare-with-big-ears-png-image_21107466.png", UriKind.Absolute)));
                            break;
                        case CellType.Egg:
                            cells[x, y].Background = new ImageBrush(new BitmapImage(new Uri("https://leclercdrive.lublin.pl/public/upload/sellasist_cache/thumb_page_1bce749b67976dc0accc252a5597d7ba.jpg", UriKind.Absolute)));
                            break;
                    }
                }
            }
        }

        void MovePlayer(int dx, int dy)
        {
            var (px, py) = FindPlayer();

            int nx = px + dx;
            int ny = py + dy;

            if (nx < 0 || ny < 0 || nx >= SIZE || ny >= SIZE)
                return;

            if (board[nx, ny] == CellType.Blocked)
                return;

            board[px, py] = CellType.Empty;
            board[nx, ny] = CellType.Player;

            Render();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up) MovePlayer(0, -1);
            if (e.Key == Key.Down) MovePlayer(0, 1);
            if (e.Key == Key.Left) MovePlayer(-1, 0);
            if (e.Key == Key.Right) MovePlayer(1, 0);
        }

        bool IsBoardSolvable()
        {
            var (px, py) = FindPlayer();

            bool[,] visited = new bool[SIZE, SIZE];
            int eggsFound = 0;

            FloodFill(px, py, visited, ref eggsFound);

            return eggsFound == 10;
        }

        void FloodFill(int x, int y, bool[,] visited, ref int eggsFound)
        {
            if (x < 0 || y < 0 || x >= SIZE || y >= SIZE)
                return;

            if (visited[x, y] || board[x, y] == CellType.Blocked)
                return;

            visited[x, y] = true;

            if (board[x, y] == CellType.Egg)
                eggsFound++;

            FloodFill(x + 1, y, visited, ref eggsFound);
            FloodFill(x - 1, y, visited, ref eggsFound);
            FloodFill(x, y + 1, visited, ref eggsFound);
            FloodFill(x, y - 1, visited, ref eggsFound);
        }
    }
}