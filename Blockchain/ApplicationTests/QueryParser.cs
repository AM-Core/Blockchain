using Application.QueryHandler;
using Application.Exceptions;
using Domain.Command;

namespace ApplicationTests;

[TestFixture]
public class QueryParserTests
{
    private QueryParser _queryParser;

    [SetUp]
    public void Setup()
    {
        _queryParser = new QueryParser();
    }

    #region Valid Command Tests

    [Test]
    public void Parse_SetDifficultyCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "SET_DIFFICULTY(5)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.SET_DIFFICULTY));
        Assert.That(result.Argument, Is.EqualTo("5"));
    }

    [Test]
    public void Parse_AddTransactionToMempoolCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(tx123.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.ADD_TRANSACTION_TO_MEMPOOL));
        Assert.That(result.Argument, Is.EqualTo("tx123.json"));
    }

    [Test]
    public void Parse_EvictMempoolCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "EVICT_MEMPOOL(10)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.EVICT_MEMPOOL));
        Assert.That(result.Argument, Is.EqualTo("10"));
    }

    [Test]
    public void Parse_MineBlockCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "MINE_BLOCK";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithoutArgument_ReturnsCommandWithEmptyArgument()
    {
        // Arrange
        string query = "MINE_BLOCK";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithArgument_ReturnsCommandWithArgument()
    {
        // Arrange
        string query = "SET_DIFFICULTY(3)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.SET_DIFFICULTY));
        Assert.That(result.Argument, Is.EqualTo("3"));
    }

    #endregion

    #region Case Sensitivity Tests

    [Test]
    public void Parse_LowercaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "mine_block";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
    }

    [Test]
    public void Parse_MixedCaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "Mine_Block";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
    }

    [Test]
    public void Parse_UppercaseCommand_ReturnsCorrectCommand()
    {
        // Arrange
        string query = "MINE_BLOCK";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
    }

    #endregion

    #region Argument Parsing Tests

    [Test]
    public void Parse_CommandWithNumericArgument_ParsesCorrectly()
    {
        // Arrange
        string query = "SET_DIFFICULTY(42)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("42"));
    }

    [Test]
    public void Parse_CommandWithStringArgument_ParsesCorrectly()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(transaction.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("transaction.json"));
    }

    [Test]
    public void Parse_CommandWithPathArgument_ParsesCorrectly()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(./data/tx.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("./data/tx.json"));
    }

    [Test]
    public void Parse_CommandWithComplexArgument_ParsesCorrectly()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(C:\\Users\\data\\transaction123.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("C:\\Users\\data\\transaction123.json"));
    }

    #endregion

    #region Invalid Command Tests

    [Test]
    public void Parse_EmptyString_ThrowsInvalidCommandException()
    {
        // Arrange
        string query = "";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_InvalidCommandName_ThrowsInvalidCommandException()
    {
        // Arrange
        string query = "INVALID_COMMAND";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_InvalidCommandWithArgument_ThrowsInvalidCommandException()
    {
        // Arrange
        string query = "INVALID_COMMAND(arg)";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    [Test]
    public void Parse_RandomString_ThrowsInvalidCommandException()
    {
        // Arrange
        string query = "not a command";

        // Act & Assert
        Assert.Throws<InvalidCommandException>(() => _queryParser.Parse(query));
    }

    #endregion

    #region Edge Cases

    [Test]
    public void Parse_CommandWithEmptyParentheses_ReturnsEmptyArgument()
    {
        // Arrange
        string query = "MINE_BLOCK()";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(CommandType.MINE_BLOCK));
        Assert.That(result.Argument, Is.EqualTo(""));
    }

    [Test]
    public void Parse_CommandWithWhitespaceInArgument_PreservesWhitespace()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(my transaction.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("my transaction.json"));
    }

    [Test]
    public void Parse_CommandWithSpecialCharactersInArgument_ParsesCorrectly()
    {
        // Arrange
        string query = "ADD_TRANSACTION_TO_MEMPOOL(tx-123_v2.json)";

        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Argument, Is.EqualTo("tx-123_v2.json"));
    }

    #endregion

    #region All Command Types Coverage

    [TestCase("SET_DIFFICULTY(1)", CommandType.SET_DIFFICULTY, "1")]
    [TestCase("ADD_TRANSACTION_TO_MEMPOOL(tx.json)", CommandType.ADD_TRANSACTION_TO_MEMPOOL, "tx.json")]
    [TestCase("EVICT_MEMPOOL(5)", CommandType.EVICT_MEMPOOL, "5")]
    [TestCase("MINE_BLOCK", CommandType.MINE_BLOCK, "")]
    public void Parse_AllCommandTypes_ReturnsCorrectCommand(string query, CommandType expectedType, string expectedArg)
    {
        // Act
        Command result = _queryParser.Parse(query);

        // Assert
        Assert.That(result.Type, Is.EqualTo(expectedType));
        Assert.That(result.Argument, Is.EqualTo(expectedArg));
    }

    #endregion

    #region Integration Tests

    [Test]
    public void Parse_MultipleCallsWithSameInput_ReturnsSameResult()
    {
        // Arrange
        string query = "SET_DIFFICULTY(5)";

        // Act
        Command result1 = _queryParser.Parse(query);
        Command result2 = _queryParser.Parse(query);

        // Assert
        Assert.That(result1.Type, Is.EqualTo(result2.Type));
        Assert.That(result1.Argument, Is.EqualTo(result2.Argument));
    }

    [Test]
    public void Parse_MultipleCallsWithDifferentInputs_ReturnsCorrectResults()
    {
        // Arrange
        string query1 = "SET_DIFFICULTY(5)";
        string query2 = "MINE_BLOCK";

        // Act
        Command result1 = _queryParser.Parse(query1);
        Command result2 = _queryParser.Parse(query2);

        // Assert
        Assert.That(result1.Type, Is.EqualTo(CommandType.SET_DIFFICULTY));
        Assert.That(result1.Argument, Is.EqualTo("5"));
        Assert.That(result2.Type, Is.EqualTo(CommandType.MINE_BLOCK));
        Assert.That(result2.Argument, Is.EqualTo(""));
    }

    #endregion
}
