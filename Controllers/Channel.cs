using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sendbird.Entities;
using SendBird.Api.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Sendbird.Enums;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace SendBird.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Channel : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ILogger<Channel> _logger;
        private readonly ChannelConfig _channelConfig;
        private readonly ApplicationConfig _appConfig;

        public Channel(IHttpClientFactory clientFactory,
                        ChannelConfig channelConfig,
                        ApplicationConfig appConfig,
                        ILogger<Channel> logger)
        {
            _clientFactory = clientFactory;
            _channelConfig = channelConfig;
            _appConfig = appConfig;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ChannelCreated(ChannelEvent<GroupChannel> channelCreatedEvent)
        {
            if (channelCreatedEvent == null)
            {
                _logger.LogWarning("Received empty channel event data");

                return StatusCode(StatusCodes.Status400BadRequest);
            }
            else
            {
                _logger.LogTrace("Channel created event notification received from webhook with following information:\n {channelCreatedEvent}", channelCreatedEvent);

                var channelRelativeUrl = channelCreatedEvent.Channel.ChannelUrl;

                //Post admin message to the channel
                try
                {
                    var postUrl = $"https://api-{_appConfig.AppId}.sendbird.com/v3/group_channels/{channelRelativeUrl}/messages";
                    var httpClient = _clientFactory.CreateClient();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.Add("Api-Token", _appConfig.MasterApiToken);
                    var json = JsonConvert.SerializeObject(_channelConfig.AdminMessage);
                    var response = await httpClient.PostAsync(postUrl, new StringContent(json));

                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Failed to post admin message to the group channel with id:{channelRelativeUrl}");
                        return StatusCode(StatusCodes.Status500InternalServerError);
                    }
                }
                catch (Exception ex)
                {
                    //either log it or alert through emails, as necessary
                    _logger.LogError($"Failed to post admin message to the group channel with id:{channelRelativeUrl} with exception:", ex);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            _logger.LogDebug($"Successfully sent admin message to channel");

            // 200 Ok result.
            return Ok();
        }


        [HttpGet]
        [Route("created")]
        public async Task<IActionResult> post()
        {

            //_logger.LogTrace("Channel created event notification received from webhook with following information:\n {channelCreatedEvent}", channelCreatedEvent);

            //AdminMessage adminMessage = new AdminMessage();
            //adminMessage.Message = _channelConfig.AdminMessage.Message;
            //adminMessage.Data = _channelConfig.AdminMessage.Data;
            //adminMessage.Type = MessageType.AdminMessage;
            //adminMessage.MessageId = Guid.NewGuid();

            //Post admin message to the channel
            var channelRelativeUrl = "sendbird_group_channel_333278804_d461e86f70235b7b51dccda0f6a41947f3e2ee89";
            //adminMessage.ChannelUrl = channelRelativeUrl;
            try
            {
                var postUrl = $"https://api-{_appConfig.AppId}.sendbird.com/v3/group_channels/{channelRelativeUrl}/messages";
                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Add("Api-Token", _appConfig.MasterApiToken);
                var json = JsonConvert.SerializeObject(_channelConfig.AdminMessage);
                var response = await httpClient.PostAsync(postUrl, new StringContent(json));
                var x = response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                //either log it or alert through emails, as necessary
                _logger.LogError($"Failed to post admin message to the group channel with id:{channelRelativeUrl}");
            }


            _logger.LogDebug($"Successfully sent admin message to channel");

            // 200 Ok result.
            return Ok();
        }
    }
}
