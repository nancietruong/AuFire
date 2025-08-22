using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float timeToDrain = 0.25f;
    [SerializeField] Gradient healthGradient;
    Image image;
    Color newHealthBarColor;
    [SerializeField] TextMeshProUGUI healthText;

    Coroutine drainCoroutine;
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.color = healthGradient.Evaluate(1f);
        SetGradientHealthBarColor(1f);
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        drainCoroutine = StartCoroutine(DrainHealthBar(currentHealth / maxHealth));
        SetGradientHealthBarColor(currentHealth / maxHealth);

        healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    IEnumerator DrainHealthBar(float targetFill)
    {
        float startFill = image.fillAmount;
        Color currentColor = image.color;

        float elapsedTime = 0f;
        while (elapsedTime < timeToDrain)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime / timeToDrain);

            image.color = Color.Lerp(currentColor, newHealthBarColor, elapsedTime / timeToDrain);
            yield return null;
        }
        image.fillAmount = targetFill;
    }

    public void SetGradientHealthBarColor(float targetFill)
    {
        newHealthBarColor = healthGradient.Evaluate(targetFill);
    }
}
