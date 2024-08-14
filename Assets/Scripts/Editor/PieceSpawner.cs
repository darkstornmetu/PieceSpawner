using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PieceSpawner : EditorWindow
{
    [MenuItem("Tools/Piece Spawner")]
    static void Init() => GetWindow<PieceSpawner>();
    
    private PiecePrefabSet _piecePrefabSet;
    private PieceIconSet _pieceIconSet;
    private PieceMaterialSet _pieceMaterialSet;
    private PieceThemeSet _themeSet;
    
    private Grid<PieceGridItem> _pieceGrid;
    
    private float _circleSize;

    private Vector2Int _gridSize = new Vector2Int(5, 5);
    private Vector2Int _lastGridSize = new Vector2Int(5, 5);
    private float _widthIncrement = 1;
    private float _heightIncrement = 1;
    
    private PieceTypes _pieceType;
    private PieceColors _colorType;
    
    private string _savePath;

    private Color _defaultColor;
    
    private void OnGUI()
    {
        EditorGUIUtility.wideMode = true;

        EditorGUILayout.Space();

        GUILayout.Label("References", EditorStyles.boldLabel);
        
        EditorGUI.BeginChangeCheck();
        
        _piecePrefabSet = (PiecePrefabSet)EditorGUILayout.ObjectField("Piece Prefab Set", _piecePrefabSet, typeof(PiecePrefabSet), false);
        _pieceIconSet = (PieceIconSet)EditorGUILayout.ObjectField("Piece Icon Set", _pieceIconSet, typeof(PieceIconSet), false);
        _pieceMaterialSet = (PieceMaterialSet)EditorGUILayout.ObjectField("Piece Mat Set", _pieceMaterialSet, typeof(PieceMaterialSet), false);
        _themeSet = (PieceThemeSet)EditorGUILayout.ObjectField("Piece Theme Set", _themeSet, typeof(PieceThemeSet), false);
        
        EditorGUILayout.Space();
        
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        
        _gridSize.x = EditorGUILayout.IntSlider("Grid Size X", _gridSize.x, 1, 20);
        _gridSize.y = EditorGUILayout.IntSlider("Grid Size Y", _gridSize.y, 1, 20);

        if (EditorGUI.EndChangeCheck())
        {
            UpdateGridContent();
            _lastGridSize = _gridSize;
        }

        _widthIncrement = EditorGUILayout.FloatField("Width Position Increment", _widthIncrement);
        _heightIncrement = EditorGUILayout.FloatField("Height Position Increment", _heightIncrement);
        
        EditorGUILayout.Space();
        
        //Bu iki tarafa da scale olmasını sagliyo
        _circleSize = Mathf.Min(position.width / (_gridSize.x + 1), (position.height - 300) / (_gridSize.y + 1));

        GUILayout.Label("Grid Item Settings", EditorStyles.boldLabel);
        _pieceType = (PieceTypes)EditorGUILayout.EnumPopup("Piece Type", _pieceType);
        _colorType = (PieceColors)EditorGUILayout.EnumPopup("Color Type", _colorType);

        EditorGUILayout.Space();
        
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Spawn Selected Pieces"))
                SpawnSelectedPieces(_pieceGrid);

            if (GUILayout.Button("Reset Grid"))
            {
                _pieceGrid = new Grid<PieceGridItem>(_gridSize.x, _gridSize.y);
                UpdateGridContent();
            }
        }

        if (GUILayout.Button("Copy Grid From Scene"))
            CopyGridFromScene();
        
        EditorGUILayout.Space();

        DrawGrid();
    }

    private void UpdateGridContent()
    {
        Grid<PieceGridItem> prevGrid = _pieceGrid;
        Vector2Int prevGridSize = _lastGridSize;

        _pieceGrid = new Grid<PieceGridItem>(_gridSize.x, _gridSize.y);

        for (int y = 0; y < _gridSize.y; y++)
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2Int coords = new Vector2Int(x, y);

                if (prevGrid != null && x < prevGridSize.x && y < prevGridSize.y
                    && prevGrid.TryGetItem(coords, out PieceGridItem item))
                {
                    _pieceGrid.SetItem(item, coords);
                }

                else
                {
                    _pieceGrid.SetItem(new PieceGridItem(default, default,
                        new GUIContent(CreateCircleTexture((int)_circleSize, _defaultColor))), coords);
                }
            }
        }
    }

    private void CopyGridFromScene()
    {
        var pieces = FindObjectsOfType<Piece>();
        
        int width = pieces.Max(x => x.GridCoords.x) + 1;
        int height = pieces.Max(x => x.GridCoords.y) + 1;

        _gridSize.x = width;
        _gridSize.y = height;

        _pieceGrid = new Grid<PieceGridItem>(width, height);
        
        foreach (var piece in pieces)
        {
            Vector2Int coords = piece.GridCoords;
            
            Color color = _pieceMaterialSet.GetDataByEnum(piece.PieceColor).color;
            Sprite pieceSprite = _pieceIconSet.GetDataByEnum(piece.PieceType);
            
            GUIContent content;
            
            if (pieceSprite != null)
                content = new GUIContent(CreateCircleTexture((int)_circleSize, color,
                    pieceSprite.texture));
            else
                content = new GUIContent(CreateCircleTexture((int)_circleSize, color));

            _pieceGrid.SetItem(new PieceGridItem(piece.PieceType, piece.PieceColor, content), coords);
        }
        
        UpdateGridContent();
    }

    private void DrawGrid()
    {
        if (_pieceMaterialSet == null || _pieceIconSet == null) return;

        GUILayout.BeginVertical();

        for (int y = _gridSize.y - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            for (int x = 0; x < _gridSize.x; x++)
            {
                Vector2Int coords = new Vector2Int(x, y);

                var pieceGridItem = _pieceGrid.GetItem(coords);

                pieceGridItem.GUIContent ??= new GUIContent(CreateCircleTexture((int)_circleSize, _defaultColor));

                if (GUILayout.Button(pieceGridItem.GUIContent, GUILayout.Width(_circleSize), 
                        GUILayout.Height(_circleSize)))
                {
                    pieceGridItem.Type = _pieceType;
                    pieceGridItem.Color = _colorType;

                    Color color = _pieceMaterialSet.GetDataByEnum(_colorType).color;
                    Sprite pieceSprite = _pieceIconSet.GetDataByEnum(_pieceType);

                    if (pieceSprite != null)
                        pieceGridItem.GUIContent = new GUIContent(CreateCircleTexture((int)_circleSize, color, pieceSprite.texture));
                    else
                        pieceGridItem.GUIContent = new GUIContent(CreateCircleTexture((int)_circleSize, color));
                }
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
    
    private Texture2D CreateCircleTexture(int diameter, Color color, Texture2D dotTexture = null)
    {
        Texture2D texture = new Texture2D(diameter, diameter, TextureFormat.ARGB32, false)
        {
            filterMode = FilterMode.Bilinear
        };
        
        Vector2 center = new Vector2(diameter / 2f, diameter / 2f);
        float radius = diameter / 2f;
        float minRadius = radius - 1;

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                Vector2 currentPosition = new Vector2(x, y);
                float distance = (currentPosition - center).sqrMagnitude;
                float alpha = Mathf.Clamp01((radius * radius - distance) / (radius * radius - minRadius * minRadius));
                Color pixelColor = new Color(color.r, color.g, color.b, color.a * alpha);

                if (dotTexture != null)
                {
                    Color dotPixelColor = dotTexture.GetPixelBilinear((float)x / diameter, (float)y / diameter);
                    pixelColor = Color.Lerp(pixelColor, dotPixelColor, dotPixelColor.a);
                }

                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        return texture;
    }

    private void SpawnSelectedPieces(Grid<PieceGridItem> pieceGrid)
    {
        if (string.IsNullOrWhiteSpace(_savePath)) 
            _savePath = EditorUtility.OpenFolderPanel("Select Save Path", "", "");

        GameObject parentObject = new GameObject(_themeSet.name);

        for (int x = 0; x < _gridSize.x; x++)
        {
            for (int y = 0; y < _gridSize.y; y++)
            {
                var currentPos = new Vector3(
                    -(_gridSize.x - 1) * _widthIncrement + x * _widthIncrement,
                    0,
                    y * _heightIncrement);

                currentPos.x += (_gridSize.x - 1) * _widthIncrement / 2f;
                currentPos.z -= (_gridSize.y - 1) * _heightIncrement / 2f;

                Vector2Int coords = new Vector2Int(x, y);

                var pieceGridItem = _pieceGrid.GetItem(coords);
                
                PieceTypes currentType = pieceGridItem.Type;
                PieceColors currentColor = pieceGridItem.Color;
                
                Piece piecePrefab = _piecePrefabSet.GetDataByEnum(currentType);

                Piece piece = PrefabUtility.InstantiatePrefab(piecePrefab, parentObject.transform) as Piece;
                piece.transform.localPosition = currentPos;
                piece.SetGridCoordinates(coords);
                piece.SetColor(currentColor);
                piece.SetType(currentType);

                PieceMesh pieceMesh = 
                    PrefabUtility.InstantiatePrefab(_themeSet.GetDataByEnum(currentType), 
                        piece.transform) as PieceMesh;

                if (pieceMesh != null)
                {
                    pieceMesh.SetColor(currentColor);
                    PieceIcon pieceIcon = pieceMesh.GetComponent<PieceIcon>();
                    
                    if (pieceIcon != null)
                        pieceIcon.SetIcon(_pieceIconSet.GetDataByEnum(currentType));
                }
                
                piece.name = "( " + x + ", " + y + " )";
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
        
        PrefabUtility.InstantiatePrefab(
            PrefabUtility.SaveAsPrefabAsset(parentObject, 
                _savePath + "/" + parentObject.name + "_" + GUID.Generate() + ".prefab"));
        
        DestroyImmediate(parentObject);
    }

    private void Awake()
    {
        _pieceGrid = new Grid<PieceGridItem>(_gridSize.x, _gridSize.y);
        _defaultColor = Color.gray;
        UpdateGridContent();
    }
    
    [Serializable]
    private class PieceGridItem : IGridItem
    {
        public PieceTypes Type;
        public PieceColors Color;
        public GUIContent GUIContent;

        public PieceGridItem(PieceTypes type, PieceColors color, GUIContent content)
        {
            Type = type;
            Color = color;
            GUIContent = content;
        }

        public Vector2Int GridCoords { get ; set ; }
    }
}


