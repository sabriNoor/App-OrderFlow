using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.MVC.Constants;
using App.MVC.Data;
using App.MVC.DTOs;
using App.MVC.DTOs.Order;
using App.MVC.Entities;
using App.MVC.Repositories.Interfaces;
using App.MVC.Services.Interfaces;

namespace App.MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;
        private readonly IRabbitMQPublisher<OrderCreatedMessageDTO> _rabbitMQPublisher;

        public OrderService(IUnitOfWork unitOfWork,
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ILogger<OrderService> logger,
        IRabbitMQPublisher<OrderCreatedMessageDTO> rabbitMQPublisher)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        public async Task<ServiceResult<OrderDTO>> CreateOrderAsync(int userId, string email, CreateOrderDTO orderDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                await ValidateProductsAndAdjustStockAsync([.. orderDTO.Products]);

                var order = new Order
                {
                    UserId = userId,
                    OrderDetails = [.. orderDTO.Products.Select(p => new OrderDetail { ProductId = p.ProductId, Quantity = p.Quantity })]
                };

                await _orderRepository.AddAsync(order);

                await _unitOfWork.SaveChangesAsync();

                var result = await _orderRepository.GetOrdersInformation(o => o.Id == order.Id);
                if (result.Count == 0)
                {
                    throw new Exception("Failed to create order");
                }

                await _unitOfWork.CommitAsync();
                _logger.LogInformation("Order {OrderId} created successfully for User {UserId}", order.Id, userId);

                await PublishOrderCreatedMessageAsync(result[0], email);

                return ServiceResult<OrderDTO>.Ok(result[0]);
            }
            catch (ArgumentException ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogWarning("Order creation failed: {Message}", ex.Message);
                return ServiceResult<OrderDTO>.Fail($"Failed to create order: {ex.Message}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                _logger.LogError(ex, "Unexpected error while creating order");
                return ServiceResult<OrderDTO>.Fail("An unexpected error occurred while creating the order.");
            }
        }


        private async Task ValidateProductsAndAdjustStockAsync(List<CreateOrderDetailDTO> products)
        {
            foreach (var od in products)
            {
                var product = await _productRepository.GetByIdAsync(od.ProductId);
                if (product is null || product.Quantity < od.Quantity)
                    throw new ArgumentException($"Product {od.ProductId} is invalid or has insufficient stock.");

                product.Quantity -= od.Quantity;
            }

        }
        private async Task PublishOrderCreatedMessageAsync(OrderDTO order, string email)
        {
            var message = new OrderCreatedMessageDTO
            {
                Order = order,
                Email = email
            };

            await _rabbitMQPublisher.PublishMessageAsync(message, RabbitMQQueues.OrderQueue);
        }

        public async Task<ServiceResult<List<OrderDTO>>> GetMyOrdersAsync(int userId)
        {
            try
            {
                _logger.LogInformation("Retrieving orders for user with ID {UserId}", userId);

                var result = await _orderRepository.GetOrdersInformation(o => o.UserId == userId);
                _logger.LogInformation("Successfully retrieved {OrderCount} orders for user with ID {UserId}", result.Count, userId);
                return ServiceResult<List<OrderDTO>>.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retriving orders for user with id {ID}", userId);
                return ServiceResult<List<OrderDTO>>.Fail("An unexpected error occurred while retriving the orders.");
            }
        }


        public async Task<ServiceResult<OrderDTO>> GetOrderByIdForUserAsync(int id, int userId)
        {
            try
            {
                _logger.LogInformation("Fetching order with ID {OrderId} for user with ID {ID}.", id, userId);
                List<OrderDTO> result = await _orderRepository.GetOrdersInformation(o => o.UserId == userId && o.Id == id);
                if (result.Count == 0)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for user with ID {ID}", id, userId);
                    return ServiceResult<OrderDTO>.Fail("Order not found.");
                }
                _logger.LogInformation("Order with ID {OrderId} retrieved successfully for user with ID {ID}.", id, userId);
                return ServiceResult<OrderDTO>.Ok(result[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retriving order with id {ID} for user with ID {ID}.", id, userId);
                return ServiceResult<OrderDTO>.Fail("An unexpected error occurred while retriving the order.");
            }
        }

        public async Task<ServiceResult<OrderDTO>> GetOrderByIdForAdminAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching order with ID {OrderId}.", id);
                List<OrderDTO> result = await _orderRepository.GetOrdersInformation(o => o.Id == id);
                if (result.Count == 0)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found.", id);
                    return ServiceResult<OrderDTO>.Fail("Order not found.");
                }
                _logger.LogInformation("Order with ID {OrderId} retrieved successfully.", id);
                return ServiceResult<OrderDTO>.Ok(result[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while retriving order with id {ID}.", id);
                return ServiceResult<OrderDTO>.Fail("An unexpected error occurred while retriving the order.");
            }
        }


    }
}
