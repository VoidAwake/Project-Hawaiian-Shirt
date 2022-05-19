using System;
using System.Collections;
using System.Collections.Generic;
using Hawaiian.Utilities;
using TMPro;
using UnityEngine;

namespace Hawaiian.Camera
{
    [Serializable]
    public class WaypointPair
    {
        [SerializeField] private Waypoint _waypointOne;
        [SerializeField] private Waypoint _waypointTwo;

        public Waypoint[] GetWaypoints() => new[] {_waypointOne, _waypointTwo};
    }

    public class CinematicCamera : MonoBehaviour
    {
        public GameEvent LevelPreviewCompleted;
        public GameEvent GameStarted;

        [SerializeField] private GameObject _mainCamera;
        [SerializeField] private GameObject _parent;
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private List<WaypointPair> waypointPairs;
        [SerializeField] private Waypoint _outroWaypoint;

        [SerializeField] private float _step;
        [SerializeField] private float _sizeStep;

        [SerializeField] private float _endSize;
        [SerializeField] private TextMeshProUGUI _countdown;


        public void Awake()
        {
            _countdown.text = "";
            _camera = GetComponent<UnityEngine.Camera>();
        }

        public void StartIntroCinematic() => StartCoroutine(RunLevelPreviewCinematic());


        IEnumerator RunLevelPreviewCinematic()
        {
            foreach (WaypointPair _pair in waypointPairs)
                yield return StartCoroutine(LerpCameraToWaypointPairs(_pair, _step));

            yield return RunGameStart();
        }

        IEnumerator LerpCameraToWaypointPairs(WaypointPair pair, float time)
        {
            float elapsedTime = 0;
            Vector3 startPos = pair.GetWaypoints()[0].GetWaypointPosition;
            Vector3 endPos = pair.GetWaypoints()[1].GetWaypointPosition;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = pair.GetWaypoints()[1].GetWaypointPosition;
        }

        IEnumerator LerpCameraSize(float time)
        {
            float elapsedTime = 0;
            float startSize = _camera.orthographicSize - 2;
            float endSize = _endSize;


            while (elapsedTime < time)
            {
                _countdown.text = $"{(int) (time - elapsedTime)}";

                if ((int) (time - elapsedTime) == 0)
                    _countdown.text = "Begin!";

                _camera.orthographicSize = Mathf.Lerp(startSize, endSize,
                    (elapsedTime / time));

                elapsedTime += Time.deltaTime;
                yield return null;
            }


            _countdown.text = "Begin!";
            _camera.orthographicSize = _endSize;
            GameStarted.Raise();
            StartCoroutine(DelayStart());
        }

        IEnumerator LerpCamera(float time)
        {
            float elapsedTime = 0;
            Vector3 startPos = transform.position;
            Vector3 endPos = _mainCamera.transform.position;
            float startSize = _camera.orthographicSize;
            float endSize = _mainCamera.GetComponent<UnityEngine.Camera>().orthographicSize;

            while (elapsedTime < time)
            {
                transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
                _camera.orthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / time);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPos;
            _camera.orthographicSize = endSize;
            Destroy(_parent);
        }


        IEnumerator DelayStart()
        {
            yield return new WaitForSeconds(0.4f);
            StartCoroutine(LerpCamera(0.4f));
        }

        IEnumerator RunGameStart()
        {
            transform.position = _outroWaypoint.GetWaypointPosition;
            yield return StartCoroutine(LerpCameraSize(_sizeStep));
        }
    }
}