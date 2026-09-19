using UnityEngine;
using System.Collections;
using TMPro;

public class HUDController : MonoBehaviour
{
    [SerializeField] RectTransform infoPanelTransform;
    private Coroutine currentMovementCoroutine;
    private TextMeshProUGUI[] infoPanelTexts;

    private void Start()
    {
        infoPanelTexts = new TextMeshProUGUI[infoPanelTransform.childCount];

        for (int i = 0; i < infoPanelTransform.childCount; i++)
        {
            infoPanelTexts[i] = infoPanelTransform.GetChild(i).GetComponent<TextMeshProUGUI>();
        }
    }

    public void ToggleInfoPanel(float speed)
    {
        // Interrompe qualquer movimento anterior
        if (currentMovementCoroutine != null)
        {
            StopCoroutine(currentMovementCoroutine);
        }

        currentMovementCoroutine = StartCoroutine(MovePanelCoroutine(speed));
    }

    private IEnumerator MovePanelCoroutine(float speed)
    {
        float targetX;

        if (speed > 0)
        {
            targetX = 165f;
            //UpdatePanelInfo();
        }
        else if (speed < 0)
        {
            targetX = -165f;
        }
        else
        {
            yield break;
        }

        while (Mathf.Abs(infoPanelTransform.anchoredPosition.x - targetX) > 0.01f)
        {
            Vector2 position = infoPanelTransform.anchoredPosition;

            position.x = Mathf.MoveTowards(position.x, targetX, Mathf.Abs(speed) * Time.deltaTime);

            infoPanelTransform.anchoredPosition = position;

            yield return null;
        }

        // Garante que termina exatamente no destino
        Vector2 finalPosition = infoPanelTransform.anchoredPosition;
        finalPosition.x = targetX;
        infoPanelTransform.anchoredPosition = finalPosition;

        currentMovementCoroutine = null;
    }

    public void UpdatePanelInfo(Character character)
    {
        infoPanelTexts[0].text = character.characterName;
        infoPanelTexts[1].text = "VIDA: " + character.currentHealth.ToString() + "/" + character.maxHealth.ToString();
        infoPanelTexts[2].text = "FORÇA: " + character.damage.ToString();
        infoPanelTexts[3].text = "RESISTÊNCIA: " + character.resistence + "%";
    }
}
