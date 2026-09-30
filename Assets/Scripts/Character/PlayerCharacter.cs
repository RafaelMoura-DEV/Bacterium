using System.Collections;
using UnityEngine;

public class PlayerCharacter : Character
{
    public bool isDefending = false;

    public SpriteRenderer characterSpriteRender;
    public Animator characterAnimator;
    public Animator silhouetteAnimator;

    // --------------------------------------------------------- //

    public override void TakeDamage(int damage)
    {
        StartCoroutine(DamageAnimationFeedbackCoroutine(damage));
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);
    }

    protected override void VirtualDie()
    {
        characterAnimator.SetTrigger("Die");
        silhouetteAnimator.gameObject.SetActive(false);
        base.VirtualDie();
    }

    public override void VirtualAttack()
    {
        characterAnimator.SetTrigger("Attack");

        base.VirtualAttack();
    }

    public void VirtualDeffend()
    {
        characterAnimator.SetTrigger("Deffend");
    }

    public override void ResetAnimation()
    {
        if (this.currentHealth <= 0) return;

        characterAnimator.Play("Idle", 0, 0f);
        silhouetteAnimator.Play("Idle", 0, 0f);
        base.ResetAnimation();
    }

    public void CheckHP()
    {
        if (!this.isAlive) VirtualDie();
    }

    // --------------------------------------- CORROTINAS --------------------------------------- //

    IEnumerator DamageAnimationFeedbackCoroutine(int damageAmount) // o certo era usar um trigger event no animator pra ativar a anim de dano so no final da anim de ataque mas fodase por enquanto XD
    {
        yield return new WaitForSeconds(1.0f);

        int finalDamage = 0;
        finalDamage += damageAmount;

        if (isDefending == false)
        {
            silhouetteAnimator.speed = 0.0f;
            characterAnimator.SetTrigger("Damaged");
        }

        else
        {
            StartCoroutine(DefenseFeedbackCoroutine(0.25f, 0.065f, characterAnimator.gameObject.transform));
            finalDamage = Mathf.RoundToInt(finalDamage * (1f - resistence / 100f));
        }

        base.TakeDamage(finalDamage);
    }

    IEnumerator DefenseFeedbackCoroutine(float duration, float intensity, Transform spriteTransform)
    {
        Vector3 originalPosition = spriteTransform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);

            spriteTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        spriteTransform.localPosition = originalPosition;
    }
}
