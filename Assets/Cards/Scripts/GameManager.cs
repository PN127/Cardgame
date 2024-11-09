using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Cards
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private static Player _walker;
        public static Player GetWalker => _walker;
        
        [SerializeField]
        private Camera camera;
        
        [Space]
        [SerializeField]
        private Player[] _players;
               

        private void Start()
        {
            _walker = _players[0];
        }

        public void TransferTurn()
        {
            _walker.FlipCards();
            ChangeWalker();
            WaitFlipCardsAsync();
        }

        private void ChangeWalker()
        {
            Vector3 rotation;
            _walker.EndOfTurn();

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
            

            if (camera.transform.rotation != Quaternion.Euler(rotation))
                StartCoroutine(CameraRotation(rotation));
            
        }

        //Метод вызывается через кнопку SampleScene.StartHand.Canvas.OK_Button
        public void SelectStartingHand()
        {
            _walker.GetDeck.AddCardsInPlayerHandByStartHand();
        }

        private IEnumerator CameraRotation(Vector3 FinishRotation)
        {
            float time = 0;
            while (time < 2)
            {
                camera.transform.rotation *= Quaternion.Euler(new Vector3(0,0,180) * 0.5f * Time.deltaTime);
                time += Time.deltaTime;               

                yield return null;
            }

            camera.transform.rotation = Quaternion.Euler(FinishRotation);
            Debug.Log("finish");
        }

        private async void WaitFlipCardsAsync()
        {
            await Task.Run(() => Thread.Sleep(5000));
            _walker.FlipCards();
        }
    }
}
