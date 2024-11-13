using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Card")]
public class CardSO : ScriptableObject
{    
    public string cName;
    public string cDescription;
    public string cType;
    public Sprite cImage;
}
