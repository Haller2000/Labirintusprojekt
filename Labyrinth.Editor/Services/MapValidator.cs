using System.Collections.Generic;

namespace Labyrinth.Editor.Services
{
    internal class MapValidator
    {
        public bool IsValidChar(char c)
        {
            return c == '.' || c == '█' || IsRoad(c);
        }

        public bool IsRoad(char c)
        {
            return c == '╬' || c == '═' || c == '╦' || c == '╩' ||
                   c == '║' || c == '╣' || c == '╠' ||
                   c == '╗' || c == '╝' || c == '╚' || c == '╔';
        }

        public bool CanGo(char c, int dr, int dc)
        {
            if (dr == -1 && dc == 0)
            {
                return c == '╬' || c == '╩' || c == '║' ||
                       c == '╣' || c == '╠' || c == '╝' || c == '╚';
            }

            if (dr == 1 && dc == 0)
            {
                return c == '╬' || c == '╦' || c == '║' ||
                       c == '╣' || c == '╠' || c == '╗' || c == '╔';
            }

            if (dr == 0 && dc == -1)
            {
                return c == '╬' || c == '═' || c == '╦' ||
                       c == '╩' || c == '╣' || c == '╗' || c == '╝';
            }

            if (dr == 0 && dc == 1)
            {
                return c == '╬' || c == '═' || c == '╦' ||
                       c == '╩' || c == '╠' || c == '╚' || c == '╔';
            }

            return false;
        }

        public int GetRoomNumber(char[,] map)
        {
            int count = 0;

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (map[row, col] == '█')
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public int GetSuitableEntrance(char[,] map)
        {
            int count = 0;

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int col = 0; col < cols; col++)
            {
                if (IsRoad(map[0, col]) && CanGo(map[0, col], -1, 0))
                {
                    count++;
                }

                if (IsRoad(map[rows - 1, col]) && CanGo(map[rows - 1, col], 1, 0))
                {
                    count++;
                }
            }

            for (int row = 0; row < rows; row++)
            {
                if (IsRoad(map[row, 0]) && CanGo(map[row, 0], 0, -1))
                {
                    count++;
                }

                if (IsRoad(map[row, cols - 1]) && CanGo(map[row, cols - 1], 0, 1))
                {
                    count++;
                }
            }

            return count;
        }

        public bool IsInvalidElement(char[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (!IsValidChar(map[row, col]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<string> GetUnavailableElements(char[,] map)
        {
            List<string> unavailables = new List<string>();

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (!IsRoad(map[row, col]))
                    {
                        continue;
                    }

                    bool hasConnection = false;

                    for (int i = 0; i < 4; i++)
                    {
                        int newRow = row + dr[i];
                        int newCol = col + dc[i];

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                        {
                            if (CanGo(map[row, col], dr[i], dc[i]))
                            {
                                hasConnection = true;
                            }

                            continue;
                        }

                        char neighbour = map[newRow, newCol];

                        if (IsRoad(neighbour) &&
                            CanGo(map[row, col], dr[i], dc[i]) &&
                            CanGo(neighbour, -dr[i], -dc[i]))
                        {
                            hasConnection = true;
                            break;
                        }

                        if (neighbour == '█' &&
                            CanGo(map[row, col], dr[i], dc[i]))
                        {
                            hasConnection = true;
                            break;
                        }
                    }

                    if (!hasConnection)
                    {
                        unavailables.Add(row + ":" + col);
                    }
                }
            }

            return unavailables;
        }

        public bool HasInvalidRooms(char[,] map)
        {
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    if (map[row, col] != '█')
                    {
                        continue;
                    }

                    bool hasConnection = false;

                    for (int i = 0; i < 4; i++)
                    {
                        int newRow = row + dr[i];
                        int newCol = col + dc[i];

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                        {
                            continue;
                        }

                        char neighbour = map[newRow, newCol];

                        if (IsRoad(neighbour) &&
                            CanGo(neighbour, -dr[i], -dc[i]))
                        {
                            hasConnection = true;
                            break;
                        }
                    }

                    if (!hasConnection)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public char[,] GenerateLabyrinth(List<string> positionsList)
        {
            int maxRow = 0;
            int maxCol = 0;

            List<int[]> positions = new List<int[]>();

            foreach (string position in positionsList)
            {
                string[] parts = position.Split(':');

                int row = int.Parse(parts[0]);
                int col = int.Parse(parts[1]);

                positions.Add(new int[] { row, col });

                if (row > maxRow)
                {
                    maxRow = row;
                }

                if (col > maxCol)
                {
                    maxCol = col;
                }
            }

            char[,] map = new char[maxRow + 1, maxCol + 1];

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    map[row, col] = '.';
                }
            }

            foreach (int[] pos in positions)
            {
                int row = pos[0];
                int col = pos[1];

                bool up = ContainsPosition(positions, row - 1, col);
                bool down = ContainsPosition(positions, row + 1, col);
                bool left = ContainsPosition(positions, row, col - 1);
                bool right = ContainsPosition(positions, row, col + 1);

                map[row, col] = GetRoadCharacter(up, down, left, right);
            }

            return map;
        }

        private bool ContainsPosition(List<int[]> positions, int row, int col)
        {
            foreach (int[] pos in positions)
            {
                if (pos[0] == row && pos[1] == col)
                {
                    return true;
                }
            }

            return false;
        }

        private char GetRoadCharacter(bool up, bool down, bool left, bool right)
        {
            if (up && down && left && right)
            {
                return '╬';
            }

            if (!up && !down && left && right)
            {
                return '═';
            }

            if (up && down && !left && !right)
            {
                return '║';
            }

            if (!up && down && left && right)
            {
                return '╦';
            }

            if (up && !down && left && right)
            {
                return '╩';
            }

            if (up && down && left && !right)
            {
                return '╣';
            }

            if (up && down && !left && right)
            {
                return '╠';
            }

            if (!up && down && left && !right)
            {
                return '╗';
            }

            if (up && !down && left && !right)
            {
                return '╝';
            }

            if (up && !down && !left && right)
            {
                return '╚';
            }

            if (!up && down && !left && right)
            {
                return '╔';
            }

            if (left || right)
            {
                return '═';
            }

            if (up || down)
            {
                return '║';
            }

            return '╬';
        }
    }
}