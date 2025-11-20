using Microsoft.Extensions.Configuration;
using TaskList.WebApi.Authentication;
using Xunit;

namespace TaskList.WebApi.Tests.Configuration;

public class JwtConfigurationTests
{
    [Fact]
    public void Constructor_WhenCalledWithoutConfiguration_ThenThrowsArgumentNullException()
    {
        // Arrange
        var action = () => new JwtConfiguration(null!);

        // Act
        var exception = Assert.Throws<ArgumentNullException>(action);

        // Assert
        Assert.Equal("configuration", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenJwtSectionIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        var action = () => new JwtConfiguration(configuration);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Section 'Jwt' not found in configuration.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenJwtKeyIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var sectionName = "Jwt";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{sectionName}:Issuer"] = "Example Issuer"
            })
            .Build();
        var action = () => new JwtConfiguration(configuration, sectionName);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Missing configuration for 'Jwt:Key'.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenJwKeyExistsInConfiguration_ThenKeysAreSet()
    {
        // Arrange
        var sectionName = "Jwt";
        var expectedKeyValue = "test-jwt-key";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{sectionName}:Key"] = expectedKeyValue
            })
            .Build();

        // Act
        var sut = new JwtConfiguration(configuration, sectionName);

        // Assert
        Assert.Equal(expectedKeyValue, sut.Key);
        Assert.NotNull(sut.SigningKey);

    }
}