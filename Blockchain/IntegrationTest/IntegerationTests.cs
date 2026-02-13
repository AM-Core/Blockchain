using System.Text.Json;
using Application.MiningApplication;
using ConsoleApp.Bootstrap;
using Domain.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest;

[TestFixture]
public class IntegrationTests
{
    [OneTimeSetUp]
    public void Cleanupfirst()
    {
        var based = AppDomain.CurrentDomain.BaseDirectory;
        var solution = Directory.GetParent(based)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var result = Path.Combine(solution, "Results");
        foreach (var file in Directory.GetFiles(result)) File.Delete(file);
    }

    [SetUp]
    public void Setup()
    {
        var provider = DependencyBootstrapper.ConfigureServices();
        _handler = provider.GetRequiredService<ApplicationHandler>();
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        _testDataPath = Path.Combine(solutionRoot, "IntegrationTest");
    }

    [TearDown]
    public void Cleanup()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var solutionRoot = Directory.GetParent(baseDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        var resultDir = Path.Combine(solutionRoot, "Results");
        foreach (var file in Directory.GetFiles(resultDir)) File.Delete(file);
    }

    private ApplicationHandler _handler;
    private string _testDataPath;
    private string _resultPath;

    [Test]
    public void Test1()
    {
        BlockCompareMainTest("Test1.txt", "Test1", "block_0000002a.json");
    }


    [Test]
    public void Test2()
    {
        BlockCompareMainTest("Test2.txt", "Test2", "block_0000002a.json");
    }

    [Test]
    public void Test3()
    {
        BlockCompareMainTest("Test3.txt", "Test3", "block_0000002a.json");
    }

    [Test]
    public void Test4()
    {
        MempoolCompareMainTest("Test4.txt", "Test4", "mempool_asc_134153834104643343.json");
    }

    [Test]
    public void Test5()
    {
        BlockCompareMainTest("Test5.txt", "Test5", "block_0000002a.json");
    }

    [Test]
    public void Test6()
    {
        BlockCompareMainTest("Test6.txt", "Test6", "block_0000002a.json");
    }

    private string GetLatestBlockFile()
    {
        var solutionRoot = Directory.GetCurrentDirectory();
        var resultDir = Path.Combine(solutionRoot, "Results");
        if (!Directory.Exists(resultDir)) return null;

        return Directory.GetFiles(resultDir, "block_*.json")
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault();
    }

    private string GetLatestMempoolFile()
    {
        var solutionRoot = Directory.GetCurrentDirectory();
        var resultDir = Path.Combine(solutionRoot, "Results");
        if (!Directory.Exists(resultDir)) return null;

        return Directory.GetFiles(resultDir, "mempool_*.json")
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault();
    }

    private void BlockCompareMainTest(string testCommandFile, string testResultFolder, string testResultFile)
    {
        _resultPath = Path.Combine(_testDataPath, "TestResult", testResultFolder);
        var commandFile = Path.Combine(_testDataPath, "TestCommands", testCommandFile);
        var expectedBlockFile = Path.Combine(_resultPath, testResultFile);
        var commands = File.ReadAllLines(commandFile);
        string actualBlockPath = null;
        foreach (var cmd in commands)
        {
            if (cmd.StartsWith("MineBlock")) actualBlockPath = GetLatestBlockFile();

            try
            {
                _handler.Handle(cmd.Trim());
            }
            catch (Exception e)
            {
            }
        }

        actualBlockPath ??= GetLatestBlockFile();

        Assert.That(File.Exists(actualBlockPath), Is.True, "Block file not created");

        var expectedBlock = JsonSerializer.Deserialize<BlockDto>(File.ReadAllText(expectedBlockFile));
        var actualBlock = JsonSerializer.Deserialize<BlockDto>(File.ReadAllText(actualBlockPath));

        Assert.That(actualBlock.Header.Difficulty, Is.EqualTo(expectedBlock.Header.Difficulty),
            "Block difficulty does not match");
        Assert.That(actualBlock.Header.BlockHash, Is.EqualTo(expectedBlock.Header.BlockHash),
            "Block hash does not match");
        Assert.That(actualBlock.Transactions.Count, Is.EqualTo(expectedBlock.Transactions.Count),
            "Transaction count does not match");

        for (var i = 0; i < expectedBlock.Transactions.Count; i++)
            Assert.That(actualBlock.Transactions[i].TxId, Is.EqualTo(expectedBlock.Transactions[i].TxId),
                $"Transaction at index {i} does not match");
    }

    private void MempoolCompareMainTest(string testCommandFile, string testResultFolder, string testResultFile)
    {
        _resultPath = Path.Combine(_testDataPath, "TestResult", testResultFolder);
        var commandFile = Path.Combine(_testDataPath, "TestCommands", testCommandFile);
        var expectedBlockFile = Path.Combine(_resultPath, testResultFile);
        var commands = File.ReadAllLines(commandFile);
        string actualBlockPath = null;

        foreach (var cmd in commands)
            try
            {
                _handler.Handle(cmd.Trim());
            }
            catch (Exception e)
            {
            }

        actualBlockPath = GetLatestMempoolFile();

        Assert.That(File.Exists(actualBlockPath), Is.True, "Mempool file not created");

        var expectedBlock = JsonSerializer.Deserialize<MempoolDto>(File.ReadAllText(expectedBlockFile));
        var actualBlock = JsonSerializer.Deserialize<MempoolDto>(File.ReadAllText(actualBlockPath));
        Assert.That(actualBlock.Transactions.Count, Is.EqualTo(expectedBlock.Transactions.Count),
            "Transaction count does not match");

        for (var i = 0; i < expectedBlock.Transactions.Count; i++)
            Assert.That(actualBlock.Transactions[i].TxId, Is.EqualTo(expectedBlock.Transactions[i].TxId),
                $"Transaction at index {i} does not match");
    }
}