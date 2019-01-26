
using HeroEditor.Common.Tools;
using UnityEngine;

public class PhotoMaker : MonoBehaviour
{
	public ScreenshotTransparent ScreenshotTransparent;
	
	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			MakePhoto();
		}
	}

	public void MakePhoto()
	{
		#if UNITY_EDITOR

		ScreenshotTransparent.Capture(ScreenshotTransparent.GetPath());

		#elif UNITY_ANDROID || UNITY_IOS

		var photoShare = GetComponent("PhotoShare");

		if (photoShare != null)
		{
			photoShare.SendMessage("Share");
		}

		#endif
	}
}