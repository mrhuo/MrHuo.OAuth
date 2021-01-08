namespace MrHuo.OAuth
{
    public interface IStateManager
    {
        string RequestState();
        void RemoveState(string state);
        bool IsStateExists(string state);
    }
}
