using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cards
{
	public enum CardUnitType : byte
	{
		None = 0,
		Murloc = 1,
		Beast = 2,
		Elemental = 3,
		Mech = 4
	}

	public enum SideType : byte
	{
		Common = 0,
		Mage = 1,
		Warrior = 2
	}

	public enum StorageType
    {
		Table,
		Hand,
		Deck
    }

	public enum Players
    {
		None,
		Player1,
		Player2,
		Both
    }
}
