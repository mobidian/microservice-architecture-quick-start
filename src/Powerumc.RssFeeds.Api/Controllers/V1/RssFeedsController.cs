﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Powerumc.RssFeeds.Domain;
using Powerumc.RssFeeds.Domain.Responses;
using Powerumc.RssFeeds.Extensions;
using Powerumc.RssFeeds.Services;

namespace Powerumc.RssFeeds.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/rssfeeds")]
    public class RssFeedsController : ApiController
    {
        private readonly TraceId _traceId;
        private readonly ILogger<RssFeedsController> _logger;
        private readonly IRssFeedsService _rssFeedsService;

        public RssFeedsController(TraceId traceId,
            ILogger<RssFeedsController> logger,
            IRssFeedsService rssFeedsService)
        {
            _traceId = traceId;
            _logger = logger;
            _rssFeedsService = rssFeedsService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Domain.Responses.V1.RssFeedResponse), 200)]
        [ProducesResponseType(typeof(Domain.Responses.ErrorResponse), 500)]
        public async Task<IActionResult> CreateAsync([FromBody] Domain.Requests.V1.RssFeedCreateRequest request)
        {
            try
            {
                return Ok(await _rssFeedsService.CreateAsync(request));
            }
            catch (Exception e)
            {
                _logger.LogError(_traceId, e);
                return Error(_traceId);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagingResult<IEnumerable<Domain.Responses.V1.RssFeedResponse>>), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> ListAsync([FromQuery] Domain.PagingInfo request)
        {
            try
            {
                return Ok(await _rssFeedsService.ListAsync(null, request));
            }
            catch (Exception e)
            {
                _logger.LogError(_traceId, e);
                return Error(_traceId);
            }
        }
    }
}