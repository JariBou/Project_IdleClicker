using System.Collections;
using System.Collections.Generic;
using ProjectClicker.Core;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("LevelsManager")]
        private LevelsManager _levelsManager;


        [Header("Enemie)")]
        [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

        public GameObject Display;

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
            if (_canSpawn && (_levelsManager.CurrentLevel % 10 != 0 || _levelsManager.CurrentLevel == 0))
            {
                StartCoroutine(SpawnEnemies(_spawnRate));
            }
            else if (_canSpawn && _levelsManager.CurrentLevel % 10 == 0)
            {
                SpawnBoss();
            }
        }

        private void SpawnBoss()
        {
           _canSpawn = false;
            GameObject boss = Instantiate(_enemiesPrefab[Random.Range(0, _enemiesPrefab.Length - 1)], _spawnPoints[2].transform.position, Quaternion.identity);
            boss.GetComponent<EnemiesBehavior>().SetStats(EnemyType.Boss);
            boss.transform.localScale *= 2;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _enemyBaseHealthBar.value = _health;
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
            if (_levelsManager.CurrentLevel >= 1) _maxHealth += 1000 * _levelsManager.CurrentLevel * 2;
            else _maxHealth += 2500;
            _health = _maxHealth;
            _enemyBaseHealthBar.maxValue = _maxHealth;
            _enemyBaseHealthBar.value = _health;
            if (_spawnRate > 3f) _spawnRate -= 0.1f;
            _canSpawn = true;
            _isDead = false;
            _healthBarGeeenBar.SetActive(true);
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
