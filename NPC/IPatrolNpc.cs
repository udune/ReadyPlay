public interface IPatrolNpc
{
    public void OnStart();
    public void OnEnd();
    public void OnUpdate(float deltaTime);
    public void SetIsSearchPlayer(bool isSearchPlayer);
}
