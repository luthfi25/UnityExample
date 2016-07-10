using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	public string name;
	[HideInInspector]public int hp;
	public int maxHp;
	public string gesture;
	public int damage;
	public int exp;
	void Awake(){
		hp = maxHp;
	}
	//ataack the player
	public virtual void Attack(){
		Debug.Log ("Enemy Attack");
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
	public int getDamage(){
		return damage;
	}
	public void setGesture(string gest){
		gesture = gest;
	}
	
}
