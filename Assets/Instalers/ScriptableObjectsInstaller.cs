using UI;
using UnityEngine;
using Zenject;

namespace Injection
{
    [CreateAssetMenu(fileName = "ScriptableObjectsInstaller", menuName = "Installers/ScriptableObjectsInstaller")]
    public class ScriptableObjectsInstaller : ScriptableObjectInstaller<ScriptableObjectsInstaller>
    {
        [SerializeField] private UIConfiguration _uiConfiguration;
        [SerializeField] private UILayersConfiguration _layersConfiguration;

        public override void InstallBindings()
        {
            Container.Bind<UIConfiguration>().FromInstance(_uiConfiguration);
            Container.Bind<UILayersConfiguration>().FromInstance(_layersConfiguration);
        }
    }
}