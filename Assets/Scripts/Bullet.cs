using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float range;
    [SerializeField] Animator bullet;
    GameObject player;
    float boom=1;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        bulletSpeed = bulletSpeed * player.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector2(bulletSpeed * Time.deltaTime * boom, 0));
        Destroy(gameObject, range);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            boom = 0;
            Destroy(this.gameObject,0.2f);
            bullet.SetBool("Shooting", true);
        }
        else
        {
            boom = 1;
        }
    }
}
