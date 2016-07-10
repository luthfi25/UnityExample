using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary; 

[Serializable]
public class CharacterData {
	public int exp;
	public int hp;
	public int damage;
	public int maxEnergy;
	public int energyCost;
	public int energyCharge;
}
