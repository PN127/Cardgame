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
        private Camera camera;
        
        [Space]
        [SerializeField]
        private Player[] _players;

       

        private void Start()
        {            
            _walker = _players[0];
        }

        public void MoveTransitiion()
        {
            Vector3 rotation;

            if (_walker == _players[0])
            {
                _walker = _players[1];
                rotation = new Vector3(90, 180, 0);
            }
            else
            {
                _walker = _players[0];
                rotation = new Vector3(90, 0, 0);
            }

            StartCoroutine(CameraRotation(rotation));
        }

        public void SelectStartingHand()
        {
            _walker.GetDeck.AddCardsInPlayerHandByStartHand();
        }

        private IEnumerator CameraRotation(Vector3 FinishRotation)
        {
            float t = 0;
            while (t < 10)
            {
                camera.transform.rotation *= Quaternion.Euler(new Vector3(0,0,180) * 0.1f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }

            camera.transform.rotation = Quaternion.Euler(FinishRotation);
            Debug.Log("finish");
        }
    }
}
