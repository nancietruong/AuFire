using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTintColor : MonoBehaviour
{
    [SerializeField] Material material;
    Color materialColor;
    float tintFadeSpeed;

    private void Awake()
    {
        material = GetComponentInChildren<SpriteRenderer>().material;
    }

    private void Start()
    {
        materialColor = new Color(1, 0, 0, 0);
        tintFadeSpeed = 6f;
    }

    private void Update()
    {
        if (materialColor.a > 0)
        {
            materialColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialColor);
        }
    }

    public void SetMaterial(Material material)
    {
        this.material = material;
    }

    public void SetTintColor(Color color)
    {
        materialColor = color;
        material.SetColor("_Tint", materialColor);
    }

    public void SetTintFadeSpeed(float speed)
    {
        tintFadeSpeed = speed;
    }
}
