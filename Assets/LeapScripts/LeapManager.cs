using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Leap;

public class LeapManager : MonoBehaviour 
{
	
 	public GameObject LeapSensorPrefab = null;
	public LineRenderer LineFingerPrefab = null;
	public Vector3 DisplayFingerPos = Vector3.zero;
	public float DisplayFingerScale = 5;
	public float clickableElapsedTime = 0f;
	public float clickTime = 0f;
	int glowTimerIndex = 0;
	public Vector3 vLastCursorPos = Vector3.zero;
	public bool containsCursor = false;
	public bool notContainCursor = false;
	public GUITexture handCursor;
	public Texture normalCursorTexture;
	public Texture2D[] progressCursorTexture;
	public GUIText debugText;
	public LeapManager leapManager;
	public LeapManager finalScore;
	public LeapGuiButton leapButton;
	public LeapGuiButton LeapGUIButton;
	private Leap.Controller leapController = null;
	private Leap.Frame leapFrame = null;
	public Int64 lastFrameID = 0;
	public Int64 leapFrameCounter = 0;
	public Leap.Pointable leapPointable = null;
	public int leapPointableID = 0;
	public int leapPointableHandID;
	public Vector3 leapPointablePos;
	public Vector3 leapPointableDir;
	public Leap.Hand leapHand = null;
	public int leapHandID = 0;
	public Vector3 leapHandPos;
	public int leapHandFingersCount;
	public int fingersCountHandID;
	public float fingersCountFiltered;
	public bool handGripDetected = false;
	public bool handReleaseDetected = false;
	public int handGripFingersCount;
	public bool leapInitialized = false;
	public Vector3 cursorNormalPos = Vector3.zero;
	public Vector3 cursorScreenPos = Vector3.zero;
	private static LeapManager instance;
	private Dictionary<int, LineRenderer> dictFingerLines = new Dictionary<int, LineRenderer>();

	public static LeapManager Instance
	{
		get
		{
			return instance;
		}
	}
	
	public bool IsLeapInitialized()
	{
		return leapInitialized;
	}
	
	public Int64 GetLeapFrameCounter()
	{
		return leapFrameCounter;
	}
	
	public bool IsPointableValid()
	{
		return (leapPointable != null) && leapPointable.IsValid;
	}
	
	public Leap.Pointable GetLeapPointable()
	{
		return leapPointable;
	}
	
	public int GetPointableID()
	{
		if((leapPointable != null) && leapPointable.IsValid)
			return leapPointableID;
		else
			return 0;
	}
	
	public Vector3 GetPointablePos()
	{
		if((leapPointable != null) && leapPointable.IsValid)
			return leapPointablePos;
		else
			return Vector3.zero;
	}
	
	public Vector3 GetPointableDir()
	{
		if((leapPointable != null) && leapPointable.IsValid)
			return leapPointableDir;
		else
			return Vector3.zero;
	}
	
	public Quaternion GetPointableQuat()
	{
		if((leapPointable != null) && leapPointable.IsValid)
			//return leapPointableQuat;
			return Quaternion.LookRotation(leapPointableDir);
		else
			return Quaternion.identity;
	}
	
	public bool IsHandValid()
	{
		return (leapHand != null) && leapHand.IsValid;
	}
	
	public Leap.Hand GetLeapHand()
	{
		return leapHand;
	}
	
	public int GetHandID()
	{
		if((leapHand != null) && leapHand.IsValid)
			return leapHandID;
		else
			return 0;
	}
	
	public Vector3 GetHandPos()
	{
		if((leapHand != null) && leapHand.IsValid)
			return leapHandPos;
		else
			return Vector3.zero;
	}
	
	public int GetFingersCount()
	{
		return leapHandFingersCount;
	}
	
	public bool IsHandGripDetected()
	{
		return handGripDetected;
	}
	
	public bool IsHandReleaseDetected()
	{
		return handReleaseDetected;
	}
	
	
	public Vector3 GetCursorNormalizedPos()
	{
		if((leapHand != null) && leapHand.IsValid)
			return cursorNormalPos;
		else
			return Vector3.zero;
	}
	
	public Vector3 GetCursorScreenPos()
	{
		if((leapHand != null) && leapHand.IsValid)
			return cursorScreenPos;
		else
			return Vector3.zero;
	}

	void Awake()
	{
		
		if(LeapSensorPrefab)
		{
			Instantiate(LeapSensorPrefab, DisplayFingerPos, Quaternion.identity);
		}
		
		if(CheckLibsPresence())
		{
			audio.Play();
			Application.LoadLevel(Application.loadedLevel);
		}

		DontDestroyOnLoad(this.gameObject);

	}
	
	void Start()
	{
		try 
		{
	
			GameObject thePlayer = GameObject.Find("DummyButton");
			LeapGUIButton = thePlayer.GetComponent<LeapGuiButton>();
			containsCursor = LeapGUIButton.isCursorIn;
			notContainCursor = LeapGUIButton.isCursorOut;

			leapController = new Leap.Controller();
			
			instance = this;
			leapInitialized = true;
			
			DontDestroyOnLoad(gameObject);
			
			string sMessage = leapController.Devices.Count > 0 ? "Ready." : "Please make sure the Leap-sensor is connected.";
			Debug.Log(sMessage);
			
		}
		catch(System.TypeInitializationException ex)
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.guiText.text = "Please check the LeapMotion installation.";
		}
		catch (System.Exception ex) 
		{
			Debug.LogError(ex.ToString());
			if(debugText != null)
				debugText.guiText.text = ex.Message;
		}
	}
	
	void OnApplicationQuit()
	{
		leapPointable = null;
		leapFrame = null;
		
		if(leapController != null)
		{
			leapController.Dispose();
			leapController = null;
		}
		
		leapInitialized = false;
		instance = null;
	}
	
	void Update() 
	{
		if(leapInitialized && leapController != null)
		{
			Leap.Frame frame = leapController.Frame();
			
			if(frame.IsValid && (frame.Id != lastFrameID))
			{
				leapFrame = frame;
				lastFrameID = leapFrame.Id;
				leapFrameCounter++;
				
				// get the prime hand
				leapHand = leapFrame.Hand(leapHandID);
				if(leapHand == null || !leapHand.IsValid)
				{
					leapHand = GetFrontmostHand();
					leapHandID = leapHand != null && leapHand.IsValid ? leapHand.Id : 0;
				}
				
				leapPointable = leapFrame.Pointable(leapPointableID);
				if(leapPointable == null || !leapPointable.IsValid)
				{
					leapPointable = leapHand.IsValid ? GetPointingFigner(leapHand) : leapFrame.Pointables.Frontmost;
					
					leapPointableID = leapPointable != null && leapPointable.IsValid ? leapPointable.Id : 0;
					leapPointableHandID = leapPointable != null && leapPointable.IsValid && leapPointable.Hand.IsValid ? leapPointable.Hand.Id : 0;
				}
				
				if(leapPointable != null && leapPointable.IsValid && 
				   leapPointable.Hand != null && leapPointable.Hand.IsValid &&
				   leapHand != null && leapHand.IsValid && leapPointable.Hand.Id == leapHand.Id)
				{
					leapPointablePos = LeapToUnity(leapPointable.StabilizedTipPosition, true);
					leapPointableDir = LeapToUnity(leapPointable.Direction, false);
				}
				else 
				{
					leapPointable = null;
					
					leapPointableID = 0;
					leapPointableHandID = 0;
				}
				
				Leap.Vector stabilizedPosition = Leap.Vector.Zero;
				if(leapHandID != 0)
				{
					leapHandPos = LeapToUnity(leapHand.StabilizedPalmPosition, true);
					stabilizedPosition = leapHand.StabilizedPalmPosition;
					
					leapHandFingersCount = leapHand.Fingers.Count;
					
					bool bCurrentHandGrip = handGripDetected;
					handGripDetected = !isHandOpen(leapHand);
					handReleaseDetected = !handGripDetected;
					
				}
				else
				{
					leapHandFingersCount = 0;
					handGripDetected = false;
					handReleaseDetected = false;
				}
				
				if(stabilizedPosition.MagnitudeSquared != 0f)
				{
					Leap.InteractionBox iBox = frame.InteractionBox;
					Leap.Vector normalizedPosition = iBox.NormalizePoint(stabilizedPosition);
					
					cursorNormalPos.x = normalizedPosition.x;
					cursorNormalPos.y = normalizedPosition.y;
					cursorScreenPos.x = cursorNormalPos.x * UnityEngine.Screen.width;
					cursorScreenPos.y = cursorNormalPos.y * UnityEngine.Screen.height;
					
				}
			}
		}
	}
	
	
	void OnGUI()
	{	

		if(handCursor != null)
		{
			Texture texture = null;


			if(containsCursor){
				clickTime = clickTime + Time.deltaTime;
				Debug.Log(clickTime);
				glowTimerIndex = Mathf.CeilToInt(2 * clickTime);
				if (glowTimerIndex > 7)
				{
					glowTimerIndex = 7;
				}
				Texture2D cursorGlowImg = progressCursorTexture[glowTimerIndex];
				handCursor.guiTexture.texture = cursorGlowImg;
				if(notContainCursor)
				{
					clickTime = 0;
				}
			}else
			{
				handCursor.guiTexture.texture = normalCursorTexture;
				handCursor.transform.position = Vector3.Lerp(handCursor.transform.position, cursorNormalPos, 3 * Time.deltaTime);
			}
			
			
			
//			if(texture == null)
//			{
//				texture = normalCursorTexture;
//			}
//			
//			if(texture != null)
//			{
//				handCursor.guiTexture.texture = texture;
//				handCursor.transform.position = Vector3.Lerp(handCursor.transform.position, cursorNormalPos, 3 * Time.deltaTime);
//			}
		
		}
	}
	
	private void ShowCameraWindow(int windowID) 
	{
	}
	
	private Vector3 LeapToUnity(Leap.Vector leapVector, bool bScaled)	
	{
		if(bScaled)
			return new Vector3(leapVector.x, leapVector.y, -leapVector.z) * .001f; 
		else
			return new Vector3(leapVector.x, leapVector.y, -leapVector.z); 
	}
	
	private Hand GetFrontmostHand()
	{
		float minZ = float.MaxValue;
		Hand forwardHand = Hand.Invalid;
		
		foreach(Hand hand in leapFrame.Hands)
		{
			if(hand.PalmPosition.z < minZ)
			{
				minZ = hand.PalmPosition.z;
				forwardHand = hand;
			}
		}
		
		return forwardHand;
	}
	
	private Pointable GetPointingFigner(Hand hand)
	{
		Pointable forwardPointable = Pointable.Invalid;
		List<Pointable> forwardPointables = forwardFacingPointables(hand);
		
		if(forwardPointables.Count > 0)
		{
			
			float minZ = float.MaxValue;
			
			foreach(Pointable pointable in forwardPointables)
			{
				if(pointable.TipPosition.z < minZ)
				{
					minZ = pointable.TipPosition.z;
					forwardPointable = pointable;
				}
			}
		}
		
		return forwardPointable;
	}
	
	private List<Pointable> forwardFacingPointables(Hand hand)
	{
		List<Pointable> forwardPointables = new List<Pointable>();
		
		foreach(Pointable pointable in hand.Pointables)
		{
			if(isForwardRelativeToHand(pointable, hand)) 
			{ 
				forwardPointables.Add(pointable); 
			}
		}
		
		return forwardPointables;
	}
	
	private bool isForwardRelativeToHand(Pointable item, Hand hand)
	{
		Vector3 vHandToFinger = (LeapToUnity(item.TipPosition, true) - LeapToUnity(hand.PalmPosition, true)).normalized;
		float fDotProduct = Vector3.Dot(vHandToFinger, LeapToUnity(hand.Direction, false));
		
		return fDotProduct > 0.7f;
	}
	
	private bool isHandOpen(Hand hand)
	{
		return hand.Fingers.Count > 2;
	}
	
	
	// copies the needed libraries in the project directory
	private bool CheckLibsPresence()
	{
		bool bOneCopied = false, bAllCopied = true;
		
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if(!File.Exists("Leap.dll"))
		{
			Debug.Log("Copying Leap library...");
			TextAsset textRes = Resources.Load("Leap.dll", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("Leap.dll", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("Leap.dll");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied Leap library.");
			}
		}
		
		if(!File.Exists("LeapCSharp.dll"))
		{
			Debug.Log("Copying LeapCSharp library...");
			TextAsset textRes = Resources.Load("LeapCSharp.dll", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("LeapCSharp.dll", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("LeapCSharp.dll");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied LeapCSharp library.");
			}
		}
		
		if(!File.Exists("msvcp100.dll"))
		{
			Debug.Log("Copying msvcp100 library...");
			TextAsset textRes = Resources.Load("msvcp100.dll", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("msvcp100.dll", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("msvcp100.dll");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied msvcp100 library.");
			}
		}
		
		if(!File.Exists("msvcr100.dll"))
		{
			Debug.Log("Copying msvcr100 library...");
			TextAsset textRes = Resources.Load("msvcr100.dll", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("msvcr100.dll", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("msvcr100.dll");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied msvcr100 library.");
			}
		}
		#endif
		
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		if(!File.Exists("libLeap.dylib"))
		{
			Debug.Log("Copying Leap library...");
			TextAsset textRes = Resources.Load("libLeap.dylib", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("libLeap.dylib", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("libLeap.dylib");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied Leap library.");
			}
		}
		
		if(!File.Exists("libLeapCSharp.dylib"))
		{
			Debug.Log("Copying LeapCSharp library...");
			TextAsset textRes = Resources.Load("libLeapCSharp.dylib", typeof(TextAsset)) as TextAsset;
			
			if(textRes != null)
			{
				using (FileStream fileStream = new FileStream ("libLeapCSharp.dylib", FileMode.Create, FileAccess.Write, FileShare.Read))
				{
					fileStream.Write (textRes.bytes, 0, textRes.bytes.Length);
				}
				
				bOneCopied = File.Exists("libLeapCSharp.dylib");
				bAllCopied = bAllCopied && bOneCopied;
				
				if(bOneCopied)
					Debug.Log("Copied LeapCSharp library.");
			}
		}
		#endif
		
		return bOneCopied && bAllCopied;
	}
	
}
