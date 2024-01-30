using OneLine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Cards
{
    [RequireComponent(typeof(CardCreator))]
    public class Deck : MonoBehaviour, IPointerClickHandler
    {
        private CardCreator _cardCreator;
        private PlayerHand _playerHand;
        private StartingHand _startingHand;
        private bool _startHandSelection;

        //public bool StarHandSelection => _startHandSelection;

        [SerializeField]
        private CardPackConfiguration[] cardPacks; //to do
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<CardPropertiesData> _playersDeck;

        private List<Card> _cardsInStartingHand;
        

        [SerializeField]
        [Header("Укажите id карт, которые будут в колоде")]
        private List<uint> _idCardsForDeck;

        private void Awake()
        {
            _cardCreator = GetComponent<CardCreator>();
            _playerHand = FindObjectOfType<PlayerHand>();
            _startingHand = FindObjectOfType<StartingHand>();
            _playersDeck = new List<CardPropertiesData>();
            _startHandSelection = false;
            _cardsInStartingHand = new List<Card>();
        }

        private void Start()
        {
            CreateDeck();
        }
       
        private void CreateDeck()
        {
            var _copyId = new List<uint>(_idCardsForDeck);
            
            foreach (CardPackConfiguration pack in cardPacks)
            {
                var array = new List<CardPropertiesData>();
                array = pack.UnionProperties(array).ToList();

                foreach (CardPropertiesData property in array)
                {
                    while (_copyId.Contains<uint>(property.Id))
                    {
                        _playersDeck.Add(property);
                        _copyId.Remove(property.Id);
                    }
                }
            }
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_startHandSelection)
                return;

            _startingHand.ActiveStartHand(true);
            DistributeCards(3, _startingHand.CardPositions);            
        }

        private void DistributeCards(int volume, IReadOnlyList<CardPosition> positions)
        {            
            float waitTime = 0;

            List<int> ids = new List<int>();
            while (ids.Count < volume)
            {
                int r = UnityEngine.Random.Range(0, _playersDeck.Count);
                if (ids.Contains(r))
                    continue;
                ids.Add(r);
            }

            foreach (CardPosition cardPosition in positions)
            {
                if (cardPosition.CardInPosition == null)
                {
                    int id = ids[0];
                    Card card;
                    _cardCreator.CreaterCard(transform.position, _playersDeck[id], out card);
                    _cardsInStartingHand.Add(card);
                    ids.Remove(id);
                    waitTime += 1;
                    StartCoroutine(MoveStartCard(card, cardPosition, waitTime));
                }
            }

        }
        

        public void AddCardsInPlayerHandByStartHand()
        {

            if (_startingHand.CardForReplace.Count == 0)
            {
                _startHandSelection = true;
                _startingHand.ActiveStartHand(false);

                int waitTime = 0;
                foreach (CardPosition cardPosition in _playerHand.CardPositions)
                {
                    if (cardPosition.CardInPosition == null)
                    {
                        Card card = _cardsInStartingHand[waitTime];

                        waitTime += 1;
                        StartCoroutine(Move(card, cardPosition, waitTime));
                    }
                    if (waitTime == _cardsInStartingHand.Count)
                        break;
                }
            }

            if (_startingHand.CardForReplace.Count > 0)
            {
                _startHandSelection = true;
                _startingHand.ActiveStartHand(false);

                int waitTime = 0;
                while (waitTime < _startingHand.CardForReplace.Count)
                {
                    Card card = _startingHand.CardForReplace[waitTime];
                    _startingHand.Replace(card);
                    _cardsInStartingHand.Remove(card);
                    CardPosition cardPosition = GetComponentInChildren<CardPosition>();
                    waitTime += 1;
                    StartCoroutine(Move(card, cardPosition, waitTime));                    
                }
                DistributeCards(waitTime, _startingHand.CardPositions);
            }
        }

        

        private IEnumerator Move(Card card, CardPosition cardPosition, float waitTime)
        {
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }

            card.gameObject.SetActive(true);
            float time = 0f;
            Vector3 startPos = card.transform.position;
            while (time < 1f)
            {
                card.transform.position = Vector3.Lerp(startPos, cardPosition.GetCardPosition, time);
                time += Time.deltaTime;
                yield return null;
            }
            cardPosition.SetCard(card);
        }

        private IEnumerator MoveStartCard(Card card, CardPosition cardPosition, float waitTime)
        {
            while (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
                yield return null;
            }

            card.gameObject.SetActive(true);
            float time = 0f;
            Vector3 startPos = card.transform.position;
            while (time < 1f)
            {
                card.transform.position = Vector3.Lerp(startPos, cardPosition.GetCardPosition, time);
                time += Time.deltaTime;
                yield return null;
            }
            cardPosition.SetCard(card);
            _startingHand.Fullness(_startHandSelection, this);
        }
    }
}