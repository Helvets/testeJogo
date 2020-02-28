using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControl2 : MonoBehaviour
{

    public float speed = 10.0f;
    public Rigidbody2D rb;
    public Vector2 movement;
    public float maxHorizontalSpeed = 100.0f;
    public float maxVerticalSpeed = 100.0f;

    public float maxXDashSpeed = 100.0f;
    public float maxYDashSpeed = 100.0f;

    [SerializeField]
    private float dashSpeed = 1f;
    [SerializeField]
    private float dashCooldown = 1f;

    private Animator anim;
    private int direction;

    private bool dashIsOnCD = false;
    private bool onAir;
    private bool buttonDash;



    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"));
        buttonDash = Input.GetButtonDown("Dash");
    }

    private void FixedUpdate()
    {
        moveCharacter(movement);
        Dash();

    }

    void moveCharacter(Vector2 direction)
    {
        Vector2 force = movement * speed * Time.deltaTime;

        rb.AddForce(force, ForceMode2D.Impulse);
        CapVelocity();

    }

    void CapVelocity()
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rb.velocity.x), maxHorizontalSpeed) * Mathf.Sign(rb.velocity.x);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rb.velocity.y), maxVerticalSpeed) * Mathf.Sign(rb.velocity.y);

        rb.velocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }

    void Dash()
    {
        if (buttonDash && dashIsOnCD == false)
        {
            dashIsOnCD = true;

            Vector2 dashForce = movement * dashSpeed * Time.deltaTime;

            rb.AddForce(dashForce, ForceMode2D.Impulse);

            DashCapVelocity();

            StartCoroutine(DashWait());

        }
    }

    void DashCapVelocity()
    {
        float cappedXVelocity = Mathf.Min(Mathf.Abs(rb.velocity.x), maxXDashSpeed) * Mathf.Sign(rb.velocity.x);
        float cappedYVelocity = Mathf.Min(Mathf.Abs(rb.velocity.y), maxYDashSpeed) * Mathf.Sign(rb.velocity.y);

        rb.velocity = new Vector2(cappedXVelocity, cappedYVelocity);
    }

    IEnumerator DashWait()
    {

        yield return new WaitForSeconds(dashCooldown);

        dashIsOnCD = false;
    }
}