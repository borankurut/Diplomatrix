using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diplomatrix{

    [System.Serializable]
    public struct AttackPattern 
    {
        public AttackPattern(float soldierAttackPeriod, float tankAttackPeriod, float airAttackPeriod){
            this.soldierAttackPeriod = soldierAttackPeriod;
            this.tankAttackPeriod = tankAttackPeriod;
            this.airAttackPeriod = airAttackPeriod;
            this.aggressiveness = 1;
            this.aggressivenessDivider = 2;
        }

        private float soldierAttackPeriod; // seconds
        private float tankAttackPeriod;
        private float airAttackPeriod;
        
        // aggressiveness of the enemy
        private int aggressiveness;
        private int aggressivenessDivider;  // controls how much the aggressiveness affects periods.

        
        public float getSoldierAttackPeriod(){
            return soldierAttackPeriod / aggressiveness;
        }
        public float getTankAttackPeriod(){
            return tankAttackPeriod / aggressiveness;
        }
        public float getAirAttackPeriod(){
            return airAttackPeriod / aggressiveness;
        }

        public void setAggressiveness(int aggressiveness){
            this.aggressiveness = Math.Clamp(aggressiveness / aggressivenessDivider, 1, 10);
        }
        public int getAggresiveness(){
            return this.aggressiveness;
        }
    }
}