using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Game : MonoBehaviour
    {
        public int width;
        public int height;
        public int numMines;
        public float clickHoldTime;
        public TMP_Text remainMineText;

        private Board board;

        private Cell[,] cells;
        private List<Cell> mineCells;
        private List<Cell> notMineCells;

        private int flaggedCells;
        private bool gameOver;

        private Vector3 mouseOrigin;
        private float timer;

        private void Awake()
        {
            board = GetComponentInChildren<Board>();
        }

        private void Start()
        {
            NewGame(width, height, numMines);
        }

        public void NewGame(int width, int height, int mines)
        {
            this.width = width;
            this.height = height;
            numMines = mines;

            gameOver = false;
            cells = new Cell[width, height];
            flaggedCells = 0;

            GenerateCells(width, height);

            board.transform.position = transform.position;
            Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
            DrawNumMinesLeft();
            board.NewGame(cells);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    NewGame(16, 16, numMines);
            //}
            if (gameOver)
                return;

            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                mouseOrigin = Input.mousePosition;
                timer = Time.time;
            }

            if (Input.GetMouseButtonUp(1) && CheckMouseUp())
            {
                Flag();
                DrawNumMinesLeft();
            }
            else if (Input.GetMouseButtonUp(0) && CheckMouseUp())
            {
                Reveal();
                CheckWinCondition();
            }
        }

        private bool CheckMouseUp()
        {
            if (Input.mousePosition == mouseOrigin)
            {
                if (Time.time - timer > clickHoldTime)
                    return false;
                else return true;
            }

            return false;
        }

        private void Reveal()
        {
            Cell cell = GetCellByMousePosition();
            if (cell == null) return;

            if (cell.status == Cell.Status.Revealed || cell.status == Cell.Status.Flagged)
                return;

            switch (cell.type)
            {
                case Cell.Type.Number:
                    RevealNumberCell(cell);
                    break;
                case Cell.Type.Mine:
                    Explode(cell);
                    break;
                default: break;
            }

            board.Draw(cells);
        }

        private void Explode(Cell cell)
        {
            gameOver = true;
            cell.exploded = true;
            cell.status = Cell.Status.Revealed;

            foreach (var mineCell in mineCells)
                mineCell.status = Cell.Status.Revealed;
        }

        private void RevealNumberCell(Cell cell)
        {
            if (cell == null || cell.status == Cell.Status.Revealed)
                return;

            cell.status = Cell.Status.Revealed;

            if (cell.number == 0)
            {
                int westCellX = Mathf.Max(cell.x - 1, 0);
                int eastCellX = Mathf.Min(cell.x + 1, width - 1);
                int northCellY = Mathf.Min(cell.y + 1, height - 1);
                int southCellY = Mathf.Max(cell.y - 1, 0);

                RevealNumberCell(cells[westCellX, northCellY]);
                RevealNumberCell(cells[cell.x, northCellY]);
                RevealNumberCell(cells[eastCellX, northCellY]);

                RevealNumberCell(cells[westCellX, cell.y]);
                RevealNumberCell(cells[eastCellX, cell.y]);

                RevealNumberCell(cells[westCellX, southCellY]);
                RevealNumberCell(cells[cell.x, southCellY]);
                RevealNumberCell(cells[eastCellX, southCellY]);
            }
        }


        public void Flag()
        {
            Cell cell = GetCellByMousePosition();
            if (cell == null) return;

            switch (cell.status)
            {
                case Cell.Status.Unreveal:
                    cell.status = Cell.Status.Flagged;
                    flaggedCells++;
                    break;
                case Cell.Status.Flagged:
                    cell.status = Cell.Status.Unreveal;
                    flaggedCells--;
                    break;
                default: return;
            }

            board.Draw(cells);
        }

        private Cell GetCellByMousePosition()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = board.Tilemap.WorldToCell(worldPosition);

            if (cellPosition.x < 0 || cellPosition.x >= width) return null;
            if (cellPosition.y < 0 || cellPosition.y >= height) return null;

            return cells[cellPosition.x, cellPosition.y];
        }

        private void GenerateCells(int width, int height)
        {
            GenerateNotMineCells(width, height);
            PlaceMines(numMines);
        }

        private void GenerateNotMineCells(int width, int height)
        {
            notMineCells = new List<Cell>();

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Cell cell = new Cell(x, y);

                    notMineCells.Add(cell);
                }
            }
        }

        private void PlaceMines(int numMines)
        {
            mineCells = new List<Cell>();

            for (int i = 0; i < numMines; ++i)
            {
                int randomIndex = UnityEngine.Random.Range(0, notMineCells.Count);

                Cell mineCell = notMineCells[randomIndex];
                mineCell.type = Cell.Type.Mine;

                cells[mineCell.x, mineCell.y] = mineCell;
                mineCells.Add(mineCell);
                notMineCells.RemoveAt(randomIndex);
            }

            foreach (var emptyCell in notMineCells)
            {
                CountMines(emptyCell);
                cells[emptyCell.x, emptyCell.y] = emptyCell;
            }
        }

        private void CountMines(Cell emptyCell)
        {
            for (int adjacentX = -1; adjacentX <= 1; ++adjacentX)
            {
                int x = emptyCell.x + adjacentX;
                if (x < 0 || x >= width) continue;

                for (int adjacentY = -1; adjacentY <= 1; ++adjacentY)
                {
                    if (adjacentX == 0 && adjacentY == 0)
                        continue;

                    int y = emptyCell.y + adjacentY;
                    if (y < 0 || y >= height) continue;

                    if (cells[x, y] != null && cells[x, y].type == Cell.Type.Mine)
                        emptyCell.number++;
                }
            }
        }

        private void CheckWinCondition()
        {
            foreach (var cell in notMineCells)
            {
                if (cell.status == Cell.Status.Unreveal)
                    return;
            }

            Debug.Log("Winner!");
            gameOver = true;

            foreach (var cell in mineCells)
                cell.status = Cell.Status.Flagged;

            board.Draw(cells);
        }

        private void DrawNumMinesLeft()
        {
            string numMinesLeft = (mineCells.Count - flaggedCells).ToString();
            remainMineText.text = numMinesLeft + "/" + numMines.ToString();
        }
    }
}
