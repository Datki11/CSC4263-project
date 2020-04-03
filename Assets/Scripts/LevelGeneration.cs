using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public int numOfBattlesFought = 0;

    public int numOfRows= 4;
    public int numOfColumns = 4;
    private int numOfCells = 0;
    public int maxNumOfCells = 12;
    private int numOfChests = 0;
    public int maxNumOfChests = 6;
    public int maxNumOfEnemies = 6;
    private int numOfEnemies = 0;
    private int numOfBatEnemies = 0;
    private int numOfTentacleEnemies = 0;
    private int roomsLeftToCreate;
    public GameObject grass;
    public GameObject playerPrefab;
    public GameObject roomPrefab;
    public GameObject camera;
    public GameObject chestPrefab;
    public GameObject treePrefab;
    public GameObject smallRockPrefab;
    public GameObject skullPrefab;
    public List<GameObject> enemies;
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

        roomsLeftToCreate = maxNumOfCells;

        foreach (CellLocation cellLocation in cellsInPath) {
            Cell cell = cells[cellLocation.row, cellLocation.column];
            //Instantiate(grass, new Vector3(cellLocation.column * 2, cellLocation.row * 2, 100), Quaternion.identity);
            InstantiateCell(cellLocation);
            int numOfTreesToAdd = Mathf.RoundToInt(Random.Range(3, 12));
            bool[,] roomSpaces = new bool[14,36];

            for (int i = 0; i < numOfTreesToAdd; i++) {
                bool foundPlace = false;
                int treeRow = 0, treeColumn = 0;
                while (!foundPlace) {
                    treeRow = Mathf.RoundToInt(Random.Range(0, 13));
                    treeColumn = Mathf.RoundToInt(Random.Range(1, 35));
                    if (!roomSpaces[treeRow, treeColumn] && !roomSpaces[treeRow + 1, treeColumn] && !roomSpaces[treeRow, treeColumn + 1] && !roomSpaces[treeRow, treeColumn - 1]) {
                        foundPlace = true;
                        roomSpaces[treeRow + 1, treeColumn] = true;
                        roomSpaces[treeRow, treeColumn] = true;
                        roomSpaces[treeRow, treeColumn + 1] = true;
                        roomSpaces[treeRow, treeColumn - 1] = true;
                    }

                }

                Instantiate(treePrefab, new Vector3(cellLocation.column * 42 - 26 + treeColumn, -cellLocation.row * 24 - treeRow + 4, -0.002f), Quaternion.identity, transform);
            }


            int numOfEnemiesToAdd = Mathf.RoundToInt(Random.Range(0,2));
            if (cellLocation.row == 2 && cellLocation.column == 0) //Don't spawn enemies in the starting room
                numOfEnemiesToAdd = 0;
            //There are just enough rooms to fill in the remaining enemies, so can't have any more empty rooms
            else if (numOfEnemiesToAdd == 0 && (maxNumOfEnemies - numOfEnemies) >= roomsLeftToCreate)
                numOfEnemiesToAdd = 1;

            for (int i = 0; i < numOfEnemiesToAdd; i++) {
                bool foundPlace = false;
                int enemyRow = 0, enemyColumn = 0;
                while (!foundPlace) {
                    enemyRow = Mathf.RoundToInt(Random.Range(6, 8));
                    enemyColumn = Mathf.RoundToInt(Random.Range(12, 24));
                    if (!roomSpaces[enemyRow, enemyColumn]) {
                        foundPlace = true;
                        roomSpaces[enemyRow, enemyColumn] = true;
                    }
                }

                int indexOfEnemyToSpawn = Mathf.RoundToInt(Random.Range(0, enemies.Count));
                //Making sure there's a good distribution
                if (indexOfEnemyToSpawn == 0 && numOfBatEnemies >= 3)
                    indexOfEnemyToSpawn = 1;
                if (indexOfEnemyToSpawn == 1 && numOfTentacleEnemies >= 3)
                    indexOfEnemyToSpawn = 0;
                
                Instantiate(enemies[indexOfEnemyToSpawn], new Vector3(cellLocation.column * 42 - 26 + enemyColumn, -cellLocation.row * 24 - enemyRow + 4, -0.0018f), Quaternion.identity, transform);
                numOfEnemies++;
                if (indexOfEnemyToSpawn == 0)
                    numOfBatEnemies++;
                else
                    numOfTentacleEnemies++;
            }

            int numOfChestsToAdd = Mathf.RoundToInt(Random.Range(0, 2));
            if (cellLocation.row == 2 && cellLocation.column == 0)
                numOfChestsToAdd = 0;
            if ( (maxNumOfChests - numOfChests) >= roomsLeftToCreate)
                numOfChestsToAdd = 1;
            for (int i = 0; i < numOfChestsToAdd; i++) {
                bool foundPlace = false;
                int chestRow = 0, chestColumn = 0;
                while (!foundPlace) {
                    chestRow = Mathf.RoundToInt(Random.Range(4, 9));
                    chestColumn = Mathf.RoundToInt(Random.Range(9, 27));
                    if (!roomSpaces[chestRow, chestColumn]) {
                        foundPlace = true;
                        roomSpaces[chestRow, chestColumn] = true;
                    }

                }

            GameObject chest = Instantiate(chestPrefab, new Vector3(cellLocation.column * 42 - 26 + chestColumn, -cellLocation.row * 24 - chestRow + 4, 0f), Quaternion.identity, transform);
            chest.GetComponent<Chest>().itemName = RandomItem();
            }

            int numOfRocksToAdd = Mathf.RoundToInt(Random.Range(0, 6));
            for (int i = 0; i < numOfRocksToAdd; i++) {
                bool foundPlace = false;
                int rockRow = 0, rockColumn = 0;
                while (!foundPlace) {
                    rockRow = Mathf.RoundToInt(Random.Range(2, 11));
                    rockColumn = Mathf.RoundToInt(Random.Range(3, 33));
                    if (!roomSpaces[rockRow, rockColumn] && !roomSpaces[rockRow + 1, rockColumn] && !roomSpaces[rockRow, rockColumn + 1] && !roomSpaces[rockRow + 1, rockColumn + 1]) {
                        foundPlace = true;
                        roomSpaces[rockRow, rockColumn] = true;
                        roomSpaces[rockRow + 1, rockColumn] = true;
                        roomSpaces[rockRow , rockColumn + 1] = true;
                        roomSpaces[rockRow + 1, rockColumn + 1] = true;
                    }

                }

                Instantiate(smallRockPrefab, new Vector3(cellLocation.column * 42 - 26 + rockColumn, -cellLocation.row * 24 - rockRow + 4, -0.001f), Quaternion.identity, transform);
            }

            int numOfSkullsToAdd = Mathf.RoundToInt(Random.Range(0, 3));
            for (int i = 0; i < numOfSkullsToAdd; i++) {
                bool foundPlace = false;
                int rockRow = 0, rockColumn = 0;
                while (!foundPlace) {
                    rockRow = Mathf.RoundToInt(Random.Range(2, 11));
                    rockColumn = Mathf.RoundToInt(Random.Range(3, 33));
                    if (!roomSpaces[rockRow, rockColumn] && !roomSpaces[rockRow + 1, rockColumn] && !roomSpaces[rockRow, rockColumn + 1] && !roomSpaces[rockRow + 1, rockColumn + 1]) {
                        foundPlace = true;
                        roomSpaces[rockRow, rockColumn] = true;
                        roomSpaces[rockRow + 1, rockColumn] = true;
                        roomSpaces[rockRow , rockColumn + 1] = true;
                        roomSpaces[rockRow + 1, rockColumn + 1] = true;
                    }

                }

                Instantiate(skullPrefab, new Vector3(cellLocation.column * 42 - 26 + rockColumn, -cellLocation.row * 24 - rockRow + 4, 100f), Quaternion.identity, transform);
            }
        }

        Instantiate(playerPrefab, new Vector3(-25, -2 * 24 - 6, -0.0017f), Quaternion.identity, transform);
        camera.GetComponent<CameraBehavior>().SetActive();
    }

    void InstantiateCell(CellLocation cellLocation) {
        GameObject newRoom = Instantiate(roomPrefab, new Vector3(cellLocation.column * 42, -cellLocation.row * 24, 0), Quaternion.identity, transform);
        Cell cell = cells[cellLocation.row, cellLocation.column];
        foreach(Direction dir in cell.openings) {
            Destroy(newRoom.transform.GetChild((int) dir).transform.GetChild(5).gameObject);
            Destroy(newRoom.transform.GetChild((int) dir).transform.GetChild(6).gameObject);
        }
        roomsLeftToCreate--;

    }

    string RandomItem() {
        if (Mathf.RoundToInt(Random.Range(0,2)) == 0)
            return "Potion";
        else
            return "Firecracker";

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
