using UnityEngine;

public class PlayerMovement : MonoBehaviour
// player movement
{
    Rigidbody2D myRigidbody;
    [SerializeField] float runSpeed = 5f; 
    Collider2D myCollider;
    //public Animator animator;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // new variable for component collider
        myCollider = GetComponent<Collider2D>();

        //finds the Rigidbody 2D component attached to Player
        myRigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        Run(); 
    }

    private void Run() 
    {
        // Get the player's input on the horizontal axis (e.g., pressing left or right arrow keys or A/D keys).
        // The "Horizontal" input maps to -1 for left, 0 for no input, and 1 for right.
        float controlDirection_x = Input.GetAxis("Horizontal");
        //animator.SetFloat("Speed", Mathf.Abs(controlDirection_x));

        float controlDirection_y = Input.GetAxis("Vertical");
       // animator.SetFloat("Speed", Mathf.Abs(controlDirection_y));

        // Create a 2D vector that represents the player's velocity.
        // The x-value is calculated by multiplying the horizontal input by the runSpeed (player's running speed).
        // The y-value keeps the player's existing vertical velocity to preserve actions like jumping or falling.
        //Vector2 playerVelocity = new Vector2(controlDirection_x * runSpeed, myRigidbody.linearVelocity.y);
        Vector2 playerVelocity = new Vector2(controlDirection_x * runSpeed, controlDirection_y * runSpeed);
        


        // Apply the newly calculated velocity to the player's Rigidbody.
        // This will make the player move horizontally based on the input and speed.
        myRigidbody.linearVelocity = playerVelocity;

        if (controlDirection_x < 0 )
            {   
                // to move left
                //transform.eulerAngles = new Vector3(0f, 180f, 0f);
                transform.localScale = new Vector3(-1,1,1);
            } 
        else if (controlDirection_x > 0)
            { 
                transform.localScale = new Vector3(1,1,1);
            }
        else if (controlDirection_y < 0)
            { 
                transform.localScale = new Vector3(1,1,1);
            }
        else if (controlDirection_y > 0)
            { 
                transform.localScale = new Vector3(1,1,1);
            }
        
        

    }
}
