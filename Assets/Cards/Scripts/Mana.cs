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

        public int UsingMana(Card card, int _manascore, out bool done)
        {
            ushort cost = card.propertiesData.Cost;
            if (_manascore < cost)
            {
                Debug.Log($"Вам не хватает {_manascore - cost} маны для разыгровки этой карты");
                done = false;
                return _manascore;
                
            }

            _manascore = _manascore - cost;
            done = true;
            return _manascore;
        }

    }
}
