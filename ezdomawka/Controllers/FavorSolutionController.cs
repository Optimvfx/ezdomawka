﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Models.Admin;
using BLL.Models.FavorSolution;
using BLL.Models.ViewModels;
using BLL.Services;
using Common.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace ezdomawka.Controllers
{
    [Authorize]
    public class FavorSolutionController : Controller
    {
        private readonly FavorSolutionService _favorSolutionService;
        private readonly IMapper _mapper;
        private readonly UserService _userService;
        public FavorSolutionController(FavorSolutionService favorSolutionService, IMapper mapper, UserService userService)
        {
            _favorSolutionService = favorSolutionService;
            _mapper = mapper;
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> AddSolution()
        {
            var model = await _favorSolutionService.GetAddSolutionModel();
            return View(_mapper.Map<AddSolutionVm>(model));
        }

        [HttpPost]
        public async Task<IActionResult> AddSolution(AddSolutionRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var model = _mapper.Map<SolutionModel>(request);
                    model.Author = await _userService
                        .GetUserById(Guid.Parse(User.Claims.FirstOrDefault(u => u.Type == Claims.UserClaim)!.Value));
                    await _favorSolutionService.AddFavor(model);
                    return StatusCode(StatusCodes.Status200OK, new {redirect = "/home/index"});
                }
                catch
                {
                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSolutions(GetSolutionsRequest request, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                var favorSolutions =
                        await _favorSolutionService.GetSolutionModels(_mapper.Map<GetSolutionsModel>(request), token);
                    var vms = favorSolutions.Select(x => _mapper.Map<FavorSolutionVm>(x));
                    return PartialView("Partials/_FavorSolutions", vms);
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> FindFavors(FindFavorsRequest request, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<GetSolutionsModel>(request);
                var count = await _favorSolutionService.GetCountSolutions(model, token);
                var favors =
                    (await _favorSolutionService.GetSolutionModels(model, token)).Select(x => _mapper.Map<FavorSolutionVm>(x));
                var vm = new FavorsWithPaginationVm()
                {
                    CountFavorSolutions = count, FavorSolutionVms = favors
                };
                return PartialView("../Home/Partials/_FavorsWithPagination", vm);
            }
            return BadRequest();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetFavorSubjects(Guid favorId)
        {
            Thread.Sleep(5000);
            var subjects = await _favorSolutionService.GetFavorSubjects(favorId);
            return Ok(subjects);
        }
    }
}
