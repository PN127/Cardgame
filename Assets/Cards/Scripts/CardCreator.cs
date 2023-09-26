using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardCreator : MonoBehaviour
    {
        [SerializeField]
        private Card _cardPrefab;

        public Card CreaterCard(Vector3 spawnPosition)
        {
            return Instantiate(_cardPrefab, spawnPosition, new Quaternion(0, 0, 180, 0));
        }

        
    }
}