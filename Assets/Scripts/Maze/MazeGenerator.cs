using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;
using System.ComponentModel;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int depth = 20;
    [SerializeField] private GameObject mazeCellPrefab;
    [SerializeField] private MazeCell[,] mazeGrid;
    [SerializeField] private PhysicMaterial lavaPhysicsMaterial;
    [SerializeField] private PhysicMaterial icePhysicsMaterial;
    [SerializeField] private Material iceGroundMaterial;
    [SerializeField] private Material lavaGroundMaterial;
    [SerializeField] private Material iceWallsMaterial;
    [SerializeField] private Material lavaWallsMaterial;
    [SerializeField] private Material startEndMaterial;
    [SerializeField] private GameObject templatePlane;
    private GameObject icePlane;
    private GameObject lavaPlane;

    [SerializeField] private List<GameObject> backTrackingCells;
    [SerializeField] private List<GameObject> endgoalPath;
    [SerializeField] private Stack<GameObject> pathStack;

    public static MazeGenerator instance;
    public event EventHandler OnMazeGenerated;
    public event EventHandler<List<GameObject>> OnPathGenerated;
    private int totalBlocks=0;

    
    private void Awake()
    {
        instance = this;
        totalBlocks = width * depth;
        backTrackingCells = new List<GameObject>();
        endgoalPath = new List<GameObject>();
        pathStack = new Stack<GameObject>();
    }
    void Start()
    {
        //GenerateGrid(width,depth);
        StartCoroutine(MakeGrid());
    }

    private IEnumerator MakeGrid()
    {
        yield return new WaitForSeconds(1);
        GenerateGrid(width, depth);
    }

    private void SetGround()
    {
        lavaPlane= Instantiate(templatePlane);
        icePlane = Instantiate(templatePlane);
        lavaPlane.name = "LavaPlane";
        icePlane.name = "icePlane";
        lavaPlane.transform.localScale = new Vector3(width * 1f / 20, 1, width *1f/ 10);
        icePlane.transform.localScale = new Vector3(width * 1f / 20, 1, width *1f/ 10);
        lavaPlane.transform.position = new Vector3(3.55f, 0, 7.45f);
        icePlane.transform.position = new Vector3(11.55f, 0, 7.45f);
        lavaPlane.GetComponent<MeshRenderer>().material = lavaGroundMaterial;
        icePlane.GetComponent<MeshRenderer>().material = iceGroundMaterial;
        lavaPlane.GetComponent<MeshCollider>().material = lavaPhysicsMaterial;
        icePlane.GetComponent<MeshCollider>().material = icePhysicsMaterial;
    }
    private void GenerateGrid(int width, int depth)
    {
        SetGround();
        GameObject Grid = new GameObject();
        Grid.transform.parent = null;
        Grid.name = "Grid";
        mazeGrid=new MazeCell[width,depth];
        for(int i=0;i<width; i++) {
            for (int j = 0; j < depth; j++){
                mazeGrid[i, j] = Instantiate(mazeCellPrefab, new Vector3(i, 0, j), Quaternion.identity).GetComponent<MazeCell>();
                mazeGrid[i, j].transform.parent = Grid.transform;
                mazeGrid[i, j].transform.name = "MazeCell (" + (i+1) + "," + (j+1) + ")";
                if (i < width / 2) { SetMaterialsAndPhysicMaterials(mazeGrid[i, j], lavaGroundMaterial, lavaPhysicsMaterial);SetWallMaterial(mazeGrid[i, j], lavaWallsMaterial); }
                else { SetMaterialsAndPhysicMaterials(mazeGrid[i, j], iceGroundMaterial, icePhysicsMaterial); SetWallMaterial(mazeGrid[i, j], iceWallsMaterial); }
                if((i==0 && j == 0) || (i == width-1 && j == depth-1)) {
                    SetWallMaterial(mazeGrid[i, j], startEndMaterial);
                    if((i == width - 1 && j == depth - 1))
                    {
                        mazeGrid[i, j].GetEntryExitTrigger().tag = "WonTrigger";
                    }
                }
            }
        }
       GenerateMaze(null,mazeGrid[0, 0]);
    }

    private void SetMaterialsAndPhysicMaterials(MazeCell cell,Material mat, PhysicMaterial physMat)
    {
        //cell.SetMaterial(mat);
        cell.SetGroundMaterial(mat);
        cell.SetGroundPhysicsAsset(physMat);

    }

    private void SetWallMaterial(MazeCell cell,Material mat)
    {
        cell.SetMaterial(mat);
    }
    private void GenerateMaze(MazeCell prevCell, MazeCell currCell)
    {
        currCell.Visit();
        totalBlocks--;
        pathStack.Push(currCell.gameObject);
        if (totalBlocks == 0)
        {
            HandlePathCompleted();
            OnMazeGenerated?.Invoke(this, EventArgs.Empty);
            return;
        }
        ClearWalls(prevCell, currCell);
        MazeCell nextCell;
        bool foundNewCell = false;
        do
        {
            nextCell = GetNextUnvisitedCell(currCell);
            if (nextCell)
            {
                foundNewCell = true;
                GenerateMaze(currCell, nextCell);
            }
        }
        while (nextCell);
        if (!foundNewCell)
        {
            pathStack.Pop();
            if (prevCell)
            {
                backTrackingCells.Add(currCell.gameObject);
            }
        }
    }
        
    private void ClearWalls(MazeCell prevCell, MazeCell currCell) {
        if (!prevCell) { return; }
        if (prevCell.transform.position.x < currCell.transform.position.x) { currCell.ClearLeft(); prevCell.ClearRight(); return; }
        if (prevCell.transform.position.x > currCell.transform.position.x) { currCell.ClearRight(); prevCell.ClearLeft(); return; }
        if (prevCell.transform.position.z < currCell.transform.position.z) { currCell.ClearDown(); prevCell.ClearUp(); return; }
        if (prevCell.transform.position.z > currCell.transform.position.z) { currCell.ClearUp(); prevCell.ClearDown(); return; }
    }
    private void HandlePathCompleted()
    {
        /*
        endgoalPath=FindPathBFS(new Vector2Int(0, 0), new Vector2Int(15, 15));
        yield return new WaitForSeconds(3f);
        foreach (GameObject go in endgoalPath){
            go.GetComponent<MazeCell>().SetHighlight();
        }
        OnPathGenerated?.Invoke(this, endgoalPath);
        */
    }
    private MazeCell GetNextUnvisitedCell(MazeCell cell)
    {
        IEnumerable<MazeCell> unvisitedCells = GetUnvisitedCells(cell);
        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell cell)
    {
        int x = ((int)cell.transform.position.x);
        int z = ((int)cell.transform.position.z);
        if (x + 1 < width) { if (!mazeGrid[x + 1, z].IsVisited) { yield return (mazeGrid[x + 1, z]); } }
        if (x - 1 >= 0) { if (!mazeGrid[x - 1, z].IsVisited) { yield return (mazeGrid[x - 1, z]); } }
        if (z + 1 < depth) { if (!mazeGrid[x, z + 1].IsVisited) { yield return (mazeGrid[x, z + 1]); } }
        if (z - 1 >= 0) { if (!mazeGrid[x, z - 1].IsVisited) { yield return (mazeGrid[x, z - 1]); } }
    }
    public List<GameObject> GetBacktrackedCells() { return backTrackingCells; }
    public Vector2 GetGridDimensions() { return new Vector2(width, depth); }
    public MazeCell GetCell(int i, int j) { return mazeGrid[i, j]; }
    public Vector2 GetGridCoordinates(MazeCell mz)
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < depth; j++)
            {
                if (mazeGrid[i, j] == mz) { return new Vector2(i, j); }
            }
        }
        return Vector2.zero;
    }
}
