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

    //Animasi
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        playerNayla = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Facing();
        Jump();
        Dash();
        Animations();
    }

    void Movement(){
        movementHorizontalNayla = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(movementHorizontalNayla,0);

        if(onGround == true){
                currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedNayla : runNayla;
            
        }

        transform.Translate(direction*Time.deltaTime*currentSpeed);
    }

    void Facing(){
        if(movementHorizontalNayla < 0){
                spriteRenderer.flipX = true;    
            }
        else if(movementHorizontalNayla >0){
                spriteRenderer.flipX = false;    
            }
    }


    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && onGround == true){
           playerNayla.velocity = new Vector2(0,1) * jumpNayla;
        }
    }

    void Dash(){
        if(Input.GetKeyDown(KeyCode.S) && onGround == true){
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
        animator.SetBool("Jump",onGround);
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Ground"){
            onGround =true;
            print(onGround);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Ground"){
            onGround = false;
            print(onGround);
        }
    }
}
