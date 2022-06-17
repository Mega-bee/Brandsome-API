using AutoMapper;
using Brandsome.BLL.Utilities;
using Brandsome.BLL.ViewModels;
using Brandsome.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Brandsome.BLL
{
    public class UserProfile : Profile
    {

        public UserProfile()
        {


            //CreateMap<Patient, Patient_VM>().AfterMap((src,dest) =>
            //{
            //    foreach (var item in src.PatientMedicines)
            //    {
            //        dest.Prescriptions.Add(new Medicine_VM()
            //        {
            //            Id = item.Medicine.Id,
            //            Title = item.Medicine.Title
            //        });
            //    }
            //})
            //    .ReverseMap();
            CreateMap<string, string>().ConvertUsing(s => s ?? string.Empty);


            //CreateMap<IQueryable<Anomaly>, List<AnomalyCount_VM>>().ConvertUsing<PatientConverter>();


























        }
    }
}
