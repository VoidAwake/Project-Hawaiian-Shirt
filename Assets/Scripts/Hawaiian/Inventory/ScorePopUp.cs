using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hawaiian.Inventory
{
    public class ScorePopUp : MonoBehaviour
    {
        public Transform parent;
        RectTransform rectTransform;
        float timer = 1.0f;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            AnimateText();
        }

        private void FixedUpdate()
        {
            SetPosition();
        }

        private void AnimateText()
        {
            if (rectTransform != null)
            {
                float scale = 1.075f + 0.15f * Mathf.Sin(timer * 5.0f);
                rectTransform.localScale = new Vector2(scale, scale);
            }

            timer -= Time.deltaTime;
            if (timer < 0.0f)
            {
                Destroy(gameObject);
            }
        }

        private void SetPosition()
        {
            if (parent != null && rectTransform != null)
            {
                rectTransform.position = parent.position + Vector3.up * 1.0f;
            }
        }

        public void SetUp(int scoreChange, Transform playerTransform)
        {
            parent = playerTransform;
            SetPosition();

            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
            text.text = (scoreChange > 0 ? "+" : "") + scoreChange;
            if (scoreChange < 0)
            {
                text.color = new Color(0.9f, 0.2f, 0.3f);
            }
        }
    }
}
