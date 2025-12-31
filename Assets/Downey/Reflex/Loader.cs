using UnityEngine;
using UnityEngine.SceneManagement;
using Reflex.Core;

namespace Downey.Reflex
{
    public class Loader : MonoBehaviour
    {
        void Start()
        {
            void InstallExtra(Scene scene, ContainerBuilder builder)
            {
                builder.AddSingleton("Beautiful");
            }
            //var scene = SceneManager.LoadScene("Reflex",new LoadSceneParameters(LoadSceneMode.Single));
            SceneScope.OnSceneContainerBuilding += InstallExtra;

            SceneManager.LoadSceneAsync("Reflex").completed += operate =>
            {
                SceneScope.OnSceneContainerBuilding -= InstallExtra;
            };
        }
    }
}