using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    [HideInInspector]
    public Player player;

    public bool requireButtonPress;
    private bool waitForPress;
    public bool destroyWhenActivated;

    [HideInInspector]
    public bool reset = false;

    // Use this for initialization
    void Start()
    {
        resultText.text = string.Empty;
        SetScoreText();
    }

    // Update is called once per frame
    void Update()
    {

        if (waitForPress && Input.GetKeyDown(KeyCode.UpArrow) && player.getKey == true)
        {
            reset = true;
            SetScoreText();
            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
            //SceneManager.LoadScene("Main");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            if (requireButtonPress)
            {
                waitForPress = true;
                return;
            }

            reset = true;
            SetScoreText();
            if (destroyWhenActivated)
            {
                Destroy(gameObject);
            }
            //SceneManager.LoadScene("Main");
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            waitForPress = false;
        }
    }

    public Text resultText;
    public Button restartButton;

    void SetScoreText()
    {
        if (reset == true)
        {
            resultText.text = "Você ganhou!";
            restartButton.gameObject.SetActive(true);
            player.canMove = false;
        }
    }

    public void RestartClick(string textParameter)
    {
        SceneManager.LoadScene("Main");
    }

}
