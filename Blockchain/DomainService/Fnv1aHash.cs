namespace DomainService;

public class Fnv1aHash
{
    public byte[] ComputeHash(byte[] data)
    {
        const uint fnvPrime = 16777619;
        const uint offsetBasis = 2166136261;
        uint hash = offsetBasis;
        foreach (byte b in data)
        {
            hash ^= b;
            hash *= fnvPrime;
        }
        return BitConverter.GetBytes(hash);
    }
}