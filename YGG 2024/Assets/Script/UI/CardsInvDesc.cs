using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardsInvDesc : MonoBehaviour
{
    [SerializeField]
    private Image itemImage;

    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private TMP_Text Description;
    [SerializeField]
    private TMP_Text Type;

    [SerializeField]
    private TMP_Text cName;
    [SerializeField]
    private TMP_Text cDescription;
    [SerializeField]
    private TMP_Text cType;




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateData(Sprite Image, string name, string desc, string type)
    {
        itemImage.sprite = Image;
        Name.text = name;
        Description.text = desc;
        Type.text = type;

        cName.text = name;
        cDescription.text = desc;
        cType.text = type;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
