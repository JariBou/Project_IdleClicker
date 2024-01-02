using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

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
        [SerializeField] float spawnRate; 
        private bool _canSpawn;

        [Header("LevelsManager")]
        private LevelsManager _levelsManager;

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
                StartCoroutine(SpawnSkeleton(spawnRate));
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _enemyBaseHealthBar.value = _health;
            if (_health <= 0)
            {
                Debug.Log("You Win");
                _health = _maxHealth;
                _enemyBaseHealthBar.value = _health;
                _levelsManager.NextLevel();
            }
        }

        [Button]
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
