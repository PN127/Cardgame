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
        private int _round;
        public int GetRound => _round;

        [SerializeField]
        private Camera camera;
        
        [Space]
        [SerializeField]
        private Player[] _players;

        private StartingHand _startingHand;

        private void Start()
        {
            _startingHand = FindObjectOfType<StartingHand>();
            _walker = _players[0];
            _walker.AddManaScore();
        }

        public void TransferTurn()
        {
            _walker.FlipCards();
            ChangeWalker();
            WaitFlipCardsAsync();
            Debug.Log("Смена хода завершена");
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

            _walker.AddManaScore();
            _walker.PossibilityCardAttack();

            if (camera.transform.rotation != Quaternion.Euler(rotation))
                StartCoroutine(CameraRotation(rotation));
            
        }

        //Метод вызывается через кнопку SampleScene.StartHand.Canvas.OK_Button
        public void SelectStartingHand()
        {
            if (_startingHand.CardForReplace.Count == 0)
                _walker.GetDeck.CardIsssuing();
            else
                _walker.GetDeck.CardReplecment();
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
            Debug.Log("Карты перевенуты");
        }
    }

    public class Entity : MonoBehaviour
    {
        [SerializeField, Min(1)]
        protected int health;
        [SerializeField, Min(0)]
        protected int attack;
        protected Player _player;
        public Player GetPlayer => _player;

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public virtual int TakeDamage(int damadge, out int counterattack)
        {
            health -= damadge;
            counterattack = attack;
            return counterattack;
        }
        public virtual StorageType GetStorageType()
        {
            StorageType type = StorageType.None;
            return type;
        }

    }
}
