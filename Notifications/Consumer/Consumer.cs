using Notifications.Services;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;

namespace Notifications
{
    public class Consumer
    {
        private readonly NotificationService _notificationService;

        public Consumer(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public void Start(string rabbitMqHost, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = rabbitMqHost };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine($"Esperando mensajes en la cola '{queueName}'...");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Mensaje recibido: {message}");

                    // Lógica para enviar el correo
                    await EnviarCorreosAsync("Mensaje de RabbitMQ", message);
                };

                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Presiona [Enter] para salir.");
                Console.ReadLine();
            }
        }

        private async Task EnviarCorreosAsync(string asunto, string cuerpo)
        {
            string remitente = "jossfarfan80@gmail.com";
            string contrasena = "ankq hjgx arpb jimp"; // Reemplaza esto con tus credenciales reales

            try
            {
                var destinatarios = await _notificationService.ObtenerUsuariosAsync();

                var smtpClient = new SmtpClient("smtp.gmail.com") // Cambia esto según tu proveedor de correo
                {
                    Port = 587,
                    Credentials = new NetworkCredential(remitente, contrasena),
                    EnableSsl = true,
                };

                foreach (var destinatario in destinatarios)
                {
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(remitente),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = false,
                    };

                    mailMessage.To.Add(destinatario.Gmail);

                    smtpClient.Send(mailMessage);

                    Console.WriteLine($"Correo enviado exitosamente a {destinatario.Gmail}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
            }
        }
    }
}

