using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneLine;

namespace Cards
{
	[System.Serializable]
	public struct CardPropertyData
	{
		[SerializeField]
		private int _cost;
		[SerializeField]
		private Texture _image;
		[SerializeField]
		private string _name;
		[SerializeField]
		private string _description;
		[SerializeField]
		private int _attack;
		[SerializeField]
		private int _health;
		[SerializeField]
		private CardUnitType _type;
	}

	[Serializable]
	public struct CardPropertiesData
	{
		[Width(30)]
		public uint Id;
        [NonSerialized]
        public ushort Cost;
		public string Name;
		[Width(50)]
		public Texture Texture;
		[Width(15)]
		public ushort Attack;
		[Width(15)]
		public ushort Health;
		[Width(65)]
		public CardUnitType Type;
		[Width(50)]
		public Players InDeck;

		public CardParamsData GetParams()
		{
			return new CardParamsData(Cost, Attack, Health);
		}
	}

	[Serializable]
	public struct CardPropertiesDataToSelect
	{
		[Width(30)]
		public uint Id;
		[Width(15)]
		public ushort Cost;
		[Width(50)]
		public string Name;
        public Texture Texture;
        [Width(15)]
		public ushort Attack;
		[Width(15)]
		public ushort Health;
        public CardUnitType Type;
        [Width(50)]
		public bool InDeck;

		public CardParamsData GetParams()
		{
			return new CardParamsData(Cost, Attack, Health);
		}
	}

	public struct CardParamsData
	{
		public ushort Cost;
		public ushort Attack;
		public ushort Health;

		public CardParamsData(ushort cost, ushort attack, ushort health)
		{
			Cost = cost; Attack = attack; Health = health;
		}
	}
}
