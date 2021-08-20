using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;
using VetClinic.Core.Interfaces.Repositories;
using VetClinic.Core.Interfaces.Services;
using static VetClinic.Core.Resources.TextMessages;

namespace VetClinic.BLL.Services
{
    public class OrderProcedureService : IOrderProcedureService
    {
        private readonly IOrderProcedureRepository _orderProcedureRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPetService _petService;
        private readonly IProcedureService _procedureService;
        private readonly IEmployeeService _employeeService;

        public OrderProcedureService(IOrderProcedureRepository orderProcedureRepository,
            IAppointmentRepository appointmentRepository,
            IPetService petService,
            IProcedureService procedureService,
            IEmployeeService employeeService)
        {
            _orderProcedureRepository = orderProcedureRepository;
            _petService = petService;
            _procedureService = procedureService;
            _appointmentRepository = appointmentRepository;
            _employeeService = employeeService;
        }

        public async Task<IList<OrderProcedure>> GetOrderProceduresAsync()
        {
            return await _orderProcedureRepository
                .GetAsync(asNoTracking: true);
        }

        public async Task<OrderProcedure> GetByIdAsync(int id)
        {
            var orderProcedure = await _orderProcedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                    throw new NotFoundException($"{nameof(OrderProcedure)} {EntityWasNotFound}");
           
            return orderProcedure;
        }

        public async Task InsertAsync(OrderProcedure entity)
        {
            await _orderProcedureRepository.InsertAsync(entity);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public void Update(int id, OrderProcedure orderProcedureToUpdate)
        {
            if (id != orderProcedureToUpdate.Id)
            {
                throw new BadRequestException($"{nameof(OrderProcedure)} {IdsDoNotMatch}");
            }

            _orderProcedureRepository.Update(orderProcedureToUpdate);
            _orderProcedureRepository.SaveChanges();

        }

        public async Task DeleteAsync(int id)
        {
            var orderProcedureToDelete = await _orderProcedureRepository
                .GetFirstOrDefaultAsync(x => x.Id == id) ??
                throw new NotFoundException($"{nameof(OrderProcedure)} {EntityWasNotFound}");

            _orderProcedureRepository.Delete(orderProcedureToDelete);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IList<int> listOfIds)
        {
            var orderProceduresToDelete = await _orderProcedureRepository.GetAsync(x => listOfIds.Contains(x.Id));

            if (orderProceduresToDelete.Count() != listOfIds.Count)
            {
                throw new BadRequestException($"{SomeEntitiesInCollectionNotFound} {nameof(OrderProcedure)}s to delete");
            }
            _orderProcedureRepository.DeleteRange(orderProceduresToDelete);
            await _orderProcedureRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrderProcedure>> GetOrderProceduresOfDoctorAsync(string doctorId)
        {
            return await _orderProcedureRepository.GetAsync(
                filter: x => x.EmployeeId == doctorId,
                include: y => y
                    .Include(p => p.Procedure)
                    .Include(p => p.Pet)
                    .ThenInclude(a => a.AnimalType),
                asNoTracking: true);
        }

        public async Task<OrderProcedure> GenerateOrderProcedureAsync(int petId, int procedureId, PaymentOption paymentOption)
        {
            var pet = await _petService.GetByIdAsync(petId);
            var procedure = await _procedureService.GetByIdAsync(procedureId);

            Order order = new Order
            {
                PaymentOption = paymentOption
            };

            return new OrderProcedure
            {
                Status = OrderProcedureStatus.NotAssigned,
                Pet = pet,
                Procedure = procedure,
                Order = order
            };
        }

        public async Task AddAppointmentAndDoctorToOrderProcedureAsync(int orderProcedureId, string employeeId, Appointment appointment)
        {
            var orderProcedure = await GetByIdAsync(orderProcedureId);
            var employee = await _employeeService.GetByIdAsync(employeeId);

            await _appointmentRepository.InsertAsync(appointment);

            orderProcedure.Status = OrderProcedureStatus.Assigned;
            orderProcedure.Appointment = appointment;
            orderProcedure.Employee = employee;

            Update(orderProcedureId, orderProcedure);
        }
    }
}
