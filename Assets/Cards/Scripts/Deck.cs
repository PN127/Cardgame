using DG.Tweening;
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
        private Player _owner;

        private CardCreator _cardCreator;
        private PlayerHand _playerHand;
        private StartingHand _startingHand;
        private bool _startHandSelection;
        private bool _cardIssue;

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
            _owner = GetComponentInParent<Player>();
            _cardCreator = GetComponent<CardCreator>();
            _startingHand = FindObjectOfType<StartingHand>();
            _playersDeck = new List<CardPropertiesData>();
            _startHandSelection = false;
            _cardsInStartingHand = new List<Card>();
        }

        private void Start()
        {
            _playerHand = _owner.GetPlayerHand;
            _cardIssue = false;
            CreateDeck();
        }
       

        //Создание колоды
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
        
        //Нажатие на колоду
        public void OnPointerClick(PointerEventData eventData)
        {
            if (_owner == GameManager.GetWalker && !_cardIssue)
             {
                
                if (_startHandSelection)
                {
                    DistributeCards(1, _playerHand.CardPositions);
                }
                else
                {                    
                    if (_owner.gameObject.name == "Player2")
                    {
                        foreach (CardPosition position in _startingHand.CardPositions)
                        {
                            position.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                        }
                    }
                    _startingHand.ActiveStartHand(true);
                    DistributeCards(3, _startingHand.CardPositions);
                }
                _cardIssue = true;
            }
        }

        //разадача определенного кол-ва карт (volume) и перемещение карты в позицию (positions)
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
                if (cardPosition.CardInPosition == null && ids.Count > 0)
                {
                    int id = ids[0];
                    Card card;                   
                    _cardCreator.CreaterCard(transform, _playersDeck[id], out card);
                    _cardsInStartingHand.Add(card);
                    ids.Remove(id);
                    waitTime += 1;


                    StartCoroutine(MoveCardTo(card, cardPosition, waitTime));

                }
            }
        }

        //Выдача карт из стартовой руки
        public void CardIsssuing()
        {
            _startHandSelection = true;
            _startingHand.ActiveStartHand(false);

            int waitTime = 0;
            foreach (CardPosition cardPosition in _playerHand.CardPositions)
            {
                if (cardPosition.CardInPosition == null)
                {
                    Card card = _cardsInStartingHand[0];
                    card.ClearPosition();
                    _cardsInStartingHand.Remove(card);
                    waitTime += 1;
                    StartCoroutine(MoveCardTo(card, cardPosition, waitTime));
                }
                if (waitTime == 3)
                    break;
            }
        }

        //Замена карт в стартовой руке игрока
        public void CardReplecment()
        {
            Tween _tween;
            _startHandSelection = true;
            _startingHand.ActiveStartHand(false);

            int waitTime = 0;
            int count = _startingHand.CardForReplace.Count;
            while (waitTime < count)
            {
                Card card = _startingHand.CardForReplace[0];
                card.ColliderSwitch(false);
                _startingHand.Replace(card, _startHandSelection);
                _cardsInStartingHand.Remove(card);
                CardPosition DeckPosition = GetComponentInChildren<CardPosition>();
                waitTime += 1;
                //StartCoroutine(Move(card, DeckPosition, waitTime));
                _tween = card.transform.DOMove(DeckPosition.transform.position, 2f).OnComplete(() => Destroy(card.gameObject));
            }
            DistributeCards(waitTime, _startingHand.CardPositions);
        }
                

        //Метод вызывается из Player для окончания хода
        public void EndOfTurn()
        {
            _cardIssue = false;
        }

        private IEnumerator MoveCardTo(Card card, CardPosition cardPosition, float waitTime)
        {
            card.ColliderSwitch(false);

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
            card.ColliderSwitch(true);
            if (cardPosition.StorageType == StorageType.Starting)
            {
                _startingHand.Fullness(_startHandSelection, this);
            }
        }        
    }
}