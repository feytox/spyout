namespace Utils
{
    public class Switcher<T>
    {
        private T first;
        private T second;
        bool isFirstSelected = false;

        public Switcher(T first, T second)
        {
            this.first = first;
            this.second = second;
        }

        public T Current => isFirstSelected ? first : second;

        /// <summary>
        /// Switches the current value with the other one
        /// </summary>
        /// <returns>Value that was switched to. On the first call it returns the first value</returns>
        public T Switch()
        {
            isFirstSelected = !isFirstSelected;
            return Current;
        }
    }
}
