using Domain;
using Domain.Interfaces;
using Domain.Transaction;
using DataStructures;
using System.Security.Cryptography;
using System.Text;

namespace DomainService;

public class HashingHandler : IHashingHandler
{
    public string ComputeHash(string data)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = sha256.ComputeHash(bytes);
        return ConvertToHex(hashBytes);
    }

    public string ComputeBlockHash(Block block)
    {
        var blockData = string.Concat(
            block.BlockHost,
            block.PrevBlockHost,
            block.Difficulty.ToString(),
            block.Nonce.ToString(),
            block.MerkleRoot
        );
        return ComputeHash(blockData);
    }

    public bool VerifyHash(string data, string hash)
    {
        var computedHash = ComputeHash(data);
        return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
    }

    public string ComputeTransactionHash(Transaction transaction)
    {
        var txData = new StringBuilder();
        txData.Append(transaction.Id);
        
        foreach (var input in transaction.Inputs)
        {
            txData.Append(input.ToString());
        }
        
        foreach (var output in transaction.Outputs)
        {
            txData.Append(output.ToString());
        }
        
        txData.Append(transaction.Fee.ToString("F8"));
        txData.Append(transaction.Size.ToString());
        
        return ComputeHash(txData.ToString());
    }

    public string ComputeMerkleRoot(List<Transaction> transactions)
    {
        if (transactions == null || transactions.Count == 0)
        {
            return ComputeHash(string.Empty);
        }

        var transactionHashes = transactions
            .Select(tx => ComputeTransactionHash(tx))
            .ToList();

        var merkleTree = new MerkleTree(transactionHashes);
        return merkleTree.Root;
    }

    private static string ConvertToHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}