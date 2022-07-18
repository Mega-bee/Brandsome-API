using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brandsome.BLL.Utilities;
using Brandsome.DAL;
using Brandsome.BLL.Utilities.Logging;

namespace Brandsome.BLL.Services
{
    public class BaseBO
    {
        protected readonly IUnitOfWork _uow;
        protected readonly IMapper _mapper;
        protected readonly NotificationHelper _notificationHelper;
        protected readonly ILoggerManager _logger;
        public BaseBO(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper,ILoggerManager logger)
        {
            _uow = unit;
            _mapper = mapper;
            _notificationHelper = notificationHelper;
            _logger = logger;
        }
    }
}
