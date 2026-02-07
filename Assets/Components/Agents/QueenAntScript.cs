using UnityEngine;
using Antymology.Terrain;

public class QueenAntScript : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private int healthdrop;
    private bool OnAcidicBlock;
    private bool OnContainerBlock;
    private bool OnMulchBlock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 30;
        health = maxHealth;
        healthdrop = 1;
        
        OnAcidicBlock = false;
        OnContainerBlock = false;
        OnMulchBlock = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0) {
            Destroy(gameObject);
        }

        if (OnAcidicBlock) {
            health -= healthdrop * 2;
        }
        else
        {
            health -= healthdrop;
        }
    }

    void dig()
    {
        if(OnMulchBlock)
        {
            health = maxHealth;
            //Can dig
        }
        else if (OnContainerBlock)
        {
           
        }
        else
        {
            //Can dig
        }
        return;
    }

    void makeNestBlock()
    {
        //setBlock()
        health = health * 1/3;
    }
}
