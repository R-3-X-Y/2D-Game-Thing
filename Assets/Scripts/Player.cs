using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private int speed = 5;
    [SerializeField] private int jumpPower = 5;
    [SerializeField] private GameObject blockBreakPrefab;
    private float movement;
    private float jump;
    private bool onGround;
    public Tilemap ground;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetKey(KeyCode.Space) && onGround)
        {
            jump = jumpPower;
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector3.Distance(worldMousePos, transform.position) <= 3 && ground.GetTile(ground.WorldToCell(worldMousePos)) != null)
            {
                Color tmpColor = ground.GetColor(ground.WorldToCell(worldMousePos));
                ground.SetTile(ground.WorldToCell(worldMousePos), null);
                GameObject particle = Instantiate(blockBreakPrefab, ground.WorldToCell(worldMousePos), Quaternion.identity);
                particle.transform.position += new Vector3(0.5f, 0.5f, 0);
                ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
                main.startColor = tmpColor;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movement, (jump > 0) ? jump : rb.velocity.y);
        jump = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            onGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            onGround = false;
        }
    }
}
