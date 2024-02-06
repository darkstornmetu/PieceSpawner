using UnityEngine;

public class PieceMesh : MonoBehaviour
{
    [SerializeField] private bool _isNotChangeable;
    
    [Header("References")]
    [HideIf("_isNotChangeable")]
    [SerializeField] private MeshRenderer[] _renderers;
    [Header("Material Set")]
    [HideIf("_isNotChangeable")]
    [SerializeField] private PieceMaterialSet _pieceMaterialSet;
    [HideIf("_isNotChangeable")]
    [SerializeField] private int _materialSlotToChange = 0;
    
    public void SetColor(PieceColors pieceColors)
    {
        if(_isNotChangeable)
            return;
        
        foreach (var rend in _renderers)
        {
            if (rend != null)
            {
                var materials = rend.sharedMaterials;
                materials[_materialSlotToChange] = _pieceMaterialSet.GetDataByEnum(pieceColors);
                rend.sharedMaterials = materials;
            }
        }
    }
    
    private void Reset()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }
}