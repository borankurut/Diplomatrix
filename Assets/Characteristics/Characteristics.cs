using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diplomatrix{
    [System.Serializable]
    public struct Characteristics
    {

        public Characteristics(int anger, int surrenderLikelihood){
            this.anger = anger;
            this.surrenderLikelihood = surrenderLikelihood;
        }

        public int anger;                   // between 0 - 10
        public int surrenderLikelihood;     // between 0 - 10;

        // other stuff later.
        public override string ToString()
        {
            return $"Characteristics (all of the characteristic attributes are between 0-10.): " +
                $"(anger: {anger}, surrenderLikelihood: {surrenderLikelihood})";
        }
    }
}
