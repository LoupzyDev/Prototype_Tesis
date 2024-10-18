using UnityEngine;
public enum objState {
    north,
    east,
    south,
    west
}
public class PlacementState : IBuildingState {

    public objState state;

    private int selectedObjectIndex = -1;
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

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }


    public void UpdateState(Vector3Int gridPosition) {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    public void ChangeGameState(objState newState) {
        state = newState;
        GameObject prefab = database.objectsData[selectedObjectIndex].Prefab;

        Transform firstChild = prefab.transform.GetChild(0);
        Transform secondChild = prefab.transform.GetChild(1);

        Transform firstChildPreviw = previewSystem.previewObject.transform.GetChild(0);
        Transform secondChildPreviw = previewSystem.previewObject.transform.GetChild(1);


        firstChild.gameObject.SetActive(false);
        secondChild.gameObject.SetActive(false);

        firstChildPreviw.gameObject.SetActive(false);
        secondChildPreviw.gameObject.SetActive(false);


        UpdateObjectSize();


        switch (state) {
            case objState.north:
                firstChild.rotation =firstChildPreviw.rotation= Quaternion.Euler(0, 0, 0);
                firstChild.gameObject.SetActive(true);
                firstChildPreviw.gameObject.SetActive(true);

                break;
            case objState.east:
                secondChild.rotation= secondChildPreviw.rotation = Quaternion.Euler(0, 90, 0);
                secondChild.gameObject.SetActive(true);
                secondChildPreviw.gameObject.SetActive(true);

                break;
            case objState.south:
                firstChild.rotation=firstChildPreviw.rotation = Quaternion.Euler(0, 180, 0);
                firstChild.gameObject.SetActive(true);
                firstChildPreviw.gameObject.SetActive(true);

                break;
            case objState.west:
                secondChild.rotation=secondChildPreviw.rotation = Quaternion.Euler(0, 270, 0);
                secondChild.gameObject.SetActive(true);
                secondChildPreviw.gameObject.SetActive(true);
                break;
        }
    }


    private void UpdateObjectSize() {
        var objectData = database.objectsData[selectedObjectIndex];
        int newSizeX = objectData.Size.x;
        int newSizeY = objectData.Size.y;


       (newSizeX, newSizeY) = (newSizeY, newSizeX);


        objectData.Size = new Vector2Int(newSizeX, newSizeY);
        previewSystem.UpdateCursorSize(newSizeX, newSizeY);
    }

}
