using UnityEngine;

[ExecuteAlways]
public class TerrainGrids : MonoBehaviour
{
    public int rows = 10;
    public int cols = 10;
    public int innerRows = 6;
    public int innerCols = 6;
    public Color gridColor = Color.red;
    public Color gridColorInside = Color.green;
    public Vector2Int innerGridStart = new Vector2Int(2, 2);

    private Terrain terrain;

    void Awake()
    {
        terrain = GetComponent<Terrain>();
    }

    void OnDrawGizmos()
    {
        if (terrain == null) return;

        Vector3 terrainSize = terrain.terrainData.size;
        Vector3 terrainPosition = terrain.transform.position;

        float cellWidth = terrainSize.x / cols;
        float cellHeight = terrainSize.z / rows;

        Gizmos.color = gridColor;
        DrawGrid(rows, cols, terrainPosition, cellWidth, cellHeight);

        Vector3 innerStartPos = terrainPosition + new Vector3(innerGridStart.x * cellWidth, 0, innerGridStart.y * cellHeight);
        float innerCellWidth = cellWidth;
        float innerCellHeight = cellHeight;

        Gizmos.color = gridColorInside;
        DrawGrid(innerRows, innerCols, innerStartPos, innerCellWidth, innerCellHeight);
    }

    void DrawGrid(int rows, int cols, Vector3 startPosition, float cellWidth, float cellHeight)
    {
        for (int i = 0; i <= cols; i++)
        {
            Vector3 start = new Vector3(startPosition.x + i * cellWidth, startPosition.y, startPosition.z);
            Vector3 end = new Vector3(startPosition.x + i * cellWidth, startPosition.y, startPosition.z + rows * cellHeight);
            Gizmos.DrawLine(start, end);
        }

        for (int j = 0; j <= rows; j++)
        {
            Vector3 start = new Vector3(startPosition.x, startPosition.y, startPosition.z + j * cellHeight);
            Vector3 end = new Vector3(startPosition.x + cols * cellWidth, startPosition.y, startPosition.z + j * cellHeight);
            Gizmos.DrawLine(start, end);
        }
    }
}

