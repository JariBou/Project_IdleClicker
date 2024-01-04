using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ProjectClicker
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private float _maxHealth;
        public float MaxHealth => _maxHealth;
        private float _health;
        [SerializeField] private Slider _enemyBaseHealthBar;

        [Header("Enemies")]
        [SerializeField] private GameObject _skeleton;
        [SerializeField] private GameObject _goblin;
        [SerializeField] private GameObject _mushroom;
        [SerializeField] private GameObject _flyingEye;

        [Header("Spawn")]
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnRate; 
        private bool _canSpawn;

        [Header("LevelsManager")]
        private LevelsManager _levelsManager;
        [SerializeField] private TextMeshProUGUI _levelText;

        // Start is called before the first frame update
        void Start()
        {
            _levelsManager = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();

            _enemyBaseHealthBar.maxValue = _maxHealth;
            _enemyBaseHealthBar.value = _maxHealth;
            _health = _maxHealth;
            _canSpawn = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_canSpawn)
            {
                StartCoroutine(SpawnSkeleton(_spawnRate));
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _enemyBaseHealthBar.value = _health;
            if (_health <= 0)
            {
                _levelsManager.NextLevel();
                _levelText.text = "Level : " + _levelsManager.CurrentLevel;
                _maxHealth += 1000 * _levelsManager.CurrentLevel;
                _health = _maxHealth;
                _enemyBaseHealthBar.value = _health;
                _spawnRate -= 0.1f;
            }
        }
        private IEnumerator SpawnSkeleton(float seconds)
        {
            _canSpawn = false;
            for (int i = 0; i<4; i++)
            {
                Instantiate(_skeleton, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length - 1)].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(2.2f);
            }
            yield return new WaitForSeconds(seconds);
            _canSpawn = true;
        }

        private void OnNextLevel()
        {

        }

        private void OnPreviousLevel()
        {

        }
    }
}
