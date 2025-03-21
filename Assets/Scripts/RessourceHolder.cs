using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RessourceHolder : MonoBehaviour
{
    public Image iconRessource;
    public TextMeshProUGUI valueText;

    public void InitHolder(Sprite icon,int value)
    {
        iconRessource.sprite = icon;
        valueText.text = value.ToString();
    }

    public void UpdateText(int value)
    {
        valueText.text = value.ToString();
    }
}