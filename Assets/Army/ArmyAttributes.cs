using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Diplomatrix{
    [System.Serializable]
    struct ArmyAttributes 
    {
        public int riffleGuyAmount;
        public int tankAmount;
        
        // other stuff, later.


        public override string ToString(){
            return $"Army:" +
                $"(riffle men: {riffleGuyAmount}, tanks: {tankAmount})";
        }
    }
}