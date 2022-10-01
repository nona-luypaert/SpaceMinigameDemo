using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RocketController : MonoBehaviour
{
    public Slider speedSlider;
    public TextMeshProUGUI outcomeText;
    public GameObject outcomePanel;
        
    // Start is called before the first frame update
    void Start()
    {
        speedSlider.value = 1;
        outcomePanel.SetActive(false);
    }

    // Update is called once per frame
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
        }
    }

    public void NoLiftoff()
    {
        outcomePanel.SetActive(true);
        outcomeText.text = "You need to choose speed before you can launch!";
    }
}
