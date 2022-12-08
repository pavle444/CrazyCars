using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Manager : MonoBehaviour
{
    private Transform 
        newCarPosition;
    private int
        trackNum,
        carCost = 0, 
        trackCost = 500, 
        incomeCost = 10, 
        mergeCost = 50;
    private AudioSource 
        audioSource;
    public GameObject 
        car1, 
        car2, 
        car3, 
        track1, 
        track2, 
        track3, 
        particles1, 
        particles2, 
        particles3, 
        holdText, 
        playCanvas,
        debugCanvas,
        musicGO;
    public Button 
        newCarButton, 
        newTrackButton, 
        newIncomeButton, 
        mergeCarsButton,
        musicButton,
        soundButton,
        cameraButton;
    public GameObject[] 
        cars1, 
        cars2, 
        cars3;
    public static int 
        money;
    public AudioClip 
        buySound,
        moneySound,
        uiSound, 
        mergeSound;
    public TMP_Text 
        moneyText, 
        carPriceText, 
        trackPriceText, 
        incomePriceText, 
        mergePriceText,
        musicText,
        soundText,
        cameraText;
    public static bool 
        dalHold,
        dalMenu,
        musicOn = true,
        sfxOn = true,
        dalCamera2;
    public Sprite
        zelenoDugme,
        crvenoDugme;
    private CinemachineVirtualCamera
        cm1,
        cm2;
    private float
        loadTimer = 0;

    void Start()
    {
        newCarPosition = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        LoadCars(cars1, car1, "howMany1");
        LoadCars(cars2, car2, "howMany2");
        LoadCars(cars3, car3, "howMany3");
        money = PlayerPrefs.GetInt("Money");
        trackNum = PlayerPrefs.GetInt("TrackNum");
        carCost = PlayerPrefs.GetInt("CarCost");
        trackCost = PlayerPrefs.GetInt("TrackCost") == 0 ? 500 : PlayerPrefs.GetInt("TrackCost");
        incomeCost = PlayerPrefs.GetInt("IncomeCost") == 0 ? 10 : PlayerPrefs.GetInt("IncomeCost");
        dalHold = PlayerPrefs.GetInt("DalHold") == 1 ? true : false;
        //musicOn = PlayerPrefs.GetInt("MusicOn") == 1 ? true : false;
        //sfxOn = PlayerPrefs.GetInt("SfxOn") == 1 ? true : false;
        dalCamera2 = PlayerPrefs.GetInt("DalCamera2") == 1 ? true : false;

        audioSource = GameObject.FindGameObjectWithTag("AS").GetComponent<AudioSource>();
    }

    void Update()
    {
        cm1 = GameObject.FindGameObjectWithTag("CM1").GetComponent<CinemachineVirtualCamera>();
        cm2 = GameObject.FindGameObjectWithTag("CM2").GetComponent<CinemachineVirtualCamera>();

        newCarPosition = GameObject.FindGameObjectWithTag("SpawnPoint").transform;

        loadTimer -= Time.deltaTime;

        if (loadTimer < -1)
        {
            PlayerPrefs.SetInt("howMany1", cars1.Length);
            PlayerPrefs.SetInt("howMany2", cars2.Length);
            PlayerPrefs.SetInt("howMany3", cars3.Length);
        }
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.SetInt("TrackNum", trackNum);
        PlayerPrefs.SetInt("CarCost", carCost);
        PlayerPrefs.SetInt("TrackCost", trackCost);
        PlayerPrefs.SetInt("IncomeCost", incomeCost);
        PlayerPrefs.SetInt("DalHold", dalHold ? 1 : 0);
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.SetInt("SfxOn", sfxOn ? 1 : 0);
        PlayerPrefs.SetInt("DalCamera2", dalCamera2 ? 1 : 0);

        moneyText.text = money.ToString();
        carPriceText.text = carCost.ToString();
        trackPriceText.text = trackNum < 2 ? trackCost.ToString() : "MAX";
        incomePriceText.text = incomeCost.ToString();
        mergePriceText.text = mergeCost.ToString();

        cars1 = GameObject.FindGameObjectsWithTag("Car1");
        cars2 = GameObject.FindGameObjectsWithTag("Car2");
        cars3 = GameObject.FindGameObjectsWithTag("Car3");

        if(cars1.Length > 0 && !dalHold)
        {
            holdText.SetActive(true);
        }
        else
        {
            holdText.SetActive(false);
        }

        if(musicOn)
        {
            musicGO.GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            musicGO.GetComponent<AudioSource>().volume = 0;
        }

        AreButtonsInteractable();
        CanvasHandler();
        CamerasHandler();

        if (cars1.Length >= 3)
        {
            MergeProduct(cars1, car2, particles2);
        }
        else if(cars2.Length >= 3)
        {
            MergeProduct(cars2, car3, particles3);
        }
    }

    void CamerasHandler()
    {
        if(!dalCamera2)
        {
            cameraText.text = "1";
            cm1.Priority = 10;
            cm2.Priority = 1;
        }
        else
        {
            cameraText.text = "2";
            cm1.Priority = 1;
            cm2.Priority = 10;
        }
    }

    public void CameraSwitch()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        dalCamera2 = !dalCamera2;
    }

    void CanvasHandler()
    {
        if(dalMenu)
        {
            playCanvas.SetActive(false);
            debugCanvas.SetActive(true);
        }
        else
        {
            debugCanvas.SetActive(false);
            playCanvas.SetActive(true);
        }

        if(musicOn)
        {
            musicButton.image.sprite = zelenoDugme;
            musicText.text = "On";
        }
        else
        {
            musicButton.image.sprite = crvenoDugme;
            musicText.text = "Off";
        }

        if (sfxOn)
        {
            soundButton.image.sprite = zelenoDugme;
            soundText.text = "On";
        }
        else
        {
            soundButton.image.sprite = crvenoDugme;
            soundText.text = "Off";
        }
    }

    void AreButtonsInteractable()
    {
        if ((cars1.Length >= 3 || cars2.Length >= 3) && money >= mergeCost)
        {
            mergeCarsButton.interactable = true;
        }
        else
        {
            mergeCarsButton.interactable = false;
        }

        if (money >= carCost)
        {
            newCarButton.interactable = true;
        }
        else
        {
            newCarButton.interactable = false;
        }

        if (money >= trackCost && trackNum < 2)
        {
            newTrackButton.interactable = true;
        }
        else
        {
            newTrackButton.interactable = false;
        }

        if (money >= incomeCost)
        {
            newIncomeButton.interactable = true;
        }
        else
        {
            newIncomeButton.interactable = false;
        }
    }

    public void MusicOnOff()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        musicOn = !musicOn;
    }

    public void SFXOnOff()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        sfxOn = !sfxOn;
    }

    public void Plus10K()
    {
        if (sfxOn) audioSource.PlayOneShot(moneySound);
        if (sfxOn) audioSource.PlayOneShot(buySound);

        money += 10000;
    }

    public void OpenMenu()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        dalMenu = true;
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        dalMenu = false;
        Time.timeScale = 1;
    }

    public void MergeCars()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        if (cars1.Length >= 3)
        {
            cars1[0].GetComponent<Car>().Merge();
            cars1[1].GetComponent<Car>().Merge();
            cars1[2].GetComponent<Car>().Merge();
        }
        else if(cars2.Length >= 3)
        {
            cars2[0].GetComponent<Car>().Merge();
            cars2[1].GetComponent<Car>().Merge();
            cars2[2].GetComponent<Car>().Merge();
        }

        money -= mergeCost;
    }

    void MergeProduct(GameObject[] c, GameObject g, GameObject p)
    {
        float car1Distance = Vector3.Distance(c[0].transform.position, newCarPosition.position);
        float car2Distance = Vector3.Distance(c[1].transform.position, newCarPosition.position);
        float car3Distance = Vector3.Distance(c[2].transform.position, newCarPosition.position);

        if (car1Distance < 0.1f && car2Distance < 0.1f)
        {
            if (loadTimer < -3)
            {
                if (car3Distance < 0.1f)
                {
                    if (sfxOn) audioSource.PlayOneShot(mergeSound);

                    Instantiate(g, newCarPosition.position, Quaternion.identity);
                    Instantiate(p, newCarPosition.position, Quaternion.identity);
                    Destroy(c[0]);
                    Destroy(c[1]);
                    Destroy(c[2]);
                }
            }
            else
            {
                PositionCars(c);
            }
        }
    }

    public void NewCar()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);
        Instantiate(car1, newCarPosition.position, Quaternion.identity);
        Instantiate(particles1, newCarPosition.position, Quaternion.identity);
        money -= carCost;
        carCost = carCost == 0 ? carCost = 5 : (int)(carCost * 1.7);
    }

    public void NewTrack()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        trackNum++;

        switch (trackNum)
        {
            case 0:
                track1.SetActive(true);
                track2.SetActive(false);
                track3.SetActive(false);
                break;
            case 1:
                track1.SetActive(false);
                track2.SetActive(true);
                track3.SetActive(false);
                break;
            case 2:
                track1.SetActive(false);
                track2.SetActive(false);
                track3.SetActive(true);
                break;
            default:
                track1.SetActive(true);
                track2.SetActive(false);
                track3.SetActive(false);
                break;
        }

        foreach (GameObject c in cars1)
        {
            Car car = c.GetComponent<Car>();
            car.ChoosePath();
        }

        foreach (GameObject c in cars2)
        {
            Car car = c.GetComponent<Car>();
            car.ChoosePath();
        }

        foreach (GameObject c in cars3)
        {
            Car car = c.GetComponent<Car>();
            car.ChoosePath();
        }

        money -= trackCost;
        trackCost *= 5;
    }

    public void IncreaseIncome()
    {
        if (sfxOn) audioSource.PlayOneShot(buySound);

        MoneyPoint.moneyAmount += 1;

        money -= incomeCost;
        incomeCost = (int)(incomeCost * 1.7);
    }

    void LoadCars(GameObject[] g, GameObject c, string k)
    {
        int howMany = PlayerPrefs.GetInt(k, 0);
        
        for (int i = 0; i < howMany; i++)
        {
            Instantiate(c, newCarPosition.position, Quaternion.identity);
        }
    }

    void PositionCars(GameObject[] g)
    {
        for (int i = 0; i < g.Length; i++)
        {
            Car car = g[i].GetComponent<Car>();
            car.RandomDistance();
        }
    }
}
