using FluentAssertions;
using Xunit;
using System.Threading.Tasks;
using FluentPasswordHashing;
using FluentHashing = FluentPasswordHashing.FluentPasswordHashing;

namespace UnitTest.FluentPasswordHashing
{
    public class FluentPasswordHashingTests
    {
        [Fact]
        public async Task Generating_A_Hash_Should_Pass()
        {

            // arrange
            var generator = FluentHashing.Create()
                .WithSaltLength(16)
                .WithDegreeOfParallelism(1)
                .WithIterations(4)
                .WithMemorySize(16)
                .WithHashLength(16)
                .WithAlgorithm(PasswordHashAlgorithm.ARGON2ID)
                .Generator();

            var expectedHashLength = 72;
            var password = "password";

            // act
            var hash = await generator.Generate(password);


            // assert
            hash.Should().HaveLength(expectedHashLength);

        }

        [Theory]
        [InlineData("$argon2id$m=16,t=4,p=1$aM8qmYw1xfzvHEYTIg0w1g==$5Hg1fRhHv1uOnOMusawtqg==", "password")]
        public async Task Verifying_Password_And_Hash_Should_Pass(string hash, string password)
        {
            // arrange
            var generator = FluentHashing.Create()
                .Generator();

            // act
            var isMatch = await generator.VerifyPassword(hash, password);


            // assert
            isMatch.Should().BeTrue();
        }
    }
}