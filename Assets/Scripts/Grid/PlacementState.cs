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
        if (!placementValidity) {
            return;
        }

        // Obtener el precio del objeto
        int price = database.objectsData[selectedObjectIndex].Price;

        // Verificar si el jugador tiene suficiente dinero
        if (!GameManager._instance.CanAfford(price)) {
            Debug.Log("eres pobre");
            return;
        }

        // Deduce el dinero del jugador
        GameManager._instance.DeductMoney(price);

        // Colocar el objeto si el jugador tiene suficiente dinero
        int index = objectPlacer.PlaceObject(
            database.objectsData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition)
        );

        GridData selectedData = furnitureData;
        selectedData.AddObjectAt(
            gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index
        );

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

        var objectData = database.objectsData[selectedObjectIndex];

        Transform firstChild = prefab.transform.GetChild(0);
        Transform secondChild = prefab.transform.GetChild(1);

        Transform firstChildPreviw = previewSystem.previewObject.transform.GetChild(0);
        Transform secondChildPreviw = previewSystem.previewObject.transform.GetChild(1);


        firstChild.gameObject.SetActive(false);
        secondChild.gameObject.SetActive(false);

        firstChildPreviw.gameObject.SetActive(false);
        secondChildPreviw.gameObject.SetActive(false);


        int originalSizeX = objectData.Size.x;
        int originalSizeY = objectData.Size.y;

        Vector2Int originalSize = new Vector2Int(originalSizeX, originalSizeY);
        Vector2Int newSize = new Vector2Int(originalSizeY, originalSizeX);


        Debug.Log($"Estado del mueble: {state}");

        switch (state) {
            case objState.north:
                firstChild.rotation =firstChildPreviw.rotation= Quaternion.Euler(0, 0, 0);
                firstChild.gameObject.SetActive(true);
                firstChildPreviw.gameObject.SetActive(true);
                objectData.Size = originalSize;
                previewSystem.UpdateCursorSize(originalSizeX, originalSizeY);
                break;
            case objState.east:
                secondChild.rotation= secondChildPreviw.rotation = Quaternion.Euler(0, 90, 0);
                secondChild.gameObject.SetActive(true);
                secondChildPreviw.gameObject.SetActive(true);
                objectData.Size = newSize;
                previewSystem.UpdateCursorSize(originalSizeY, originalSizeX);

                break;
            case objState.south:
                firstChild.rotation=firstChildPreviw.rotation = Quaternion.Euler(0, 180, 0);
                firstChild.gameObject.SetActive(true);
                firstChildPreviw.gameObject.SetActive(true);
                objectData.Size = originalSize;
                previewSystem.UpdateCursorSize(originalSizeX, originalSizeY);

                break;
            case objState.west:
                secondChild.rotation=secondChildPreviw.rotation = Quaternion.Euler(0, 270, 0);
                secondChild.gameObject.SetActive(true);
                secondChildPreviw.gameObject.SetActive(true);
                objectData.Size = newSize;
                previewSystem.UpdateCursorSize(originalSizeY, originalSizeX);
                break;
        }
    }

}
