using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace ProjectClicker
{
    public class LevelsManager : MonoBehaviour
    {

        [Header("Levels")]
        [SerializeField] GameObject _level;
        [SerializeField] private int _currentLevel;
        public int CurrentLevel => _currentLevel;
        [SerializeField] private Sprite[] _backgroundLevels;
        [SerializeField] private TMP_Text _levelsText;

        // Start is called before the first frame update

        [Header("Champions Team")]
        [SerializeField] private GameObject _championTeam;
        [SerializeField] private List<Vector3> _championTeamSpawn = new();

        [Header("Changing Level Animation")]
        [SerializeField] private Animator _animatorNextLevel;
        [SerializeField] private Animator _animatorPreviousLevel;

        
        public static LevelsManager Instance { get; private set; }


        public event Action ResetTeamHealth;
        public static event Action OnChangeLevel;
        public static event Action OnPrestige;

        void Awake()
        {
            for (int i = 0; i < _championTeam.transform.childCount; i++)
            {
                if (_championTeam.transform.GetChild(i).gameObject.activeSelf) _championTeamSpawn.Add(_championTeam.transform.GetChild(i).transform.position);
            }

            Instance = this;
        }

        [Button]
        public void NextLevel()
        {
            _currentLevel++;
            _animatorNextLevel.SetTrigger("ChangingLevel");
            if (_currentLevel >= _backgroundLevels.Length)
            {
                _currentLevel = 0;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            ResetTeamHealth?.Invoke();
            CallLevelChanged();
            ResetTeamPosition();
        }

        private void CallLevelChanged()
        {
            OnChangeLevel?.Invoke();
            _levelsText.text = $"Level: {_currentLevel}";
        }

        [Button]
        public void PreviousLevel()
        {
            if (_currentLevel > 0 ) _animatorPreviousLevel.SetTrigger("ChangingLevel");
            _currentLevel--;
            if (_currentLevel < 0)
            {
                _currentLevel = 0;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            ResetTeamHealth?.Invoke();
            CallLevelChanged();
            ResetTeamPosition();
        }

        [Button]
        public void PrestigeGame()
        {
            _currentLevel = 0;
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel % _backgroundLevels.Length];
            OnPrestige?.Invoke();
            CallLevelChanged();
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


        public void SetLevel(int currLevel)
        {
            _currentLevel = currLevel;
            ResetTeamHealth?.Invoke();
            OnChangeLevel?.Invoke();
        }
    }
}
