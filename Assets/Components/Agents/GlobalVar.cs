using UnityEngine;


namespace Antymology.GlobalVars {
    public class GlobalVar : Singleton<GlobalVar>
    {
        public Vector3 queenLocation;
        public bool firstGen = true;

        //Adjustible: from 0.0 to 1.0
        public float healthPercentMulchHeals;

        //Adjustible
        public float antLearningRate;

        // Variables to transfer from 1 generation of ants to the next
        public int minHealthForNest;

        public int minHealthForTransfer;
        public int minHealthToTransfer;

        public float charityModifier;

    }
}
