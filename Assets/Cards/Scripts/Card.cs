using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards
{
    public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private static GameObject _draggingObj;

        [SerializeField]
        private CardPosition _currentPosition;
        public CardPosition GetCurrentPosition => _currentPosition;
        [NonSerialized]
        public CardPropertiesData propertiesData;
        private Player _player;


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
                CardPosition cardPosition = hitInfo.transform.GetComponent<CardPosition>();
                if (cardPosition.CardInPosition == null)
                {
                    if (_currentPosition.StorageType == StorageType.Hand && cardPosition.StorageType == StorageType.Table && _player.UsingCard(this))
                    {
                        _currentPosition.Clear();
                        _currentPosition = cardPosition;
                        _currentPosition.SetCard(this);
                        _primaryParent = cardPosition.transform;
                        _primaryPosition = cardPosition.transform.position + new Vector3(0, 0.2f, 0);

                    }

                }

                transform.parent = _primaryParent;
                _draggingObj = null;
                return;
            }

            _currentPosition.SetCard(this);
            _draggingObj = null;
        }

        public void SetProperties()
        {
            materialImage.GetComponent<Renderer>().material.mainTexture = propertiesData.Texture;
            DisplayCost.text = propertiesData.Cost.ToString();
            DisplayName.text = propertiesData.Name;
            DisplayDescription.text = CardUtility.GetDescriptionById(propertiesData.Id);
            DisplayType.text = propertiesData.Type.ToString();
            DisplayAttack.text = propertiesData.Attack.ToString();
            DisplayHealth.text = propertiesData.Health.ToString();
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

        public void SetPlayer(Player parent)
        {
            _player = parent;
        }

        public void Twist_method()
        {
            
            StartCoroutine(Twist());
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
    }
}