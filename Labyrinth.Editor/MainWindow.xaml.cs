using Labyrinth.Editor.Models;
using Labyrinth.Editor.Services;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Labyrinth.Editor
{
    public partial class MainWindow : Window
    {
        private MapModel map;

        private char selectedTile = '.';

        private Dictionary<string, string> currentLanguage;

        private List<Button> tileButtons = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();

            CreateTileButtons();

            cbLanguage.SelectedIndex = 0;
        }

        private void CreateTileButtons()
        {
            char[] tiles =
            {
                '.',
                '█',
                '═',
                '║',
                '╔',
                '╗',
                '╚',
                '╝',
                '╦',
                '╩',
                '╠',
                '╣',
                '╬'
            };

            foreach (char tile in tiles)
            {
                Button btn = new Button();

                btn.Content = tile;

                btn.Width = 40;
                btn.Height = 40;

                btn.Margin = new Thickness(3);

                btn.FontSize = 18;

                btn.Click += TileButton_Click;

                // Színezés

                if (tile == '█')
                {
                    btn.Background = Brushes.Gold;
                }
                else if (tile == '.')
                {
                    btn.Background = Brushes.DarkGray;
                    btn.Foreground = Brushes.White;
                }
                else
                {
                    btn.Background = Brushes.LightGray;
                }
                tileButtons.Add(btn);
                TilePanel.Children.Add(btn);
            }
        }

        private void TileButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            selectedTile =
                clickedButton.Content.ToString()[0];

            // Highlight reset

            foreach (Button btn in tileButtons)
            {
                btn.BorderBrush = Brushes.Gray;
                btn.BorderThickness = new Thickness(1);
            }

            // Aktív elem highlight
            clickedButton.BorderThickness = new Thickness(3);

            UpdateStatusBar();
        }

        private void BtnNewMap_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbWidth.Text, out int width) ||
                !int.TryParse(tbHeight.Text, out int height))
            {
                MessageBox.Show("Adj meg érvényes szélességet és magasságot!");
                return;
            }

            if (width <= 0 || height <= 0)
            {
                MessageBox.Show("A szélességnek és magasságnak nagyobbnak kell lennie 0-nál!");
                return;
            }

            map = new MapModel(width, height);

            DrawMap();

            UpdateStatusBar();
        }

        private void DrawMap()
        {
            MapGrid.Rows = map.Height;
            MapGrid.Columns = map.Width;
            MapGrid.Children.Clear();

            for (int row = 0; row < map.Height; row++)
            {
                for (int col = 0; col < map.Width; col++)
                {
                    Button cell = new Button();

                    char tile = map.Map[row, col];

                    if (tile == '.')
                    {
                        cell.Content = "";
                    }
                    else
                    {
                        cell.Content = tile;
                    }

                    cell.Tag = row + ";" + col;
                    cell.FontSize = 14;
                    cell.Margin = new Thickness(1);
                    cell.BorderBrush = Brushes.Gray;
                    cell.BorderThickness = new Thickness(1);

                    if (tile == '█')
                    {
                        cell.Background = Brushes.Gold;
                        cell.Foreground = Brushes.Black;
                    }
                    else if (tile == '.')
                    {
                        cell.Background = Brushes.DimGray;
                        cell.Foreground = Brushes.White;
                    }
                    else
                    {
                        cell.Background = Brushes.WhiteSmoke;
                        cell.Foreground = Brushes.Black;
                    }

                    cell.Click += Cell_Click;

                    MapGrid.Children.Add(cell);
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button cell = (Button)sender;

            string[] positions =
                cell.Tag.ToString().Split(';');

            int row = int.Parse(positions[0]);

            int col = int.Parse(positions[1]);

            map.Map[row, col] = selectedTile;

            if (selectedTile == '.')
            {
                cell.Content = "";
            }
            else
            {
                cell.Content = selectedTile;
            }

            // Színezés frissítése

            if (selectedTile == '█')
            {
                cell.Background = Brushes.Gold;
                cell.Foreground = Brushes.Black;
            }
            else if (selectedTile == '.')
            {
                cell.Background = Brushes.Black;
                cell.Foreground = Brushes.White;
            }
            else
            {
                cell.Background = Brushes.WhiteSmoke;
                cell.Foreground = Brushes.Black;
            }

            UpdateStatusBar();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (map == null)
            {
                MessageBox.Show(currentLanguage["NoMap"]);
                return;
            }

            MapValidator validator = new MapValidator();

            if (validator.GetRoomNumber(map.Map) == 0)
            {
                MessageBox.Show(currentLanguage["NoRoom"]);
                return;
            }

            if (validator.GetSuitableEntrance(map.Map) == 0)
            {
                MessageBox.Show(currentLanguage["NoExit"]);
                return;
            }

            if (validator.IsInvalidElement(map.Map))
            {
                MessageBox.Show(currentLanguage["InvalidCharacter"]);
                return;
            }

            if (validator.HasInvalidRooms(map.Map))
            {
                MessageBox.Show(currentLanguage["InvalidRoom"]);
                return;
            }

            List<string> unavailableElements = validator.GetUnavailableElements(map.Map);

            if (unavailableElements.Count > 0)
            {
                MessageBox.Show(
                    currentLanguage["UnavailableElement"] + "\n" +
                    currentLanguage["Positions"] + ": " + string.Join(", ", unavailableElements)
                );
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "Text files (*.txt)|*.txt";

            if (dialog.ShowDialog() == true)
            {
                MapFileService service = new MapFileService();

                service.SaveMap(map, dialog.FileName);

                MessageBox.Show(currentLanguage["SaveSuccess"]);
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Text files (*.txt)|*.txt";

            if (dialog.ShowDialog() == true)
            {
                MapFileService service =
                    new MapFileService();

                map = service.LoadMap(dialog.FileName);

                DrawMap();

                UpdateStatusBar();
            }
        }

        private void CbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguageService service =
                new LanguageService();

            if (cbLanguage.SelectedIndex == 0)
            {
                currentLanguage =
                    service.LoadLanguage(
                        "Resources/lang_hu.json");
            }
            else
            {
                currentLanguage =
                    service.LoadLanguage(
                        "Resources/lang_en.json");
            }

            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            txtWidthLabel.Text =
                currentLanguage["Width"];

            txtHeightLabel.Text =
                currentLanguage["Height"];

            btnNewMap.Content =
                currentLanguage["NewMap"];

            btnSave.Content =
                currentLanguage["Save"];

            btnLoad.Content =
                currentLanguage["Load"];

            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            if (map == null)
            {
                return;
            }

            MapValidator validator = new MapValidator();

            txtMapSize.Text =
                $"{currentLanguage["MapSize"]}: {map.Width}x{map.Height}";

            txtRoomCount.Text =
                $"{currentLanguage["RoomCount"]}: {validator.GetRoomNumber(map.Map)}";

            txtExitCount.Text =
                $"{currentLanguage["ExitCount"]}: {validator.GetSuitableEntrance(map.Map)}";

            txtSelectedTile.Text =
                $"{currentLanguage["SelectedTile"]}: {selectedTile}";
        }
    }
}