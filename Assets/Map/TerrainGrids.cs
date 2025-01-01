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
    public Terrain terrain;

    private float innerMinX;
    private float innerMaxX;
    private float innerMinZ;
    private float innerMaxZ;
    private float cellWidth;
    private float cellHeight;

    void Awake()
    {
        terrain = GetComponent<Terrain>();
        RecalculateGridBounds();
    }

    void OnValidate()
    {
        RecalculateGridBounds();
    }

    void OnDrawGizmos()
    {
        if (terrain == null) return;

        Gizmos.color = gridColor;
        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;
        DrawGrid(rows, cols, terrainPosition, cellWidth, cellHeight);

        Vector3 innerStartPos = terrainPosition + new Vector3(innerGridStart.x * cellWidth, 0, innerGridStart.y * cellHeight);
        Gizmos.color = gridColorInside;
        DrawGrid(innerRows, innerCols, innerStartPos, cellWidth, cellHeight);
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

    public bool IsValidCoordinate(Vector3 point)
    {
        // Check if the hitpoint is within the inner grid
        return point.x >= innerMinX && point.x <= innerMaxX &&
               point.z >= innerMinZ && point.z <= innerMaxZ;
    }

    public bool IsValidInsideEnemySide(Vector3 point)
    {
        return  point.x <= innerMaxX && point.x >= innerMinX &&
                point.y >= 0.0f &&
                point.z <= innerMaxZ && 
                point.z >= innerMinZ + halfWayDistanceZ();
    }

    public bool IsValidInsidePlayerSide(Vector3 point)
    {
        return  point.x <= innerMaxX && point.x >= innerMinX &&
                point.y >= 0.0f &&
                point.z <= innerMinZ + halfWayDistanceZ() &&
                point.z >= innerMinZ;
    }

    private float halfWayDistanceZ(){
        return ((innerMaxZ - innerMinZ) / 2.0f);
    }
    

    private void RecalculateGridBounds()
    {
        if (terrain == null) return;

        Vector3 terrainPosition = terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        // Calculate cell dimensions
        cellWidth = terrainSize.x / cols;
        cellHeight = terrainSize.z / rows;

        // Calculate inner grid bounds
        innerMinX = terrainPosition.x + innerGridStart.x * cellWidth;
        innerMaxX = innerMinX + innerCols * cellWidth;
        innerMinZ = terrainPosition.z + innerGridStart.y * cellHeight;
        innerMaxZ = innerMinZ + innerRows * cellHeight;

        Debug.Log($"Inner Grid Bounds Calculated - MinX: {innerMinX}, MinZ: {innerMinZ}, MaxX: {innerMaxX}, MaxZ: {innerMaxZ}");
    }
    public Vector2 GetWorldCoordinates(int row, int col)
    {
        Vector3 terrainPosition = terrain.transform.position;

        float worldX = terrainPosition.x + col * cellWidth;
        float worldZ = terrainPosition.z + row * cellHeight;

        return new Vector2(worldX, worldZ);
    }

    public Vector2Int GetGridCoordinates(float x, float z)
    {
        Vector3 terrainPosition = terrain.transform.position;

        int col = Mathf.FloorToInt((x - terrainPosition.x) / cellWidth);
        int row = Mathf.FloorToInt((z - terrainPosition.z) / cellHeight);

        return new Vector2Int(row, col);
    }
    public Vector3 GetRandomValidPoint()
    {
        float randomX = Random.Range(innerMinX, innerMaxX);
        float y = 0.0f;
        float randomZ = Random.Range(innerMinZ, innerMaxZ);

        return new Vector3(randomX, y, randomZ);
    }

    public Vector3 GetRandomValidPointForEnemy()
    {
        float randomX = Random.Range(innerMinX, innerMaxX);
        float y = 0.0f;
        float randomZ = Random.Range(innerMinZ + halfWayDistanceZ(), innerMaxZ);

        return new Vector3(randomX, y, randomZ);
    }
}
