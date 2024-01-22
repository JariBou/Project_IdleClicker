using UnityEngine;
using TMPro;
using ProjectClicker.Core;

namespace ProjectClicker
{
    public class GoldAnim : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _goldText;

    // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Vector2.Lerp(transform.position, transform.position + Vector3.up * 3, Time.deltaTime);
        }

        public void Initialize(decimal x)
        {
            _goldText.text = Utils.NumberToString(x);
        }

        public void Die()
        {
            Destroy(gameObject);
        }
        
    }
}
