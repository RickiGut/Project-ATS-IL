using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaylaMovement : MonoBehaviour
{
    float movementHorizontalNayla ;
    
    float speedNayla = 2;

    [SerializeField]
    float jumpNayla;

    Rigidbody2D playerNayla;

    bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        playerNayla = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Facing();
        Jump();
    }

    void Movement(){
        movementHorizontalNayla = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(movementHorizontalNayla,0);

        transform.Translate(direction*Time.deltaTime*speedNayla);
    }

    void Facing(){
        if(movementHorizontalNayla < 0){
            transform.localScale = new Vector3(-3.1226f,3.1226f,3.1226f);
        }else{
            transform.localScale = new Vector3(3.1226f,3.1226f,3.1226f);
        }
    }

    void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && onGround == true){
           playerNayla.velocity = new Vector2(0,1) * jumpNayla;
        }
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
