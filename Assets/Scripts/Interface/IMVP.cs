public interface IView
{
    void Show();
    void Hide();
}

public interface IPresenter
{
    void Initialize();
    void UpdateView();
}