using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cards
{
    public class CardPosition : MonoBehaviour, IPointerClickHandler
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
            CardInPosition.SetPosition(this);
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

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_storageType)
            {
                case (StorageType.Deck):
                    var deck = transform.parent.gameObject;
                    deck.GetComponent<Deck>().OnPointerClick(eventData);
                    break;
            }
        }
    }
}