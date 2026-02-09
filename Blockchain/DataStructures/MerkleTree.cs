using System.Security.Cryptography;
using System.Text;

namespace DataStructures
{
    public class MerkleTree
    {
        public string Root { get; private set; }

        public MerkleTree(List<string> dataBlocks)
        {
            if (dataBlocks == null || dataBlocks.Count == 0)
            {
                Root = Hash(string.Empty);
                return;
            }

            Root = BuildTree(dataBlocks);
        }

        string BuildTree(List<string> dataBlocks)
        {
            if (dataBlocks.Count == 1)
            {
                return Hash(dataBlocks[0]);
            }

            var parentHashes = new List<string>();

            for (int i = 0; i < dataBlocks.Count; i += 2)
            {
                var left = dataBlocks[i];
                var right = (i + 1 < dataBlocks.Count) ? dataBlocks[i + 1] : left;

                var combinedHash = Hash(left + right);
                parentHashes.Add(combinedHash);
            }

            return BuildTree(parentHashes);
        }

        string Hash(string data)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(data);
            var hashBytes = sha256.ComputeHash(bytes);
            return ConvertToHex(hashBytes);
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
}
