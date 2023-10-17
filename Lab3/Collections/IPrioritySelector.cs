namespace Lab3.Collections;

public interface IPrioritySelector<T, TPriority>
{
    TPriority GetPriority(T item);
}
