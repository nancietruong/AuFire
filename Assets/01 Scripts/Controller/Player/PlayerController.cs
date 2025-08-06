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
    [SerializeField] GunBase gun;
    SpriteRenderer playerSpriteRenderer;
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
    }


    public void PlayerMoving()
    {
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (movement.magnitude > 1)
        {
            movement = movement.normalized;
        }
        playerRB.velocity = movement * speed;
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
}
