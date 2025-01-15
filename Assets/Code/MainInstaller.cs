using Code.GlobeLayerSelectorFeature;
using Zenject;

namespace Code
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GlobeLayerSelectorController>().AsTransient();
        }
    }
}