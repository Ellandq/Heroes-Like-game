using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    [SerializeField] City relatedCity;
    [SerializeField] Mine relatedMine;
    [SerializeField] Army relatedArmy;
    [SerializeField] ResourcesObj relatedResource;
    //[SerializeField] Building relatedBuilding;

    internal string objectTag;

    void Start ()
    {
        objectTag = this.gameObject.tag;

        switch (objectTag)
        {
            case "CityEnterance":
                relatedCity = this.gameObject.GetComponentInParent<City>();
            break;

            case "MineEnterance":
                relatedMine = this.gameObject.GetComponentInParent<Mine>();
            break;

            case "Army":
                relatedArmy = this.gameObject.GetComponentInParent<Army>();
            break;

            case "BuildingEnterance":
                //relatedBuilding = this.gameObject.GetComponentInParent<Building>();
            break;

            case "Resource":
                relatedResource = this.gameObject.GetComponentInParent<ResourcesObj>();
            break;

            default:
                Debug.Log("No object detected");
            break;
        }
    }

    public void ObjectInteractionEvent(GameObject interactingArmy)
    {
        switch (objectTag)
        {
            case "CityEnterance":
                relatedCity.CityInteraction(interactingArmy);
            break;

            case "MineEnterance":
                relatedMine.MineInteraction(interactingArmy);
            break;

            case "Army":
                relatedArmy.ArmyInteraction(interactingArmy);
            break;

            case "BuildingEnterance":
                //relatedBuilding.BuildingInteraction(interactingArmy);
            break;

            case "Resource":
                relatedResource.ResourceInteraction(interactingArmy);
            break;

            default:
                Debug.Log("No object detected");
            break;
        }
    }

    public void ChangeObjectName (string _name){
        this.gameObject.name = this.gameObject.name + ": " + _name;
    }
}
