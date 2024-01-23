using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ProjectClicker.Enemies
{
    public class EnemyBase : MonoBehaviour
    {

        [SerializeField] private float _baseMaxHealth = 500;
        private float _maxHealth;
        public float MaxHealth => _maxHealth;
        private float _health;
        [SerializeField] private Slider _enemyBaseHealthBar;
        [SerializeField] private GameObject _healthBarGeeenBar;
        private bool _isDead;

        [Header("Enemies")]
        [SerializeField] private GameObject[] _enemiesPrefab;
/*        [SerializeField] private GameObject _skeleton;
        [SerializeField] private GameObject _goblin;
        [SerializeField] private GameObject _mushroom;
        [SerializeField] private GameObject _flyingEye;*/

        [Header("Spawn")]
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnRate; 
        private bool _canSpawn;
        public float SpawnRate => _spawnRate;
        [Header("LevelsManager")]
        private LevelsManager _levelsManager;


        [Header("Enemie)")]
        [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

        [SerializeField] private GameObject _particlesPrefab;

        public GameObject Display;

        private void Awake()
        {
            Instance = this;
        }

        public static EnemyBase Instance { get; private set; }

        // Start is called before the first frame update
        private void Start()
        {
            _levelsManager = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();
            ResetBase();
            LevelsManager.OnChangeLevel += ClearEnemies;
            LevelsManager.OnPrestige += Prestige;
        }

        private void Prestige()
        {
            ClearEnemies();
            ResetBase();
        }

        private void OnDisable()
        {
            LevelsManager.OnChangeLevel -= ClearEnemies;
            LevelsManager.OnPrestige -= Prestige;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_canSpawn && (_levelsManager.CurrentLevel % 5 != 0 || _levelsManager.CurrentLevel == 0))
            {
                StartCoroutine(SpawnEnemies(_spawnRate));
            }
            else if (_canSpawn && _levelsManager.CurrentLevel % 5 == 0)
            {
                SpawnBoss();
            }
        }

        private void SpawnBoss()
        {
           _canSpawn = false;
            GameObject boss = Instantiate(_enemiesPrefab[Random.Range(0, _enemiesPrefab.Length - 1)], _spawnPoints[2].transform.position, Quaternion.identity);
            boss.transform.localScale *= 2;
            boss.GetComponent<EnemiesBehavior>().SetStats(EnemyType.Boss);
            boss.GetComponent<EnemiesBehavior>().Config(_particlesPrefab);
            _enemies.Add(boss);
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _enemyBaseHealthBar.value = _health;

            for (int i = 0; i < Random.Range(3, 7); i++)
            {
                Vector3 offset = new(Random.Range(-1.7f, 1.7f), Random.Range(-1.7f, 1.7f), -1);
                Instantiate(_particlesPrefab, transform.position + offset, Quaternion.identity);
            }
            
            if (_health <= 0 && !_isDead)
            {
                _isDead = true;
                StartCoroutine(Die());
            }
        }
        private IEnumerator SpawnEnemies(float seconds)
        {
            if (_enemies.Count < 8)
            {
                _canSpawn = false;
                for (int i = 0; i < 5; i++)
                {
                    GameObject skeleton = Instantiate(_enemiesPrefab[Random.Range(0,_enemiesPrefab.Length - 1)], _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)].transform.position, Quaternion.identity);
                    skeleton.GetComponent<EnemiesBehavior>().Config(_particlesPrefab);
                    _enemies.Add(skeleton);
                    yield return new WaitForSeconds(1f);
                }
                yield return new WaitForSeconds(seconds);
                _canSpawn = true;
            } 
        }

        private IEnumerator Die()
        {
            _healthBarGeeenBar.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            _levelsManager.NextLevel();
            CalculateHealth();
            _spawnRate = 3 * (1 - _levelsManager.CurrentLevel / (float)(100 + _levelsManager.CurrentLevel));
            _canSpawn = true;
            _isDead = false;
            _healthBarGeeenBar.SetActive(true);
        }

        public void CalculateHealth()
        {
            _maxHealth = 500* _levelsManager.CurrentLevel + _baseMaxHealth;
            _health = _maxHealth;
            _enemyBaseHealthBar.maxValue = _maxHealth;
            _enemyBaseHealthBar.value = _health;
        }

        public void LoadSave()
        {
            CalculateHealth();
            _spawnRate = 3 * (1 - _levelsManager.CurrentLevel / (float)(100 + _levelsManager.CurrentLevel));
        }

        public void ResetBase()
        {
            _maxHealth = _baseMaxHealth;
            _enemyBaseHealthBar.maxValue = _maxHealth;
            _enemyBaseHealthBar.value = _maxHealth;
            _health = _maxHealth;
            _canSpawn = true;
        }

        private void ClearEnemies()
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                Destroy(_enemies[i]);
                _enemies.Remove(_enemies[i]);
            }
        }

        public void RemoveEnemy(GameObject skeleton)
        {
            _enemies.Remove(skeleton);
        }

    }
}
