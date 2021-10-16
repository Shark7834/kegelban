using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public static MenuMode menuMode { get; set; }
    public static bool IsActive { get; set; }
    private Text Title;
    void Start()
    {
        Title = GameObject.Find("Title").GetComponent<Text>();
      //  IsActive = SetActive(false);
    }
    private void LateUpdate()
    {
        switch (menuMode)
        {
            case MenuMode.Start:
                Title.text = "Game start";
                break;
            case MenuMode.Pause:
                Title.text = "Pause";
                break;
            case MenuMode.GameOver:
                Title.text = "Game over";
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public enum MenuMode
    {
        Start,
        Pause,
        GameOver
    }
}
