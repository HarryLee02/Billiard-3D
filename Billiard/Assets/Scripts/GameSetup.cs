using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{   
    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    float ballRadius;
    float ballDiameter;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform cueBallPosition;
    [SerializeField] Transform headBallPosition;

    // Awake is called before the Start method is called
    private void Awake()
    {
        ballRadius = ballPrefab.GetComponent<SphereCollider>().radius * 100f;
        ballDiameter = ballRadius * 2f;
        PlaceAllBalls();
    }

    void PlaceAllBalls()
    {
        PlaceCueBall();
        PlaceRandomBalls();
    }

    void PlaceCueBall()
    {
        GameObject ball = Instantiate(ballPrefab, cueBallPosition.position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeCueBall();
    }

    void PlaceEightBall(Vector3 position)
    {
        GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
        ball.GetComponent<Ball>().MakeEightBall();
    }

    void PlaceRandomBalls()
    {
        int NumInThisRow = 1;
        int rand;
        Vector3 FirstInRowPosition = headBallPosition.position;
        Vector3 CurrentPosition = FirstInRowPosition;

        void PlaceRedBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(true);
            redBallsRemaining--;
        }

        void PlaceBlueBall(Vector3 position)
        {
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity);
            ball.GetComponent<Ball>().BallSetup(false);
            blueBallsRemaining--;
        }

        // Outer loop are the 5 rows
        for (int i = 0; i < 5; i++)
        {   
            //Inner loop is the number of balls in each row
            for (int j = 0; j < NumInThisRow; j++)
            {   
                // Check to see thats a middle spot for the 8 ball
                if (i == 2 && j == 1)
                {
                    PlaceEightBall(CurrentPosition);
                }
                // If there are still red and blue balls remaining, randomly place one
                else if (redBallsRemaining > 0 && blueBallsRemaining > 0)
                {
                    rand = Random.Range(0, 2);
                    if (rand == 0)
                    {
                        PlaceRedBall(CurrentPosition);
                    }
                    else
                    {
                        PlaceBlueBall(CurrentPosition);
                    }
                }
                // If there are only red balls remaining, place a red ball
                else if (redBallsRemaining > 0)
                {
                    PlaceRedBall(CurrentPosition);
                }
                // If there are only blue balls remaining, place a blue ball
                else if (blueBallsRemaining > 0)
                {
                    PlaceBlueBall(CurrentPosition);
                }  
                // Move to the next position to the right
                CurrentPosition += new Vector3(1, 0, 0).normalized * ballDiameter;
            }
            // Once the row is complete, move to the next row
            FirstInRowPosition += Vector3.back * (Mathf.Sqrt(3) * ballRadius) + Vector3.left * ballRadius;
            CurrentPosition = FirstInRowPosition;
            NumInThisRow++;
        }
    }
}
