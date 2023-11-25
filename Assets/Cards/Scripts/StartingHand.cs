using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Cards
{
    public class StartingHand : MonoBehaviour
    {
        private List<CardPosition> _cardPositions;
        public IReadOnlyList<CardPosition> CardPositions => _cardPositions;

        [SerializeField]
        private Canvas _canvas;

        private void Awake()
        {
            _cardPositions = GetComponentsInChildren<CardPosition>().ToList();
            TurnOnChildren(false);
        }

        public void TurnOnChildren(bool state)
        {
            foreach (CardPosition card in _cardPositions)
            {
                card.gameObject.SetActive(state);
            }
            _canvas.gameObject.SetActive(state);
        }
    }
}