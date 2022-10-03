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
    public GameObject earth;

    // animation stuff
    private Vector2 _rocketPosition;
    private Vector2 _initialRocketPosition;
    private Quaternion _initialRocketRotation;
    private bool _readyForLaunch;
    private bool _inOrbit;
    private float _rocketSpeed;

    // path stuff
    private float _t;
    public Transform[] paths;
    public int currentPath;

    private void Start()
    {
        _readyForLaunch = true;
        _inOrbit = false;
        speedSlider.value = 1;
        _initialRocketPosition = gameObject.transform.position;
        _initialRocketRotation = gameObject.transform.rotation;
        launchButton.SetActive(true);
        okButton.SetActive(false);
        outcomePanel.SetActive(false);
    }

    private void Update()
    {
        // rotation around earth for "stuck in orbit"-scenario
        if (_inOrbit)
        {
            transform.RotateAround(earth.transform.position, Vector3.forward, Time.deltaTime * _rocketSpeed * 70f);
        }
    }

    public void ResetRocket()
    {
        if (_readyForLaunch)
        {
            _inOrbit = false;
            okButton.SetActive(false);
            launchButton.SetActive(true);
            outcomePanel.SetActive(false);
            transform.position = _initialRocketPosition;
            transform.rotation = _initialRocketRotation;
            gameObject.GetComponent<Image>().sprite = rocket;
        }
    }

    public void Launch()
    {
        outcomePanel.SetActive(false);
        switch (speedSlider.value)
        {
            case 0:
                NoLiftoff();
                break;
            case 1:
                CrashRocket();
                break;
            case 2:
                LiftOff();
                break;
            case 3:
                ToMoon();
                break;
            case 4:
                TooMuchSpeed();
                break;
        }
    }

    public void NoLiftoff()
    {
        SetOutcome(0);
    }

    public void CrashRocket()
    {
        launchButton.SetActive(false);
        currentPath = 0;
        _rocketSpeed = 0.7f;
        if (_readyForLaunch)
        {
            StartCoroutine(FollowPath(currentPath));
        }
    }
    
    public void LiftOff()
    {
        launchButton.SetActive(false);
        currentPath = 1;
        _rocketSpeed = 0.7f;
        if (_readyForLaunch)
        {
            StartCoroutine(FollowPath(currentPath));
        }
    }

    public void ToMoon()
    {
        launchButton.SetActive(false);
        currentPath = 2;
        _rocketSpeed = 0.5f;
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
        SetOutcome(4);
    }

    public IEnumerator FollowPath(int path)
    {
        _readyForLaunch = false;
        _t = 0f;
        
        Vector2 p0 = paths[path].GetChild(0).position;
        Vector2 p1 = paths[path].GetChild(1).position;
        Vector2 p2 = paths[path].GetChild(2).position;
        Vector2 p3 = paths[path].GetChild(3).position;

        while (_t < 1)
        {
            // t = location parameter on bezier curve (range 0 to 1)
            // t = deltaTime * speed = sec/frame * meter/s = meter/frame
            // bezier curve is created in the editor via points and paths
            // rocket position is calculated every frame, via t which determines that position on the curve
            _t += Time.deltaTime * _rocketSpeed;
            _rocketPosition = Mathf.Pow(1 - _t, 3) * p0 
                              + 3 * Mathf.Pow(1 - _t, 2) * _t * p1 
                              + 3 * (1 - _t) * Mathf.Pow(_t, 2) * p2 
                              + Mathf.Pow(_t, 3) * p3;
            transform.position = _rocketPosition;
            
            // rotations of different paths
            // again * speed * deltaTime to make rotation independent of fps 
            switch (path)
            {
                case 0 :
                    transform.Rotate(Vector3.forward, 160f * _rocketSpeed * Time.deltaTime, Space.Self);
                    break;
                case 1 :
                    if (_t < 0.65)
                    {
                        transform.Rotate(Vector3.forward, 200f * _rocketSpeed * Time.deltaTime, Space.Self);
                    }
                    break;
                case 2 :
                    transform.Rotate(Vector3.forward, 200f * _rocketSpeed * Time.deltaTime, Space.Self);
                    break;
            }
            yield return new WaitForEndOfFrame();
        }

        _t = 0f;
        _readyForLaunch = true;

        switch (path)
        {
            case 0 :
                okButton.SetActive(true);
                gameObject.GetComponent<Image>().sprite = explosion;
                SetOutcome(2);
                break;
            case 1 : 
                _inOrbit = true;
                okButton.SetActive(true);
                SetOutcome(3);
                break;
            case 2 : 
                SetOutcome(4);
                break;
        }
    }

    public void SetOutcome(int scenario)
    {
        outcomePanel.SetActive(true);
        switch (speedSlider.value)
        {
            case 0:
                outcomeText.text = "You need some speed before you can launch.";
                break;
            case 1:
                outcomeText.text = "Your rocket was way too slow and it crashed.";
                break;
            case 2:
                outcomeText.text = "Your rocket was too slow. It didn't crash, but it's stuck in orbit around earth.";
                break;
            case 3:
                outcomeText.text = "You were fast enough to escape earth's gravity and your rocket it now travelling to the Moon.";
                break;
            case 4:
                outcomeText.text = "You put too much pressure on the engines and the rocket exploded.";
                break;
        }
    }
}
