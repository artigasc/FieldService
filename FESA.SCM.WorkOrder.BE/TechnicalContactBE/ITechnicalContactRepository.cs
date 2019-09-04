using FESA.SCM.Common.Base;
using System.Collections.Generic;

namespace FESA.SCM.WorkOrder.BE.TechnicalContactBE
{
    public interface ITechnicalContactRepository : IBaseRepository<TechnicalContact>
    {        
        IList<TechnicalContact> GetContactByIdOrder(string orderId);

        void DeleteByOrderAndContactId(string orderId, string contactId);

    }
}