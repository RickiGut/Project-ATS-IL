using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaylaMovement : MonoBehaviour
{
    float movementHorizontalNayla ;
    
    [SerializeField]
    float runNayla ;
    float currentSpeed;
    [SerializeField]
    float speedNayla;

    [SerializeField]
    float jumpNayla;

    Rigidbody2D playerNayla;
    SpriteRenderer spriteRenderer;

    bool onGround = false;

    //Detection Ground 
    [SerializeField] Vector2 boxSize;
    [SerializeField] float castDistance;
    [SerializeField] LayerMask groundLayer;


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
        playerNayla = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        respawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Facing();
        Jump();
        Dash();
        Detection();
        Animations();
    }

    

    void Movement(){
        movementHorizontalNayla = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(movementHorizontalNayla,0);

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedNayla : runNayla;

        transform.Translate(direction*Time.deltaTime*currentSpeed);
    }

    void Facing(){
        if(movementHorizontalNayla < 0){
                // spriteRenderer.flipX = true;
                transform.localScale= new Vector3(-0.8f,0.8f,0.8f);
            }
        else if(movementHorizontalNayla > 0){
                transform.localScale= new Vector3(0.8f,0.8f,0.8f);
               }
    }


    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded()){
           playerNayla.velocity = new Vector2(0,1) * jumpNayla;
        }
    }

    public bool isGrounded(){
        if(Physics2D.BoxCast(transform.position,boxSize,0,-transform.up,castDistance,groundLayer)){
            return true;
        }else{
            return false;
        }
    }

    void Detection(){
        fallDetector.transform.position = new Vector2(transform.position.x,fallDetector.transform.position.y);
    }


    private void OnDrawGizmos(){
        Gizmos.DrawCube(transform.position-transform.up*castDistance,boxSize);
    }

    void Dash(){
        if(Input.GetKeyDown(KeyCode.S) && isGrounded()){
            if(movementHorizontalNayla < 0){
             playerNayla.velocity = new Vector2(-1,0) * jumpNayla; 
            }
            if(movementHorizontalNayla > 0){
             playerNayla.velocity = new Vector2(1,0) * jumpNayla; 
            }
        }
    }

    void Animations(){
        animator.SetFloat("Moving",Mathf.Abs(movementHorizontalNayla));
        animator.SetBool("Jump",isGrounded());
    }


    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Detection" && !isHurt){
           HealtManager.health--;
            transform.position = respawnPoint;
           if(HealtManager.health < 0) {
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
