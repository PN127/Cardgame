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

        
        [SerializeField]
        private CardPackConfiguration[] cardPacks; //to do
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private List<CardPropertiesData> _playersDeck;

        
        

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
            if (!_startHandSelection)
                StartingHand();
            //else
            //    AddCardsInPlayerHand();
        }

        private void StartingHand()
        {
            _startingHand.ActiveStartHand(true);
            float waitTime = 0;

            List<int> ids = new List<int>();
            while (ids.Count < 3)
            {
                int r = UnityEngine.Random.Range(0, _playersDeck.Count);
                if (ids.Contains(r))
                    continue;
                ids.Add(r);
            }

            foreach (CardPosition cardPosition in _startingHand.CardPositions)
            {
                if (cardPosition.CardInPosition == null)
                {
                    int id = ids[0];
                    Card card;
                    CreateCard(cardPosition, waitTime, id, out card);
                    ids.Remove(id);
                    waitTime += 1;
                    StartCoroutine(MoveStartCard(card, cardPosition, waitTime));
                }
            }

        }

        public void AddCardsInPlayerHandByStartHand()
        {
            _startHandSelection = true;
            _startingHand.ActiveStartHand(false);
            float waitTime = 0;
            foreach (CardPosition cardPosition in _playerHand.CardPositions)
            {
                if (cardPosition.CardInPosition == null)
                {
                    Card card = _startingHand.CardPositions[(int)waitTime].GetCard();
                    waitTime += 1;
                    StartCoroutine(Move(card, cardPosition, waitTime));
                }
                if (waitTime == 3)
                    break;                
            }
        }

        private void CreateCard(CardPosition cardPosition, float waitTime, int id, out Card card)
        {
            card = _cardCreator.CreaterCard(transform.position);
            
            CardFilling(card, _playersDeck[id]);
            card.SetProperties();
            card.gameObject.SetActive(false);
        }
        private void CardFilling(Card card, CardPropertiesData data)
        {
            card.propertiesData.Id = data.Id;
            card.propertiesData.Cost = data.Cost;
            card.propertiesData.Name = data.Name;
            card.propertiesData.Texture = data.Texture;
            card.propertiesData.Attack = data.Attack;
            card.propertiesData.Health = data.Health;
            card.propertiesData.Type = data.Type;
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
            _startingHand.Fullness();
            Debug.Log("затмение завершено");
        }
    }
}