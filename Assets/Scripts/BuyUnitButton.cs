using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuyUnitButton : MonoBehaviour
{
    public Button button;
    public Image imageUnit;
    public TextMeshProUGUI nbUnitText;
    public Image imageCoin;
    public TextMeshProUGUI nbCoinText;

    public void InitButton(UnityAction action, Sprite iconUnit, string nameUnit,Sprite iconCoin, int nbCoin)
    {
        button.onClick.AddListener(action);

        imageUnit.sprite = iconUnit;
        nbUnitText.text = nameUnit;

        imageCoin.sprite = iconCoin;
        nbCoinText.text = nbCoin.ToString();
    }
}