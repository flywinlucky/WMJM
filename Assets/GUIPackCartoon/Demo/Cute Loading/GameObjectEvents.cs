using UnityEngine;
using System.Collections;

public class GameObjectEvents : MonoBehaviour {

	public void DestroyParentGameObject()
	{
		Destroy(transform.parent.gameObject);
	}

	public void DestroyGameObject()
	{
		Destroy(gameObject);
	}

	public void PlayMouthBacteriaJumpSound()
	{
		//SoundManager.Instance.PlayBacteriaJumpSound();
	}

	public void PlayBacteriaSquashedSound()
	{
		//SoundManager.Instance.PlayBacteriaKillSound();
	}

	public void PlayLoadingSwooshSound()
	{
		//SoundManager.Instance.PlayLoadingSwooshSound();
	}

	public void PlayLoadingTextComeSound()
	{
		//SoundManager.Instance.PlayLoadingTextComeSound();
	}

	public void PLayLoadingTextJumpSound()
	{
		//SoundManager.Instance.PlayLoadingJumpSound();
	}
}
