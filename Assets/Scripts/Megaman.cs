using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Megaman : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float Initimeofdash;
    [SerializeField] float DSM;
    [SerializeField] BoxCollider2D pies;
    [SerializeField] Sprite idleSprite;
    [SerializeField] Sprite fallingSprite;
    [SerializeField] float nextFire;
    [SerializeField] GameObject bullet;

    float NS;
    int canjump;
    bool canjump1;
    float DS;
    float timeofdash;
    Animator myAnimator;
    Rigidbody2D myBody;
    BoxCollider2D mycollider;
    float tamX, canFire;
    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        mycollider = GetComponent<BoxCollider2D>();
        tamX = (GetComponent<SpriteRenderer>()).bounds.size.x;
        timeofdash = Initimeofdash;
        NS = jumpSpeed;
        DS = jumpSpeed * DSM;
    }

    // Update is called once per frame
    void Update()
    {
        Mover();
        Saltar();
        Falling();
        Fire();
        Dash();

        

    } 
    void Fire()
    {
        if (Input.GetKey(KeyCode.L))
        {
            myAnimator.SetLayerWeight(1, 1);
            if (Time.time >= canFire && Input.GetKeyDown(KeyCode.L))
            {
                Instantiate(bullet, transform.position - new Vector3(-tamX * gameObject.transform.localScale.x, 0, 0), transform.rotation);
                canFire = Time.time + nextFire;
            }
        }
        else
        {
            myAnimator.SetLayerWeight(1, 0);
        }
    }
    void Mover()
    {
        
        
        float mov = Input.GetAxis("Horizontal");

        if (mov != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(mov), 1);
            myAnimator.SetBool("running", true);
            transform.Translate(new Vector2(mov * speed * Time.deltaTime, 0));
        }
        else
        {
            myAnimator.SetBool("running", false);
        }
    }
    void Dash()
    {

        if (!myAnimator.GetBool("dash") && !myAnimator.GetBool("jumping") && !myAnimator.GetBool("falling"))
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {

                if (timeofdash <= 0)
                {
                     timeofdash = Initimeofdash;
                    myBody.velocity = Vector2.zero;
                    myAnimator.SetBool("dash", false);
                }
                else
                {
    
                    timeofdash -= Time.deltaTime;
                    myAnimator.SetBool("dash", true);
                    StartCoroutine(jumpspeed());
                    if (transform.localScale.x == 1)
                    {
                        myBody.velocity = Vector2.right * (speed * 3);
                    }
                    else if (transform.localScale.x == -1)
                    {
                        myBody.velocity = Vector2.left * (speed * 3);


                    }
                }


            }
        }
        else
        {
            myAnimator.SetBool("dash", false);
        }
            
    }
    void Saltar()
    {


        if (isGrounded() && !myAnimator.GetBool("jumping"))
        {
            myAnimator.SetBool("falling", false);
            myAnimator.SetBool("jumping", false);


            if (Input.GetKeyDown(KeyCode.Space) && canjump==0)
            {


                myAnimator.SetTrigger("Takeoff");
                myAnimator.SetBool("jumping", true);
                myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                canjump = 1;
                canjump1 = true;

            }
        }
        else if (!myAnimator.GetBool("falling") && canjump1)
        {
                       

            if (Input.GetKeyDown(KeyCode.Space) && canjump <= 1)
            {
                myAnimator.SetTrigger("Takeoff");
                myAnimator.SetBool("jumping", true);
                myBody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
                canjump1 = false;
                canjump = 2;


            }

        }
    

    }



    void Falling()
    {
        if (myBody.velocity.y < 0 && (!myAnimator.GetBool("jumping") && !myAnimator.GetBool("dash")))
        {
            myAnimator.SetBool("falling", true);
        }
       
    }
    bool isGrounded()
    {
        //return pies.IsTouchingLayers(LayerMask.GetMask("Ground"));
        RaycastHit2D myRaycast =  Physics2D.Raycast(mycollider.bounds.center, Vector2.down, mycollider.bounds.extents.y + 0.2f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(mycollider.bounds.center, new Vector2(0, ((mycollider.bounds.extents.y + 0.2f) * -1)), Color.green);
        myAnimator.SetBool("falling", false);
        canjump = 0;
        return myRaycast.collider != null;
    }
    void AfterTakeOfEvent()
    {
        myAnimator.SetBool("jumping", false);
        myAnimator.SetBool("falling", false);
    }
    IEnumerator jumpspeed()
    {
        jumpSpeed = DS;
        yield return new WaitForSeconds(1f);
        jumpSpeed = NS;
    }

}
