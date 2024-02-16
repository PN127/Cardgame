using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Deck _deck;
        public Deck GetDeck => _deck;
        [SerializeField]
        private PlayerHand _hand;
        public PlayerHand GetPlayerHand => _hand;

        private void Awake()
        {
            _deck = GetComponentInChildren<Deck>();
            _hand = GetComponentInChildren<PlayerHand>();
        }

        public void FlipCards()
        {
            foreach (CardPosition position in _hand.CardPositions)
            {
                Card card = position.GetCard();
                if (card == null)
                    return;
                card.Twist_method();
            }

        }
        
    }
}