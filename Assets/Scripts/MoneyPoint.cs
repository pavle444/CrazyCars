using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPoint : MonoBehaviour
{
    public static int moneyAmount = 1;
    public AudioClip moneySound;
    public GameObject popupGO;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("AS2").GetComponent<AudioSource>();
        moneyAmount = PlayerPrefs.GetInt("Income") == 0 ? 1 : PlayerPrefs.GetInt("Income");
    }

    void Update()
    {
        PlayerPrefs.SetInt("Income", moneyAmount);
    }

    void OnTriggerEnter(Collider trig)
    {
        Popup popup = popupGO.GetComponent<Popup>();

        if (Manager.sfxOn) audioSource.PlayOneShot(moneySound);

        if (trig.gameObject.tag == "Car1")
        {
            popup.HowMuchCoins(moneyAmount);
            Instantiate(popupGO, transform.position, Quaternion.identity);
            Manager.money = (int)(Manager.money + moneyAmount);
        }
        else if (trig.gameObject.tag == "Car2")
        {
            popup.HowMuchCoins(moneyAmount * 3);
            Instantiate(popupGO, transform.position, Quaternion.identity);
            Manager.money = (int)(Manager.money + moneyAmount * 5);
        }
        else if (trig.gameObject.tag == "Car3")
        {
            popup.HowMuchCoins(moneyAmount * 9);
            Instantiate(popupGO, transform.position, Quaternion.identity);
            Manager.money = (int)(Manager.money + moneyAmount * 15);
        }
    }
}
