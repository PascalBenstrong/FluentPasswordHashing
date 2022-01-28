
namespace FluentPasswordHashing
{
    public static class FluentHashing
    {
        public static IFluentPasswordHashing Create()
            => new FluentPasswordHashingImpl();
    }
}
