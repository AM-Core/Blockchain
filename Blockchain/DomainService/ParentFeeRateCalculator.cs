using DataStructures;
using Domain.Transaction;

namespace DomainService;

public class ParentFeeRateCalculator
{
    public void CalculateParentFee(TransactionEntry transaction, DAG<TransactionEntry> dag)
    {
        double parentFee = 0;
        var parentSize = 0;

        var dependencyList = dag.GetDependencies(transaction);
        foreach (var transactionEntry in dependencyList)
        {
            parentFee += transactionEntry.Fee;
            parentSize += transactionEntry.Size;
        }

        transaction.ParentFee = parentFee - transaction.Fee;
        transaction.ParentSize = parentSize - transaction.Size;
    }
}