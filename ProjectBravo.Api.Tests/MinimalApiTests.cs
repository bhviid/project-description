using NSubstitute.ReturnsExtensions;
using ProjectBravo.Core;

namespace ProjectBravo.Api.Tests
{
    public class MinimalApiTests
    {
        private readonly IGitRepoRepository _substituteRepo;

        public MinimalApiTests()
        {
            _substituteRepo = Substitute.For<IGitRepoRepository>();
        }

        [Fact]
        public void Frequency_given_fresh_or_unseen_repo()
        {
            // Arrange
            _substituteRepo.FindAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var 

            // Assert
        }
    }
}