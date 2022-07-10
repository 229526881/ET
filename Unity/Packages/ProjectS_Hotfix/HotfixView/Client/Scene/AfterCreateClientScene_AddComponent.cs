namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterCreateClientScene_AddComponent: AEvent<Scene, EventType.AfterCreateClientScene>
    {
        protected override async ETTask Run(Scene scene, EventType.AfterCreateClientScene args)
        {
            scene.AddComponent<FUIPackageManagerComponent>();
            scene.AddComponent<FUIManagerComponent>();

            await ETTask.CompletedTask;
        }
    }
}