using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
	public string ReadyLevel;
	public string GameLevel;
	public string TutorialLevel;
	public int Coins = 3;
	public bool SkipTutorial = false;
	public GameObject PlayerPrefab;
	public float SpawnRadius = 1;
	public float MaxPlayers = 4;
	bool _gameOver = true;
	int _playerCount = 0;
	void Awake()
	{
	 DontDestroyOnLoad(gameObject);
	}
	 
	void Start()
	{
		StartCoroutine(WaitForPlayerOne());
	}
	
	void Update()
	{
		_playerCount = 0;
		foreach(var it in TagManager.FindAny((long)AllTags.Values.Player))
		{
			_playerCount++;
		}
	}
	
	bool Detect(Vector3 pos)
	{
		return false;
	}
	
	void SpawnPlayer()
	{
		// spawn point
        Vector3 pos = TagManager.FindAny((long)AllTags.Values.Spawn).First().transform.position;
		// any player already?
        foreach (var it in TagManager.FindAny((long)AllTags.Values.Player))
		{
			do
			{
				 pos = it.transform.position + SpawnRadius * (Quaternion.Euler(0,0,Random.Range(0,360)) * Vector3.up);  
			}
			while(Detect(pos)==true);
			break;
		}
		
		GameObject player = Instantiate(
			PlayerPrefab,
			pos,
			Quaternion.Euler(0,0,Random.Range(0, 360))) as GameObject;
		Coins--;
	}
	
	void StartGame()
	{
		SceneManager.LoadScene(GameLevel);
		SpawnPlayer();
		StartCoroutine(WaitForPlayers());
		StartCoroutine(WaitForGameOver());
	}
	
    bool AnyStartDown()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("Start." + i))
            {
                return true;
            }
        }
        return false;
    }

	IEnumerator WaitForPlayerOne()
	{
		while(_playerCount==0)
		{
            if (AnyStartDown())
			{
				StartGame();
				yield break;
			}
			yield return null;
		}
	}
	
	IEnumerator WaitForPlayers()
	{
		while(_playerCount>0)
		{
			if (_playerCount<MaxPlayers &&
                    AnyStartDown())
			{
				SpawnPlayer();
			}
			yield return null;
		}
	}
	
	IEnumerator WaitForGameOver()
	{
		yield return new WaitWhile(() => _playerCount > 0);
		// yuck everyone is dead
		// todo: display game over animation
		SceneManager.LoadScene(GameLevel);
		StartCoroutine(WaitForPlayerOne());
	}
}