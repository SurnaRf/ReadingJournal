namespace ReadingJournal.Services
{
    public class StateContainer<T> where T : class
    {
        public T Value { get; set; }
        public event Action OnStateChange;

        public void SetValue(T value)
        {
            this.Value = value;
            NotifyStateChanged();
        }
        private void NotifyStateChanged() => OnStateChange?.Invoke();
    }
}
