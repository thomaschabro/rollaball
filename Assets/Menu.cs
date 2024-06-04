using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public void PlayGame()
    {
        // Load scene by number
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
