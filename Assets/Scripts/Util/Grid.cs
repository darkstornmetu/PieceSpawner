using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Grid<T> where T : class, IGridItem
{
    [SerializeField] int _width;
    [SerializeField] int _height;

    [SerializeField] T[] _grid;

    public int Width => _width;
    public int Height => _height;

    public Grid(int width, int height)
    {
        _width = width;
        _height = height;

        _grid = new T[width * height];
    }

    public bool IsEmpty(Vector2Int coords) =>
        !IsValidCoordinates(coords) || _grid[GetIndex(coords)] == null;

    public void SetItem(T item, Vector2Int coordinates)
    {
        _grid[GetIndex(coordinates)] = item;

        if (item != null)
            item.GridCoords = coordinates;
    }

    public void RemoveItemFromGrid(Vector2Int coordinates)
    {
        _grid[GetIndex(coordinates)] = null;
    }

    public bool TryGetItem(Vector2Int coords, out T item)
    {
        item = null;

        if (IsEmpty(coords))
            return false;

        item = GetItem(coords);
        return true;
    }

    public T GetItem(Vector2Int coords) => _grid[GetIndex(coords)];

    public void MoveItem(Vector2Int from, Vector2Int to, bool swap = false)
    {
        if (!IsValidCoordinates(from) || !IsValidCoordinates(to))
            return;

        if (!swap)
        {
            SetItem(_grid[GetIndex(from)], to);
            SetItem(null, from);
        }
        else
        {
            var item = _grid[GetIndex(to)];
            SetItem(_grid[GetIndex(from)], to);
            SetItem(item, from);
        }
    }

    public IEnumerable<T> GetRow(int columnIndex)
    {
        if (columnIndex >= _height || columnIndex < 0)
            throw new IndexOutOfRangeException();

        return Enumerable.Range(0, _grid.GetLength(0))
            .Select(x => _grid[GetIndex(new Vector2Int(x, columnIndex))]);
    }

    public IEnumerable<T> GetColumn(int rowIndex)
    {
        if (rowIndex >= _width || rowIndex < 0)
            throw new IndexOutOfRangeException();

        return Enumerable.Range(0, _grid.GetLength(1))
            .Select(y => _grid[GetIndex(new Vector2Int(rowIndex, y))]);
    }

    public Vector2 GetCoordinate(T item)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);

                if (_grid[GetIndex(coords)].Equals(item))
                    return coords;
            }
        }

        throw new ArgumentNullException();
    }

    private bool IsValidCoordinates(Vector2Int coordinates)
    {
        return coordinates.x >= 0 && coordinates.x < _width && coordinates.y >= 0 && coordinates.y < _height;
    }

    private int GetIndex(Vector2Int coordinates)
    {
        return coordinates.y * _width + coordinates.x;
    }
}