using KafkaProducer.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KafkaProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {

        private readonly IBusControl _busControl;
        private readonly ITopicProducer<OrderRequest> _topicProducer;

        public ProducerController(IBusControl busControl, ITopicProducer<OrderRequest> topicProducer)
        {
            _busControl = busControl;
            _topicProducer = topicProducer;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderRequest orderRequest, CancellationToken cancellationToken)
        {
            return Ok(await SendOrderRequest(orderRequest, cancellationToken));
        }

        private async Task<bool> SendOrderRequest(OrderRequest message, CancellationToken cancellationToken)
        {

          //  await _busControl.StartAsync(cancellationToken);
            try
            {
                await _topicProducer.Produce(message, cancellationToken);
                Debug.WriteLine($"Delivery Timestamp:{DateTime.UtcNow}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured: {ex.Message}");
                return false;
            }
            finally
            {
             //   await _busControl.StopAsync(cancellationToken);
            }
           
        }
    }
}
