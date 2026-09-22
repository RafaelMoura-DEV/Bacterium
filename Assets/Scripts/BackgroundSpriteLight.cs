using UnityEngine;
using System.Collections;

public class BackgroundSpriteLight : MonoBehaviour
{
    [SerializeField] Sprite lightsOffBackgorund;
    [SerializeField] Sprite lighsOnBackground;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(SwitchBackgoundCoroutine());
    }

    IEnumerator SwitchBackgoundCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3.5f, 7.5f));

            spriteRenderer.sprite = lightsOffBackgorund;

            yield return new WaitForSeconds(0.065f);

            spriteRenderer.sprite = lighsOnBackground;

            yield return new WaitForSeconds(0.095f);

            spriteRenderer.sprite = lightsOffBackgorund;

            yield return new WaitForSeconds(0.075f);

            spriteRenderer.sprite = lighsOnBackground;

            yield return new WaitForSeconds(0.045f);

            spriteRenderer.sprite = lightsOffBackgorund;

            yield return new WaitForSeconds(Random.Range(0.35f, 0.75f));

            spriteRenderer.sprite = lighsOnBackground;
        }
    }
}
