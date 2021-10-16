using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static MenuMode MenuMode { get; set; }
    public static bool IsActive { get; set; }

    private Text Title;

    void Start()
    {
        Title = GameObject.Find("Title").GetComponent<Text>();
        IsActive = true;
    }

    void LateUpdate()
    {
        switch (MenuMode)
        {
            case MenuMode.Start:
                Title.text = "������ ����";
                break;
            case MenuMode.Pause:
                Title.text = "�����";
                break;
            case MenuMode.GameOver:
                Title.text = "����� ����";
                break;
        }
    }
}

public enum MenuMode
{
    Start,
    Pause,
    GameOver
   
}