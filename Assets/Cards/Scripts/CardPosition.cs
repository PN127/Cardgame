using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardPosition : MonoBehaviour
    {
        [SerializeField]
        private StorageType _storageType;

        public StorageType StorageType => _storageType;
        public Card CardInPosition { get; private set; } = null;

        public Vector3 GetCardPosition { get; private set; }

        public void SetCard(Card card)
        {
            CardInPosition = card;
            CardInPosition.transform.position = GetCardPosition;
            card.CurrentPosition = this;
        }
        public Card GetCard()
        {
            return CardInPosition;
        }

        public void Clear()
        {
            CardInPosition = null;
        }

        private void Awake()
        {
            GetCardPosition = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        }
    }
}