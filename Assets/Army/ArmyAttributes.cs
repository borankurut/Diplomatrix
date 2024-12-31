using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Diplomatrix{
    [System.Serializable]
    public struct ArmyAttributes 
    {
        public ArmyAttributes(int soldierAmount, int tankAmount){
            this.soldierAmount = soldierAmount;
            this.tankAmount = tankAmount;
        }
        public int soldierAmount;
        public int tankAmount;
        
        // other stuff, later.
        public override string ToString(){
            return $"(riffle men: {soldierAmount}, tanks: {tankAmount})";
        }
    }
}