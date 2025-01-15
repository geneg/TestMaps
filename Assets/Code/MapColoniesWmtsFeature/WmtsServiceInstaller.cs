using UnityEngine;
using Zenject;

namespace Code
{
    public class WmtsServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Debug.Log("MapColoniesServiceInstaller install");
            Container.BindInterfacesAndSelfTo<WmtsRequestBuilder>().AsTransient();
            Container.BindInterfacesAndSelfTo<WmtsNetworkService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<WmtsController>().AsSingle();
            Debug.Log("MapColoniesService installed");
        }
    }
}