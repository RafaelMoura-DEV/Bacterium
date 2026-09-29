using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingController : MonoBehaviour
{
    BattleManager battleManager;

    private Volume globalVolume;
    private Vignette vignette;

    private void Awake()
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        globalVolume = GetComponent<Volume>();
    }

    private void Start()
    {
        globalVolume.profile.TryGet(out vignette);
    }

    private void OnEnable()
    {
        battleManager.OnGameOver += SetGameOverPostProcessing;
    }

    private void OnDisable()
    {
        battleManager.OnGameOver -= SetGameOverPostProcessing;
    }

    void SetGameOverPostProcessing(BattleManager.GameOverType gameOverType)
    {
        if (gameOverType == BattleManager.GameOverType.Defeat)
        {
            vignette.color.value = Color.red;
            vignette.intensity.value = 1.0f;
        }     
    }
}
