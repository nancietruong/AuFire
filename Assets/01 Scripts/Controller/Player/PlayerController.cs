using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, ITakeDamage
{
    [Header("Player Settings")]
    public Rigidbody2D playerRB;
    [SerializeField] float speed;
    public PlayerAnimation playerAnimation;
    [SerializeField] MaterialTintColor materialTintColor;

    [Header("Player Health")]
    public float health;
    public float maxHealth = 100;
    [SerializeField] PlayerHealth playerHealthUI;
    public float potionHealAmount = 20f;

    public StateMachine<PlayerController> playerStateMachine;
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

    MovementState movementState;

    private Vector2 lastMoveDirection = Vector2.right;

    public event Action OnPlayerDeath;

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

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        materialTintColor = GetComponent<MaterialTintColor>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();
        playerStateMachine = new StateMachine<PlayerController>(this);
        movementState = new MovementState();

        playerStateMachine.ChangeState(movementState);

        inventoryManager.OnSelectedItemChanged += OnInventoryItemSelected;
        GameManager.OnGameReset += ResetHealth;
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
                // Add cooldown when switching to this gun
                var gunType = gun.GetType();
                var timerField = gunType.GetField("timer", System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance);
                var fireCooldownField = gunType.GetField("fireCooldown", System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Instance | 
                    System.Reflection.BindingFlags.FlattenHierarchy);
                if (timerField != null && fireCooldownField != null)
                {
                    timerField.SetValue(gun, fireCooldownField.GetValue(gun));
                }
                else
                {
                    // fallback for protected fields
                    gun.GetType().BaseType.GetField("timer", System.Reflection.BindingFlags.NonPublic | 
                        System.Reflection.BindingFlags.Instance)
                        ?.SetValue(gun, gunType.GetField("fireCooldown", System.Reflection.BindingFlags.NonPublic
                        | System.Reflection.BindingFlags.Instance | 
                        System.Reflection.BindingFlags.FlattenHierarchy)?.GetValue(gun));
                }
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
            if (gun != null && gun.item != null && gun.item.itemType == Item.ItemType.Weapon)
            {
                gun.Shoot();
            }
        }

        PlayerDodgeRoll();

        if (Input.GetKeyDown(KeyCode.H))
        {
            Item selectedItem = inventoryManager.GetSelectedItem(false);
            if (selectedItem != null && selectedItem.isConsumable && selectedItem.onUseItem == Item.ActionType.Heal)
            {
                
                inventoryManager.GetSelectedItem(true);
                Heal(potionHealAmount);
            }
        }
    }

    void ResetHealth()
    {
        health = maxHealth;
        playerHealthUI.UpdateHealthBar(health, maxHealth);
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        materialTintColor.SetTintColor(new Color(1, 0, 0, 1));
        playerHealthUI.UpdateHealthBar(health, maxHealth);
        AudioManager.PlaySound(TypeOfSoundEffect.Hurt);
        if (health <= 0)
        {
            OnPlayerDeath?.Invoke();
            AudioManager.PlaySound(TypeOfSoundEffect.Death);
        }
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        materialTintColor.SetTintColor(new Color(0, 1, 0, 1));
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        playerHealthUI.UpdateHealthBar(health, maxHealth);
    }
}
