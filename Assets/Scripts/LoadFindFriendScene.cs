using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadFindFriendScene : MonoBehaviour
{

	public void LoadFindFriend()
	{
		SceneManager.LoadScene("FriendDetectionScene", LoadSceneMode.Single);
	}
}

