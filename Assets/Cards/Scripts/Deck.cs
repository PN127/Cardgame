using OneLine;

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

        [SerializeField]
        private CardPackConfiguration[] cardPacks; //to do
        [SerializeField]
        private CardPackConfiguration cost1;
        private List<CardPropertiesData> array;
        [SerializeField, OneLine(Header = LineHeader.Short)]
        private CardPropertiesDataToSelect[] _cardsForDeck;

        private void Awake()
        {
            //_cardsForDeck = new CardPropertiesDataToSelect[46];
            _cardCreator = GetComponent<CardCreator>();
            _playerHand = FindObjectOfType<PlayerHand>();
            array = new List<CardPropertiesData>();
                        
        }

        //private void Start()
        //{
        //    DeckFiiling();
        //}

        //private void DeckFiiling()
        //{
        //    byte e = 0;
        //    foreach (CardPackConfiguration pack in cardPacks)
        //    {
        //        var list = pack.UnionProperties(array).ToList();
        //        byte i = 0;
        //        while (i < list.Count)
        //        {
        //            _cardsForDeck[e].Id = list[i].Id;
        //            _cardsForDeck[e].Cost = list[i].Cost;
        //            _cardsForDeck[e].Name = list[i].Name;
        //            _cardsForDeck[e].Texture = list[i].Texture;
        //            _cardsForDeck[e].Attack = list[i].Attack;
        //            _cardsForDeck[e].Health = list[i].Health;
        //            _cardsForDeck[e].Type = list[i].Type;
        //            i++;
        //            e++;
        //        }
        //    }
        //}

        public void OnPointerClick(PointerEventData eventData)
        {
            array = cost1.UnionProperties(array).ToList(); //to do - Задал вопрос в Дискорде обучения. Ответ есть и применен
            AddCardsInPlayerHand();
        }
        private void AddCardsInPlayerHand()
        {
            float waitTime = 0;
            foreach (CardPosition cardPosition in _playerHand.CardPositions)
            {
                if (cardPosition.CardInPosition == null)
                {
                    Card card;
                    CreateCard(cardPosition, waitTime, out card);
                    waitTime += 1;
                    StartCoroutine(Move(card, cardPosition, waitTime));
                }
            }
        }
        private void CreateCard(CardPosition cardPosition, float waitTime, out Card card)
        {
            card = _cardCreator.CreaterCard(transform.position);
            CardFilling(card, array[1]);
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

    }
}