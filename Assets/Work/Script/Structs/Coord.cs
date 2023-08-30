using UnityEngine;

[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Coord Up { get { return new Coord(0, 1); } }
    public static Coord Down { get { return new Coord(0, -1); } }
    public static Coord Left { get { return new Coord(1, 0); } }
    public static Coord Right { get { return new Coord(-1, 0); } }

    public static Coord[] FourDirection
    {
        get
        {
            Coord[] _result = new Coord[4];
            int _index = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x == 0 || y == 0)
                    {
                        _result[_index] = new Coord(x, y);
                        _index++;
                    }
                }
            }
            return _result;
        }
    }
    public static Vector2 operator *(Coord coord, Vector2 vector)
    {
        return new Vector2(coord.x * vector.x, coord.y * vector.y);
    }
    public static Vector2 operator *(Vector2 vector, Coord coord)
    {
        return coord * vector;
    }
    public static bool operator ==(Coord _coord1, Coord _coord2)
    {
        return _coord1.x == _coord2.x && _coord1.y == _coord2.y;
    }
    public static bool operator !=(Coord _coord1, Coord _coord2)
    {
        return _coord1 != _coord2;
    }
}