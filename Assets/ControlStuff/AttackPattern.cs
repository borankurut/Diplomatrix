using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diplomatrix{
    [System.Serializable]
    public struct AttackPattern 
    {
        public AttackPattern(int airAttackPeriod, int soldierAttackPeriod, int tankAttackPeriod, int soldierPerSoldierAttack, int tankPerTankAttack){
            this.airAttackPeriod = airAttackPeriod;
            this.soldierAttackPeriod = soldierAttackPeriod;
            this.tankAttackPeriod = tankAttackPeriod;
            this.soldierPerSoldierAttack = soldierPerSoldierAttack; 
            this.tankPerTankAttack = tankPerTankAttack;
        }


        public int airAttackPeriod;     // seconds
        public int soldierAttackPeriod;
        public int tankAttackPeriod;
        public int soldierPerSoldierAttack;
        public int tankPerTankAttack;
    }
}

