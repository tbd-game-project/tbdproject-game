using UnityEngine;

public class Stone : MonoBehaviour
{

    public Player owner { get; private set; }

    public void SetOwner(Player player)
    {
        owner = player;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
