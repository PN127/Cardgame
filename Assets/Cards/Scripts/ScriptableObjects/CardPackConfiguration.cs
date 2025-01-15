using OneLine;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Cards
{
	[CreateAssetMenu(fileName = "NewCardPackConfiguration", menuName = "CardConfigs/Card Pack Configuration")]
	public class CardPackConfiguration : ScriptableObject
	{
		private bool _isConstruct;

		[SerializeField]
		private SideType _sideType;
		[SerializeField]
		private ushort _cost;                                    //notes3 - В сцене имеется 7 ScriptableObject и у каждого установлена своя цена. Позже она присваивается массиву карт в заметке notes2 
		[SerializeField, OneLine(Header = LineHeader.Short)]
		private CardPropertiesData[] _cards;                     //notes1 - CardPropertiesData это структура из Structs

		

		public IEnumerable<CardPropertiesData> UnionProperties(IEnumerable<CardPropertiesData> array)
		{
			_isConstruct = false;

			TryToContruct();

			return array = _cards; //return array.Union(_cards);
		}
		
		private void TryToContruct()
		{
			if (_isConstruct) return;

			for(int i = 0; i < _cards.Length; i++)
			{
				_cards[i].Cost = _cost;                          //notes2 - объявление стоимости карты. 
			}

			_isConstruct = true;
		}
	}
}