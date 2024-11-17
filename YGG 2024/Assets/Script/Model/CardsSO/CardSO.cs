using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Card")]
public class CardSO : ScriptableObject
{

    [field: SerializeField]
    public string UniqueID { get; private set; }

    [field: SerializeField]
    public int MaxStackableSize { get; set; } = 1;

    public int ID => GetInstanceID();

    [field: SerializeField]
    public bool IsStackable { get; set; }

    [field: SerializeField]
    public int cHealth { get; set; }

    [field: SerializeField]
    public int cAttack { get; set; }

    [field: SerializeField]
    public string cName { get; set; }
    [field: SerializeField]
    public string cDescription { get; set; }
    [field: SerializeField]
    public string cType { get; set; }
    [field: SerializeField]
    public Sprite cImage { get; set; }
    [field: SerializeField]
    public string ardriveLink { get; set; }
    
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(UniqueID))
        {
            UniqueID = Guid.NewGuid().ToString();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }
    public CardSO CreateCopy()
    {
        // Create a new instance of QuestSO
        CardSO newQuest = ScriptableObject.CreateInstance<CardSO>();

        // Copy all fields from this QuestSO to the new instance
        newQuest.UniqueID = this.UniqueID;
        newQuest.MaxStackableSize = this.MaxStackableSize;
        newQuest.IsStackable = this.IsStackable;
        newQuest.cHealth = this.cHealth;
        newQuest.cAttack = this.cAttack;
        newQuest.cName = this.cName;
        newQuest.cDescription = this.cDescription;
        newQuest.cType = this.cType;
        newQuest.cImage = this.cImage;
        newQuest.ardriveLink = this.ardriveLink;
        return newQuest;
    }

}
