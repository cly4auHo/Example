namespace UI
{
    public interface IUIManager
    {
        void Init();
        void Present(WidgetName widgetName);
        void PresentWithData<T>(WidgetName widgetName, T data);
    }
}
