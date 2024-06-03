using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEditor.VersionControl;
using System.Runtime.InteropServices;

public class GameManager : MonoBehaviour
{
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

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);
    public BoxCollider boundsCollider;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = CurrentPlayer.Player1;
        currentCamera = cueStickCamera;
        currentTimer = shotTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaitingForBallMovementToStop && !isGameOver)
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
                if (willSwapPlayers || !ballPocketed)
                {
                    if (isFoul)
                    {
                        ReplaceCueBall();
                    }
                    NextPlayerTurn();
                }
                else
                {
                    if (isFoul)
                    {
                        ReplaceCueBall();
                    }
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
        SwitchCamera();
    }

    bool CheckBall(Ball ball)
    {
        if (ball.IsCueBall())
        {
            if (currentPlayer == CurrentPlayer.Player1)
            {
                willSwapPlayers = true;
                return false;
            }
            else
            {
                willSwapPlayers = true;
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

    public void ReplaceCueBall()
    {
        Cursor.lockState = CursorLockMode.None;
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Convert the bounds to screen space
        Bounds bounds = boundsCollider.bounds;
        Vector3 minBounds = Camera.main.WorldToScreenPoint(bounds.min);
        Vector3 maxBounds = Camera.main.WorldToScreenPoint(bounds.max);

        // Clamp the mouse position within the specified bounds
        mousePosition.x = Mathf.Clamp(mousePosition.x, minBounds.x, maxBounds.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minBounds.y, maxBounds.y);

        // Set the cursor position
        SetCursorPos((int)mousePosition.x, (int)(Screen.height - mousePosition.y));
    }

    private void OnTriggerEnter(Collider other)
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
                other.gameObject.transform.position = boundsCollider.transform.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }
    }
}
