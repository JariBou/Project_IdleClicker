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
        private bool _isDead;

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


        [Header("Enemie)")]
        [SerializeField] private List<GameObject> _enemies = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {
            _levelsManager = GameObject.FindWithTag("Managers").GetComponent<LevelsManager>();
            _enemyBaseHealthBar.maxValue = _maxHealth;
            _enemyBaseHealthBar.value = _maxHealth;
            _health = _maxHealth;
            _canSpawn = true;
            _levelsManager.ChangeLevel += ClearEnemies;
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
            if (_health <= 0 && !_isDead)
            {
                _isDead = true;
                StartCoroutine(Die());
            }
        }
        private IEnumerator SpawnSkeleton(float seconds)
        {
            _canSpawn = false;
            for (int i = 0; i<4; i++)
            {
                GameObject skeleton = Instantiate(_skeleton, _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length - 1)].transform.position, Quaternion.identity);
                _enemies.Add(skeleton);
                yield return new WaitForSeconds(2.2f);
            }
            yield return new WaitForSeconds(seconds);
            _canSpawn = true;
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(1.5f);
            _levelsManager.NextLevel();
            _levelText.text = "Level : " + _levelsManager.CurrentLevel;
            if (_levelsManager.CurrentLevel >= 1) _maxHealth += 1000 * _levelsManager.CurrentLevel * 2;
            else _maxHealth += 2500;
            _health = _maxHealth;
            _enemyBaseHealthBar.value = _health;
            if (_spawnRate > 3f) _spawnRate -= 0.1f;
            _canSpawn = true;
            _isDead = false;
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

        private void OnNextLevel()
        {

        }

        private void OnPreviousLevel()
        {

        }
    }
}
