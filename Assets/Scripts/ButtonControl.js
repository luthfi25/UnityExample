#pragma strict
var creditPanelCanvas : Canvas;
var creditShown = false;
var pauseMenuCanvas : Canvas;
var paused=false;

function Start () {

}

function Update () {

}
function BackToMainMenu(){
    Application.LoadLevel(0);
}
function StartGame(){
	Application.LoadLevel(2);
}
function CharacterSelect(){
    Application.LoadLevel(1);
}
function ExitGame(){
	Application.Quit();
}
function Play(){
	Application.LoadLevel(2);
}
function Sound(){
	if(!AudioListener.pause){
		AudioListener.pause=true;
	}else if(AudioListener.pause){
		AudioListener.pause=false;
	}
}

function CreditInfo(){
	if(!creditShown){
		creditPanelCanvas.enabled=true;
		creditShown=true;
	}else if(creditShown){
		creditPanelCanvas.enabled=false;
		creditShown=false;
	}
}
function PauseMenu(){
	if(!paused){
		pauseMenuCanvas.enabled=true;
		paused=true;
		Time.timeScale=0f;
	}else if(paused){
		pauseMenuCanvas.enabled=false;
		paused=false;
		Time.timeScale=1f;
	}
}