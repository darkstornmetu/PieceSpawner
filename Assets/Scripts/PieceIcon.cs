using UnityEngine;

public class PieceIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    public void SetIcon(Sprite sprite)
    {
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = sprite;
    }
    
    private void Reset()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}