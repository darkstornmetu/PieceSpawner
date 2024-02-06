using UnityEngine;

[SelectionBase]
public class Piece : MonoBehaviour
{
    [ReadOnly]
    [SerializeField] protected Vector2Int _coords;
    [ReadOnly]
    [SerializeField] protected PieceColors _pieceColor;
    [ReadOnly]
    [SerializeField] protected PieceTypes _pieceType;
    
    public Vector2Int GridCoords => _coords;
    public PieceColors PieceColor => _pieceColor;
    public PieceTypes PieceType => _pieceType;
    
    public void SetGridCoordinates(Vector2Int coords)
    {
        _coords.x = coords.x;
        _coords.y = coords.y;
    }

    public void SetColor(PieceColors pieceColor)
    {
        _pieceColor = pieceColor;
    }

    public void SetType(PieceTypes pieceType)
    {
        _pieceType = pieceType;
    }
}