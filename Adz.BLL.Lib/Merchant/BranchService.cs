using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class BranchService : IBranchService
    {
        public Response<Branch> GetBranchById(int BranchId)
        {
            Response<Branch> response = null;
            Branch Branch = new Branch();
            using (var context = new TheAdzEntities())
            {
                var entityBranch = from d in context.Branches
                                     where d.id == BranchId
                                     select d;
                var v = entityBranch.First();
                if (v != null)
                {
                    Branch.BranchId = v.id;
                    Branch.AddressLine1 = v.address_line1;
                    Branch.AddressLine2 = v.address_line2;
                    Branch.City = new City { CityId = (int)v.city_id, Name = v.City.name };
                    Branch.Country = new Country { CountryId = (int)v.City.country_id, Name = v.City.Country.name };
                    Branch.LastAction = v.last_action;
                    Branch.Latitude = (double)v.latitude;
                    Branch.Longitude = (double)v.longitude;
                    Branch.PostCode = v.postcode;
                }
                response = Response<Branch>.Create(Branch);
            }

            return response;
        }

        public Response<Branch> CreateEditBranch(Branch Branch)
        {
            Response<Branch> response = null;
            using (var context = new TheAdzEntities())
            {
                if (Branch.BranchId != 0)
                {
                    var entityBranch = from d in context.Branches
                                         where d.id == Branch.BranchId
                                         select d;
                    if (entityBranch.Count() > 0)
                    {
                        entityBranch.First().address_line1 = Branch.AddressLine1;
                        entityBranch.First().address_line2 = Branch.AddressLine2;
                        entityBranch.First().city_id = Branch.City.CityId;
                        entityBranch.First().postcode = Branch.PostCode;
                        entityBranch.First().latitude = Branch.Latitude;
                        entityBranch.First().longitude = Branch.Longitude;
                        entityBranch.First().last_updated = DateTime.UtcNow;
                        entityBranch.First().last_action = "3";
                        context.SaveChanges();

                        response = Response<Branch>.Create(new Branch() { Success = true });                      
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.BranchNotFound);
                    }
                }
                else
                {
                    Adz.DAL.EF.Branch mmentity = new Adz.DAL.EF.Branch();
                    mmentity.address_line1 = Branch.AddressLine1;
                    mmentity.address_line2 = Branch.AddressLine2;
                    mmentity.city_id = Branch.City.CityId;
                    mmentity.postcode = Branch.PostCode;
                    mmentity.latitude = Branch.Latitude;
                    mmentity.longitude = Branch.Longitude;
                    mmentity.last_created = DateTime.UtcNow;
                    mmentity.last_updated = DateTime.UtcNow;
                    mmentity.last_action = "1";
                    mmentity.merchant_id = Branch.Merchant.MerchantId;
                    context.Branches.Add(mmentity);
                    context.SaveChanges();
                    int BranchId = mmentity.id;

                    response = Response<Branch>.Create(new Branch() { BranchId = BranchId, Success = true });
                }
            }

            return response;
        }

        public Response<bool> DeleteBranch(int BranchId)
        {
            Response<bool> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityBranch = from d in context.Branches
                                     where d.id == BranchId
                                     select d;
                if (entityBranch.Count() > 0)
                {
                    entityBranch.First().last_action = "5";
                    context.SaveChanges();
                    response = Response<bool>.Create(true);
                }
                else
                {
                    throw new CustomException(CustomErrorType.BranchFailedDelete);
                }
            }

            return response;
        }

    }
}
