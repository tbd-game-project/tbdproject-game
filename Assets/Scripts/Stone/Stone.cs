using UnityEngine;

public class Stone : MonoBehaviour
{

    public Player owner { get; private set; }

    public void SetOwner(Player player)
    {
        owner = player;
    }

}
