using System;
using System.Linq;
using UnityEngine;

public abstract class BasePieceSet<TData, TEnum> : ScriptableObject 
    where TEnum : Enum
{
    [SerializeField, NonReorderable]
    private PieceDataParent[] _pieceDataArray;

    private void Reset()
    {
        var enumValues = Enum.GetValues(typeof(TEnum)) as TEnum[];
        int count = enumValues.Length;

        _pieceDataArray = new PieceDataParent[count];

        for (int i = 0; i < count; i++) 
            _pieceDataArray[i] = new PieceDataParent(enumValues[i]);
    }
    
    public TData GetDataByEnum(TEnum enumValue)
    {
        return _pieceDataArray.First(x => Equals(x.Type, enumValue)).Data;
    }
    
    [Serializable]
    private class PieceDataParent
    {
        [ReadOnly] 
        public TEnum Type;
        public TData Data;

        public PieceDataParent(TEnum type)
        {
            Type = type;
        }
    }
}