using DataStructures;
using Domain.Exceptions;
using Domain.Transaction;

namespace DomainService;

public class FeeRateCalculator
{
    
    //public void CalculateParentFee(TransactionEntry transaction, DAG<TransactionEntry> dag)
    //{
    //    double parentFee = 0;
    //    var parentSize = 0;

    //    var dependencyList = dag.GetDependencies(transaction);
    //    foreach (var transactionEntry in dependencyList)
    //    {
    //        parentFee += transactionEntry.Fee;
    //        parentSize += transactionEntry.Size;
    //    }

    //    transaction.ParentFee = Math.Round(parentFee - transaction.Fee, 2);
    //    transaction.ParentSize = parentSize - transaction.Size;
    //}

    public void CalculateFee(TransactionEntry transaction,HashMap<String,TransactionEntry> map)
    {
        double inputTotal = 0;
        double outputTotal = 0;

        foreach (Input input in transaction.Inputs)
        {
            if (input.PrevId != null)
            {
                var transactionEntry = map.TryGet(input.PrevId);
                if (transactionEntry != null)
                {
                    inputTotal += transactionEntry.Outputs[input.PrevIndex].Value;
                }
                else
                {
                    return;
                }
            }
            else
            {
                transaction.Fee = 0;
                return;
            }

        }
    
        foreach (Output output in transaction.Outputs)
        {
            outputTotal += output.Value;
        }

        double calculatedFee = inputTotal - outputTotal;
        if (calculatedFee < 0)
        {
            throw new InvalidFeeException($"Transaction {transaction.Id} has negative fee: {calculatedFee:F2}. Outputs exceed inputs.");
        }

        transaction.Fee = Math.Round(calculatedFee, 2);
    }
}