using UnityEngine;

public static class PlayerRegistry
{
    public static Transform Player { get; private set; }

    public static void Register(Transform player)
    {
        Player = player;
    }

    public static void Unregister()
    {
        Player = null;
    }
}
