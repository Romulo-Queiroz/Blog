namespace Blog.ViewModels

{
    public class ResultViewModel<T>
    {
        public T Data { get; private set; }

        public List <string> Erros { get; private set; } = new();
    }
}