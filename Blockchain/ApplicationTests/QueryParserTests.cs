using Application.Exceptions;
using Application.QueryHandler;
using Application.QueryHandler.Command;

namespace ApplicationTests;

[TestFixture]
public class QueryParserTests
{
    [SetUp]
    public void Setup()
    {
        _queryParser = new QueryParser();
    }

    private QueryParser _queryParser;

    [Test]
    public void Parse_SetDifficultyCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "SETDIFFICULTY 5";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.SETDIFFICULTY));
        Assert.That(result.Argument, Is.EqualTo("5"));
    }

    [Test]
    public void Parse_AddTransactionToMempoolCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL tx123.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.ADDTRANSACTIONTOMEMPOOL));
        Assert.That(result.Argument, Is.EqualTo("tx123.json"));
    }

    [Test]
    public void Parse_EvictMempoolCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "EVICTMEMPOOL 10";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.EVICTMEMPOOL));
        Assert.That(result.Argument, Is.EqualTo("10"));
    }

    [Test]
    public void Parse_MineBlockCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "MINEBLOCK";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithoutArgument_ReturnsCommandWithEmptyArgument()
    {
        // Arrange
        var query = "MINEBLOCK";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithArgument_ReturnsCommandWithArgument()
    {
        // Arrange
        var query = "SETDIFFICULTY 3";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.SETDIFFICULTY));
        Assert.That(result.Argument, Is.EqualTo("3"));
    }

    [Test]
    public void Parse_LowercaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "mineblock";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
    }

    [Test]
    public void Parse_MixedCaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "MineBlock";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
    }

    [Test]
    public void Parse_UppercaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        var query = "MINEBLOCK";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
    }

    [Test]
    public void Parse_CommandWithNumericArgument_ParsesCorrectly()
    {
        // Arrange
        var query = "SETDIFFICULTY 42";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("42"));
    }

    [Test]
    public void Parse_CommandWithStringArgument_ParsesCorrectly()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL transaction.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("transaction.json"));
    }

    [Test]
    public void Parse_CommandWithPathArgument_ParsesCorrectly()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL ./data/tx.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("./data/tx.json"));
    }

    [Test]
    public void Parse_CommandWithComplexArgument_ParsesCorrectly()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL C:\\Users\\data\\transaction123.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("C:\\Users\\data\\transaction123.json"));
    }

    [Test]
    public void Parse_EmptyString_ThrowsInvalidCommandException()
    {
        // Arrange
        var query = "";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_InvalidCommandName_ThrowsInvalidCommandException()
    {
        // Arrange
        var query = "INVALID_COMMAND";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_InvalidCommandWithArgument_ThrowsInvalidCommandException()
    {
        // Arrange
        var query = "INVALID_COMMAND arg";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_RandomString_ThrowsInvalidCommandException()
    {
        // Arrange
        var query = "not a command";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_CommandWithEmptyParentheses_ReturnsEmptyArgument()
    {
        // Arrange
        var query = "MINEBLOCK";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINEBLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithWhitespaceInArgument_PreservesWhitespace()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL my_transaction.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("my_transaction.json"));
    }

    [Test]
    public void Parse_CommandWithSpecialCharactersInArgument_ParsesCorrectly()
    {
        // Arrange
        var query = "ADDTRANSACTIONTOMEMPOOL tx-123_v2.json";

        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("tx-123_v2.json"));
    }

    [TestCase("SETDIFFICULTY 1", CommandType.SETDIFFICULTY, "1")]
    [TestCase("ADDTRANSACTIONTOMEMPOOL tx.json", CommandType.ADDTRANSACTIONTOMEMPOOL, "tx.json")]
    [TestCase("EVICTMEMPOOL 5", CommandType.EVICTMEMPOOL, "5")]
    [TestCase("MINEBLOCK", CommandType.MINEBLOCK, "")]
    public void Parse_AllCommandTypes_ReturnsCorrectCommand(string query, CommandType expectedType, string expectedArg)
    {
        // Act
        var result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(expectedType));
        Assert.That(result.Argument, Is.EqualTo(expectedArg));
    }

    [Test]
    public void Parse_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        var query = "SETDIFFICULTY 5";

        // Act
        var result1 = _queryParser.Parse(query);
        var result2 = _queryParser.Parse(query);

        // Assert
        Assert.That(result1.Type, Is.EqualTo(result2.Type));
        Assert.That(result1.Argument, Is.EqualTo(result2.Argument));
    }

    [Test]
    public void Parse_MultipleCallsWithDifferentInputs_ReturnsCorrectResults()
    {
        // Arrange
        var query1 = "SETDIFFICULTY 5";
        var query2 = "MINEBLOCK";

        // Act
        var result1 = _queryParser.Parse(query1);
        var result2 = _queryParser.Parse(query2);

        // Assert
        Assert.That(result1.Type, Is.EqualTo(CommandType.SETDIFFICULTY));
        Assert.That(result1.Argument, Is.EqualTo("5"));
        Assert.That(result2.Type, Is.EqualTo(CommandType.MINEBLOCK));
        Assert.That(result2.Argument, Is.EqualTo(""));
    }
}