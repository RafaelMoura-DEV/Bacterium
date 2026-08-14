using UnityEngine;

public class RNGTest : MonoBehaviour
{
    public void AttackFirst()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque + 25%");
                break;

            case 1:
                Debug.Log("Ataque + alvo não pode usar cartas");
                break;

            case 2:
                Debug.Log("Quebra ambas máquinas");
                break;
        }
    }

    public void AttackSecond()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque + 25%");
                break;

            case 1:
                Debug.Log("Ataque + ambos perdem 1 de energia");
                break;

            case 2:
                Debug.Log("Quebra ambas máquinas");
                break;
        }
    }

    public void AttackThird()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque + 25%");
                break;

            case 1:
                Debug.Log("Ataque + ganha 10 de HP");
                break;

            case 2:
                Debug.Log("Quebra ambas máquinas");
                break;
        }
    }

    public void AttackFourth()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque + 25%");
                break;

            case 1:
                Debug.Log("Ataque + ambos não podem defender");
                break;

            case 2:
                Debug.Log("Quebra ambas máquinas");
                break;
        }
    }

    public void AttackFith()
    {
        int rng = Random.Range(0, 3);

        switch (rng)
        {
            case 0:
                Debug.Log("Ataque + 25%");
                break;

            case 1:
                Debug.Log("Ataque + alvo não pode realizar ações");
                break;

            case 2:
                Debug.Log("Quebra ambas máquinas");
                break;
        }
    }
}