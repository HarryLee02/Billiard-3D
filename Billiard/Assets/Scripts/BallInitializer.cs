using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallInitializer : MonoBehaviour
{
    [SerializeField] public GameObject cueBall;
    [SerializeField] public GameObject eightBall;
    [SerializeField] public GameObject ball1;
    [SerializeField] public GameObject ball2;
    [SerializeField] public GameObject ball3;
    [SerializeField] public GameObject ball4;
    [SerializeField] public GameObject ball5;
    [SerializeField] public GameObject ball6;
    [SerializeField] public GameObject ball7;
    [SerializeField] public GameObject ball9;
    [SerializeField] public GameObject ball10;
    [SerializeField] public GameObject ball11;
    [SerializeField] public GameObject ball12;
    [SerializeField] public GameObject ball13;
    [SerializeField] public GameObject ball14;
    [SerializeField] public GameObject ball15;

    [SerializeField] public Transform cueBallPos;
    [SerializeField] public Transform eightBallPos;
    [SerializeField] public Transform ball1Pos;
    [SerializeField] public Transform ball2Pos;
    [SerializeField] public Transform ball3Pos;
    [SerializeField] public Transform ball4Pos;
    [SerializeField] public Transform ball5Pos;
    [SerializeField] public Transform ball6Pos;
    [SerializeField] public Transform ball7Pos;
    [SerializeField] public Transform ball9Pos;
    [SerializeField] public Transform ball10Pos;
    [SerializeField] public Transform ball11Pos;
    [SerializeField] public Transform ball12Pos;
    [SerializeField] public Transform ball13Pos;
    [SerializeField] public Transform ball14Pos;
    [SerializeField] public Transform ball15Pos;
    private void Start()
    {
            BallSetup();
    }

    public void BallSetup()
    {
        Instantiate(cueBall, cueBallPos.position, Quaternion.identity);
        Instantiate(eightBall, eightBallPos.position, Quaternion.identity);
        GameObject[] Balls = { ball1, ball2, ball3, ball4, ball5, ball6, ball7, ball9, ball10, ball11, ball12, ball13, ball14, ball15 };
        Transform[] Positions = { ball1Pos, ball2Pos, ball3Pos, ball4Pos, ball5Pos, ball6Pos, ball7Pos, ball9Pos, ball10Pos, ball11Pos, ball12Pos, ball13Pos, ball14Pos, ball15Pos };
        //Shuffle(Balls);
        for (int i = 0; i < Balls.Length; i++)
        {
            Instantiate(Balls[i], Positions[i].position, Positions[i].rotation);
        }
    }
    void Shuffle(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
