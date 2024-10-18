using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    [SerializeField]
    private List<Vector3Int> tilesToBlock;

    private GridData floorData, furnitureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    [SerializeField] private NavMeshSurface navMeshSurface;

    private void Start()
    {
        gridVisualization.SetActive(false);
        floorData = new();
        furnitureData = new();


        // Bloquear todos los tiles en la lista
        foreach (var tilePosition in tilesToBlock) {
            BlockTile(tilePosition);
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);

        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           floorData,
                                           furnitureData,
                                           objectPlacer);

        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }
    public void CloseWindows() {

        StopPlacement();

    }
    public void StartRemoving()     
    {
        StopPlacement();
        gridVisualization.SetActive(true) ;
        buildingState = new RemovingState(grid, preview, floorData, furnitureData, objectPlacer); 
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    private void PlaceStructure()
    {
        if(inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);


        buildingState.OnAction(gridPosition);
        navMeshSurface.BuildNavMesh();


    }



    private void StopPlacement()
    {

        if (buildingState == null)
            return;
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }
    public void BlockTile(Vector3Int gridPosition) {
        floorData.BlockTile(gridPosition);
        furnitureData.BlockTile(gridPosition);
    }

    private void Update() {
        if (buildingState == null)
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPosition) {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            if (buildingState is PlacementState placementState) {
                objState newState = placementState.state switch {
                    objState.north => objState.west,
                    objState.west => objState.south,
                    objState.south => objState.east,
                    objState.east => objState.north,
                    _ => objState.north
                };
                placementState.ChangeGameState(newState);
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (buildingState is PlacementState placementState) {

                objState newState = placementState.state switch {
                    objState.north => objState.east,
                    objState.east => objState.south,
                    objState.south => objState.west,
                    objState.west => objState.north,
                    _ => objState.north
                };
                placementState.ChangeGameState(newState);
            }
        }
    }



}
