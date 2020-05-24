using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField]
    private GameObject[]    _screens;
    private int             _screensIndex;

    // Start is called before the first frame update
    void Start()
    {
        _screensIndex = 0;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueScreen()
    {
        if (_screens.Length <= _screensIndex)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        else
        {
            foreach (GameObject go in _screens)
                go.SetActive(false);
            _screens[_screensIndex].SetActive(true);
            _screensIndex++;
        }
    }
}
