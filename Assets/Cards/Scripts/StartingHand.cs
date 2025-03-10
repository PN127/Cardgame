﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class StartingHand : MonoBehaviour
    {
        private List<CardPosition> _cardPositions;
        private List<Card> _cardsForReplace;

        public IReadOnlyList<CardPosition> CardPositions => _cardPositions;
        public IReadOnlyList<Card> CardForReplace => _cardsForReplace;

        [SerializeField]
        private Button buttonOK;

        [SerializeField]
        private Canvas _canvas;

        public GameObject RedCross;
        
        [Header("StartingHand properties")]
        [SerializeField]
        private GameObject _blackout;
        [SerializeField]
        private float _stepToBlack;

        

        private void Awake()
        {
            buttonOK.interactable = false;
            _cardPositions = GetComponentsInChildren<CardPosition>().ToList();
            SwitchActiveChildren(false);
            _cardsForReplace = new List<Card>();
        }

        public void ActiveStartHand(bool state)
        {
            SwitchActiveChildren(state);
            StartCoroutine(Eclipse(state));
        }

        public void SwitchActiveChildren(bool state)
        {
            foreach (CardPosition card in _cardPositions)
            {
                card.gameObject.SetActive(state);
            }
            _canvas.gameObject.SetActive(state);
        }       
        private IEnumerator Eclipse(bool state)
        {
            if (state)
            {
                _blackout.SetActive(state);
                var c = _blackout.GetComponent<Renderer>().material.color;

                while (c.a < 0.8)
                {
                    c.a += _stepToBlack / 100;
                    _blackout.GetComponent<Renderer>().material.color = c;
                    yield return null;
                }
            }
            if (!state)
            {
                var c = _blackout.GetComponent<Renderer>().material.color;

                while (c.a > 0.8)
                {
                    c.a -= _stepToBlack / 100;
                    _blackout.GetComponent<Renderer>().material.color = c;
                    yield return null;
                    
                }
                _blackout.SetActive(state);
            }
        }
        public void Fullness(bool startHandSelection, Deck deck)
        {
            foreach (CardPosition card in _cardPositions)
            {
                if (card.GetCard() == null)
                {
                    Debug.Log("стартовая рука NE заполнена");
                    return;
                }
            }
            Debug.Log("стартовая рука заполнена");
            buttonOK.interactable = true;

            if (!startHandSelection)
                return;

            deck.CardIsssuing();
        }

        // Замена и удаление карты
        public void Replace(Card card, bool StartingHandSelection = false)
        {
            // Если карта не содержиться в списке _cardsForReplace (список карт на удаление), то на ней ставится красный крест
            if (!_cardsForReplace.Contains(card))
            {
                _cardsForReplace.Add(card);
                var red_cross = Instantiate(RedCross, card.transform);
                red_cross.transform.position += new Vector3(0, 2, 0);

                return;
            }

            // Если карта содержится в списке _cardsForReplace, то карта удаляется
            if (_cardsForReplace.Contains(card))
            {
                _cardsForReplace.Remove(card);
                Destroy(card.transform.Find(RedCross.name + "(Clone)").gameObject);
                if (StartingHandSelection)
                    card.ClearPosition();
            }
        }
    }
}