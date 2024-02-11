using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Player _walker;

        [SerializeField]
        private Player[] _players;
        

        private void Start()
        {            
            _walker = _players[0];
        }

        public void MoveTransitiion()
        {
            if (_walker == _players[0])
            {
                _walker = _players[1];
            }
            else
            {
                _walker = _players[0];
            }
            
        }
    }
}
