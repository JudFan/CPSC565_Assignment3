using UnityEngine;
using UnityEngine.UI;

namespace Antymology.NestUI
{
    public class NumOfNestUI : Singleton<NumOfNestUI>
    {

        public Text output;
        public int nestBlockNum;

        public int nestHighScore;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            nestBlockNum = 0;
            nestHighScore = 0;
            output.text = "Nest blocks: " + nestBlockNum;
        }

        // Update is called once per frame
        void Update()
        {
            output.text = "Nest blocks: " + nestBlockNum;
        }

        public void IncrementNest()
        {
            nestBlockNum++;
            
            
        }

        //Remeber high score on nests made
        public void UpdateHighScore()
        {
            if(nestBlockNum > nestHighScore)
            {
                nestHighScore = nestBlockNum;
            }
        }
    }
}
