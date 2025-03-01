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
        private Hand _hand;
        public Hand GetPlayerHand => _hand;
        [SerializeField]
        private Table _table;
        public Table GetTable => _table;
        [SerializeField]
        private Transform _fold;
        public Transform GetFold => _fold;
        [SerializeField]
        private Hero _hero;
        public Hero GetHero => _hero;


        [Space]
        [SerializeField]
        private int _manascore;
        public int GetManaScoe => _manascore;
        [SerializeField]
        private int _step;
        public int GetStep => _step;

        [SerializeField]
        private Text _manaSoreText;

        private Mana _mana;

        public GameObject Shield;
        public bool TauntOnTable;

        private void Awake()
        {
            _deck = GetComponentInChildren<Deck>();
            _hand = GetComponentInChildren<Hand>();
            _table = GetComponentInChildren<Table>();
            _hero = GetComponentInChildren<Hero>();
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

        public void PossibilityCardAttack()
        {
            foreach (CardPosition position in _table.CardPositions)
            {
                Card card = position.GetCard();
                if (card == null)
                    continue;
                card.SwitchCanAttack(true);
            }
        }

        public void TauntPutOnTable(Card cardTaunt, bool alive)
        {
            TauntOnTable = alive;
            if (alive)
            {
                foreach (CardPosition position in _table.CardPositions)
                {
                    Card card = position.GetCard();
                    if (card == null)
                        continue;
                    if (card == cardTaunt |
                        card.propertiesData.Effect == MinionEffects.Taunt)
                        continue;

                    card.SwitchCanAttacked(false);
                }
            }
            else
            {
                List<Card> anotherCard = new List<Card>();
                foreach (CardPosition position in _table.CardPositions)
                {
                    Card card = position.GetCard();                    
                    if (card == null ||
                        card == cardTaunt)
                        continue;
                    if (card.propertiesData.Effect == MinionEffects.Taunt)
                    {
                        TauntOnTable = true;
                        return;
                    }
                    anotherCard.Add(card);                    
                }
                
                foreach (Card card in anotherCard)
                {
                    card.SwitchCanAttacked(true);
                }
            }
        }
        public void RestoreHealth_Hero(int health_count)
        {
            _hero.RestoreHealth(health_count);
            Debug.Log($"Герою было востановлено {health_count} очков здоровья");
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