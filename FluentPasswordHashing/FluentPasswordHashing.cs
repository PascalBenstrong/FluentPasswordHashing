
namespace FluentPasswordHashing
{
    public static class FluentPasswordHashing
    {
        public static IFluentPasswordHashing Create()
            => new FluentPasswordHashingImpl();
    }
}
