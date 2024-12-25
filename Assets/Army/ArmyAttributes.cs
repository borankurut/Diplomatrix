using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Diplomatrix{
    [System.Serializable]
    public struct ArmyAttributes 
    {
        public int soldierAmount;
        public int tankAmount;
        
        // other stuff, later.
        
        public override string ToString(){
            return $"(riffle men: {soldierAmount}, tanks: {tankAmount})";
        }
    }
}