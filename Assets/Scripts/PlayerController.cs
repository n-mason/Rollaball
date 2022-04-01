using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;

    private bool touchingGround;
    private int jumpCount;
    private bool firstJump;
    private bool secondJump;

    private float movementX;
    private float movementY;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        touchingGround = true;
        firstJump = false;
        secondJump = false;

        SetCountText();
        winTextObject.SetActive(false);
    }

    void Update()
    {
        
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    // Found a little bit of help on how to do jump with Unity Input from: https://www.youtube.com/watch?v=cnSqgA4OIEk&t=622s
    void OnJump()
    {
        //Debug.Log("JUMP PRESSED");

        if (touchingGround)
        {
            //Debug.Log("APPLIED FORCE");

            firstJump = true;
            jumpCount = 1;
        }

        if ((touchingGround == false) && (jumpCount == 1))
        {
            secondJump = true;
            jumpCount = 0; // reset jumpCount after doing the second jump
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        Vector3 jump = new Vector3(0.0f, 500.0f, 0.0f);

        rb.AddForce(movement * speed);

        if (firstJump)
        {
            //Debug.Log("Applied the JUMP");
            rb.AddForce(jump);
            
            firstJump = false;
        }
        if (secondJump)
        {
            //Debug.Log("Applied the 2nd JUMP");
            rb.AddForce(jump);
            secondJump = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            SetCountText();
        }
    }

    // Found some help on implementing double jump from: https://answers.unity.com/questions/858742/how-do-i-check-if-an-object-is-touching-another-ob.html
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            touchingGround = true;
            //Debug.Log("touching ground");
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            touchingGround = false;
            //Debug.Log("left the ground");
        }
    }
}
