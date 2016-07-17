using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CharacterChange : MonoBehaviour {
	//list of character available
    string[] Names = new string[] { "Batio", "Owlio", "Sheepo", "Goato" };
    string[] CloneNames = new string[] { "Batio(Clone)", "Owlio(Clone)", "Sheepo(Clone)", "Goato(Clone)" };
	public List<GameObject> CharList = new List<GameObject> ();
	public int index;

	//Character rendered on the screen
	public GameObject CharNowObj;
    public string CharacterNow="Owlio";

	//Friends number
	public int charNumber;

	//Selected friends character
    public CharacterSelected friendsSelected;
	public GameObject[] selected= new GameObject[3];


	public Text friendName;
    public Text exp;
    public Text energy;
    public Text damage;

	// Use this for initialization
    void Start() {
		//Instantiating the first friend character on the screen
		index = 1;
		charNumber = 1;
		GameObject dontDestroy = GameObject.FindGameObjectWithTag("DontDestroy");
		friendsSelected = dontDestroy.GetComponent<CharacterSelected> ();
        CharacterNow = "Owlio";
        EnableRenderer(CharacterNow);
		//if (!friendsSelected.IsEmpty()) {
			//for(int i = 0;i<3;i++){
				//Debug.Log(friendsSelected.charIndex[i] + " nama char " + friendsSelected.SelectedChar[i]);
			//}
		//}
	}
	//Right or Left button is clicked
    public void Right() {
		index++;
        if (index >= Names.Length)
        {
            index = 0;
            CharacterNow = Names[index];
        }
        else {
            CharacterNow = Names[index];
        }
		GameObject obj = CharNowObj;
		DisableRenderer (obj);
		EnableRenderer (CharacterNow);
		//CharNowObj.GetComponent<Renderer>().enabled=false;
		//EnableRenderer (CharacterNow);
        //Debug.Log("index ke- "+index);

		//Debug.Log (CharNowObj.GetComponent<Player>().name);
        //Debug.Log("namanya kanan" + CharacterNow);
    }
    public void Left() {
		index--;

        if (index < 0)
        {
            index = Names.Length-1;
			CharacterNow = Names[index];
        }
        else {
			CharacterNow = Names[index];
		}
		GameObject obj = CharNowObj;
		DisableRenderer (obj);
		EnableRenderer (CharacterNow);
		//CharNowObj.GetComponent<Renderer>().enabled=false;
		//EnableRenderer (CharacterNow);
        //Debug.Log("index ke- " + index);
        //Debug.Log("namanya kiri" + CharacterNow);
    }
	
	//Check if this object already instantiated
	void EnableRenderer(string str){
		GameObject obj = Instantiate (Resources.Load (CharacterNow)) as GameObject;
        CharNowObj = obj;
        friendName.text = str;
		exp.text = CharNowObj.GetComponent<Player>().exp.ToString();
		energy.text = CharNowObj.GetComponent<Player>().maxEnergy.ToString();
		damage.text = CharNowObj.GetComponent<Player>().damage.ToString();
		CharNowObj.GetComponent<Player>().exp++;
	}
	void DisableRenderer(GameObject obj){
		DestroyObject (obj);
	}

	//Character selection
	public void OnSelect(){
		if (friendsSelected.CheckAvailability(CharacterNow,charNumber))
        {
			if(selected[charNumber-1]){
				DestroyObject(selected[charNumber-1]);
				makeInstance();
				friendsSelected.charIndex[charNumber-1]=index;
			}else{
				makeInstance();
				friendsSelected.charIndex[charNumber-1]=index;
			}
        }
	}
	void makeInstance(){
		selected[charNumber-1]=Instantiate (Resources.Load (Names[index])) as GameObject;
		selected[charNumber-1].transform.position = GameObject.Find("Selected"+charNumber).transform.position;
		var x = 0.02f;
		selected [charNumber - 1].transform.localScale -= new Vector3 (x,x,0);
	}
	//void makeInstance(int selectedNum,int nameIndex){
		
	//}
	public void LetsPlay(){
		if(friendsSelected.IsSelected()){
			Application.LoadLevel(2);
		}
	}
    public void c1() {
        charNumber = 1;
    }
    public void c2()
    {
        charNumber = 2;
    }
    public void c3()
    {
        charNumber = 3;
    }
}
public static class ListExtenstions
{
	public static void AddMany<T>(this List<T> list, params T[] elements)
	{
		list.AddRange(elements);
	}
}