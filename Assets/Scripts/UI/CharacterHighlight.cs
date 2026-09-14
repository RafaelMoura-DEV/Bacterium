using UnityEngine;

public class CharacterHighlight : MonoBehaviour
{
    BossManager bossManager;

    [SerializeField] private SpriteRenderer playerSilhouette;
    [SerializeField] private SpriteRenderer[] bossSilhouettes;
    private SpriteRenderer currentBossSilhouette;

    void Start()
    {
        bossManager = FindAnyObjectByType<BossManager>();     
    }

    public void ActivateHighlight()
    {
        if (this.gameObject.tag == "ButtonPlayer") playerSilhouette.color = new Vector4(playerSilhouette.color.r, playerSilhouette.color.g, playerSilhouette.color.b, 0.1f);
        else
        {
            if (currentBossSilhouette == null) currentBossSilhouette = bossSilhouettes[bossManager.currentBossIndex];
            currentBossSilhouette.color = new Vector4(currentBossSilhouette.color.r, currentBossSilhouette.color.g, currentBossSilhouette.color.b, 0.1f);
        }
    }

    public void DeactivateHighlight()
    {
        if (this.gameObject.tag == "ButtonPlayer") playerSilhouette.color = new Vector4(playerSilhouette.color.r, playerSilhouette.color.g, playerSilhouette.color.b, 0.0f);
        else
        {
            if (currentBossSilhouette == null) currentBossSilhouette = bossSilhouettes[bossManager.currentBossIndex];
            currentBossSilhouette.color = new Vector4(currentBossSilhouette.color.r, currentBossSilhouette.color.g, currentBossSilhouette.color.b, 0.0f);
        }
    }
}
