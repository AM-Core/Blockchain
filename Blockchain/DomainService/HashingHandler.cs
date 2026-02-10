using System.Text;
using DataStructures;
using Domain;
using Domain.Transaction;
using Hash;

namespace DomainService;

public class HashingHandler
{
    public string ComputeHash(string data)
    {
        var hasher = new Fnv1AHash();
        var bytes = Encoding.UTF8.GetBytes(data);
        var hashBytes = hasher.ComputeHash(bytes);
        return ConvertToHex(hashBytes);
    }

    public string ComputeBlockHash(Block block)
    {
        var blockData = string.Concat(
            block.BlockHash,
            block.PrevBlockHash,
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

    public string ComputeTransactionHash(TransactionEntry transactionEntry)
    {
        var txData = new StringBuilder();
        txData.Append(transactionEntry.Id);

        foreach (var input in transactionEntry.Inputs) txData.Append(input);

        foreach (var output in transactionEntry.Outputs) txData.Append(output);

        txData.Append(transactionEntry.Fee.ToString("F8"));
        txData.Append(transactionEntry.Size.ToString());

        return ComputeHash(txData.ToString());
    }

    public string ComputeMerkleRoot(List<TransactionEntry> transactions)
    {
        if (transactions == null || transactions.Count == 0) return ComputeHash(string.Empty);

        var transactionHashes = transactions
            .Select(tx => ComputeTransactionHash(tx))
            .ToList();

        var merkleTree = new MerkleTree(transactionHashes);
        return merkleTree.Root;
    }

    private static string ConvertToHex(byte[] bytes)
    {
        var sb = new StringBuilder(bytes.Length * 2);
        foreach (var b in bytes) sb.Append(b.ToString("x2"));
        return sb.ToString();
    }
}