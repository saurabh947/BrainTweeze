using UnityEngine;
using System.Collections;
using System.IO;

public class LeapGuiButton : MonoBehaviour 
{
	public bool toggleButton = false;
	public float selectionTime = 0.5f;
	public float clickTime = 0f;
	public Texture normalTexture;
	public Texture hoverTexture;
	public Texture pressedTexture;

	private LeapGuiButton buttonPlay;
	private LeapGuiButton buttonGame1;
	private LeapGuiButton buttonBackMainMenu;
	private LeapGuiButton buttonQ1Option1;

	private LeapGuiButton buttonQ1Option2;
	private LeapGuiButton buttonQ1Option3;
	private LeapGuiButton buttonQ1Option4;
	private LeapGuiButton buttonQ2Option1;
	private LeapGuiButton buttonQ2Option2;
	private LeapGuiButton buttonQ2Option3;
	private LeapGuiButton buttonQ2Option4;
	private LeapGuiButton buttonQ3Option1;
	private LeapGuiButton buttonQ3Option2;
	private LeapGuiButton buttonQ3Option3;
	private LeapGuiButton buttonQ3Option4;
	private LeapGuiButton buttonQ4Option1;
	private LeapGuiButton buttonQ4Option2;
	private LeapGuiButton buttonQ4Option3;
	private LeapGuiButton buttonQ4Option4;
	private LeapGuiButton buttonQ5Option1;
	private LeapGuiButton buttonQ5Option2;
	private LeapGuiButton buttonQ5Option3;
	private LeapGuiButton buttonQ5Option4;
	private LeapGuiButton buttonQ6Option1;
	private LeapGuiButton buttonQ6Option2;
	private LeapGuiButton buttonQ6Option3;
	private LeapGuiButton buttonQ6Option4;

	public GUIText finalScoreText;

	public int finalScore = 0;

	private LeapGuiButton buttonBackMainMenu3;
	private LeapGuiButton buttonBackMainMenu4;
	private LeapGuiButton buttonBackMainMenu5;

	private LeapGuiButton buttonClose;
	
	public bool bBtnPressed = false;
	public bool bPressReported = false;
	public Vector3 vLastCursorPos = Vector3.zero;
	public float fSelectionTimer = 0f;
	public LeapManager leapManager;
	public LeapManager showFinalScore;
	int a = 0;

	public bool isCursorIn = false;
	public bool isCursorOut = false;

		int glowTimerIndex = 0;

	public bool IsButtonPressed()
	{
		Rect btnRect = this.guiTexture.GetScreenRect();
		if(!toggleButton && bBtnPressed && !bPressReported)
		{
			DepressButton();
			
			bPressReported = true;
			return true;
		}

		return bBtnPressed;
	}
	
	
	public void DepressButton()
	{
		bBtnPressed = false;
		this.guiTexture.texture = normalTexture;
		
		vLastCursorPos = Vector3.zero;
		fSelectionTimer = 0;
	}



	void Start () 
	{
		leapManager = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<LeapManager>();
	}
	
	void OnGUI()
	{
		if(!this.guiTexture)
			return;

		if(leapManager && leapManager.IsPointableValid()
			/**(leapManager.GetPointableTouchStatus() == Leap.Pointable.Zone.ZONEHOVERING || leapManager.GetPointableTouchStatus() == Leap.Pointable.Zone.ZONETOUCHING)*/)
		{
			Vector3 posCursor = leapManager.GetCursorScreenPos();
			Rect btnRect = this.guiTexture.GetScreenRect();
			Texture buttonTexture = !bBtnPressed ? normalTexture : pressedTexture;

			if(!bBtnPressed)
			{
				if(!bPressReported && btnRect.Contains(posCursor))
				{
					isCursorIn = true;
					buttonTexture = hoverTexture;
					
					if(!btnRect.Contains(vLastCursorPos))
					{
						isCursorOut = true;
						fSelectionTimer = Time.realtimeSinceStartup + selectionTime;
					}
					else if((fSelectionTimer > 0) && (Time.realtimeSinceStartup >= fSelectionTimer))
					{
						bBtnPressed = true;
						buttonTexture = pressedTexture;
						fSelectionTimer = 0;
					}
				}
				else if(bPressReported && !btnRect.Contains(posCursor))
				{
					bPressReported = false;
				}
			}else if(toggleButton)
			{
				if(btnRect.Contains(posCursor))
				{
					if(!btnRect.Contains(vLastCursorPos))
					{
						fSelectionTimer = Time.realtimeSinceStartup + selectionTime;
					}
					else if((fSelectionTimer > 0) && (Time.realtimeSinceStartup >= fSelectionTimer))
					{
						bBtnPressed = false;
						buttonTexture = normalTexture;
						fSelectionTimer = 0;
					}
				}
			}
			
			this.guiTexture.texture = buttonTexture;
			vLastCursorPos = posCursor;
		}
	}
	
	void Update() {

		GameObject objButton = GameObject.Find("PlayButton");
		buttonPlay = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("Game1");
		buttonGame1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("BackMainMenu");
		buttonBackMainMenu = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q1Option1");
		buttonQ1Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q1Option2");
		buttonQ1Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q1Option3");
		buttonQ1Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q1Option4");
		buttonQ1Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q2Option1");
		buttonQ2Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q2Option2");
		buttonQ2Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q2Option3");
		buttonQ2Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q2Option4");
		buttonQ2Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q3Option1");
		buttonQ3Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q3Option2");
		buttonQ3Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q3Option3");
		buttonQ3Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q3Option4");
		buttonQ3Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q4Option1");
		buttonQ4Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q4Option2");
		buttonQ4Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q4Option3");
		buttonQ4Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q4Option4");
		buttonQ4Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q5Option1");
		buttonQ5Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q5Option2");
		buttonQ5Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q5Option3");
		buttonQ5Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q5Option4");
		buttonQ5Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q6Option1");
		buttonQ6Option1 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q6Option2");
		buttonQ6Option2 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q6Option3");
		buttonQ6Option3 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("q6Option4");
		buttonQ6Option4 = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		objButton = GameObject.Find("CloseButton");
		buttonClose = objButton ? objButton.GetComponent<LeapGuiButton>() : null;

		if(buttonPlay && buttonPlay.IsButtonPressed()){
			audio.Play();
			Invoke("PlayGame", 0.25f);
		}
		if(buttonGame1 && buttonGame1.IsButtonPressed()){
			audio.Play();
			Invoke("Game1", 0.25f);
		}
		if(buttonBackMainMenu && buttonBackMainMenu.IsButtonPressed()){
			audio.Play();
			Invoke("BackMainMenu", 0.25f);
		}
		if(buttonQ1Option1 && buttonQ1Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q1Option1", 0.25f);
		}
		if(buttonQ1Option2 && buttonQ1Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q1Option2", 0.25f);
		}
		if(buttonQ1Option3 && buttonQ1Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q1Option3", 0.25f);
		}
		if(buttonQ1Option4 && buttonQ1Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q1Option4", 0.25f);
		}
		if(buttonQ2Option1 && buttonQ2Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q2Option1", 0.25f);
		}
		if(buttonQ2Option2 && buttonQ2Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q2Option2", 0.25f);
		}
		if(buttonQ2Option3 && buttonQ2Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q2Option3", 0.25f);
		}
		if(buttonQ2Option4 && buttonQ2Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q2Option4", 0.25f);
		}
		if(buttonQ3Option1 && buttonQ3Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q3Option1", 0.25f);
		}
		if(buttonQ3Option2 && buttonQ3Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q3Option2", 0.25f);
		}
		if(buttonQ3Option3 && buttonQ3Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q3Option3", 0.25f);
		}
		if(buttonQ3Option4 && buttonQ3Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q3Option4", 0.25f);
		}
		if(buttonQ4Option1 && buttonQ4Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q4Option1", 0.25f);
		}
		if(buttonQ4Option2 && buttonQ4Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q4Option2", 0.25f);
		}
		if(buttonQ4Option3 && buttonQ4Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q4Option3", 0.25f);
		}
		if(buttonQ4Option4 && buttonQ4Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q4Option4", 0.25f);
		}
		if(buttonQ5Option1 && buttonQ5Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q5Option1", 0.25f);
		}
		if(buttonQ5Option2 && buttonQ5Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q5Option2", 0.25f);
		}
		if(buttonQ5Option3 && buttonQ5Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q5Option3", 0.25f);
		}
		if(buttonQ5Option4 && buttonQ5Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q5Option4", 0.25f);
		}
		if(buttonQ6Option1 && buttonQ6Option1.IsButtonPressed()){
			audio.Play();
			Invoke("Q6Option1", 0.25f);
		}
		if(buttonQ6Option2 && buttonQ6Option2.IsButtonPressed()){
			audio.Play();
			Invoke("Q6Option2", 0.25f);
		}
		if(buttonQ6Option3 && buttonQ6Option3.IsButtonPressed()){
			audio.Play();
			Invoke("Q6Option3", 0.25f);
		}
		if(buttonQ6Option4 && buttonQ6Option4.IsButtonPressed()){
			audio.Play();
			Invoke("Q6Option4", 0.25f);
		}

		if(buttonClose && buttonClose.IsButtonPressed()){
			audio.Play();
			Invoke("CloseGame", 0.25f);
		}

		finalScore = (finalScore / 6) * 100;

//		finalScoreText.Equals(finalScore.ToString());

	}


	void PlayGame() {
		Application.LoadLevel("PlayMenu");
	}
	void Game1() {
		Application.LoadLevel("Game1Ques1");
	}
	void BackMainMenu() {
		Application.LoadLevel("MainMenu");
	}

	void Q1Option1() {
		finalScore++;
		Application.LoadLevel("Game1Ques2");
	}
	void Q1Option2() {
		Application.LoadLevel("Game1Ques2");
	}
	void Q1Option3() {
		Application.LoadLevel("Game1Ques2");
	}
	void Q1Option4() {
		Application.LoadLevel("Game1Ques2");
	}
	void Q2Option1() {
		Application.LoadLevel("Game1Ques3");
	}
	void Q2Option2() {
		Application.LoadLevel("Game1Ques3");
	}
	void Q2Option3() {
		finalScore++;
		Application.LoadLevel("Game1Ques3");
	}
	void Q2Option4() {
		Application.LoadLevel("Game1Ques3");
	}
	void Q3Option1() {
		Application.LoadLevel("Game1Ques4");
	}
	void Q3Option2() {
		Application.LoadLevel("Game1Ques4");
	}
	void Q3Option3() {
		finalScore++;
		Application.LoadLevel("Game1Ques4");
	}
	void Q3Option4() {
		Application.LoadLevel("Game1Ques4");
	}
	void Q4Option1() {
		Application.LoadLevel("Game1Ques5");
	}
	void Q4Option2() {
		finalScore++;
		Application.LoadLevel("Game1Ques5");
	}
	void Q4Option3() {
		Application.LoadLevel("Game1Ques5");
	}
	void Q4Option4() {
		Application.LoadLevel("Game1Ques5");
	}
	void Q5Option1() {
		Application.LoadLevel("Game1Ques6");
	}
	void Q5Option2() {
		Application.LoadLevel("Game1Ques6");
	}
	void Q5Option3() {
		Application.LoadLevel("Game1Ques6");
	}
	void Q5Option4() {
		finalScore++;
		Application.LoadLevel("Game1Ques6");
	}
	void Q6Option1() {
		Application.LoadLevel("FinalScore");
	}
	void Q6Option2() {
		Application.LoadLevel("FinalScore");
	}
	void Q6Option3() {
		finalScore++;
		Application.LoadLevel("FinalScore");
	}
	void Q6Option4() {
		Application.LoadLevel("FinalScore");
	}

	void CLoseGame() {
		Application.Quit();
	}
}
