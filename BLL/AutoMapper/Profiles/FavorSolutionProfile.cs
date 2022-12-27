﻿using AutoMapper;
using BLL.Models.Admin;
using BLL.Models.FavorSolution;
using BLL.Models.ViewModels;
using DAL.Entities;

namespace BLL.AutoMapper.Profiles
{
    public class FavorSolutionProfile : Profile
    {
        public FavorSolutionProfile()
        {
            CreateMap<SubjectVm, Subject>();
            CreateMap<ThemeVm, Theme>();
            CreateMap<AddSolutionRequest, SolutionModel>()
                .ForMember(d=>d.Created, f=>f.MapFrom(a => DateTimeOffset.UtcNow));
            CreateMap<SolutionModel, AddSolutionRequest>();
            CreateMap<SolutionModel, FavorSolution>()
                .ForMember(u => u.Created, k => k.MapFrom(d => DateTimeOffset.UtcNow))
                .ForMember(d => d.FavorSubjects, s => s.MapFrom(a => a.Subjects))
                .ForMember(f => f.Theme, opt => opt.Ignore())
                .ForMember(f=>f.ThemeId, k=>k.MapFrom(a=>a.Theme.Id));
            CreateMap<SubjectModel, FavorSubject>().ForMember(d => d.SubjectId, f => f.MapFrom(m => m.Id));
            CreateMap<FavorSolutionVm, SolutionModel>();
            CreateMap<SolutionModel, FavorSolutionVm>().ForMember(d => d.Nick, k => k.MapFrom(a => a.Author.Nick));
            CreateMap<FavorSolution, SolutionModel>()
                .ForMember(f => f.Subjects, k => k.MapFrom(d => d.FavorSubjects));
            CreateMap<FavorSubject, SubjectModel>()
                .ForMember(k => k.Id, f => f.MapFrom(l => l.Subject.Id))
                .ForMember(m => m.Name, f => f.MapFrom(s => s.Subject.Name));
            CreateMap<AddSolutionModel, AddSolutionVm>();

        }
    }
}
