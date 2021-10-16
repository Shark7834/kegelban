using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text,Image,Button

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Ball;
    private GameObject ForceIndecatorGO;
    private Rigidbody ballRigitBody;
    private Vector3 ballStartPosition;
    private bool isBallMoving;

    private GameObject Arrow;
    private GameObject ArrowTail;
    private float arrowAngle;

    private Image ForceIndecator;
    private Text GameStat;

    private const float MIN_FORCE = 1000f;
    private const float MAX_FORCE = 2000f;
    private int attempt;

    private GameObject GameMenu;



    void Start()
    {
        GameMenu = GameObject.Find("Menu");
        Menu.menuMode = Menu.MenuMode.Start;

        ForceIndecator = GameObject.Find("ForceIndicator").GetComponent<Image>();
        GameStat = GameObject.Find("GameStat").GetComponent<Text>();

        Arrow = GameObject.Find("Arrow");
        ArrowTail = GameObject.Find("ArrowTail");
        Ball = GameObject.Find("Ball");
        //push ball - приложить силу к го твердому телу
        //Rigidbody rb = Ball.GetComponent<Rigidbody>().
        ballStartPosition = Ball.transform.position;
        arrowAngle = 0f;


        ballRigitBody = Ball.GetComponent<Rigidbody>();
        isBallMoving = false;
    }

    // Update is called once per frame
  
    void Update()
    {
        #region 
        if (ballRigitBody.velocity.magnitude < 0.1f && isBallMoving)//
        {
            Debug.Log("Ball stopped");
            Arrow.SetActive(true);
            isBallMoving = false;
            Ball.transform.position = ballStartPosition;
            ballRigitBody.velocity = Vector3.zero;
            ballRigitBody.angularVelocity = Vector3.zero;
            int kegelsUp =-10;
            int kegelsDown = 0;
            foreach (GameObject kegel in GameObject.FindGameObjectsWithTag("Kegel"))
            {
                if(kegel.transform.position.y > 0.3)
                {
                    kegel.SetActive(false);
                    kegelsDown++;
                }
                else
                {
                    kegelsUp++;
                    kegel.transform.rotation = Quaternion.Euler(0, 0, 0);
                    kegel.transform.position.Set(kegel.transform.position.x, 0, kegel.transform.position.z);
                }
            }
            GameStat.text = "\n”пало - " + kegelsDown + "\nќсталось -  " + kegelsUp;            

        }
        #endregion 



        if (Input.GetKeyDown(KeyCode.Space) && !isBallMoving)
        {
            Debug.Log(Arrow.transform.forward);

            Vector3 forceDirection = Arrow.transform.forward;
            Arrow.SetActive(false);


            float forceFactor = MIN_FORCE+ (MAX_FORCE-MIN_FORCE)*ForceIndecator.fillAmount;
            ballRigitBody.AddForce(forceFactor *  (-forceDirection));
            ballRigitBody.velocity = forceDirection * 0.1f;
            isBallMoving = true;
        }
        #region
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            if(arrowAngle > -20)
            {
                Arrow.transform.RotateAround(
               ArrowTail.transform.position,
               Vector3.up,
               -1
               );
                arrowAngle--;
            }
           
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(arrowAngle < 20)
            {
                Arrow.transform.RotateAround(
               ArrowTail.transform.position,
               Vector3.up,
               1
               );
                arrowAngle++;
            }
           
        }
        #endregion 

        
        if(!isBallMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // ForceIndecator.fillAmount += 0.005f;
                ForceIndecator.fillAmount += Time.deltaTime ;
            }
            if (Input.GetKey(KeyCode.DownArrow) )
            {
                ForceIndecator.fillAmount -= Time.deltaTime ;

            }
        }
       

    }
    public void PlayClick()
    {
        // Debug.Log("Click");
        GameMenu.SetActive(false);

    }
}
