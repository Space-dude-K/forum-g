﻿using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumBaseCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public ForumBaseCreationController(IMapper mapper, IRepositoryApiManager repositoryApiManager)
        {
            _mapper = mapper;
            _repositoryApiManager = repositoryApiManager;
        }
        [HttpGet]
        public async Task<IActionResult> RedirectToCreateForumBase(string model)
        {
            if(string.IsNullOrEmpty(model))
                return BadRequest(ModelState);

            var catAddModel = _mapper.Map<ForumBaseCreationView>(JsonConvert.DeserializeObject<ForumHomeViewModel>(model));
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", catAddModel);
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateForumBase(ForumBaseCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = (int)HttpContext.Items["userId"];
            var forumToAdd = _mapper.Map<ForumBaseForCreationDto>(model);

            if (userId > 0)
            {
                forumToAdd.ForumUserId = userId;
                var res = await _repositoryApiManager.ForumApis.CreateForumBase(model.SelectedCategoryId, forumToAdd);
                //var resCounter = await _forumService.UpdatePostCounter(categoryId, true);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}