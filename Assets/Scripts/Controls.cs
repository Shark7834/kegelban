using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;   // Text, Image, Button
using System.IO;   

public class Controls : MonoBehaviour
{
    private const float MIN_FORCE = 1000f;
    private const float MAX_FORCE = 2000f;
    private const string BEST_RES_FILE = "best.xml";

    private GameObject Ball;
    private Rigidbody ballRigidbody;
    private Vector3 ballStartPosition;
    private bool isBallMoving;

    private GameObject Arrow;
    private GameObject ArrowTail;
    private float arrowAngle;  // ���� �������� �������
    private float maxArrowAngle = 20f;  // ��������� ���� ��������

    private Image ForceIndicator;   // ������ �� Image ForceIndicator

    private Text GameStat;
    private Text ScoresText;
    private int attempt;

    private GameObject GameMenu;
    private List<GameResult> bestResults; //������� ��������



    void Start()
    {
        
        GameMenu = GameObject.Find("Menu");
        Menu.MenuMode = MenuMode.Start;

        attempt = 0;
        GameStat = GameObject.Find("GameStat").GetComponent<Text>();
        ScoresText = GameObject.Find("ScoreText").GetComponent<Text>();

        // �������� ������ �� ��������� Image ������� ForceIndicator
        ForceIndicator = GameObject.Find("ForceIndicator").GetComponent<Image>();

        Arrow = GameObject.Find("Arrow");
        ArrowTail = GameObject.Find("ArrowTail");
        arrowAngle = 0f;

        // ������� �����
        Ball = GameObject.Find("Ball");  // �� ����� � ��������
        // ��������� ��� ��������� �������
        ballStartPosition = Ball.transform.position;
        // ��������� ������ �� ��� ��.�
        ballRigidbody = Ball.GetComponent<Rigidbody>();
        isBallMoving = false;
        LoadBestResults();
    }

    // Update is called once per frame
    void Update()
    {
        #region �������������� � ����
        // ���� ���� ������� - �� ��������� ������� ��������
        if (Menu.IsActive) return;

        // ���� ������ Escape - ������������ ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameMenu.SetActive(true);
            Menu.IsActive = true;
            Menu.MenuMode = MenuMode.Pause;
        }
        #endregion

        #region ��������� ������
        if (ballRigidbody.velocity.magnitude < 0.1f && isBallMoving)
        {
            isBallMoving = false;
            Ball.transform.position = ballStartPosition;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
            // �������� ���������� � ������
            int kegelsUp = 0;
            int kegelsDown = 0;
            foreach (GameObject kegel in
                GameObject.FindGameObjectsWithTag("Kegel"))
            {
                // ? ��� ���������� ������� � ������� �����
                // ����� - �==0, ����� - y==0.6 ��� ��������, 
                // ����� �������, ��� � > 0.1 - �����
                if (kegel.transform.position.y > 0.1)
                {
                    kegel.SetActive(false);
                    kegelsDown++;
                }
                else
                {
                    kegelsUp++;
                    // ���� ����� ��������� (�� ����� ��� �� ������) - ��������� ��
                    kegel.transform.rotation = Quaternion.Euler(0, 0, 0);
                    kegel.transform.position.Set(kegel.transform.position.x, 0, kegel.transform.position.z);
                }
                // Debug.Log(kegel.transform.position);
            }
            // ���������� �������
            Arrow.SetActive(true);
            // ������� ����������
            attempt++;
            GameStat.text += "\n" + attempt + "  " + Clock.StringValue + "  " +
                +kegelsDown + "  " + kegelsUp;
        }
        #endregion

        #region ������ ������
        if (Input.GetKeyDown(KeyCode.Space) && !isBallMoving)
        {
            // �������� ����� - ��������� ���� � ��� �������� ����
            // ����������� ���� - �� �������
            Vector3 forceDirection = Arrow.transform.forward;
            // �������� ���� - �� ���������� (MIN - MAX)
            float forceFactor = MIN_FORCE + (MAX_FORCE - MIN_FORCE) * ForceIndicator.fillAmount;
            ballRigidbody.AddForce(forceFactor * (-forceDirection));
            ballRigidbody.velocity = (-forceDirection) * 0.1f;
            isBallMoving = true;
            Arrow.SetActive(false);
        }
        // �������: ������ �������� ForceIndicator ��� ������� ����
        // �������: ������� ��������� �� ����� �������� ������ 
        #endregion

        #region �������� �������
        if (Input.GetKey(KeyCode.LeftArrow)
         && arrowAngle > -maxArrowAngle)
        {
            Arrow.transform.RotateAround(      // ��������:
                ArrowTail.transform.position,  // ����� ��������
                Vector3.up,                    // ��� ��������
                -1f                            // ���� ��������
            );
            arrowAngle -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow)
         && arrowAngle < maxArrowAngle)
        {
            Arrow.transform.RotateAround(
                ArrowTail.transform.position,
                Vector3.up,
                1f
            );
            arrowAngle += 1f;
        }
        #endregion

        #region ��������� ����
        if (!isBallMoving)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                // ForceIndicator.fillAmount += 0.01f;  // ! time dependent !
                float val = ForceIndicator.fillAmount + Time.deltaTime / 2;
                if (val <= 1)
                    ForceIndicator.fillAmount = val;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                float val = ForceIndicator.fillAmount - Time.deltaTime / 2;
                if (val >= .1f)
                    ForceIndicator.fillAmount = val;
            }
        }
        // �������: ���������� �������� 0.1 <= fillAmount <= 1
        #endregion

        if (Input.GetKeyDown(KeyCode.B) && !isBallMoving)
        {
            GameMenu.SetActive(true);
            Menu.IsActive = true;
            Menu.MenuMode = MenuMode.Pause;

        }

    }

    /**
     * ���������� ������ Play ����
     * 
     */
    public void PlayClick()
    {
        // Debug.Log("Click");
        GameMenu.SetActive(false);
        Menu.IsActive = false;
    }

    /**
     * C������� ������� ������ �����������.
     * ���� �� ��� - ������� ��������
     * 
     */
    private void LoadBestResults()
    {
        if (File.Exists(BEST_RES_FILE))
        {
            using(StreamReader reader = new StreamReader(BEST_RES_FILE))
            {
                XmlSerializer serializer = new XmlSerializer(
                    typeof(List<GameResult>));
                bestResults = (List<GameResult>)
                    serializer.Deserialize(reader);
            }
            bestResults.Sort();

            foreach (var res in bestResults)
            {
                Debug.Log(res);
                ScoresText.text += res.ToString();
            }
        }
        else
        {
            //����� ���, ������� ��������
            bestResults = new List<GameResult>();

            bestResults.Add(new GameResult { Balls = 20, Time = 200 });
            bestResults.Add(new GameResult { Balls = 10, Time = 100 });
            bestResults.Add(new GameResult { Balls = 30, Time = 300 });

            


            using (StreamWriter writer = new StreamWriter(BEST_RES_FILE))
            {
                XmlSerializer serializer = new XmlSerializer
                (
                    bestResults.GetType()
                );
                serializer.Serialize(writer, bestResults);
            }
        }
    }
}


public class GameResult: System.IComparable<GameResult>
{
    
    public int Balls { get; set; } //���-�� �������
    public float Time { get; set; }//����� ������

    public int CompareTo(GameResult y)
    {
        if (this.Balls < y.Balls) return -1;
        else if (this.Balls == y.Balls)
        {
            if (this.Time < y.Time) return -1;
            else if (this.Time == y.Time) return 0;
        }
        return 1;
    }

    public override string ToString()
    {
        return "Balls: " + Balls + ", Time: " + Time + "\n";
    }

}
