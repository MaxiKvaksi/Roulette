using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject roulette;
    public GameObject arrow;
    public GameObject resultImage;
    public GameObject statsPanel;
    public GameObject messagePanel;
    public GameObject skipButton;
    public GameObject[] elementsUI;
    private RouletteScript r;
    [SerializeField]
    private int tickets = 5;
    static GameManager instance;
    private bool spinning = false;
    private int element = -1;


    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance != this)
        {
            instance = this;
        }
        else
        {
            return;
        }
        GetProgress();
        r = roulette.GetComponent<RouletteScript>();
        statsPanel.GetComponentInChildren<Text>().text = "x" + tickets;
        InitElementsUI();
    }

    private void InitElementsUI()
    {
        foreach (GameObject item in elementsUI)
        {
            item.GetComponentInChildren<Text>().text = "x" + item.GetComponent<ElementUI>().count;
        }
    }

    public void Spin()
    {
        if (!spinning)
        {
            if (tickets < 1)
            {
                messagePanel.GetComponent<MessageScript>().Open("You don't have tickets!");
                return;
            }
            spinning = true;
            UpdateTickets(-1);
            element = gameObject.GetComponent<VariantsScript>().GetMagicValue();
            r.SpinningStart(element);
            skipButton.SetActive(true);
        }
    }
    public void Skip()
    {
        if (spinning)
        {
            r.SkipIt();
        }
    }
    public void Result(int res)
    {
        spinning = false;
        resultImage.GetComponent<ResultScript>().
            Anim(gameObject.GetComponent<VariantsScript>().
            variantsList[element].GetComponent<Element>().isGoodGain);
        switch (element)
        {
            case 0: UpdateTickets(1); break;
            case 2: ClearStats(); break;
            default: UpdateStats(); break;
        }
        skipButton.SetActive(false);
        
    }

    void UpdateTickets(int ticket)
    {
        tickets += ticket;
        statsPanel.GetComponentInChildren<Text>().text = "x" + tickets;
        SaveProgress();
    }

    void UpdateStats()
    {
        foreach (GameObject elementUI in elementsUI)
        {
            var script = elementUI.GetComponent<ElementUI>();
            if (GetComponent<VariantsScript>().variantsList[element].
                GetComponent<Element>().elementName == script.elementName)
            {
                script.count++;
                script.GetComponentInChildren<Text>().text = "x" + script.count;
                SaveProgress();
                return;
            }
        }
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("Tickets", tickets);
        foreach (GameObject item in elementsUI)
        {
            PlayerPrefs.SetInt(item.GetComponent<ElementUI>().elementName, 
                item.GetComponent<ElementUI>().count);
        }
        PlayerPrefs.Save();
    }
    void GetProgress()
    {
        if (PlayerPrefs.HasKey("Tickets"))
        {
            tickets = PlayerPrefs.GetInt("Tickets");
        }
        foreach (GameObject item in elementsUI)
        {
            item.GetComponent<ElementUI>().count = 
                PlayerPrefs.GetInt(item.GetComponent<ElementUI>().elementName);
        }
    }

    public void SecretButton()
    {
        UpdateTickets(5);
    }
    void ClearStats()
    {
        foreach(var item in elementsUI)
        {
            item.GetComponent<ElementUI>().count = 0;
        }
        InitElementsUI();
        SaveProgress();
    }
}
