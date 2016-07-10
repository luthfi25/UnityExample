using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public string name;
	public int exp;
	[HideInInspector]public int hp;
	public int maxHp;
	public string gesture;
	public int damage;
	public bool isFly;
	//Energy
	public int maxEnergy;
	public int energyCost;
	public int currentEnergy;
	public int energyCharge;
	private float energyWait = 2f;
	void Awake(){
		StartCoroutine( EnergyCount ());
		currentEnergy = maxEnergy;
		hp = maxHp;
	}
	//ataack the enemies
	public virtual void attack(){
		Debug.Log ("Player Attack");
	}

	public void setHp(int damage){
		hp = hp-damage;
	}
	public int getHp(){
		return hp;
	}
	public string getName(){
		return name;
	}
	public string getGesture(){
		return gesture;
	}
	public void setGesture(string gest){
		gesture = gest;
	}
	public void minEnergy(){
		currentEnergy -= energyCost;
	}
	IEnumerator EnergyCount(){
		yield return new WaitForSeconds (energyWait);
		while (currentEnergy <= maxEnergy) {
			if (currentEnergy + energyCharge >= maxEnergy) {
				currentEnergy = maxEnergy;
			} else {
				currentEnergy += energyCharge;
			}
			yield return new WaitForSeconds (energyWait);
		}
	}
}
