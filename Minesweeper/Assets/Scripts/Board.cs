using Assets.Scripts;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap Tilemap { get; private set; }

    public Tile tileNum1;
    public Tile tileNum2;
    public Tile tileNum3;
    public Tile tileNum4;
    public Tile tileNum5;
    public Tile tileNum6;
    public Tile tileNum7;
    public Tile tileNum8;
    public Tile tileUnknown;
    public Tile tileEmpty;
    public Tile tileMine;
    public Tile tileExploded;
    public Tile tileFlag;

    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    public void NewGame(Cell[,] cells)
    {
        Tilemap.ClearAllTiles();
        Draw(cells);
    }

    public void Draw(Cell[,] cells)
    {
        foreach (var cell in cells)
        {
            Tilemap.SetTile(new Vector3Int(cell.x, cell.y), GetTile(cell));
        }
    }

    private Tile GetTile(Cell cell)
    {
        switch (cell.status)
        {
            case Cell.Status.Unreveal:
                return tileUnknown;
            case Cell.Status.Revealed:
                return GetReaveledTile(cell);
            case Cell.Status.Flagged:
                return tileFlag;
            default: return null;
        }
    }

    private Tile GetReaveledTile(Cell cell)
    {
        switch (cell.type)
        {
            case Cell.Type.Number:
                return GetNumberTile(cell);
            case Cell.Type.Mine:
                return cell.exploded ? tileExploded : tileMine;
            default: return null;
        }
    }

    private Tile GetNumberTile(Cell cell)
    {
        switch (cell.number)
        {
            case 0: return tileEmpty;
            case 1: return tileNum1;
            case 2: return tileNum2;
            case 3: return tileNum3;
            case 4: return tileNum4;
            case 5: return tileNum5;
            case 6: return tileNum6;
            case 7: return tileNum7;
            case 8: return tileNum8;

            default: return null;
        }
    }
}


