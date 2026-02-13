using UnityEngine;


namespace Antymology.GlobalVars {
    public class GlobalVar : Singleton<GlobalVar>
    {
        public Vector3 queenLocation;
        public bool firstGen = true;
    }
}
