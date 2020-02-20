using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //////////// Inspector Values
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private float horizontalJetSpeed = 3f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float jetPackForce = 3f;
    [SerializeField]
    private float jetTime = 3f;
    [SerializeField]
    private float dashSpeed = 1f;
    [SerializeField]
    private float startDashTime = 1f;
    [SerializeField]
    private float dashCooldown = 1f;


    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatisGround;

    //////////// States
    private bool isGrounded = true;
    private bool onAir;

    //////////// Controller Variables
    private float moveInput;
    private float moveVerticalInput;
    private Rigidbody2D rigidbody;
    private float jetTimeCounter;
    private float dashTime;
    private int direction;
    private bool dashIsOnCD = false;
    private Animator anim;
    private bool jetting = false;
    private bool dashing = false;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        moveInput = Input.GetAxis("MoveHorizontal");
        moveVerticalInput = Input.GetAxis("MoveVertical");

        if (isGrounded == true && onAir == false)
        {
            //Debug.Log(moveInput);
            rigidbody.velocity = new Vector2(moveInput * moveSpeed * Time.deltaTime, rigidbody.velocity.y);

        }
        else if (isGrounded == false && Input.GetButton("Jetpack"))
        {
            rigidbody.velocity = new Vector2(moveInput * horizontalJetSpeed * Time.deltaTime, rigidbody.velocity.y);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"), 0.0f);

        Grounded();
        Flip();
        Jump();
        Jetpack();
        Dash();
        Falling();
    }

    void Grounded()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatisGround);
        
        if (isGrounded)
        {
            jetTimeCounter = jetTime;
            onAir = false;
            anim.SetInteger("Condicoes", 0);
            Debug.Log("triggei chao");
        }
    }

    void Flip()
    {
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void Jump()
    {
        if (isGrounded == true && Input.GetButtonDown("Jump"))
        {
            anim.SetInteger("Condicoes", 1);
            rigidbody.AddForce(new Vector2(0f, jumpForce));
            onAir = true;
        }
    }

    void Falling()
    {
        if (rigidbody.velocity.y < -0.0000001f)
        {
            Debug.Log("tocaindo");
            anim.SetInteger("Condicoes", 2);
        }
    }

    void Jetpack()
    {
        if (Input.GetButton("Jetpack"))
        {
            if (jetTimeCounter > 0)
            {
                anim.SetInteger("Condicoes", 3);
                jetting = true;
                onAir = true;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jetPackForce * Time.deltaTime);

                jetTimeCounter -= Time.deltaTime;
            }
        }

        if(Input.GetButtonUp("Jetpack"))
        {
            jetting = false;
        }
    }

    void Dash()
    {
        Vector2 dir = new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"));
        Vector3 movement = new Vector3(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"), 0.0f);
        if (direction == 0)
        {
            if (Input.GetButtonDown("Dash") && onAir == true && dashIsOnCD == false)
            {
                if (moveInput < 0)
                    direction = 1;
                else if (moveInput > 0)
                    direction = 2;
                else if (moveVerticalInput < 0)
                    direction = 3;
                else if (moveVerticalInput > 0)
                    direction = 4;
                //else if (moveInput == 0 && moveVerticalInput == 0)
                    //direction = 5;


            }
            
        }
        else
        {
            if(dashTime <= 0)
            {
                direction = 0;
                dashTime = startDashTime;
                //rigidbody.velocity = Vector2.zero;
            }
            else
            {
                dashTime -= Time.deltaTime;

                if (direction > 0)
                {
                    anim.SetInteger("Condicoes", 4);
                    dashing = true;
                    dashIsOnCD = true;
                    transform.position = transform.position + movement * Time.deltaTime * dashSpeed;
                    
                    //rigidbody.velocity += dir.normalized * dashSpeed;
                    //rigidbody.AddForce(dir * dashSpeed * Time.deltaTime);
                    StartCoroutine(DashWait());
                }
                    
                    

            }
        }
    }


    IEnumerator DashWait()
    {

        yield return new WaitForSeconds(dashCooldown);

        dashIsOnCD = false;
        dashing = false;
    }
}

