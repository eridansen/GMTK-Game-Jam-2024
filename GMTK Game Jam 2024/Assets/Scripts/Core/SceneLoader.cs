﻿using UnityEngine.SceneManagement;
using System.Collections;
using Helpers;

namespace Core
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        private string _loadingScene = Constants.Scenes.Loading;
        
        public void LoadSceneWithLoadingScreen(string targetSceneName)
        {
            StartCoroutine(LoadSceneWithLoadingScreenCoroutine(targetSceneName));
        }
        
        public void LoadSceneWithoutLoadingScreen(string targetSceneName)
        {
            StartCoroutine(LoadSceneWithoutLoadingScreenCoroutine(targetSceneName));
        }

        private IEnumerator LoadSceneWithLoadingScreenCoroutine(string targetSceneName)
        {
            yield return SceneManager.LoadSceneAsync(_loadingScene);

            var asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
        private IEnumerator LoadSceneWithoutLoadingScreenCoroutine(string targetSceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }

}