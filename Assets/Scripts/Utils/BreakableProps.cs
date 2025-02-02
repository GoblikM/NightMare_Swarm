using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float health;
    private float damageFlashDuration = 0.1f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(DamageFlashProp());
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageFlashProp()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = Color.white;
    }

}
