using System;
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
    SpriteRenderer playerSpriteRenderer;
    [Header("Gun Settings")]
    public GunBase gun;

    [Header("Inventory Settings")]
    public InventoryManager inventoryManager;

    [Header("Dodge Roll Settings")]
    [SerializeField] float dodgeRollSpeed;
    public float dodgeRollDuration;
    public float dodgeRollCooldown;
    [SerializeField] public bool isDodging = false;

    [Header("Weapon Hierarchy")]
    [SerializeField] private Transform gunsParent;

    private Vector2 lastMoveDirection = Vector2.right;
    public void Init()
    {
        if (gun == null && gunsParent != null)
        {
            gun = gunsParent.GetComponentInChildren<GunBase>();
        }
        if (gun != null)
        {
            gun.Init(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();

        playerStateMachine.ChangeState(new MovementState(this, playerStateMachine));

        inventoryManager.OnSelectedItemChanged += OnInventoryItemSelected;
    }

    private void OnInventoryItemSelected(Item selectedItem)
    {
        if (selectedItem == null || selectedItem.itemType != Item.ItemType.Weapon)
        {
            foreach (var weapon in gunsParent.GetComponentsInChildren<GunBase>(true))
            {
                weapon.gameObject.SetActive(false);
            }
            gun = null;
            return;
        }

        GunBase[] allGuns = gunsParent.GetComponentsInChildren<GunBase>(true);
        foreach (var weapon in allGuns)
        {
            if (weapon.item == selectedItem)
            {
                weapon.gameObject.SetActive(true);
                gun = weapon;
                gun.Init(this);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerStateMachine.UpdateState();
        FlippedMoving();
        playerAnimation.UpdateBlendTree();
        if (Input.GetMouseButton(0))
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
