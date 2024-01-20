using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectClicker
{
    public class TimeManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        [Button]
        public void Pause()
        {
            Time.timeScale = 0;
        }
        [Button]
        public void Resume()
        {
            Time.timeScale = 1;
        }
        [Button]
        public void SpeedUp()
        {
            Time.timeScale = 2;
        }
        [Button]
        public void SpeedUpMore()
        {
            Time.timeScale = 4;
        }
    }
}
