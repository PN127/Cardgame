using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class CardCreator : MonoBehaviour
    {
        [SerializeField]
        private Card _cardPrefab;

        public void CreaterCard(Transform spawnTransform, CardPropertiesData data, out Card card)
        {
            card = Instantiate(_cardPrefab, spawnTransform.position, spawnTransform.rotation * new Quaternion(0,0,180,0));             
            CardFilling(card, data);
            card.SetProperties();
            card.gameObject.SetActive(false);
        }

        private void CardFilling(Card card, CardPropertiesData data)
        {
            card.propertiesData.Id = data.Id;
            card.propertiesData.Cost = data.Cost;
            card.propertiesData.Name = data.Name;
            card.propertiesData.Texture = data.Texture;
            card.propertiesData.Attack = data.Attack;
            card.propertiesData.Health = data.Health;
            card.propertiesData.Type = data.Type;
        }
    }
}