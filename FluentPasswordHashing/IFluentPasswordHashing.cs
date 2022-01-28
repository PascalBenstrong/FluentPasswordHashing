
namespace FluentPasswordHashing
{
    public interface IFluentPasswordHashing
    {
        IFluentPasswordHashing WithSaltLength(int saltLength);
        IFluentPasswordHashing WithDegreeOfParallelism(int degreeOfParallelism);
        IFluentPasswordHashing WithIterations(int iterations);
        IFluentPasswordHashing WithMemorySize(int memorySize);
        IFluentPasswordHashing WithHashLength(int hashLength);
        IFluentPasswordHashing WithAlgorithm(PasswordHashAlgorithm algorithm);

        IPasswordGenerator Generator();
    }
}
