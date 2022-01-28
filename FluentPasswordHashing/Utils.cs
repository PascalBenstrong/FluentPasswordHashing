namespace FluentPasswordHashing
{
    internal static class Utils
    {
        public static string Base64(byte[] bytes) => Convert.ToBase64String(bytes);
        public static byte[] Base64Decode(string base64String) => Convert.FromBase64String(base64String);

        private static Argon2 GetAlgorithm(byte[] password, PasswordHashAlgorithm algorithm = PasswordHashAlgorithm.ARGON2ID)
        {
            return algorithm switch
            {
                PasswordHashAlgorithm.ARGON2ID => new Argon2id(password),
                PasswordHashAlgorithm.ARGON2D => new Argon2d(password),
                PasswordHashAlgorithm.ARGON2I => new Argon2i(password),
                _ => throw new ArgumentOutOfRangeException(nameof(algorithm)),
            };
        }

        private static byte[] CreateSalt(int bytes = 16)
        {
            byte[] salt = new byte[bytes];
            RandomNumberGenerator.Create().GetNonZeroBytes(salt);
            return salt;
        }

        private static async ValueTask<byte[]> HashPassword(char[] password, PasswordHashArguments arguments)
        {
            var argon2 = GetAlgorithm(Encoding.UTF8.GetBytes(password), arguments.HashAlgorithm);
            argon2.Salt = arguments.Salt;
            argon2.DegreeOfParallelism = arguments.DegreeOfParallelism;
            argon2.Iterations = arguments.Iterations;
            argon2.MemorySize = arguments.MemorySize;

            return await argon2.GetBytesAsync(arguments.HashLength).ConfigureAwait(false);
        }


        internal static async ValueTask<PasswordHashArguments> GenerateArgumentsWithHash(PasswordHashArguments arguments, char[] password, int saltLength = 16)
        {
            var salt = arguments.Salt ?? CreateSalt(saltLength);
            var hash = await HashPassword(password,arguments).ConfigureAwait(false);

            arguments.Salt = salt;
            arguments.Hash = hash;
            return arguments;
        }

        internal static ValueTask<PasswordHashArguments> GenerateArgumentsWithHash(PasswordHashArguments arguments, string password, int saltLength = 16)
            => GenerateArgumentsWithHash(arguments, password.ToCharArray(), saltLength);

        internal static async Task<bool> VerifyHashAsync(PasswordHashArguments arguments, char[] password)
        {
            var newHash = await GenerateArgumentsWithHash(arguments, password);
            return arguments.Hash.SequenceEqual(newHash.Hash);
        }

        internal static async Task<bool> VerifyHashAsync(string password, string hash)
            => await VerifyHashAsync(PasswordHashArguments.From(hash), password.ToCharArray());

    }
}
