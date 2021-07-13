using Moq;
using System.Threading.Tasks;
using VetClinic.BLL.Services;
using VetClinic.BLL.Tests.FakeData;
using VetClinic.Core.Entities;
using VetClinic.Core.Interfaces.Repositories;
using Xunit;

namespace VetClinic.BLL.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly OrderService _orderService;
        private readonly Mock<IOrderRepository> _orderRepository = new Mock<IOrderRepository>();
        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _orderRepository.Object);
        }
    }
}
