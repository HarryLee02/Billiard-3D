using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
    public string ballLayerName = "BallLayer";

    enum CurrentPlayer
    {
        Player1,
        Player2
    }

    CurrentPlayer currentPlayer;
    bool isWinningShotForPlayer1 = false;
    bool isWinningShotForPlayer2 = false;
    bool isFirstShot = true;
    int redBallsRemaining = 7;
    int blueBallsRemaining = 7;
    int player1BallsRemaining;
    int player2BallsRemaining;
    bool isWaitingForBallMovementToStop = false;
    bool willSwapPlayers = false;
    bool isGameOver = false;
    bool ballPocketed = false;
    bool player1GotColor = false;
    bool player2GotColor = false;
    bool player1BallIsRed = false;
    bool player2BallIsRed = false;
    bool player1BallIsBlue = false;
    bool player2BallIsBlue = false;
    bool isFoul = false;
    bool isFalling = false;

    [SerializeField] float shotTimer = 3f;
    private float currentTimer;
    [SerializeField] float movementThreshold;

    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;
    [SerializeField] GameObject restartButton;
    [SerializeField] Transform headPosition;
    [SerializeField] Camera cueStickCamera;
    [SerializeField] Camera overheadCamera;
    Camera currentCamera;

    private Vector3 mousePosition;
    private Vector3 worldPosition;
    public LayerMask ballLayer1;

    // Start is called before the first frame update
    void Start()
    {
        /*GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        int ballLayer = LayerMask.NameToLayer(ballLayerName);

        foreach (GameObject ball in balls)
        {
            ball.layer = ballLayer;
        }*/

        StaticToken.p1Turn = true;
        currentPlayer = CurrentPlayer.Player1;
        currentCamera = cueStickCamera;
        currentTimer = shotTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFoul && !isGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                Debug.Log("Mouse Position on Click (Screen): " + mousePosition);

                Vector3 worldPosition = currentCamera.ScreenToWorldPoint(new Vector3(mousePosition.x + (float)0.12, mousePosition.y + (float)0.9, currentCamera.nearClipPlane + (float)0.45));

                Debug.Log("Mouse Position on Click (World): " + worldPosition);

                foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
                {
                    if (ball.GetComponent<Ball>().IsCueBall())
                    {
                        ball.transform.position = worldPosition;
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    }
                }
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isFoul = false; 
                isFalling = true;
            }
        }


        if (isFalling)
        {
            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                if (ball.GetComponent<Ball>().IsCueBall())
                {
                    float cueBallYPosition = ball.transform.position.y;
                    if (cueBallYPosition >= 1.9)
                    {
                        isFalling = true;
                    }
                    else
                    {
                        isFalling = false;
                    }
                }
            }
        }

        if (isWaitingForBallMovementToStop && !isGameOver && !isFalling && !isFoul)
        {
            currentTimer -= Time.deltaTime;
            if (currentTimer > 0)
            {
                return;
            }

            bool allStopped = true;
            foreach (GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
            {
                if (ball.GetComponent<Rigidbody>().velocity.magnitude >= movementThreshold)
                {
                    allStopped = false;
                    break;
                }
            }
            if (allStopped)
            {
                isWaitingForBallMovementToStop = false;
                if (willSwapPlayers || !ballPocketed || !isFoul)
                {
                    NextPlayerTurn();
                }
                else
                {
                    SwitchCamera();
                }
                currentTimer = shotTimer;
                ballPocketed = false;
            }
        }
    }

    public void Win(string message)
    {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        restartButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lose(string message)
    {
        isGameOver = true;
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        restartButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(2);
    }

    public void EarlyEightBall()
    {
        if (currentPlayer == CurrentPlayer.Player1)
        {
            Lose("Player 1 loses by pocketing the 8-ball early!");
        }
        else
        {
            Lose("Player 2 loses by pocketing the 8-ball early!");
        }
    }

    public void SwitchCamera()
    {
        if (currentCamera == cueStickCamera)
        {
            cueStickCamera.enabled = false;
            overheadCamera.enabled = true;
            currentCamera = overheadCamera;
            isWaitingForBallMovementToStop = true;
        }
        else
        {
            cueStickCamera.enabled = true;
            overheadCamera.enabled = false;
            currentCamera = cueStickCamera;
            currentCamera.gameObject.GetComponent<CameraController>().ResetCamera();
        }
    }

    void NextPlayerTurn()
    {
        if (currentPlayer == CurrentPlayer.Player1)
        {
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Player 2's Turn";
        }
        else
        {
            currentPlayer = CurrentPlayer.Player1;
            currentTurnText.text = "Player 1's Turn";
        }
        willSwapPlayers = false;
        StaticToken.p1Turn = !StaticToken.p1Turn;
        SwitchCamera();
    }

    bool CheckBall(Ball ball)
    {
        if (ball.IsCueBall())
        {
            if (currentPlayer == CurrentPlayer.Player1)
            {
                willSwapPlayers = true;
                isFoul = true;
                return false;
            }
            else
            {
                willSwapPlayers = true;
                isFoul = true;
                return false;
            }
        }
        else if (ball.IsEightBall())
        {
            if (currentPlayer == CurrentPlayer.Player1)
            {
                if (isWinningShotForPlayer1)
                {
                    Win("Player 1");
                    return true;
                }
            }
            else
            {
                if (isWinningShotForPlayer2)
                {
                    Win("Player 2");
                    return true;
                }
            }
            EarlyEightBall();
        }
        else
        {
            if (ball.IsRedBall())
            {
                redBallsRemaining--;
                if (currentPlayer == CurrentPlayer.Player1 && isFirstShot)
                {
                    willSwapPlayers = false;
                }
                else if (currentPlayer == CurrentPlayer.Player1 && !isFirstShot)
                {
                    if (!player1GotColor && !player2GotColor)
                    {
                        player1BallsRemaining = redBallsRemaining;
                        willSwapPlayers = false;
                        player1BallIsRed = true;
                        player2BallIsBlue = true;
                    }
                    else if (player1BallIsRed && player2BallIsBlue)
                    {
                        willSwapPlayers = false;
                        if (redBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer1 = true;
                        }
                    }
                    else if (player1BallIsBlue && player2BallIsRed)
                    {
                        willSwapPlayers = true;
                        if (redBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer2 = true;
                        }
                        isFoul = true;
                    }
                }
                else if (currentPlayer == CurrentPlayer.Player2 && !isFirstShot)
                {
                    if (!player1GotColor && !player2GotColor)
                    {
                        player1BallsRemaining = redBallsRemaining;
                        willSwapPlayers = false;
                        player2BallIsRed = true;
                        player1BallIsBlue = true;
                    }
                    else if (player1BallIsBlue && player2BallIsRed)
                    {
                        willSwapPlayers = false;
                        if (redBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer2 = true;
                        }
                    }
                    else if (player1BallIsRed && player2BallIsBlue)
                    {
                        willSwapPlayers = true;
                        if (redBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer1 = true;
                        }
                        isFoul = true;
                    }
                }
                return true;
            }
            else
            {
                blueBallsRemaining--;
                if (currentPlayer == CurrentPlayer.Player1 && isFirstShot)
                {
                    willSwapPlayers = false;
                }
                else if (currentPlayer == CurrentPlayer.Player1 && !isFirstShot)
                {
                    if (!player1GotColor && !player2GotColor)
                    {
                        player1BallsRemaining = blueBallsRemaining;
                        willSwapPlayers = false;
                        player1BallIsBlue = true;
                        player2BallIsRed = true;
                    }
                    else if (player1BallIsBlue && player2BallIsRed)
                    {
                        willSwapPlayers = false;
                        if (blueBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer1 = true;
                        }
                    }
                    else if (player1BallIsRed && player2BallIsBlue)
                    {
                        willSwapPlayers = true;
                        if (blueBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer2 = true;
                        }
                        isFoul = true;
                    }
                }
                else if (currentPlayer == CurrentPlayer.Player2 && !isFirstShot)
                {
                    if (!player1GotColor && !player2GotColor)
                    {
                        player1BallsRemaining = blueBallsRemaining;
                        willSwapPlayers = false;
                        player2BallIsBlue = true;
                        player1BallIsRed = true;
                    }
                    else if (player1BallIsRed && player2BallIsBlue)
                    {
                        willSwapPlayers = false;
                        if (blueBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer2 = true;
                        }
                    }
                    else if (player1BallIsBlue && player2BallIsRed)
                    {
                        willSwapPlayers = true;
                        if (blueBallsRemaining <= 0)
                        {
                            isWinningShotForPlayer1 = true;
                        }
                        isFoul = true;
                    }
                }
                return true;
            }
        }
        return true;
    }

    // Test, enable istrigger to run also
    /*public void OnCollisionEnter(Collision collision)
    {
        if ((ballLayer1.value & (1 << collision.gameObject.layer)) != 3)
        {
            Debug.Log("Hit the ball");
            isFoul = false;
        }
        else
        {
            isFoul = true;
        }
    }*/

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            ballPocketed = true;
            AudioManager.instance.PlaySFX("BallPocketed");
            if (CheckBall(other.gameObject.GetComponent<Ball>()))
            {
                Destroy(other.gameObject);
            }
            else
            {
                isFoul = true;
                Debug.Log("Foul");
            }
        }
    }
}