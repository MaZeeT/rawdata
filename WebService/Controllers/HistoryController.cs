using AutoMapper;
using DatabaseService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseService.Modules;
using DatabaseService.Services;
using WebService.DTOs;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/history")]
    [Authorize]
    public class HistoryController : SharedController
    {
        private IHistoryService _historyService;
        private ISharedService _sharedService;
        private IMapper _mapper;

        public HistoryController(IHistoryService historyService, ISharedService sharedService, IMapper mapper)
        {
            _historyService = historyService;
            _sharedService = sharedService;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetHistory))]
        //example http://localhost:5001/api/history 
        public ActionResult GetHistory()
        {
            var userId = GetAuthUserId().Item1;
            var history = _historyService.GetHistoryList(userId);
            if (history == null)
            {
                return NotFound();
            }

            return Ok(ConvertToDto(history));
        }

        [HttpDelete("delete/all", Name = nameof(ClearHistory))]
        //example http://localhost:5001/api/history/delete/all
        public ActionResult ClearHistory()
        {
            var userId = GetAuthUserId().Item1;
            var result = _historyService.DeleteUserHistory(userId);
            if (!result)
            {
                return NotFound();
            }

            return Ok(result);
        }

        private List<HistoryDTO> ConvertToDto(IEnumerable<History> history)
        {
            List<HistoryDTO> list = new List<HistoryDTO>();
            foreach (var mark in history)
            {
                list.Add(new HistoryDTO
                {
                    Title = _sharedService.GetPost(mark.Postid).Title,
                    Date = mark.Date,
                    ThreadUrl = Url.Link(
                        nameof(QuestionsController.GetThread), 
                        new {questionId = mark.Postid}
                        )
                });
            }

            return list;
        }
    }
}