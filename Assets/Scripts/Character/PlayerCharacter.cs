using System.Collections;
using UnityEngine;

public class PlayerCharacter : Character
{
    public bool isDefending = false;

    public SpriteRenderer characterSpriteRender;
    public Animator characterAnimator;
    public Animator silhouetteAnimator;

    public override void TakeDamage(int damage)
    {
        int finalDamage = 0;
        finalDamage += damage;

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

    protected override void VirtualDie()
    {
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
        characterAnimator.Play("Idle", 0, 0f);
        silhouetteAnimator.Play("Idle", 0, 0f);
        base.ResetAnimation();
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

    IEnumerator DamageFeedbackCoroutine() // para o personagem piscar, mas fodase por enquanto XD
    {
        characterSpriteRender.color = Color.gray;
        yield return new WaitForSeconds(0.15f);
        characterSpriteRender.color = Color.white;
    }
}
