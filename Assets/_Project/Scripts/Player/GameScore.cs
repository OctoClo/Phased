using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameScore
{
   static private uint multiplicator = 1;
   static private uint score = 0;

    static public uint Multiplicator
    {
        set
        {
            multiplicator = (value < 1) ? 1 : value;
        }
        get
        {
            return multiplicator;
        }
    }
    
    static public void AddToScore( uint pointAmount )
    {
        score += (pointAmount * multiplicator);
    }

    static public uint Score
    {
        get
        {
            return score;
        }
    }
}
