using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField]
        private int _manascore;
        public int GetManaScoe => _manascore;
        [SerializeField]
        private int _step;
        public int GetStep => _step;

        [SerializeField]
        private Text _manaSoreText;

        private Mana _mana;

        private void Awake()
        {
            _deck = GetComponentInChildren<Deck>();
            _hand = GetComponentInChildren<PlayerHand>();
            _mana = new Mana();
        }

        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            _manaSoreText.text = _manascore.ToString();
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

        public void AddManaScore()
        {
            _step++;
            _manaSoreText.gameObject.SetActive(true);
            _manascore += _mana.Scoring(this);
            
        }

        public bool UsingCard(Card card)
        {
            _manascore = _mana.UsingMana(card, _manascore, out bool done);
            return done;
        }
                

        public void EndOfTurn()
        {
            _deck.EndOfTurn();
            _manaSoreText.gameObject.SetActive(false);
            
        }
        
    }
}