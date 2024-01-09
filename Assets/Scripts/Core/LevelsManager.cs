using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ProjectClicker
{
    public class LevelsManager : MonoBehaviour
    {

        [Header("Levels")]
        [SerializeField] GameObject _level;
        [SerializeField] private int _currentLevel;
        public int CurrentLevel => _currentLevel;
        [SerializeField] private Sprite[] _backgroundLevels;

        // Start is called before the first frame update

        [Header("Champions Team")]
        [SerializeField] private GameObject _championTeam;
        [SerializeField] private List<Vector3> _championTeamSpawn = new List<Vector3>();



        public Action ChangeLevel;

        void Awake()
        {
            for (int i = 0; i < _championTeam.transform.childCount; i++)
            {
                if (_championTeam.transform.GetChild(i).gameObject.activeSelf) _championTeamSpawn.Add(_championTeam.transform.GetChild(i).transform.position);
            }


        }

        // Update is called once per frame
        void Update()
        {
            
        }
        [Button]
        public void NextLevel()
        {
            _currentLevel++;
            if (_currentLevel >= _backgroundLevels.Length)
            {
                _currentLevel = 0;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            ResetTeamHealth?.Invoke();
            ChangeLevel?.Invoke();
            ResetTeamPosition();
        }

        [Button]
        public void PreviousLevel()
        {
            _currentLevel--;
            if (_currentLevel < 0)
            {
                _currentLevel = 0;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            ResetTeamHealth?.Invoke();
            ChangeLevel?.Invoke();
            ResetTeamPosition();
        }

        [Button]
        public void PrestigeGame()
        {
            _currentLevel = 0;
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            OnResetGame?.Invoke();
            ChangeLevel?.Invoke();
            ResetTeamPosition();
        }

        private void ResetTeamPosition()
        {
            int index = 0;
            for (int i = 0; i < _championTeam.transform.childCount; i++)
            {
                if (_championTeam.transform.GetChild(i).gameObject.activeSelf)
                {
                    _championTeam.transform.GetChild(i).transform.position = _championTeamSpawn[index];
                    index++;
                }
            }
        }



/*        public void TakeDamage(float damage)
        {
            _health -= damage;
            _enemyBaseHealthBar.value = _health;
            if (_health <= 0)
            {
                Debug.Log("You Win");
                _health = _maxHealth;
                _enemyBaseHealthBar.value = _health;
                NextLevel();
                ResetTeamHealth?.Invoke();
                _championTeam.transform.position = _championTeamSpawn.position;
            }
        }*/

        public event Action ResetTeamHealth;
        public static event Action OnResetGame;

    }
}
