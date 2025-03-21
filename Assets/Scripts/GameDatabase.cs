using UnityEngine;

[System.Serializable]
public struct Unit
{
    public Sprite sprite;
    public string name;
    public int score;
    public int price;
}

[CreateAssetMenu(fileName = "UnitDatabase", menuName = "UnitDatabase")]
public class GameDatabase : ScriptableObject
{
    public Unit[] units;
    public Sprite coinSprite;
    [SerializeField] private Sprite[] chestSprite;

    public Sprite ClosedChest => chestSprite[0];
    public Sprite OpenChest => chestSprite[1];
}