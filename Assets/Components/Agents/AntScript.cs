using UnityEngine;
using Antymology.Terrain;

public class AntScript : MonoBehaviour
{
    private int health;
    private int maxHealth;
    private int healthdrop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 3000;
        health = maxHealth;
        healthdrop = 1;

        transform.Rotate(-90.0f, 0.0f, 0.0f, Space.Self);

        while(true) {
            AbstractBlock block = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
            if(block is not AirBlock)
            {
                break;
            }
            transform.position = transform.position - new Vector3(0, 1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0) {
            Destroy(gameObject);
        }
        AbstractBlock currentBlock = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
        if (currentBlock is AcidicBlock) {
            health -= healthdrop * 2;
        }
        else
        {
            health -= healthdrop;
        }
        Move();

        //Dig();
        //GiveHealth();

        //If ant finds itself in the air, fall down
        while(transform.position.y >= 1) {
            AbstractBlock block = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
            if(block is not AirBlock)
            {
                break;
            }
            transform.position = transform.position - new Vector3(0, 1, 0);
        }
    }

    void Move()
    {
        //The blocks ahead of the ant (to determine if it can climb)
        AbstractBlock blockAhead;
        AbstractBlock blockAhead2;
        AbstractBlock blockAhead3;
        AbstractBlock blockAhead4;

        int random = UnityEngine.Random.Range(1, 5);
        switch (random) {
            case 1:
                blockAhead = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y, (int)transform.position.z + 1);
                blockAhead2 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y + 1, (int)transform.position.z + 1);
                blockAhead3 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z + 1);
                blockAhead4 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 2, (int)transform.position.z + 1);
                transform.rotation = Quaternion.Euler(-90,0,0);
                break;
            case 2:
                blockAhead = WorldManager.Instance.GetBlock((int)transform.position.x - 1, (int)transform.position.y, (int)transform.position.z);
                blockAhead2 = WorldManager.Instance.GetBlock((int)transform.position.x - 1, (int)transform.position.y + 1, (int)transform.position.z);
                blockAhead3 = WorldManager.Instance.GetBlock((int)transform.position.x - 1, (int)transform.position.y - 1, (int)transform.position.z);
                blockAhead4 = WorldManager.Instance.GetBlock((int)transform.position.x - 1, (int)transform.position.y - 2, (int)transform.position.z);
                transform.rotation = Quaternion.Euler(-90,-90,0);
                break;
            case 3:
                blockAhead = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y, (int)transform.position.z - 1);
                blockAhead2 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y + 1, (int)transform.position.z - 1);
                blockAhead3 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z - 1);
                blockAhead4 = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 2, (int)transform.position.z - 1);
                transform.rotation = Quaternion.Euler(-90,180,0);
                break;
            case 4:
                blockAhead = WorldManager.Instance.GetBlock((int)transform.position.x + 1, (int)transform.position.y, (int)transform.position.z);
                blockAhead2 = WorldManager.Instance.GetBlock((int)transform.position.x + 1, (int)transform.position.y + 1, (int)transform.position.z);
                blockAhead3 = WorldManager.Instance.GetBlock((int)transform.position.x + 1, (int)transform.position.y - 1, (int)transform.position.z);
                blockAhead4 = WorldManager.Instance.GetBlock((int)transform.position.x + 1, (int)transform.position.y - 2, (int)transform.position.z);
                transform.rotation = Quaternion.Euler(-90,90,0);
                break;
            default:
                blockAhead = null;
                blockAhead2 = null;
                blockAhead3 = null;
                blockAhead4 = null;
                break;

        }
        
        if(blockAhead2 is not AirBlock || (blockAhead3 is AirBlock && blockAhead4 is AirBlock))
        {
            //Cannot climb
            //Debug.Log("Climb is too steep");
            return;
        }
        if(blockAhead is not AirBlock)
        {
            //To simulate climbing
            switch (random) {
            case 1:
                transform.position += new Vector3(0, 1, 1);
                break;
            case 2:
                transform.position += new Vector3(-1, 1, 0);
                break;
            case 3:
                transform.position += new Vector3(0, 1, -1);
                break;
            case 4:
                transform.position += new Vector3(1, 1, 0);
                break;
            }
        }
        else
        {
            switch (random) {
            case 1:
                transform.position += new Vector3(0, 0, 1);
                break;
            case 2:
                transform.position += new Vector3(-1, 0, 0);
                break;
            case 3:
                transform.position += new Vector3(0, 0, -1);
                break;
            case 4:
                transform.position += new Vector3(1, 0, 0);
                break;
            }
        }
    }


    void GiveHealth()
    {
         
    }

    void OnTriggerEnter(Collider other)
        {
            // Log a message to the console
            Debug.Log("Trigger Entered by: " + other.name);

            if (other.CompareTag("Ant"))
            {
                Debug.Log("Ant met an ant at " + transform.position);
            }
        }

    void Dig()
    {
        AbstractBlock digblock = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
        AirBlock dugUpSite = new AirBlock();

        if(digblock is MulchBlock)
        {
            health = maxHealth;
            Debug.Log("Health Refilled!");
            //Can dig
        }
        else if (digblock is ContainerBlock)
        {
            //End method prematurely to stop digging
            return;
        }

        if(transform.position.y >= 1) {
            WorldManager.Instance.SetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z, dugUpSite);
            transform.position = transform.position - new Vector3(0, 1, 0);
        }
        //Can dig
        return;
    }
}
