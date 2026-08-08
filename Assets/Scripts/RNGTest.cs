using UnityEngine;

public class RNGTest : MonoBehaviour
{
    public void AttackFirst()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque normal");
                break;

            case 1:
                Debug.Log("Ataque metade + alvo não pode usar cartas");
                break;

            case 2:
                Debug.Log("Ataque metade + alvo perde metade da força");
                break;
        }
    }

    public void AttackSecond()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque normal");
                break;

            case 1:
                Debug.Log("Ataque metade + alvo não pode atacar");
                break;

            case 2:
                Debug.Log("Ataque metade + alvo perde 1 de energia");
                break;
        }
    }

    public void AttackThird()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque normal");
                break;

            case 1:
                Debug.Log("Ataque metade + ambos não podem defender");
                break;

            case 2:
                Debug.Log("Ataque metade + ganha 10 de HP");
                break;
        }
    }
}
