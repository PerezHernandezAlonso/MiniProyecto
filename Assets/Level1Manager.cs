using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [Header("Puertas")]
    public Door[] Puertas;

    [Header("Targets")]
    public int ActiveTargets;

    [Header("Traps")]
    public int ActiveTraps;

    public void EnableFirstDoor()
    {
        Puertas[0].canOpen = true;
    }

    public void EnableSecondDoor()
    {
        Puertas[1].canOpen = true;
    }
}
