using System;
using System.Collections.Generic;

namespace ConsoleApp23
{
    class Program
    {
        static bool IsValidChar(char c)
        {
            return c == '.' || c == '█' || IsRoad(c);
        }

        static bool IsRoad(char c)
        {
            return c == '╬' || c == '═' || c == '╦' || c == '╩' ||
                   c == '║' || c == '╣' || c == '╠' ||
                   c == '╗' || c == '╝' || c == '╚' || c == '╔';
        }

        static bool CanGo(char c, int dr, int dc)
        {
            if (dr == -1 && dc == 0)
                return c == '╬' || c == '╩' || c == '║' || c == '╣' || c == '╠' || c == '╝' || c == '╚';

            if (dr == 1 && dc == 0)
                return c == '╬' || c == '╦' || c == '║' || c == '╣' || c == '╠' || c == '╗' || c == '╔';

            if (dr == 0 && dc == -1)
                return c == '╬' || c == '═' || c == '╦' || c == '╩' || c == '╣' || c == '╗' || c == '╝';

            if (dr == 0 && dc == 1)
                return c == '╬' || c == '═' || c == '╦' || c == '╩' || c == '╠' || c == '╚' || c == '╔';

            return false;
        }

        static int GetRoomNumber(char[,] map)
        {
            int count = 0;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '█')
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        static int GetSuitableEntrance(char[,] map)
        {
            int count = 0;
            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            for (int j = 0; j < cols; j++)
            {
                if (IsRoad(map[0, j]) && CanGo(map[0, j], -1, 0))
                    count++;

                if (IsRoad(map[rows - 1, j]) && CanGo(map[rows - 1, j], 1, 0))
                    count++;
            }

            for (int i = 0; i < rows; i++)
            {
                if (IsRoad(map[i, 0]) && CanGo(map[i, 0], 0, -1))
                    count++;

                if (IsRoad(map[i, cols - 1]) && CanGo(map[i, cols - 1], 0, 1))
                    count++;
            }

            return count;
        }

        static bool IsInvalidElement(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (!IsValidChar(map[i, j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        static List<string> GetUnavailableElements(char[,] map)
        {
            List<string> unavailables = new List<string>();

            int rows = map.GetLength(0);
            int cols = map.GetLength(1);

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!IsRoad(map[i, j]))
                        continue;

                    bool hasConnection = false;

                    for (int k = 0; k < 4; k++)
                    {
                        int newRow = i + dr[k];
                        int newCol = j + dc[k];

                        if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols)
                            continue;

                        if (IsRoad(map[newRow, newCol]) &&
                            CanGo(map[i, j], dr[k], dc[k]) &&
                            CanGo(map[newRow, newCol], -dr[k], -dc[k]))
                        {
                            hasConnection = true;
                            break;
                        }
                    }

                    if (!hasConnection)
                    {
                        unavailables.Add(i + ":" + j);
                    }
                }
            }

            return unavailables;
        }

        static char[,] GenerateLabyrinth(List<string> positionsList)
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

                if (row > maxRow) maxRow = row;
                if (col > maxCol) maxCol = col;
            }

            char[,] map = new char[maxRow + 1, maxCol + 1];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = '.';
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

        static bool ContainsPosition(List<int[]> positions, int row, int col)
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

        static char GetRoadCharacter(bool up, bool down, bool left, bool right)
        {
            if (up && down && left && right) return '╬';

            if (!up && !down && left && right) return '═';
            if (up && down && !left && !right) return '║';

            if (!up && down && left && right) return '╦';
            if (up && !down && left && right) return '╩';
            if (up && down && left && !right) return '╣';
            if (up && down && !left && right) return '╠';

            if (!up && down && left && !right) return '╗';
            if (up && !down && left && !right) return '╝';
            if (up && !down && !left && right) return '╚';
            if (!up && down && !left && right) return '╔';

            if (left || right) return '═';
            if (up || down) return '║';

            return '╬';
        }


    }
}