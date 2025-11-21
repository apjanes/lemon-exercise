using Microsoft.Extensions.Configuration;
using TaskList.WebApi.Authentication;
using Xunit;

namespace TaskList.WebApi.Tests.Configuration;

public class JwtConfigurationTests
{
    [Fact]
    public void Constructor_WhenAllDataInConfiguration_ThenKeysAreSet()
    {
        // Arrange
        var sectionName = "Jwt";
        var expectedKey = "test-jwt-key";
        var expectedIssuer = "Issuer";
        var expectedAudience = "Audience";
        var configuration = CreateConfiguration(sectionName, expectedKey, expectedIssuer, expectedAudience);

        // Act
        var sut = new JwtConfiguration(configuration, sectionName);

        // Assert
        Assert.Equal(expectedKey, sut.Key);
        Assert.Equal(expectedIssuer, sut.Issuer);
        Assert.Equal(expectedAudience, sut.Audience);
        Assert.NotNull(sut.SigningKey);

    }

    [Fact]
    public void Constructor_WhenAudienceIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var sectionName = "Jwt";
        var configuration = CreateConfiguration(sectionName, Guid.NewGuid().ToString(), "issuer", null);
        var action = () => new JwtConfiguration(configuration, sectionName);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Missing configuration for 'Jwt:Audience'.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenConfigrationIsMissing_ThenThrowsArgumentNullException()
    {
        // Arrange
        var action = () => new JwtConfiguration(null!);

        // Act
        var exception = Assert.Throws<ArgumentNullException>(action);

        // Assert
        Assert.Equal("configuration", exception.ParamName);
    }

    [Fact]
    public void Constructor_WhenIssuerIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var sectionName = "Jwt";
        var configuration = CreateConfiguration(sectionName, Guid.NewGuid().ToString(), null, "audience");
        var action = () => new JwtConfiguration(configuration, sectionName);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Missing configuration for 'Jwt:Issuer'.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenJwtSectionIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var configuration = CreateConfiguration(null, null, null, null);
        var action = () => new JwtConfiguration(configuration);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Section 'Jwt' not found in configuration.", exception.Message);
    }

    [Fact]
    public void Constructor_WhenKeyIsMissingFromConfiguration_ThenThrowsInvalidOperationException()
    {
        // Arrange
        var sectionName = "Jwt";
        var configuration = CreateConfiguration(sectionName, null, "issuer", "audience");
        var action = () => new JwtConfiguration(configuration, sectionName);

        // Act
        var exception = Assert.Throws<InvalidOperationException>(action);

        // Assert
        Assert.Equal("Missing configuration for 'Jwt:Key'.", exception.Message);
    }

    private static IConfiguration CreateConfiguration(
        string? sectionName,
        string? key,
        string? issuer,
        string? audience)
    {
        var buider = new ConfigurationBuilder();

        if (sectionName != null)
        {
            buider.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{sectionName}:Key"] = key,
                [$"{sectionName}:Issuer"] = issuer,
                [$"{sectionName}:Audience"] = audience
            });
        }

        var result = buider.Build();

        return result;
    }
}