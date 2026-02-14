using UnityEngine;
using Antymology.Terrain;
using Antymology.NestUI;
using Antymology.GlobalVars;
using System.Collections.Generic;

public class QueenAntScript : MonoBehaviour
{
    public int health;
    public int maxHealth;
    private int healthdrop;
    private bool noAntNearby;
    private List<AntScript> otherAnts;
    private int prevMoveResult;
    bool prevDigResult;

    //Rule Set Variables
    private int minHealthForNest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = 3000;
        health = maxHealth;
        healthdrop = 1;
        noAntNearby = true;
        otherAnts = new List<AntScript>();
        prevMoveResult = -1;
        prevDigResult = true;


        InitialiseRuleVars();

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

    // RuleSet Variables initialised in first Generation of ants. They are modified and carried over new generations
    void InitialiseRuleVars()
    {

        if(GlobalVar.Instance.firstGen)
        {
            // Set up variables for the rules
            minHealthForNest = 1/3 * maxHealth;
            GlobalVar.Instance.minHealthForNest = minHealthForNest;
        }
        else
        {
            minHealthForNest = GlobalVar.Instance.minHealthForNest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GlobalVar.Instance.queenLocation = transform.position;
        if (health <= 0) {
            NumOfNestUI.Instance.UpdateHighScore();
            ModifyRuleSetVar();
            WorldManager.Instance.KillOldAntsSpawnNew();
            NumOfNestUI.Instance.nestBlockNum = 0;
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

        RuleMakeNest();

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

    void ModifyRuleSetVar()
    {
        Debug.Log("Queen has modified her rules.");
        int modifier = 0;

        if(NumOfNestUI.Instance.nestHighScore >= NumOfNestUI.Instance.nestBlockNum)
        {
            int difference = NumOfNestUI.Instance.nestHighScore - NumOfNestUI.Instance.nestBlockNum;
            modifier = (int)((1 + difference) * GlobalVar.Instance.antLearningRate); 
            Debug.Log("Modifier in percent: " + modifier);
        }
        
        if(minHealthForNest < maxHealth)
        {
            minHealthForNest += modifier/100 * maxHealth;
            GlobalVar.Instance.minHealthForNest = minHealthForNest;
        }
        else
        {
            minHealthForNest -= modifier/100 * maxHealth;
            GlobalVar.Instance.minHealthForNest = minHealthForNest;
        }
    }

    void Explore()
    {
        Move(UnityEngine.Random.Range(1, 5));
        AbstractBlock block = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
        if(block is MulchBlock)
        {
            Dig();
        }
    }

    void MoveToCoordinate(int x, int y, int z)
    {
        int xDiff = x - (int)transform.position.x;
        int yDiff = y - (int)transform.position.y;
        int zDiff = z - (int)transform.position.z;
        
        int chosenDirection = 0;

        //To prevent the ant from being stuck, randomly prioritise to go via x or z axis first to get closer
        int random = UnityEngine.Random.Range(1, 3);
        if(random == 1) {
            if(xDiff > 0)
            {
                chosenDirection = 4;
            }
            else if(xDiff < 0)
            {
                chosenDirection = 2;
            }
            else if(zDiff > 0)
            {
                chosenDirection = 1;
            }
            else if(zDiff < 0)
            {
                chosenDirection = 3;
            }
        }
        else
        {
            if (zDiff > 0)
            {
                chosenDirection = 1;
            }
            else if(zDiff < 0)
            {
                chosenDirection = 3;
            }
            else if(xDiff > 0)
            {
                chosenDirection = 4;
            }
            else if(xDiff < 0)
            {
                chosenDirection = 2;
            }
        }

        // prevDigResult is true by default, only if a later dig fails then the ant must try something else
        if(prevDigResult) {
            prevMoveResult = Move(chosenDirection);
        }
        else
        {
            //If dig fails, try sidestepping
            prevMoveResult = 1;
        }
        
        //If Climb is too high, move to either side
        if(prevMoveResult == 1)
        {
            random = UnityEngine.Random.Range(1, 3);
            if (chosenDirection == 2 || chosenDirection == 4)
            {
                if(random == 1) {
                    prevMoveResult = Move(1);
                }
                else if(random == 2)
                {
                    prevMoveResult = Move(3);
                }
            }
            else
            {
                if(random == 1) {
                    prevMoveResult = Move(2);
                }
                else if(random == 2)
                {
                    prevMoveResult = Move(4);
                }
            }
        }
        //If the drop is too deep, dig
        else if (prevMoveResult == 2)
        {
            prevDigResult = Dig();
        }
    }

    //This function returns an int code:
    // 0 = Move successful
    // 1 = Climb is too high (the blocks in front is taller than the ant by 2 units or more)
    // 2 = Descent is too deep (the blocks in front is deeper than the ant's current Y coordinate by 2 units or more)
    int Move(int moveIndex)
    {
        //The blocks ahead of the ant (to determine if it can climb)
        AbstractBlock blockAhead;
        AbstractBlock blockAhead2;
        AbstractBlock blockAhead3;
        AbstractBlock blockAhead4;

        //int random = UnityEngine.Random.Range(1, 5);
        switch (moveIndex) {
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
        
        if(blockAhead2 is not AirBlock)
        {
            //Cannot climb, too high
            //Debug.Log("Climb is too steep");
            return 1;
        }

        if(blockAhead3 is AirBlock && blockAhead4 is AirBlock)
        {
            //Steep drop
            return 2;
        }

        if(blockAhead is not AirBlock)
        {
            //To simulate climbing
            switch (moveIndex) {
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
            switch (moveIndex) {
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
        return 0;
    }

    void OnTriggerEnter(Collider other)
    {
        // Log a message to the console
        Debug.Log("Trigger Entered by: " + other.name);
        noAntNearby = false;

        if (other.CompareTag("Ant"))
        {
            AntScript otherAnt =  other.gameObject.GetComponent<AntScript>();
            otherAnts.Add(otherAnt);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Log a message to the console
        Debug.Log("Ant that left: " + other.name);
        AntScript otherAnt =  other.gameObject.GetComponent<AntScript>();
        otherAnts.Remove(otherAnt);
        if(otherAnts.Count == 0) {
            noAntNearby = true;
        }
    }

    bool Dig()
    {
        AbstractBlock digblock = WorldManager.Instance.GetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z);
        AirBlock dugUpSite = new AirBlock();

        
        if(digblock is MulchBlock && !noAntNearby)
        {
            //Cannot dig
            Debug.Log("Ant blocking the way!");
            return false;
        }
        else if (digblock is ContainerBlock)
        {
            //End method prematurely to stop digging
            return false;
        }

        if(transform.position.y >= 1) {
            WorldManager.Instance.SetBlock((int)transform.position.x, (int)transform.position.y - 1, (int)transform.position.z, dugUpSite);
            transform.position = transform.position - new Vector3(0, 1, 0);
            if(digblock is MulchBlock)
            {
                health = (int)(maxHealth * GlobalVar.Instance.healthPercentMulchHeals);
                Debug.Log("Queen Health Refilled!");
                //Can dig
            }
        }
        //Can dig
        return true;
    }

    void MakeNestBlock()
    {
        NestBlock nest = new NestBlock();
        WorldManager.Instance.SetBlock((int)transform.position.x, (int)transform.position.y, (int)transform.position.z, nest);
        Debug.Log("Hello: Nest Block at X: " + transform.position.x + ", Y: " + transform.position.y + ", Z: " + transform.position.z);
        NumOfNestUI.Instance.IncrementNest();

        health -= maxHealth * 1/3;
        Vector3 offset = new Vector3(0, 1, 0);
        transform.position = transform.position + offset;
    }

    //RULES BELOW
    void RuleMakeNest() {
        Debug.Log("Queen has " + health + " health, and she needs " + minHealthForNest + " to lay a nest");
        if(health > minHealthForNest) {
            MakeNestBlock();
            //To climb down nest block so it won't get trapped by building a nest block tower and not being able to climb down.
            int direction = 1;
            int moveResult = Move(direction);
            while(moveResult != 0 && direction < 4)
            {
                direction++;
                moveResult = Move(direction);
            }
        }
        else
        {
            Explore();
        }
    }
}
