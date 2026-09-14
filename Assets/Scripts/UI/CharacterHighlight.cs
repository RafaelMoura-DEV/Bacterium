using UnityEngine;

public class CharacterHighlight : MonoBehaviour
{
    BossManager bossManager;
    HUDController hudController;

    [SerializeField] private SpriteRenderer playerSilhouette;
    [SerializeField] private SpriteRenderer[] bossSilhouettes;
    private SpriteRenderer currentBossSilhouette;

    void Start()
    {
        bossManager = FindAnyObjectByType<BossManager>();     
        hudController = FindAnyObjectByType<HUDController>();
    }

    public void ActivateHighlight()
    {
        if (this.gameObject.tag == "ButtonPlayer")
        {
            playerSilhouette.color = new Vector4(playerSilhouette.color.r, playerSilhouette.color.g, playerSilhouette.color.b, 0.1f);
        }

        else
        {
            if (currentBossSilhouette == null) currentBossSilhouette = bossSilhouettes[bossManager.currentBossIndex];
            currentBossSilhouette.color = new Vector4(currentBossSilhouette.color.r, currentBossSilhouette.color.g, currentBossSilhouette.color.b, 0.1f);
        }

        hudController.UpdatePanelInfo(GetComponent<Character>());
    }

    public void DeactivateHighlight()
    {
        if (this.gameObject.tag == "ButtonPlayer")
        {
            playerSilhouette.color = new Vector4(playerSilhouette.color.r, playerSilhouette.color.g, playerSilhouette.color.b, 0.0f);
        }

        else
        {
            if (currentBossSilhouette == null) currentBossSilhouette = bossSilhouettes[bossManager.currentBossIndex];
            currentBossSilhouette.color = new Vector4(currentBossSilhouette.color.r, currentBossSilhouette.color.g, currentBossSilhouette.color.b, 0.0f);
        }
    }
}
