using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Transaction;

namespace Domain.Contracts
{
    public class TransactionDto
    {
        public string TxId { get; set; }

        public TransactionDto(string transactionId)
        {
            TxId = transactionId;
        }
    }
}
