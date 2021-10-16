using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    // Start is called before the first frame update

    public static float Value { get; private set; }
   
    public static string StringValue { get
        {
            int total = (int)Value;
            int sec = total % 60;
            int min = total / 60;
            return (min < 10 ? "0" : "") + min + ":" + (min < 10 ? "0" : "") + sec;
        }
    }
    private Text time;

    void Start()
    {
        time = GetComponent<Text>();

        Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Value += Time.deltaTime;
      
        time.text = StringValue;

    }
}
