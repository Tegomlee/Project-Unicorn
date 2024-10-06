using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectUnicorn.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public enum AnimationType
        {
            FromBottom,
            FromTop,
            FromLeft,
            FromRight,
            ToBottom,
            ToTop,
            ToLeft,
            ToRight,
        }

        [Serializable]
        private class AnimationProperties
        {
            public string AnimatorTriggerName;
            public AnimationType AnimationType;
        }

        [SerializeField] private Animator _animator;
        [SerializeField] private AnimationProperties[] _animationProperties = new AnimationProperties[8];

        public static event Action<Vector2> OnPlayerPositionChange;

        private void Awake()
        {
            // Search for other objects of the same type in the heirarchy and Destroy itself it one is already present.
            GameObject[] existingTransitions = GameObject.FindGameObjectsWithTag("SceneTransition");
            if (existingTransitions.Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }

            SceneSwitcher.OnSceneLoaded += LoadNewScene;
        }

        private void OnDestroy()
        {
            SceneSwitcher.OnSceneLoaded -= LoadNewScene;
        }

        private void LoadNewScene(string newSceneName, AnimationType fromType, AnimationType toType, Vector2 newPosition)
        {
            StartCoroutine(TransitionToScene(newSceneName, fromType, toType, newPosition));
        }

        private IEnumerator TransitionToScene(string sceneName, AnimationType fromType, AnimationType toType, Vector2 newPosition)
        {
            string fromTrigger = GetAnimatorTriggerFrom(fromType);
            _animator.SetTrigger(fromTrigger);

            yield return new WaitForSeconds(GetAnimationStateLength(fromTrigger));

            AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneName);

            while (!loadAsync.isDone)
            {
                yield return null;
            }

            OnPlayerPositionChange?.Invoke(newPosition);

            string toTrigger = GetAnimatorTriggerFrom(toType);
            _animator.SetTrigger(toTrigger);
        }

        private string GetAnimatorTriggerFrom(AnimationType animationType)
        {
            foreach (var property in _animationProperties)
            {
                if (property.AnimationType == animationType)
                {
                    return property.AnimatorTriggerName;
                }
            }
            Debug.LogError("No matching animation trigger found for: " + animationType);
            return string.Empty;
        }

        private float GetAnimationStateLength(string triggerName)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.length;
        }
    }
}