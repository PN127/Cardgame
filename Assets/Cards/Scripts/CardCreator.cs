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
            card = Instantiate(_cardPrefab, spawnTransform.position, spawnTransform.rotation * new Quaternion(0,0,180,0), spawnTransform);             
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

            var description = CardUtility.GetDescriptionById(data.Id);
            switch (description)
            {
                case string a when a.Contains("Charge"):
                    card.propertiesData.Effect = MinionEffects.Charge;
                    break;
                case string b when b.Contains("Taunt"):
                    card.propertiesData.Effect = MinionEffects.Taunt;
                    break;
                case string c when c.Contains("Battlecry"):
                    card.propertiesData.Effect = MinionEffects.Battlecry;
                    break;

            }
        }
    }
}