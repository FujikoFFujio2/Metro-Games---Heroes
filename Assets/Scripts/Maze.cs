using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {

	[System.Serializable]
	public class Cell {
		public bool visited;
		public GameObject north; // 1
		public GameObject east; // 2
		public GameObject west; // 3
		public GameObject south; // 4
	}

	// What is the different between gameObject and GameObject
	// GameObject is the class, gameObject is the variable that
	// the script gets attached to
	public GameObject wall;
	// the value of the wall length (on the z axis)
	public float wallLength = 1.0f;
	// number of cells we going to access
	public int xSize = 5;
	public int ySize = 5;
	private Vector3 initialPos;
	private GameObject wallHolder;
	public Cell[] cells;

	// variables for find neighbour cells, this currentCell will be a random value
	private int currentCell = 0;
	private int totalCells;

	// variables for applying Depth First Search to create the maze
	private int visitedCells = 0;
	private bool startedBuilding = false;
	private int currentNeighbour = 0;
	private List<int> lastCells = new List<int>(); // a cell stack
	// TODO: document backingUp variable
	private int backingUp = 0;
	private int wallToBreak = 0;

	// Use this for initialization
	void Start () {
		CreatePlane ();
		CreateWalls ();
		CreatePlayer ();
	}

	/*
	 * Vector3 (1, 1, 1) equivalent to a plane which cover the ground
	 * of a 10 by 10 maze.
	 * 
	 */
	void CreatePlane() {
		float xSizeOfPlane = (float)xSize / 10;
		float ySizeOfPlane = (float)ySize / 10;
		GameObject.Find ("Plane").transform.localPosition = new Vector3 (0, -0.5f, 0); 
		GameObject.Find ("Plane").transform.localScale = new Vector3 (xSizeOfPlane, 1, ySizeOfPlane); 
		// gamePlane.transform.localScale = new Vector3 (10, 1, 10); 
		//print (gamePlane);
	}

	void CreatePlayer() {
		print (GameObject.Find ("Player").transform);
		float initialPlayerPosX = -xSize / 2.0f + wallLength / 2;
		float initialPlayerPosY = -ySize / 2.0f + wallLength / 2;
		print (initialPlayerPosX); 
		GameObject.Find ("Player").transform.localPosition = new Vector3 (initialPlayerPosX, 0, initialPlayerPosY);
	}

	void CreateWalls () {
		wallHolder = new GameObject ();
		wallHolder.name = "Maze";

		float inititalXPos = -xSize / 2.0f;
		float inititalZPos = -ySize / 2.0f + wallLength / 2;
		initialPos = new Vector3 (inititalXPos, 0.0f, inititalZPos);
		Vector3 myPos = initialPos;
		GameObject tempWall;

		// For x Axis
		for (int i = 0; i < ySize; i++) {
			for (int j = 0; j <= xSize; j++) {
				float startAtX = initialPos.x + (j * wallLength);
				float startAtZ = initialPos.z + (i * wallLength);
				myPos = new Vector3 (startAtX, 0.0f, startAtZ);
				tempWall = Instantiate (wall, myPos, Quaternion.identity) as GameObject;
				// Makes the GameObject "wallHolder" the parent of the GameObject "tempWall".
				print (tempWall.transform);
				tempWall.transform.parent = wallHolder.transform;
			}
		}

		// For y Axis
		for (int i = 0; i <= ySize; i++) {
			for (int j = 0; j < xSize; j++) {
				float startAtX = initialPos.x + wallLength /2 + (j * wallLength);
				float startAtZ = initialPos.z - wallLength /2 + (i * wallLength);
				myPos = new Vector3 (startAtX, 0.0f, startAtZ);
				// The "as" keyword is used to cast the result to a specific type
				tempWall = Instantiate
					(wall, myPos, Quaternion.Euler (0.0f, 90.0f, 0.0f)) as GameObject;
				tempWall.transform.parent = wallHolder.transform;
			}
		}
		CreateCells ();
	}

	void CreateCells () {
		lastCells.Clear ();
		GameObject[] allWalls;
		totalCells = xSize * ySize;
		int children = wallHolder.transform.childCount;
		allWalls = new GameObject[children];
		this.cells = new Cell[xSize * ySize];
		int wallCreatedAlongXAxisNum = ySize * (xSize + 1);
		int eastWestProcess = 0;

		// Get all the children
		// More info on gameObject vs transform.gameObject could be found here:
		// https://forum.unity3d.com/threads/class-hierarchy-gameobject-vs-transform-gameobject.63897/
		for (int i = 0; i < children; i++) { 
			allWalls [i] = wallHolder.transform.GetChild (i).gameObject;
		}

		// Assign walls to the cells
		for (int cellprocess = 0; cellprocess < cells.Length; cellprocess++) {
			cells [cellprocess] = new Cell ();
			cells[cellprocess].west = allWalls[eastWestProcess];
			cells[cellprocess].east = allWalls[eastWestProcess + 1];
			cells [cellprocess].south = allWalls [cellprocess + wallCreatedAlongXAxisNum];
			cells [cellprocess].north = allWalls [cellprocess + wallCreatedAlongXAxisNum + xSize];
			eastWestProcess++;
			if (cellprocess % xSize == xSize - 1) {
				eastWestProcess++;
			}
		}
		Destroy(cells [0].south);
		Destroy(cells [ySize * xSize - 1].north);
		CreateMaze ();
	}

	void CreateMaze () {
		// Choose a random cell as starting point for Depth First Search Algorithm
		currentCell = Random.Range (0, totalCells);
		cells [currentCell].visited = true;
		visitedCells++;
		while (visitedCells < totalCells) {
			GiveCurrentCellNeighbours ();
			if (!cells[currentNeighbour].visited && cells[currentCell].visited) {
				BreakWall ();
				cells [currentNeighbour].visited = true;
				visitedCells++;
				lastCells.Add (currentCell);
				currentCell = currentNeighbour;
				if (lastCells.Count > 0) {
					backingUp = lastCells.Count - 1;
				}
			}
		}
	}

	/* void iteration() {
		GiveCurrentCellNeighbours ();
		if (!cells[currentNeighbour].visited && cells[currentCell].visited) {
			BreakWall ();
			cells [currentNeighbour].visited = true;
			visitedCells++;
			lastCells.Add (currentCell);
			currentCell = currentNeighbour;
			if (lastCells.Count > 0) {
				backingUp = lastCells.Count - 1;
			}
		}
		Invoke ("iteration", 0.5f);
	} */

	void BreakWall() {
		switch (wallToBreak) {
		case 1:
			Destroy (cells [currentCell].north);
			break;
		case 2:
			Destroy (cells [currentCell].east);
			break;
		case 3:
			Destroy (cells [currentCell].south);
			break;
		case 4:
			Destroy (cells [currentCell].west);
			break;
		}
	}

	void GiveCurrentCellNeighbours () {
		totalCells = xSize * ySize;
		// length holds the number of neighbours that we have found
		int length = 0;
		// 4 is the maximum adjacent neighbours that a maze cell can have
		int[] neighbours = new int[4];
		int[] connectingWalls = new int[4];
		bool isExceedingLastCellNum = currentCell + 1 > totalCells;
		bool isEastBoundCell = currentCell % xSize == xSize - 1;
		bool isSouthBoundCell = currentCell + 1 <= xSize;
		bool isWestBoundCell = currentCell % xSize ==  0;
		bool isNorthBoundCell = 
			currentCell + 1 <= totalCells
			&& 
			currentCell >= totalCells - xSize;
		
		// North neighbour
		if (!isExceedingLastCellNum && !isNorthBoundCell) {
			if (cells[currentCell + xSize].visited == false) {
				neighbours [length] = currentCell + xSize;
				connectingWalls [length] = 1;
				length++;
			}
		}

		// East neighbour
		if (!isExceedingLastCellNum && !isEastBoundCell) {
			if (cells[currentCell + 1].visited == false) {
				neighbours [length] = currentCell + 1;
				connectingWalls [length] = 2;
				length++;
			}
		}

		// South neighbour
		if (!isExceedingLastCellNum && !isSouthBoundCell) {
			if (cells[currentCell - xSize].visited == false) {
				neighbours [length] = currentCell - xSize;
				connectingWalls [length] = 3;
				length++;
			}
		}

		// West neighbour
		if (!isExceedingLastCellNum && !isWestBoundCell) {
			if (cells[currentCell - 1].visited == false) {
				neighbours [length] = currentCell - 1;
				connectingWalls [length] = 4;
				length++;
			}
		}

		if (length != 0) {
			// choose between unvisited neighbours of the cell
			int chosenNeighbourIndex = Random.Range (0, length);
			currentNeighbour = neighbours[chosenNeighbourIndex];
			wallToBreak = connectingWalls [chosenNeighbourIndex];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells[backingUp];
				backingUp--;
			}
		}
	}
}
