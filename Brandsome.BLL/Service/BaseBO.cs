using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brandsome.BLL.Utilities;
using Brandsome.DAL;

namespace Brandsome.BLL.Service
{
    public class BaseBO
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly NotificationHelper _notificationHelper;
        public BaseBO(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper)
        {
            _uow = unit;
            _mapper = mapper;
            _notificationHelper = notificationHelper;
        }
    }
}
