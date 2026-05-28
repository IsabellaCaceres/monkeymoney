using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class MonkeyMovement : MonoBehaviour
{
    private Rigidbody2D monkeyRigidBody;
    private SpriteRenderer _monkeySpriteRenderer;
    private GameObject heldItem = null;
    [Header("Animation")]
    public Sprite spriteUp;
    public Sprite spriteRight;
    public Sprite spriteLeft;
    public Sprite spriteDown;
    [Header("Movement")]
    public float speed = 3;
    public float acceleration = 80;
    InputAction moveAction;
    public SpriteRenderer bound1;
    public SpriteRenderer bound2;
    private AudioSource mainMonkeyAudio;
    public AudioClip nomNom;
    [Header("Order Stuff")]
    public GameObject OrderManager;
    private OrderManager actualOrderManager;
    public GameObject monkeyBreadPrefab;
    public GameObject mudcakePrefab;
    public GameObject muffinPrefab;

    void Awake()
    { 
        moveAction = InputSystem.actions.FindAction("Move");
    }

    void Start()
    {
        monkeyRigidBody = GetComponent<Rigidbody2D>();
        _monkeySpriteRenderer = GetComponent<SpriteRenderer>();
        actualOrderManager = OrderManager.GetComponent<OrderManager>();
        mainMonkeyAudio = GetComponent<AudioSource>();
        bound1.color = new Color(1f, 1f, 1f, 0);
        bound2.color = new Color(1f, 1f, 1f, 0);
    }

    void Update()
    {
        Move();
        UpdateSprite();
    }
    void Move()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 velocity = monkeyRigidBody.linearVelocity;

        float targetVelX = moveInput.x * speed;
        if (moveInput.x == 0) // if let go of left/right arrow key, slow deceleration
        {
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelX, acceleration * Time.deltaTime / 20);
            mainMonkeyAudio.Stop();
        }
        else 
        {
            velocity.x = Mathf.MoveTowards(velocity.x, targetVelX, acceleration * Time.deltaTime);
            mainMonkeyAudio.Play();

        }
        if (moveInput.y > 0)
        {
            float targetVelY = moveInput.y * speed;
            velocity.y = Mathf.MoveTowards(velocity.y, targetVelY, acceleration * Time.deltaTime);
            mainMonkeyAudio.Play();
        }

        monkeyRigidBody.linearVelocity = velocity;
    }
    void UpdateSprite()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        Vector2 velocity = monkeyRigidBody.linearVelocity;
        transform.rotation = Quaternion.identity;
        _monkeySpriteRenderer.flipX = false;

       
        if ((moveInput.x > 0 && moveInput.y > 0) || moveInput.x > 0) {
            _monkeySpriteRenderer.sprite = spriteRight;
            transform.rotation = Quaternion.Euler(0, 0, -45f);

        }
        else if ((moveInput.x < 0 && moveInput.y > 0) || moveInput.x < 0)
        {
            _monkeySpriteRenderer.sprite = spriteLeft;
            transform.rotation = Quaternion.Euler(0, 0, 45f);
            _monkeySpriteRenderer.flipX = true;
        }
        else if (moveInput.y > 0)
        {
            _monkeySpriteRenderer.sprite = spriteUp;
        }
        else
        {
            _monkeySpriteRenderer.sprite = spriteDown;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // Pick up item if we don't have one
        if (heldItem == null && other.CompareTag("Pickup"))
        {
            heldItem = other.gameObject;
            Vector3 heldItemOrig = heldItem.transform.position;
            // Optional: disable physics so it follows the monkey
            Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
            if (itemRb != null) itemRb.simulated = false;
            heldItem.transform.SetParent(transform); // attach to monkey
            heldItem.transform.localPosition = (Vector2.up * .13f) + (Vector2.right * 0.02f); // offset above monkey
            if (heldItem.transform.name == "monkeybread")
            {
                Instantiate(monkeyBreadPrefab, heldItemOrig, Quaternion.identity);
            } 
            else if (heldItem.transform.name == "mudcake")
            {
                Instantiate(mudcakePrefab, heldItemOrig, Quaternion.identity);
            }
            else
            {
                Instantiate(muffinPrefab, heldItemOrig, Quaternion.identity);
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Drop off automatically if holding an item and overlapping Dropoff
        if (heldItem != null && other.CompareTag("Dropoff"))
        {
            heldItem.transform.SetParent(other.transform);
            heldItem.transform.localPosition = Vector3.zero;
            Rigidbody2D itemRb = heldItem.GetComponent<Rigidbody2D>();
            if (itemRb != null) itemRb.simulated = true;
            actualOrderManager.FulfillOrder(heldItem.transform.name, other.gameObject.transform.name, heldItem);
            heldItem = null;
        }

        if (heldItem != null && other.CompareTag("Trash"))
        {
            Destroy(heldItem.gameObject);
            heldItem = null;
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(nomNom);
        }
    }
}


/*
    void Move()
    {
        Vector2 velocity = monkeyRigidBody.linearVelocity;
        Vector2 move = moveAction.ReadValue<Vector2>();
        float horizontal = move.x;
        float vertical = move.y;
        velocity.x = horizontal * speed;

        if (moveAction.WasPressedThisFrame())
        {
            velocity.y = vertical * speed;
        }
        
         monkeyRigidBody.linearVelocity = velocity;

    }
}

*/