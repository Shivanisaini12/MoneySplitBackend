using DomainLayer.Data;
using DomainLayer.DTO;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Icao;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.ICustomServices;
using Service_Layer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.CustomServices
{
    public class GroupService : Repository<Group>, IGroupService
    {
        private readonly RxSplitterContext _context;
        private readonly ISprocRepository _sprocRepo;

        public GroupService(RxSplitterContext context, ISprocRepository sprocRepo) : base(context)
        {
            _context = context;
            _sprocRepo = sprocRepo;
      
        }
        public override bool Delete(Group group)
        {
            try
            {
                group.UpdatedOn = DateTime.UtcNow;
                group.IsDeleted = true;
                _context.Groups.Update(group);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Group>> GetGroupDataWithMembersByGroupId(int GroupId)
        {
            var a = await _context.Groups.Where(x => x.Id == GroupId).Include(x=>x.GroupMembers).ToListAsync();
            return a;
        }

        public async Task<IEnumerable<SP_GetAllGroupsOfUser>> GetAllDetailGroupsOfUser(Guid UserId)
        {
            var obj =  _sprocRepo.GetStoredProcedure("SP_GetAllGroupsOfUser")
                .WithSqlParams(("UserId",UserId.ToString()))
                .ExecuteStoredProcedureAsync<SP_GetAllGroupsOfUser>();
            return obj.Result;
        }

        public async Task<IEnumerable<sp_GetAllGroups>> GetAllDetailGroups(Guid UserId)
        {
            var obj = _sprocRepo.GetStoredProcedure("sp_GETAllGroups")
                .WithSqlParams(("UserId", UserId.ToString()))
                .ExecuteStoredProcedureAsync<sp_GetAllGroups>();
            return obj.Result;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return _context.Categories.ToList();
        }

        public async Task<List<Currency>> GetAllCurrency()
        {
            return _context.Currencies.ToList();
        }
        public bool CreateMyExpense(Guid id)
        {
            Group group = new Group();
            group.GroupName = "My Expense";
            group.CurrencyId = 4;
            group.AddedBy = id;
           _context.Groups.Add(group);
        
            if (_context.SaveChanges()>0)
            {
               

                GroupMember gm = new GroupMember() { Email = _context.UserDetails.FirstOrDefault(x => x.Id == id).Email, GroupId = group.Id, UserId = id };
                _context.GroupMembers.Add(gm);
                _context.SaveChanges();

                MemberInvitation mi = new MemberInvitation() { MemberId = gm.Id, GroupId = group.Id, TokenGeneratedByUser = id, InvitationStatus = "1" };
                _context.MemberInvitations.Add(mi);
                _context.SaveChanges();
                return true;
            }
            throw new NotImplementedException();
        }
    }
}
