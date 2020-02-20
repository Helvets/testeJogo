using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    private Rigidbody2D rigidbody;

    [SerializeField]
    private float dashSpeed = 1;
    [SerializeField]
    private float startDashTime = 1;

    private float dashTime;
    private int direction;
    private float pc_Hmoveinput;
    private float pc_Vmoveinput;
    private bool isdashing = false;

    private void Start()
    {
        Debug.Log("iniciei dash");
        rigidbody = GetComponent<Rigidbody2D>();
        dashTime = startDashTime;

    }

    private void Update()
    {
        //pc_Hmoveinput = gameObject.GetComponent<PlayerController>().MoveInput;
        //pc_Vmoveinput = gameObject.GetComponent<PlayerController>().MoveVerticalInput;

        Vector3 movement = new Vector3(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"), 0.0f);

        if (Input.GetButtonDown("Dash") && isdashing == false)
        {
            isdashing = true;
            rigidbody.velocity = Vector2.zero;
            Vector2 dir = new Vector2(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"));

            rigidbody.velocity += dir.normalized * dashSpeed;

            StartCoroutine(DashWait());
        }
    }

    IEnumerator DashWait()
    {
        yield return new WaitForSeconds(1.3f);
        isdashing = false;
    }

}
