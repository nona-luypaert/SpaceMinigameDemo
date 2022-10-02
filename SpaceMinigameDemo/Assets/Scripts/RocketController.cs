using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RocketController : MonoBehaviour
{
    // ui stuff
    public Slider speedSlider;
    public Sprite explosion;
    public Sprite rocket;
    public GameObject okButton;
    public GameObject launchButton;
    public GameObject outcomePanel;
    public TextMeshProUGUI outcomeText;

    // animation stuff
    private Vector2 _rocketPosition;
    private Vector2 _initialRocketPosition;
    private Quaternion _initialRocketRotation;
    private bool _readyForLaunch;
    private float _rocketSpeed;

    // path stuff
    private float _t;
    public Transform[] paths;
    public int currentPath;

    void Start()
    {
        _readyForLaunch = true;
        speedSlider.value = 1;
        _initialRocketPosition = gameObject.transform.position;
        _initialRocketRotation = gameObject.transform.rotation;
        outcomePanel.SetActive(false);
        launchButton.SetActive(true);
        okButton.SetActive(false);
    }
    
    public void ResetRocket()
    {
        if (_readyForLaunch)
        {
            outcomePanel.SetActive(false);
            okButton.SetActive(false);
            launchButton.SetActive(true);
            transform.position = _initialRocketPosition;
            transform.rotation = _initialRocketRotation;
            gameObject.GetComponent<Image>().sprite = rocket;
        }
    }

    public void Launch()
    {
        switch (speedSlider.value)
        {
            case 0:
                NoLiftoff();
                break;
            case 1:
                CrashRocket();
                break;
            case 4:
                TooMuchSpeed();
                break;
        }
    }

    public void NoLiftoff()
    {
        outcomePanel.SetActive(true);
        outcomeText.text = "You need some speed before you can launch.";
    }

    public void CrashRocket()
    {
        launchButton.SetActive(false);
        currentPath = 0;
        _t = 0f;
        _rocketSpeed = 0.7f;
        if (_readyForLaunch)
        {
            StartCoroutine(FollowPath(currentPath));
        }
    }

    public void TooMuchSpeed()
    {
        launchButton.SetActive(false);
        okButton.SetActive(true);
        gameObject.GetComponent<Image>().sprite = explosion;
        outcomePanel.SetActive(true);
        outcomeText.text = "You put too much pressure on the engines and the rocket exploded.";
    }

    public IEnumerator FollowPath(int path)
    {
        _readyForLaunch = false;

        Vector2 p0 = paths[path].GetChild(0).position;
        Vector2 p1 = paths[path].GetChild(1).position;
        Vector2 p2 = paths[path].GetChild(2).position;
        Vector2 p3 = paths[path].GetChild(3).position;

        while (_t < 1)
        {
            // t = parameter in bezier curve
            // t = deltaTime * speed = sec/frame * meter/s = meter/frame
            // bezier curve is created in the editor via points and paths
            // rocket position is calculated every frame, via t which determines that position on the curve
            _t += Time.deltaTime * _rocketSpeed;
            _rocketPosition = Mathf.Pow(1 - _t, 3) * p0 + 3 * Mathf.Pow(1 - _t, 2) * _t * p1 + 3 * (1 - _t) * Mathf.Pow(_t, 2) * p2 + Mathf.Pow(_t, 3) * p3;
            transform.position = _rocketPosition;
            
            if (path == 0)
            {
                // again * speed * deltaTime to make rotation independent of fps 
                transform.Rotate(Vector3.forward, 160f * _rocketSpeed * Time.deltaTime, Space.Self);
            }
            
            yield return new WaitForEndOfFrame();
        }

        _t = 0f;
        _readyForLaunch = true;

        if (path == 0)
        {
            okButton.SetActive(true);
            gameObject.GetComponent<Image>().sprite = explosion;
            outcomePanel.SetActive(true);
            outcomeText.text = "Your rocket was way too slow and it crashed.";
        }
    }
}
