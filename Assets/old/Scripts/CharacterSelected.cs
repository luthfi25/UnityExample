using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelected : MonoBehaviour
{

    //list of character selected
    public string[] SelectedChar = new string[3];
	public int[] charIndex = new int[3];
    void Update() {
        if (Application.loadedLevel == 0) {
			SelectedChar= new string[3];
			//for(int i = 0 ; i < 3 ; i++){
				//SelectedChar[i]="";
				//charIndex=new int[3];
			//}
        }
    }

    public bool CheckAvailability(string charName,int charNum)
    {
		charNum -= 1;
		bool isSelected = true;
		//Checking if character already selected
		for(int i = 0 ; i<3; i++){
			if(charName.Equals(SelectedChar[i])){
				return false;
			}else{
				isSelected=false;
			}
		}
		//
		if (!isSelected) {
			SelectedChar[charNum]=charName;
		}
		return true;
    }
	public bool IsEmpty(){
		bool cek = false;
		foreach(string str in SelectedChar){
			if(str.Equals("")){
				cek=true;
			}
		}
		if (cek) {
			return  cek;
		}
		return false;
	}
	public bool IsSelected(){
		for(int i = 0 ; i<3 ; i++){
			if(SelectedChar[i] == null){
				return false;
			}
		}
		return true;
	}
}