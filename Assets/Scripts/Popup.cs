using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public TMP_Text text;
    public Image coin;
    public Transform finalPos;

    void Update()
    {
        text.color = Color.Lerp(text.color, Color.clear, 3 * Time.deltaTime);
        coin.color = Color.Lerp(coin.color, Color.clear, 3 * Time.deltaTime);
        text.transform.position = Vector3.MoveTowards(text.transform.position, finalPos.position, 1f * Time.deltaTime);
    }

    public void HowMuchCoins(int amount)
    {
        text.text = "+" + amount.ToString();
    }
}
