using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed;

    [Header("プレイヤーカメラ")]
    [SerializeField] PlayerCam pCam;

    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    LayerMask whatIsGround;
    bool grounded;

    bool Run;

    Rigidbody rb;
    [SerializeField]Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, pCam.YRotation, 0);

        MyInput();
        SpeedControll();

        if (grounded)
        {
            rb.drag = 10f;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 || verticalInput == 0)
        {
            anim.SetBool("Walk_F", false);
            anim.SetBool("Walk_B", false);
            anim.SetBool("Walk_L", false);
            anim.SetBool("Walk_R", false);
        }

        if(Input.GetKey(KeyCode.A))
        {
            anim.SetBool("Walk_L",true);
            if(Run)
            {
                anim.SetBool("Walk_L", false);
                anim.SetBool("Run_L", true);
            }
        }
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("Walk_R", true); 
            if (Run)
            {
                anim.SetBool("Walk_R", false);
                anim.SetBool("Run_R", true);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("Walk_B", true);
            if (Run)
            {
                anim.SetBool("Walk_B", false);
                anim.SetBool("Run_B", true);
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            anim.SetBool("Walk_F", true);
            if (Run)
            {
                anim.SetBool("Walk_F", false);
                anim.SetBool("Run_F", true);
            }
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            Run = true;
            moveSpeed = 10;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)) 
        {
            Run = false;
            anim.SetBool("Run_R", false);
            anim.SetBool("Run_L", false);
            anim.SetBool("Run_B", false);
            anim.SetBool("Run_F", false);
            moveSpeed = 2;
        }

        Debug.Log("走り" + Run);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

#if false
        Ray ray = new Ray(transform.position,Vector3.down);
        if(Physics.Raycast(ray,10f,whatIsGround))
        {
            grounded = true;
        }

        if(grounded)
        {
            rb.drag = 10;
        }
        else
        {
            rb.drag = 0;
            Debug.Log("地上にいない" + grounded);
        }
        Debug.DrawRay(transform.position, Vector3.down, Color.red);
#endif
    }

    private void SpeedControll()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}
