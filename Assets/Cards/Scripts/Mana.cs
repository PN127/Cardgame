using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Mana : MonoBehaviour
    {

        public int Scoring(Player player, int score = 0)
        {
            if (player.GetStep < 2) score = 1;
            else if (player.GetStep < 4) score = 2;
            else if (player.GetStep < 6) score = 3;

            return score;
        }

    }
}
