using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public float maxJumpHeight = 2;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 5;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    [HideInInspector]
    public bool facingRight = true;

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    public Text scoreText;
    private int score;

    private Animator anim;

    Controller2D controller;

    [HideInInspector]
    public bool canMove = true;

    [HideInInspector]
    public bool getKey = false;

    void Start()
    {
        score = 0;
        anim = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        print("Gravity: " + gravity + "  Jump Velocity: " + maxJumpVelocity);
    }

    void Update()
    {
        if (!canMove)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        bool wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
            {
                velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (input.x != wallDirX && input.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallSliding)
            {
                if (wallDirX == input.x)
                {
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }
                else if (input.x == 0)
                {
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }
                else
                {
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if (controller.collisions.below)
            {
                velocity.y = maxJumpVelocity;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
            }
        }


        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        float h = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", Mathf.Abs(h));

        if (h > 0 && !facingRight)
            Flip();
        else if (h < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            var elevator = other.GetComponent<PlatformController>();

            if (elevator.id == 2 || elevator.id == 1)
            {
                Reposicionar();
            }

            if (elevator.id == 3)
            {
                elevator.elevatorStart();
            }
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            score++;
            SetScoreText();
        }

        if (other.gameObject.CompareTag("Key"))
        {
            other.gameObject.SetActive(false);
            getKey = true;
        }

        if (other.gameObject.CompareTag("ColliderToTalk"))
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("DestroyOnTouch"))
        {
            Destroy(other.gameObject);
        }


    }
    public void Reposicionar()
    {
        var resps = GameObject.FindGameObjectsWithTag("Respawn");
        Debug.logger.Log(resps.Length);

        GameObject nexter = null;

        float distance = float.MaxValue;

        var length = resps.Length;
        foreach (var item in resps)
        {

            var distanceLoop = Vector3.Distance(this.transform.position, item.transform.position);

            if (distance > distanceLoop)
            {
                nexter = item;
                distance = distanceLoop;
            }


        }

        this.transform.position = nexter.GetComponent<Transform>().position;

    }

    void SetScoreText()
    {
        scoreText.text = "Moedas: " + score.ToString();
    }
}