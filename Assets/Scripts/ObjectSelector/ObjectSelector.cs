using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelector : MonoBehaviour
{
    public static ObjectSelector Instance;

    [Header ("Events")]
    public UnityEvent onSelectedObjectChange;

    [Header ("Camera referances: ")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera uiCamera;

    [Header("Raycast layers: ")]
    [SerializeField] private LayerMask layersToHit;
    [SerializeField] private LayerMask uiLayers;
    
    [Header("Object selection information")]
    private Dictionary<PlayerTag, WorldObject> playerObjectDictionary;
    private WorldObject selectedObject;
    private PlayerTag currentPlayer;

    private bool isSelectorActive;

    private void Awake () { Instance = this; }

    private void Start () {
        playerObjectDictionary = InitializePlayerObjectDictionary();
        TurnManager.Instance.OnNewDay += SetUpSelectorForNewPlayer;
        InputManager.Instance.mouseInput.OnLeftMouseButtonDown += CheckForObject;
        GameManager.Instance.StartGame();
    }

    private Dictionary<PlayerTag, WorldObject> InitializePlayerObjectDictionary()
    {
        var dictionary = new Dictionary<PlayerTag, WorldObject>();
        
        foreach (PlayerTag tag in Enum.GetValues(typeof(PlayerTag))){
            dictionary[tag] = null;
        }
        
        return dictionary;
    }

    private void CheckForObject (){
        if (InputManager.Instance.mouseInput.IsMouseOverUI() || !isSelectorActive) return;
        GameObject obj = InputManager.Instance.mouseInput.GetMouseOverWorldObject(layersToHit);
        if (obj != null) SelectObject(obj);
        else{
            selectedObject = null;
        }
    }

    private void SelectObject (GameObject obj)
    {
        switch (obj.tag)
        {
            case "WorldObject":
                HandleWorldObjects(obj.GetComponentInParent<WorldObject>());
            break;

            case "Enterance":
                HandleEnterance(obj.GetComponent<ObjectEnterance>());
            break;

            case "GridCell":
                HandleGridCells(obj.GetComponent<GridCell>());
            break;

            default:
                selectedObject = null;
            break;
        }
         
    }

    public void HandleWorldObjects (WorldObject obj){
        if (obj is City){
            HandleCity(obj as City);
        }
        else if (obj is Army){
            HandleArmy(obj as Army);
        }
        else if (obj is Mine){
            HandleMine(obj as Mine);
        }
        else if (obj is ResourcesObject){
            HandleResourceObject(obj as ResourcesObject);
        }
    }

    private void HandleCity (City city){
        if (selectedObject == null){
            city.ObjectSelected();
            HandleSelectionChange(city);
        }
    }

    private void HandleArmy (Army army){
        if (IsSelectedObjectArmy()){
            if (selectedObject == army) army.ObjectSelected();
            else {
                if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
                (selectedObject as Army).Move(army.transform.position);
            }
        }else {
            HandleSelectionChange(army);
        }
    }

    private void HandleMine (Mine mine){
        if (IsSelectedObjectArmy() && (selectedObject as Army).IsMoving()){
            (selectedObject as Army).Stop();
        }
        mine.ObjectSelected();
    }

    private void HandleResourceObject(ResourcesObject rObj){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(rObj.transform.position);
        }
    }

    private void HandleEnterance (ObjectEnterance enterance){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(enterance.transform.position);
        }
    }

    private void HandleGridCells (GridCell cell){
        if (IsSelectedObjectArmy()){
            if ((selectedObject as Army).IsMoving()) (selectedObject as Army).Stop();
            (selectedObject as Army).Move(cell.transform.position);
        }
    }

    private void HandleSelectionChange (WorldObject obj){
        if (selectedObject != null){
            obj.ObjectDeselected();
        }
        selectedObject = obj;
        onSelectedObjectChange.Invoke();
    }

    public void CancelSelection (WorldObject obj){
        if (obj != null && obj == selectedObject){
            HandleSelectionChange(null);
        }
    }

    private void SetUpSelectorForNewPlayer (){
        PlayerTag tag = PlayerManager.Instance.GetCurrentPlayer();
        playerObjectDictionary[currentPlayer] = selectedObject;
        if (playerObjectDictionary[tag] == null){
            playerObjectDictionary[tag] = PlayerManager.Instance.GetPlayer(tag).GetObjectToSelect();
        }
        HandleWorldObjects(playerObjectDictionary[tag]);
    }

    private bool IsSelectedObjectArmy (){
        return selectedObject != null && selectedObject is Army;
    }
}