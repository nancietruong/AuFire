using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public Rigidbody2D playerRB;
    [SerializeField] float speed;
    public PlayerAnimation playerAnimation;
    PlayerStateMachine playerStateMachine;
    public GunBase gun;
    SpriteRenderer playerSpriteRenderer;

    [Header("Dodge Roll Settings")]
    [SerializeField] float dodgeRollSpeed;
    public float dodgeRollDuration;
    public float dodgeRollCooldown;
    [SerializeField] public bool isDodging = false;

    private Vector2 lastMoveDirection = Vector2.right;
    public void Init()
    {
        if (gun == null)
        {
            gun = GetComponentInChildren<GunBase>();
        }
        gun.Init(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        playerStateMachine.ChangeState(new MovementState(this, playerStateMachine));
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.UpdateState();
        FlippedMoving();
        playerAnimation.UpdateBlendTree();
        if (Input.GetMouseButton(1))
        {
            gun.Shoot();
        }

        PlayerDodgeRoll();
    }


    public void PlayerMoving()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (movement.magnitude > 0.01f)
        {
            lastMoveDirection = movement.normalized;
        }

        if (!isDodging)
        {
            if (movement.magnitude > 1)
            {
                movement = movement.normalized;
            }
            playerRB.velocity = movement * speed;
        }
    }

    void FlippedMoving()
    {
        if(Input.GetAxis("Horizontal") < 0)
        {
            playerSpriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            playerSpriteRenderer.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void PlayerDodgeRoll()
    {
        Vector2 currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging && currentInput.magnitude > 0.01f)
        {
            isDodging = true;
            dodgeRollDuration = dodgeRollCooldown;
            playerRB.velocity = lastMoveDirection * dodgeRollSpeed;
        }

        if (isDodging)
        {
            dodgeRollDuration -= Time.deltaTime;
            playerRB.velocity = lastMoveDirection * dodgeRollSpeed;

            if (dodgeRollDuration <= 0f)
            {
                isDodging = false;
                playerRB.velocity = Vector2.zero;
            }
        }
    }
}
