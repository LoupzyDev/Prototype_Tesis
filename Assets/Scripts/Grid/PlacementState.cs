using UnityEngine;

public class PlacementState : IBuildingState {
    private int selectedObjectIndex = -1;
    private int rotationIndex = 0; // Agregado para manejar la rotación
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          ObjectPlacer objectPlacer) {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1) {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        } else
            throw new System.Exception($"No object with ID {iD}");
    }

    public void EndState() {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition) {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false) {
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            furnitureData :
            furnitureData;
        selectedData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex) {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
            furnitureData :
            furnitureData;

        return selectedData.CanPlaceObejctAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition) {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    public void RotateObject(bool clockwise) {
        if (clockwise) {
            rotationIndex = (rotationIndex + 1) % 4;
        } else {
            rotationIndex = (rotationIndex - 1 + 4) % 4;
        }

        UpdateObjectSize();
    }
    public void RotateFuture() {
        GameObject prefab = database.objectsData[selectedObjectIndex].Prefab;


        Transform firstChild = prefab.transform.GetChild(0);


        if (Quaternion.Angle(firstChild.rotation, Quaternion.Euler(0, 0, 0)) < 0.01f) {
            firstChild.rotation = Quaternion.Euler(0, 180, 0);
        } else {

            firstChild.rotation = Quaternion.Euler(0, 0, 0);
        }   
    }

    private void UpdateObjectSize() {   
        var objectData = database.objectsData[selectedObjectIndex];
        int newSizeX = objectData.Size.x;
        int newSizeY = objectData.Size.y;

        // Ajustar tamaño según la rotación
        if (rotationIndex % 2 == 1) // Rotación 90 o 270 grados
        {
            (newSizeX, newSizeY) = (newSizeY, newSizeX);
        }

        // Actualiza el tamaño en la base de datos
        objectData.Size = new Vector2Int(newSizeX, newSizeY);
        previewSystem.UpdateCursorSize(newSizeX, newSizeY);
    }
}
