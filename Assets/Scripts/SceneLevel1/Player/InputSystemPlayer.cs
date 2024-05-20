using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class InputSystemPlayer : MonoBehaviour
{
    public Rigidbody2D rb;

    //Movement
    [Header("Movement")]
    public float speedMove = 5f;
    float horizontalMovement;
    SpriteRenderer spriteRenderer;

    //Jump
    [Header("Jump")]
    public float jumpPower = 10f;

    [Header("GroundCheck")]
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f,0.05f);
    public LayerMask groundLayer;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 2f;
    private float dashDirection;
    private bool isDashing = false;



      //Detection
    private Vector3 respawnPoint;
    public GameObject fallDetector;


    //Heart
    private bool isHurt = false; 

    //Animasi
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDashing){
            playerVelocity();
        }
        Animations();
        Detection();
    }

    void playerVelocity(){
        rb.velocity = new Vector2(horizontalMovement * speedMove,rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context){
        horizontalMovement = context.ReadValue<Vector2>().x;
        if(horizontalMovement < 0){
            spriteRenderer.flipX = true;
        }else if(horizontalMovement > 0){
            spriteRenderer.flipX = false;
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded()){
            if(context.performed)
            {
                rb.velocity = new Vector2(rb.velocity.x,jumpPower);
            }else if(context.canceled)
            {
                rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y * 0.5f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPosition.position,groundCheckSize);
    }

    private bool isGrounded(){
        return Physics2D.OverlapBox(groundCheckPosition.position,groundCheckSize,0,groundLayer) != null;
    }
    
    void Detection(){
        fallDetector.transform.position = new Vector2(transform.position.x,fallDetector.transform.position.y);
    }

    public void Dash(InputAction.CallbackContext context){
        if(context.performed && isGrounded() && !isDashing && horizontalMovement != 0){
            dashDirection =spriteRenderer.flipX ? -1 : 1;
            StartCoroutine(PerformDash());    
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        animator.SetBool("Dash",true);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashDirection * dashSpeed,0);
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("Dash",false);
    }

      void Animations(){
        animator.SetFloat("Moving",Mathf.Abs(horizontalMovement));
        animator.SetBool("Jump",isGrounded());
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Detection" && !isHurt){
           HealtManager.health--;
            transform.position = respawnPoint;
           if(HealtManager.health <= 0) {
            PlayerManager.isGameOver = true;
            gameObject.SetActive(false);
           }else{
                StartCoroutine(GetHurt());
           }
        }else if(other.tag == "checkpoint"){
            respawnPoint = transform.position;
        }
    }

    IEnumerator GetHurt(){
        isHurt = true;
        Physics2D.IgnoreLayerCollision(6,7);
        yield return new WaitForSeconds(0.4f);
        Physics2D.IgnoreLayerCollision(6,7,false);
        isHurt = false;
    }
}
