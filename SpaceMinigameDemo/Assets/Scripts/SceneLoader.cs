using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ToSpace()
    {
        SceneManager.LoadScene(1);
    }

    public void ToMoon()
    {
        SceneManager.LoadScene(2);
    }

    public void ToEarth()
    {
        SceneManager.LoadScene(0);
    }
}
