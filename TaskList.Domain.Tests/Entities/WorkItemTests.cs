using TaskList.Domain.Entities;
using Xunit;

namespace TaskList.Domain.Tests.Entities;

public class WorkItemTests
{
    [Fact]
    public void Constructor_WhenCreatedAtIsInTheFuture_ThenThrowsArgumentException()
    {
        // Arrange
        var action = () => new WorkItem(Guid.NewGuid(), "title", "description", DateTime.UtcNow.AddDays(1));

        // Act
        var exception = Assert.Throws<ArgumentException>(action);

        // Assert
        Assert.Equal("createdAt", exception.ParamName);
        Assert.Equal("The creation date cannot be in the future. (Parameter 'createdAt')", exception.Message);
    }

    [Fact]
    public void Constructor_WhenTitleIsEmpty_ThenThrowsArgumentException()
    {
        // Arrange
        var action = () => new WorkItem(Guid.NewGuid(), string.Empty, "description", new DateTime(2025, 11, 20));

        // Act
        var exception = Assert.Throws<ArgumentException>(action);

        // Assert
        Assert.Equal("title", exception.ParamName);
        Assert.Equal("The value cannot be an empty string or composed entirely of whitespace. (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void Constructor_WhenTitleIsNull_ThenThrowsArgumentNullException()
    {
        // Arrange
        var action = () => new WorkItem(Guid.NewGuid(), null!, "description", new DateTime(2025, 11, 20));

        // Act
        var exception = Assert.Throws<ArgumentNullException>(action);

        // Assert
        Assert.Equal("title", exception.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'title')", exception.Message);
    }

    [Fact]
    public void IsComplete_WhenCompletedAtIsNull_ThenFalse()
    {
        // Act
        var sut = new WorkItem(Guid.NewGuid(), "title", "description", DateTime.UtcNow, null);

        // Assert
        Assert.False(sut.IsComplete);
    }

    [Fact]
    public void IsComplete_WhenCompletedAtIsNonNull_ThenTrue()
    {
        // Act
        var sut = new WorkItem(Guid.NewGuid(), "title", "description", DateTime.UtcNow, DateTime.UtcNow);

        // Assert
        Assert.True(sut.IsComplete);
    }
}