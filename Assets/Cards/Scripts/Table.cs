using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cards
{
    public class Table : MonoBehaviour
    {
        private List<CardPosition> _cardPositions;

        public IReadOnlyList<CardPosition> CardPositions => _cardPositions;

        private void Awake()
        {
            _cardPositions = GetComponentsInChildren<CardPosition>().ToList();
        }
    }
}
