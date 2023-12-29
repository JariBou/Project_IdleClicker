using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class LevelsManager : MonoBehaviour
    {

        [Header("Levels")]
        [SerializeField] GameObject _level;
        [SerializeField] private int _currentLevel;
        [SerializeField] private Sprite[] _backgroundLevels;

        [Header("Enemies")]
        [SerializeField] private GameObject _skeleton;
        [SerializeField] private GameObject _goblin;
        [SerializeField] private GameObject _mushroom;
        [SerializeField] private GameObject _flyingEye;

        [Header("Base")]
        [SerializeField] private GameObject _teamBase;
        [SerializeField] private GameObject _enemyBase;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        [Button]
        private void NextLevel()
        {
            _currentLevel++;
            if (_currentLevel >= _backgroundLevels.Length)
            {
                _currentLevel = 0;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel];
        }

        [Button]
        private void PreviousLevel()
        {
            _currentLevel--;
            if (_currentLevel < 0)
            {
                _currentLevel = _backgroundLevels.Length - 1;
            }
            _level.GetComponent<SpriteRenderer>().sprite = _backgroundLevels[_currentLevel];
        }

        [Button]
        private void SpawnSkeleton()
        {
            Instantiate(_skeleton, new Vector3(0, 0, 0), Quaternion.identity);
        }

    }
}
