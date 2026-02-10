namespace Hash;

public class Fnv1AHash
{
    public byte[] ComputeHash(byte[] data)
    {
        const uint fnvPrime = 16777619;
        const uint offsetBasis = 2166136261;
        var hash = offsetBasis;
        foreach (var b in data)
        {
            hash ^= b;
            hash *= fnvPrime;
        }

        return BitConverter.GetBytes(hash);
    }
}