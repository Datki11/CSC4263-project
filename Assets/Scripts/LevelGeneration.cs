using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{

    public int numOfRows= 4;
    public int numOfColumns = 4;
    private int numOfCells = 0;
    public int maxNumOfCells = 12;
    public GameObject grass;
    public GameObject playerPrefab;
    public GameObject roomPrefab;
    public GameObject camera;
    public Cell[,] cells;
    public enum Direction {
        North,
        East,
        South,
        West
    }
    public class Cell {

        public HashSet<Direction> openings;
        public Cell() {
            openings = new HashSet<Direction>();
        }

    }

    public class CellLocation {
        public int row;
        public int column;
        public CellLocation (int i, int j) {
            row = i;
            column = j;
        }
        public CellLocation() {
            row = 0;
            column = 0;
        }
    }
    void Awake()
    {
         cells = new Cell[4,4];
         CreatePath();
        
    }

    private void CreatePath() {
        int row = 2, column = 0;
        List<CellLocation> cellsInPath = new List<CellLocation>();
        Cell cell = new Cell();
        cells[row, column] = cell;
        cellsInPath.Add(new CellLocation(row, column));
        numOfCells++;
        ChooseDirection(row,column,cellsInPath);
    }
    private void ChooseDirection(int row, int column, List<CellLocation> cellsInPath) {

        int iterations = 0;
        CellLocation cellLocation = new CellLocation(row, column);
        Cell newCell = new Cell();
        int randomNum = Mathf.RoundToInt(Random.Range(0,4));
        bool foundNewCell = false;

        while (iterations < 4 && !foundNewCell) {

            if (randomNum == 0) {
                int newColumn = column - 1;
                if (newColumn >= 0 && cellsInPath.Find(x => x.row == row && x.column == newColumn) == null) {
                    cellLocation.column = newColumn;
                    newCell.openings.Add(Direction.East);
                    cells[row, column].openings.Add(Direction.West);
                    foundNewCell = true;
                }
                else
                    randomNum++;
            }

            else if (randomNum == 1) {
                int newRow = row - 1;
                if (newRow >= 0 && cellsInPath.Find(x => x.row == newRow && x.column == column) == null ) {
                    cellLocation.row = newRow;
                    newCell.openings.Add(Direction.South);
                    cells[row,column].openings.Add(Direction.North);
                    foundNewCell = true;
                }
                else
                    randomNum++;
            }

            else if (randomNum == 2) {
                int newColumn = column + 1;
                if (newColumn < numOfColumns && cellsInPath.Find(x => x.row == row && x.column == newColumn) == null) {
                    cellLocation.column = newColumn;
                    newCell.openings.Add(Direction.West);
                    cells[row, column].openings.Add(Direction.East);
                    foundNewCell = true;
                }
                else
                    randomNum++;
            }

            else if (randomNum == 3) {
                int newRow = row + 1;
                if (newRow < numOfRows && cellsInPath.Find(x => x.row == newRow && x.column == column) == null) {
                    cellLocation.row = newRow;
                    newCell.openings.Add(Direction.North);
                    cells[row, column].openings.Add(Direction.South);
                    foundNewCell = true;
                }
                else
                    randomNum++;
            }

            iterations++;
        }

        if (foundNewCell) {
            cells[cellLocation.row, cellLocation.column] = newCell;
            numOfCells++;
            cellsInPath.Add(cellLocation);
            if (numOfCells < maxNumOfCells) {
                ChooseDirection(cellLocation.row, cellLocation.column, cellsInPath);
            }
            else {
                CreateLevel(cellsInPath);
            }
        }

        else {
            int randomIndex = Mathf.RoundToInt(Random.Range(0, cellsInPath.Count));
            CellLocation newCellLocation = cellsInPath[randomIndex];
            ChooseDirection(newCellLocation.row, newCellLocation.column, cellsInPath);
        }
    }

    void CreateLevel(List<CellLocation> cellsInPath) {

        foreach (CellLocation cellLocation in cellsInPath) {
            Cell cell = cells[cellLocation.row, cellLocation.column];
            //Instantiate(grass, new Vector3(cellLocation.column * 2, cellLocation.row * 2, 100), Quaternion.identity);
            InstantiateCell(cellLocation);
        }

        Instantiate(playerPrefab, new Vector2(0, -2 * 24 - 10), Quaternion.identity);
        camera.GetComponent<CameraBehavior>().SetActive();
    }

    void InstantiateCell(CellLocation cellLocation) {
        GameObject newRoom = Instantiate(roomPrefab, new Vector3(cellLocation.column * 42, -cellLocation.row * 24, 0), Quaternion.identity);
        Cell cell = cells[cellLocation.row, cellLocation.column];
        if (cell.openings.Contains(Direction.North)) {
            Destroy(newRoom.transform.GetChild(0).transform.GetChild(5).gameObject);
        }
        if (cell.openings.Contains(Direction.East)) {
            Destroy(newRoom.transform.GetChild(1).transform.GetChild(5).gameObject);
        }
        if (cell.openings.Contains(Direction.South)) {
            Destroy(newRoom.transform.GetChild(2).transform.GetChild(5).gameObject);
        }
        if (cell.openings.Contains(Direction.West)) {
            Destroy(newRoom.transform.GetChild(3).transform.GetChild(5).gameObject);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
