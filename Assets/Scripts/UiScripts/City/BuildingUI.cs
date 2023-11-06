using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buildingName;
    [SerializeField] private Image status;
    [SerializeField] private Image image;
    [SerializeField] private int level;

    public bool Interactable
    {
        get { return button.interactable; }
        set { button.interactable = value; }
    }

    public string Name
    {
        get { return buildingName.text; }
        set { buildingName.text = value; }
    }

    public Sprite Status
    {
        get { return status.sprite; }
        set { status.sprite = value; }
    }

    public Sprite Image
    {
        get { return image.sprite; }
        set { image.sprite = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    
}
