namespace RPG.Saving2
{
    public interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}