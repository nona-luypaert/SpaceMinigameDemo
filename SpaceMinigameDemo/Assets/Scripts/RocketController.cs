using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    public Slider speedSlider;
    public TextMeshProUGUI outcomeText;
    public GameObject outcomePanel;

    public Sprite explosion;

    void Start()
    {
        speedSlider.value = 1;
        outcomePanel.SetActive(false);
    }

    void Update()
    {
        
    }

    public void Launch()
    {
        switch (speedSlider.value)
        {
            case 0 :
                NoLiftoff();
                break;
            case 4 :
                TooMuchSpeed();
                break;
        }
    }

    public void NoLiftoff()
    {
        outcomePanel.SetActive(true);
        outcomeText.text = "You need some speed before you can launch.";
    }

    public void TooMuchSpeed()
    {
        gameObject.GetComponent<Image>().sprite = explosion;
        outcomePanel.SetActive(true);
        outcomeText.text = "You put too much pressure on the engines and the rocket exploded.";
    }
}
