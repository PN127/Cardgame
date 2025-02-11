using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{   

    public class Card : Entity, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler 
    {
        private static GameObject _draggingObj;

        [SerializeField]
        private CardPosition _currentPosition;
        public CardPosition GetCurrentPosition => _currentPosition;
        //[NonSerialized]
        public CardPropertiesData propertiesData;      

        [SerializeField]
        private GameObject materialImage;
        [SerializeField]
        private TMPro.TextMeshPro DisplayCost;
        [SerializeField]
        private TMPro.TextMeshPro DisplayName;
        [SerializeField]
        private TMPro.TextMeshPro DisplayDescription;
        [SerializeField]
        private TMPro.TextMeshPro DisplayType;
        [SerializeField]
        private TMPro.TextMeshPro DisplayAttack;
        [SerializeField]
        private TMPro.TextMeshPro DisplayHealth;

        private StartingHand _startingHand;
        private Collider _collider;
        public bool _canAttack;


        private Vector3 _primaryPosition;
        private StorageType _primaryStorageType;
        private Transform _primaryParent;

        private void Awake()
        {
            _startingHand = FindObjectOfType<StartingHand>();
            _collider = GetComponent<Collider>();
        }

        private void Start()
        {
            _canAttack = false;
            health = propertiesData.Health;
            attack = propertiesData.Attack;
        }

        public void SetPosition(CardPosition position)
        {
            _currentPosition = position;
        }
        public void ClearPosition()
        {
            _currentPosition.Clear();
            _currentPosition = null;
        }

        public void ColliderSwitch(bool enabled)
        {
            _collider.enabled = enabled;
        }

        public void OnMouseEnter()
        {
            if (_primaryPosition == Vector3.zero) _primaryPosition = transform.position;
            transform.position += new Vector3(0, 0, 3);
            transform.localScale += transform.localScale;
        }
        public void OnMouseExit()
        {
            transform.position -= new Vector3(0, 0, 3);
            transform.localScale -= transform.localScale / 2;
            if (_primaryPosition != null)
            {
                transform.position = _primaryPosition;
                _primaryPosition = Vector3.zero;
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canAttack && _currentPosition.StorageType == StorageType.Table)
                return;

            _primaryParent = transform.parent;
            transform.parent = null;
            _draggingObj = gameObject;

            _draggingObj.transform.position += new Vector3(0, 3, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_draggingObj == null)
                return;

            Vector3 draggingPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            draggingPosition.y = transform.position.y;
            _draggingObj.transform.position = draggingPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 100f, 1 << 8))
            {
                Transform targetPosition = hitInfo.transform;

                if (targetPosition.GetComponent<CardPosition>())  
                {
                    CardPosition cardPosition = targetPosition.GetComponent<CardPosition>();

                    if (cardPosition.CardInPosition == null)
                    {
                        if (_currentPosition.StorageType == StorageType.Hand && cardPosition.StorageType == StorageType.Table && _player.UsingCard(this))
                        {
                            MoveToTable(cardPosition);
                        }
                    }
                }

                else
                {
                    Entity entity = targetPosition.GetComponent<Entity>();
                    Player player = entity.GetPlayer;
                    StorageType storageType = entity.GetStorageType();
                    
                    if (entity == null)
                        return;
                    
                    if (_currentPosition.StorageType == StorageType.Table &&
                        player != _player &&
                        (storageType == StorageType.Table || storageType == StorageType.Hero))
                    {
                        MoveToAtack(entity);
                    }
                }

                transform.parent = _primaryParent;
                _draggingObj = null;
                return;
            }

            _currentPosition.SetCard(this);
            _draggingObj = null;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (_currentPosition.StorageType)
            {
                case (StorageType.Deck):
                    var deck = _currentPosition.transform.parent.gameObject;
                    deck.GetComponent<Deck>().OnPointerClick(eventData);
                    break;

                case (StorageType.Starting):
                    _startingHand.Replace(this);
                    break;

                case (StorageType.Hand):
                    break;

                case (StorageType.Table):
                    break;
            }
        }

        private void MoveToTable(CardPosition cardPosition)
        {
            _currentPosition.Clear();
            _currentPosition = cardPosition;
            _currentPosition.SetCard(this);
            _primaryParent = cardPosition.transform;
            _primaryPosition = cardPosition.transform.position + new Vector3(0, 0.2f, 0);

        }

        private void MoveToAtack(Entity targetEntity)
        {
            targetEntity.TakeDamage(attack, out int counterattack);
            if (counterattack > 0)
                TakeDamage(counterattack, out int v);
        }

        public override int TakeDamage(int damadge, out int counterattack)
        {
            health -= damadge;
            refreshProp();
            counterattack = attack;
            if (health <= 0)
            {
                Death();
            }
            return counterattack;            
        }  

        private void Death() //логика смерти
        {
            Debug.LogError($"здоровья нет = {health}. Я помер Х/ {gameObject.name}");
            _currentPosition.Clear();
            _currentPosition = null;
            Twist_method(); //карты не переворачиваются to do

            Fold.FoldStatic.CardsDie(gameObject.transform); //перемещение карт в битое
        }

        public void Twist_method()
        {

            StartCoroutine(Twist());
        }

        public override StorageType GetStorageType()
        {
            StorageType type = _currentPosition.StorageType;
            return type;
        }

        private IEnumerator Twist()
        {
            while (transform.position.y < 5)
            {
                transform.position += Vector3.up * 3f * Time.deltaTime;
                yield return null;
            }
            Vector3 pos = transform.position;
            pos.y = 5.01f;
            transform.position = pos;

            float t = 0;
            while (t < 1)
            {
                transform.rotation *= Quaternion.Euler(new Vector3(0, 0, 180) * 1f * Time.deltaTime);
                t += Time.deltaTime;
                yield return null;
            }
            if (transform.rotation.z > 0.5)
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            else
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            while (transform.position.y > 0.3)
            {
                transform.position -= Vector3.up * 3f * Time.deltaTime;
                yield return null;
            }
            pos = transform.position;
            pos.y = 0.3f;
            transform.position = pos;
        }

        public void SetProperties()
        {
            Start();
            materialImage.GetComponent<Renderer>().material.mainTexture = propertiesData.Texture;
            DisplayCost.text = propertiesData.Cost.ToString();
            DisplayName.text = propertiesData.Name;
            DisplayType.text = propertiesData.Type.ToString();
            DisplayAttack.text = attack.ToString();
            DisplayHealth.text = health.ToString();
            DisplayDescription.text = CardUtility.GetDescriptionById(propertiesData.Id);
        }

        private void refreshProp()
        {
            DisplayHealth.text = health.ToString();
        }
    }
}