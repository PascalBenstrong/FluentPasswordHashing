
namespace FluentPasswordHashing
{

    internal class FluentPasswordHashingImpl : IFluentPasswordHashing
    {
        int _saltLength = 16;
        int _degreeOfParallelism = 1;
        int _iterations = 4;
        int _memorySize = 16;
        int _hashLength = 16;
        PasswordHashAlgorithm _algorithm = PasswordHashAlgorithm.ARGON2ID;

        public IPasswordGenerator Generator()
            => new PasswordGenerator(this);

        public IFluentPasswordHashing WithAlgorithm(PasswordHashAlgorithm algorithm)
        {
            _algorithm = algorithm;
            return this;
        }

        public IFluentPasswordHashing WithDegreeOfParallelism(int degreeOfParallelism)
        {
            _degreeOfParallelism = degreeOfParallelism;
            return this;
        }

        public IFluentPasswordHashing WithHashLength(int hashLength)
        {
            _hashLength = hashLength;
            return this;
        }

        public IFluentPasswordHashing WithIterations(int iterations)
        {
            _iterations = iterations;
            return this;
        }

        public IFluentPasswordHashing WithMemorySize(int memorySize)
        {
            _memorySize = memorySize;
            return this;
        }

        public IFluentPasswordHashing WithSaltLength(int saltLength)
        {
            _saltLength = saltLength;
            return this;
        }

        private class PasswordGenerator : IPasswordGenerator
        {
            private readonly FluentPasswordHashingImpl _fluent;

            internal PasswordGenerator(FluentPasswordHashingImpl fluent)
            {
                _fluent = fluent;
            }

            public async Task<string> Generate(string password)
            {
                PasswordHashArguments args = new(_fluent._algorithm, null, _fluent._degreeOfParallelism, _fluent._iterations, _fluent._memorySize, _fluent._hashLength, null);
                return await Utils.GenerateArgumentsWithHash(args,password, _fluent._saltLength);
            }

            public Task<bool> VerifyPassword(string hash, string password)
                => Utils.VerifyHashAsync(password, hash);
        }
    }
}
