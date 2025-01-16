using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Diplomatrix{
    [System.Serializable]
    public struct ArmyAttributes 
    {
        public ArmyAttributes(int soldierAmount, int tankAmount, int airStrikeAmount){
            this.soldierAmount = soldierAmount;
            this.tankAmount = tankAmount;
            this.airStrikeAmount = airStrikeAmount;
        }
        public int soldierAmount;
        public int tankAmount;
        public int airStrikeAmount;
        public override string ToString(){
            return $"(riffle men: {soldierAmount}, tanks: {tankAmount}, airstrikes: {airStrikeAmount})";
        }
    }
}