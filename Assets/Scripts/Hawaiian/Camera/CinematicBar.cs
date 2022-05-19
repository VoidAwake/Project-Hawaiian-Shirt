using System;
using Hawaiian.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hawaiian.Camera
{
    public class CinematicBar : MonoBehaviour
    {
        [SerializeField] private float _step;

        private RectTransform _topBar;
        private RectTransform _bottomBar;

        private float size;
        private bool flag;


        private void Awake()
        {
            GenerateBars();
        }

        private void GenerateBars()
        {
            GenerateTopBar();
            GenerateBottomBar();
            Show(300, 2f);
        }

        private void Start()
        {
        }

        private void GenerateTopBar()
        {
            GameObject gameObject = new GameObject("Top_cinBar", typeof(Image));

            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            _topBar = gameObject.GetComponent<RectTransform>();

            //Set it to the top area of the camera
            _topBar.anchorMin = new Vector2(0, 1);
            _topBar.anchorMax = new Vector2(1, 1);
            _topBar.sizeDelta = new Vector2(0, 300);
        }

        private void GenerateBottomBar()
        {
            GameObject gameObject = new GameObject("Bot_cinBar", typeof(Image));
            gameObject.transform.SetParent(transform, false);
            gameObject.GetComponent<Image>().color = Color.black;
            _bottomBar = gameObject.GetComponent<RectTransform>();

            //Set it to the top area of the camera
            _bottomBar.anchorMin = new Vector2(0, 0);
            _bottomBar.anchorMax = new Vector2(1, 0);
            _bottomBar.sizeDelta = new Vector2(0, 300);
        }


        private void Update()
        {
            if (flag)
            {
                Vector2 sizeDelta = _topBar.sizeDelta;
                sizeDelta.y += _step * Time.deltaTime;

                if (_step > 0)
                {
                    if (sizeDelta.y >= size)
                    {
                        sizeDelta.y = size;
                        flag = false;
                    }
                    
                   
                }  else 
                {
                    
                    if (sizeDelta.y <= size)
                    {
                        sizeDelta.y = size;
                        flag = false;
                    }
               
                }


                _topBar.sizeDelta = sizeDelta;
                _bottomBar.sizeDelta = sizeDelta;
            }
        }

        public void Show(float size, float time)
        {
            this.size = size;
            _step = (size - _topBar.sizeDelta.y) / time;
            flag = true;
        }

        public void Hide(float time)
        {
            size = 0;
            _step = (size - _topBar.sizeDelta.y) / time;
            flag = true;
        }

        public void Hide()
        {
            Hide(0.5f);
        }
    }
}