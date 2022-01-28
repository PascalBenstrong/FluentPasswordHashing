namespace FluentPasswordHashing
{
    internal class PasswordHashArguments
    {
        /// <summary>
        /// Argon2 hash algorithm to use
        /// </summary>
        public PasswordHashAlgorithm HashAlgorithm { get; private set; }
        internal byte[] Salt { get; set; }

        /// <summary>
        /// Number of virtual cores to use for parallelization
        /// </summary>
        public int DegreeOfParallelism { get; set; }
        public int Iterations { get; set; }
        /// <summary>
        /// How much memory to consume when hashing in Kilobytes (KB)
        /// </summary>
        public int MemorySize { get; set; }
        /// <summary>
        /// The length of the hash generated, a number from 4 to 100
        /// </summary>
        public int HashLength { get; set; }

        /// <summary>
        /// The hash generated for the provided password and hash arguments
        /// <seealso cref=""/>
        /// </summary>
        public byte[] Hash { get; internal set; }

        internal PasswordHashArguments(PasswordHashAlgorithm hashAlgorithm = PasswordHashAlgorithm.ARGON2ID
            , byte[] salt = default, int degreeOfParallelism = 1, int iterations = 4, int memorySize = 16
            , int hashLength = 16, byte[] hash = default)
        {
            HashAlgorithm = hashAlgorithm;
            Salt = salt;
            DegreeOfParallelism = degreeOfParallelism;
            Iterations = iterations;
            MemorySize = memorySize;
            HashLength = hashLength;
            Hash = hash;

        }

        internal static PasswordHashArguments From(string hash)
        {
            return hash;
        }

        public override string ToString()
        {
            var hash = $"${HashAlgorithm.GetString()}$m={MemorySize},t={Iterations},p={DegreeOfParallelism}";
            hash += $"${Utils.Base64(Salt)}${Utils.Base64(Hash)}";
            return hash;
        }

        public static implicit operator string(PasswordHashArguments arguments)
            => arguments.ToString();

        /// <summary>
        /// Uses formated hash string and returns a <see cref="PasswordHashArguments"/>.
        /// hash format: $algorithm$memorySize=integer,iterations=integer,degreeOfParallelism=integer$salt$hash
        /// </summary>
        /// <param name="hashString"></param>

        public static implicit operator PasswordHashArguments(string hashString)
        {
            if (!hashString.StartsWith("$"))
                throw new ArgumentException($"The hash is of invalid format, {nameof(hashString)}");

            hashString = hashString.Substring(1); // get the remaing characters from index 1 to the end
            var args = hashString.Split('$');

            if (args.Length != 4)
                throw new ArgumentException($"The hash is of invalid format, {nameof(hashString)}");

            var algorithm = args[0].FromStringPasswordHashAlgorithm();
            var salt = Utils.Base64Decode(args[2]);
            var hash = Utils.Base64Decode(args[3]);

            var paramters = args[1].Split(',');

            if (paramters.Length != 3)
                throw new ArgumentException($"The hash is of invalid format, {nameof(hashString)}");

            var equalSeperator = new char[] { '=' };
            var memory = int.Parse(paramters[0].Split(equalSeperator)[1]);
            var iterations = int.Parse(paramters[1].Split(equalSeperator)[1]);
            var degreeOfParallelism = int.Parse(paramters[2].Split(equalSeperator)[1]);

            return new(salt: salt, hash: hash)
            {
                HashAlgorithm = algorithm,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memory,
                HashLength = hash.Length

            };

        }
    }
}
