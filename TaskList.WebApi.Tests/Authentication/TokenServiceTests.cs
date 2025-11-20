using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Text;
using TaskList.WebApi.Authentication;
using Xunit;

namespace TaskList.WebApi.Tests.Authentication;

public class TokenServiceTests
{
    [Fact]
    public void CreateAccessToken_WhenCalledWithUserIdAndLifetime_ThenReturnsToken()
    {
        // Arrange
        var userId = "testUser";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f3078d22-bb0a-4d9d-ab19-e63f8f13295d"));
        var configMock = new Mock<IJwtConfiguration>();
        var sut = new TokenService(configMock.Object);

        configMock.Setup(x => x.SigningKey).Returns(key);

        // Act
        var result = sut.CreateAccessToken(userId, TimeSpan.FromMinutes(20));

        // Assert
        Assert.NotNull(result);
        configMock.VerifyGet(x => x.SigningKey);
    }
}