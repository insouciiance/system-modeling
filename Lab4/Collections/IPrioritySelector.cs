namespace Lab4.Collections;

public interface IPrioritySelector<T, TPriority>
{
    TPriority GetPriority(T item);
}
