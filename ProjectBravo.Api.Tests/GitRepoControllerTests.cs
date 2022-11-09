using NSubstitute.ReturnsExtensions;


namespace ProjectBravo.Api.Tests
{
    public class GitRepoControllerTests
    {
        private readonly IGitRepoRepository _substituteRepo;
        private readonly GitRepoController _sut;

        public GitRepoControllerTests()
        {
            //Maybe we want to substitute the GitInsights class to?
            // that way we can actually control what the controller does?

            _substituteRepo = Substitute.For<IGitRepoRepository>();

            _sut = new GitRepoController(_substituteRepo);
        }

        [Fact]
        public void Frequency_given_fresh_or_unseen_repo()
        {
            // Arrange
            _substituteRepo.FindAsync(Arg.Any<string>()).ReturnsNull();

            // Act
            var res = _sut.GetFrequency("bhviid", "project-description");

            // Assert
            res.Should().Be()
        }
    }
}