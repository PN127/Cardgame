using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class StartingHand : MonoBehaviour
    {
        private List<CardPosition> _cardPositions;
        public IReadOnlyList<CardPosition> CardPositions => _cardPositions;
        [SerializeField]
        private Button buttonOK;

        [SerializeField]
        private Canvas _canvas;

        [Header("StartingHand properties")]
        [SerializeField]
        private GameObject _blackout;
        [SerializeField]
        private float _step;

        private void Awake()
        {
            buttonOK.interactable = false;
            _cardPositions = GetComponentsInChildren<CardPosition>().ToList();
            SwitchActiveChildren(false);
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
                    c.a += _step / 100;
                    _blackout.GetComponent<Renderer>().material.color = c;
                    yield return null;
                }
            }
            if (!state)
            {
                var c = _blackout.GetComponent<Renderer>().material.color;

                while (c.a > 0.8)
                {
                    c.a -= _step / 100;
                    _blackout.GetComponent<Renderer>().material.color = c;
                    yield return null;
                    _blackout.SetActive(state);
                }
            }
        }
        
        public void Fullness()
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
        }


    }
}