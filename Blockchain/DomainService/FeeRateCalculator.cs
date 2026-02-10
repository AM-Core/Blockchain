using DataStructures;
using Domain.Exceptions;
using Domain.Transaction;

namespace DomainService;

public class FeeRateCalculator
{
    public void CalculateFee(TransactionEntry transaction, HashMap<string, TransactionEntry> map)
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
        if (inputTotal > 0 && calculatedFee < 0)
        {
            throw new InvalidFeeException($"{transaction.Id}negative fee exception");
        }
        else if (inputTotal == 0 && calculatedFee < 0)
        {
            transaction.Fee = 0;
            return;
        }

        transaction.Fee = Math.Round(calculatedFee, 2);
    }
}