using AutoMapper;
using DomainLayer.Data;
using DomainLayer.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.ICustomServices;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;




namespace Service_Layer.CustomServices
{
    public class TransactionService : Repository<Transaction>, ITransactionService
    {
        private readonly RxSplitterContext _context;
        private readonly IMapper _mapper;
        private readonly ISprocRepository _sprocRepo;

      
        public TransactionService(RxSplitterContext context, IMapper mapper, ISprocRepository sprocRepo) : base(context)
        //public ExpenseService(RxSplitterContext context) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _sprocRepo = sprocRepo;
        }

        public bool AcceptTransaction(Guid UserId, int TransactionId)
        {
            var trans = _context.Transactions.FirstOrDefault(x=>x.Id==TransactionId);
            if (trans!=null)
            {
                trans.Status = "1";
                trans.UpdatedOn= DateTime.UtcNow;
                var groupDetails = _context.Summaries.First(x => x.ParticipantId == trans.PaidFromMemberId);
                var paidFrom = _context.Summaries.First(x => x.GroupId == groupDetails.GroupId && x.IsActive == true && x.ParticipantId == trans.PaidFromMemberId);
                var paidTO = _context.Summaries.First(x => x.GroupId == groupDetails.GroupId && x.IsActive == true && x.ParticipantId == trans.PaidToMemberId);

                if (trans.Amount > 0)
                {
                    paidFrom.RemainingAmount = paidFrom.RemainingAmount + trans.Amount;


                    paidTO.RemainingAmount = paidTO.RemainingAmount - trans.Amount;
                }
                else
                {
                    paidFrom.RemainingAmount = paidFrom.RemainingAmount - trans.Amount;


                    paidTO.RemainingAmount = paidTO.RemainingAmount + trans.Amount;
                }
                _context.Update(paidFrom);
                _context.Update(paidTO);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> AddTransaction(Guid UserId, int GroupId)
        {
            var obj = _sprocRepo.GetStoredProcedure("SP_AddSettleUpTransactionDetail")
               .WithSqlParams(
               (nameof(UserId), UserId),
               (nameof(GroupId), GroupId))
               .ExecuteStoredProcedureAsync<SP_GetAllExpensesDetailsAccGroupId>();
            return true;
        }
        public async Task<bool> AddTransactionbyCsharp(int groupId, SP_GetSettleUpPaymentData expenseDTO)
        {
            //string json = System.Text.Json.JsonSerializer.Serialize(expense);
           // SP_GetSettleUpPaymentData expenseDTO = JsonConvert.DeserializeObject<SP_GetSettleUpPaymentData>(json);

            Summary summary = new Summary();
            var paidFrom = _context.Summaries.First(x => x.GroupId == groupId && x.IsActive == true && x.ParticipantId == expenseDTO.PaidById);
            var paidTO = _context.Summaries.First(x => x.GroupId == groupId && x.IsActive == true && x.ParticipantId == expenseDTO.PaidToId);

            //if (expenseDTO.Amount>0)
            //{
            //    paidFrom.RemainingAmount = paidFrom.RemainingAmount + expenseDTO.Amount;


            //    paidTO.RemainingAmount = paidTO.RemainingAmount - expenseDTO.Amount;
            //}
            //else
            //{
            //    paidFrom.RemainingAmount = paidFrom.RemainingAmount - expenseDTO.Amount;


            //    paidTO.RemainingAmount = paidTO.RemainingAmount + expenseDTO.Amount;
            //}

            Transaction transaction = new Transaction();
            transaction.Amount = expenseDTO.Amount;
            transaction.PaidFromMemberId = paidFrom.ParticipantId;
            transaction.PaidToMemberId = paidTO.ParticipantId;
            transaction.AddedBy= paidFrom.ParticipantId;
            //using (var context = new RxSplitterContext())
            //{
            //    using (IDbContextTransaction trans = context.Database.BeginTransaction())
            //    {
            //        try
            //        {
                        _context.Transactions.Add(transaction);
                        //_context.Update(paidFrom);
                        //_context.Update(paidTO);
                        _context.SaveChanges();

            //            trans.Commit();
            //        }
            //        catch (Exception ex)
            //        {
            //            trans.Rollback();
            //            Console.WriteLine("Error occurred.");
            //        }
            //    }
            //}
            //var obj = _unitOfWork.Expense.GetAllExpensesAccGroupId(GroupId);
            //int paidBy = expenseDTO.PaidBy;
            //decimal totalAMount = expenseDTO.Amount;
            return true;
        }

        public async Task<SP_GetSettleUpPaymentData> GetSettleUpPaymentData(Guid UserId, int GroupId)
        {
            var obj = _sprocRepo.GetStoredProcedure("SP_GetSettleUpPaymentData")
              .WithSqlParams(
              (nameof(UserId), UserId),
              (nameof(GroupId), GroupId))
              .ExecuteStoredProcedureAsync<SP_GetSettleUpPaymentData>();
            return obj.Result[0];
        }

        public async Task<IEnumerable<SP_GetAllTransactionDetailsAccGroupId>> GetTransactionDataAccGroupId(Guid UserId, int GroupId)
        {
            var obj = _sprocRepo.GetStoredProcedure("SP_GetAllTransactionDetailsAccGroupId")
              .WithSqlParams(
              (nameof(UserId), UserId),
              (nameof(GroupId), GroupId))
              .ExecuteStoredProcedureAsync<SP_GetAllTransactionDetailsAccGroupId>();
            return obj.Result;
        }

        public async Task<IEnumerable<SP_GetExpenseSummaryDetailAccGroupId>> GetExpenseSummaryDetailAccGroupId(Guid UserId, int GroupId)
        {
            var obj = _sprocRepo.GetStoredProcedure("SP_GetExpenseSummaryDetailAccGroupId")
              .WithSqlParams(
              (nameof(UserId), UserId),
              (nameof(GroupId), GroupId))
              .ExecuteStoredProcedureAsync<SP_GetExpenseSummaryDetailAccGroupId>();
            return obj.Result;
        }
    }
}
