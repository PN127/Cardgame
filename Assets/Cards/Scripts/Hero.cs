using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
    public class Hero : Entity
    {
        private void Start()
        {
            _player = GetComponentInParent<Player>();
        }

        private void FixedUpdate()
        {
            if (_player.TauntOnTable)
                _canAttacked = false;
            else
                _canAttacked = true;
        }

        public override int TakeDamage(int damadge, out int counterattack)
        {
            health -= damadge;
            counterattack = attack;
            if (health < 0)
            {
                Debug.LogWarning($"Игрок {_player.name} проиграл");
                UnityEditor.EditorApplication.isPaused = true;
            }
            return counterattack;
        }

        public override StorageType GetStorageType()
        {
            StorageType type = StorageType.Hero;
            return type;
        }





    }
}