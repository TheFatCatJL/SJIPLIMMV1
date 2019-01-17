using AutoMapper;
using SJIP_LIMMV1.Models;
using SJIP_LIMMV1.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SJIP_LIMMV1.App_Start
{
    public class BoxMapProfile : Profile
    {
        public BoxMapProfile()
        {
            CreateMap<PreBoxInfo, PreBoxInfoViewModel>()
            .ForMember(dest => dest.postalcode, opt => opt.MapFrom(src => src.postalcode))
            .ForMember(dest => dest.lift, opt => opt.MapFrom(src => src.lift))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.preboxId));

            CreateMap<PreBoxInfoViewModel, PreBoxInfo>()
            .ForPath(dest => dest.postalcode, opt => opt.MapFrom(src => src.postalcode))
            .ForPath(dest => dest.lift, opt => opt.MapFrom(src => src.lift))
            .ForPath(dest => dest.preboxId, opt => opt.MapFrom(src => src.Id));

            CreateMap<ComBoxInfo, ComBoxInfoViewModel>()
            .ForMember(dest => dest.rptpostalcode, opt => opt.MapFrom(src => src.postalcode))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.comboxId))
            .ForMember(dest => dest.rptcomdate, opt => opt.MapFrom(src => src.rptdate))
            .ForMember(dest => dest.comrec, opt => opt.MapFrom(src => src.CommissionRecord));

            CreateMap<ComBoxInfoViewModel, ComBoxInfo>()
            .ForPath(dest => dest.postalcode, opt => opt.MapFrom(src => src.rptpostalcode))
            .ForPath(dest => dest.CommissionRecord, opt => opt.MapFrom(src => src.comrec))
            .ForPath(dest => dest.rptdate, opt => opt.MapFrom(src => src.rptcomdate))
            .ForPath(dest => dest.comboxId, opt => opt.MapFrom(src => src.Id));

            CreateMap<CommissionRecord, CommissionRecordVM>()
            .ForMember(dest => dest.vms, opt => opt.MapFrom(src => src.ComBoxInfoes))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.comrecId));

            CreateMap<CommissionRecordVM, CommissionRecord>()
            .ForPath(dest => dest.ComBoxInfoes, opt => opt.MapFrom(src => src.vms))
            .ForPath(dest => dest.comrecId, opt => opt.MapFrom(src => src.Id));

            //CreateMap<ContactForm, ContactFormViewModel>();
            //CreateMap<ContactFormViewModel, ContactForm>();

            CreateMap<ComBoxInfo, BoxInfo>()
            .ForPath(dest => dest.comboxID, opt => opt.MapFrom(src => src.comboxId))
            .ForPath(dest => dest.comboxlmpdnum, opt => opt.MapFrom(src => src.lmpdnum))
            .ForPath(dest => dest.comboxhistory, opt => opt.MapFrom(src => src.history))
            .ForPath(dest => dest.comboxrptcomdate, opt => opt.MapFrom(src => src.rptdate))
            .ForPath(dest => dest.comboxrptcomment, opt => opt.MapFrom(src => src.rptcomment))
            .ForPath(dest => dest.comboxrptlift, opt => opt.MapFrom(src => src.rptlift))
            .ForPath(dest => dest.comboxrptpostalcode, opt => opt.MapFrom(src => src.postalcode))
            .ForPath(dest => dest.comboxblock, opt => opt.MapFrom(src => src.Address.blk))
            .ForPath(dest => dest.comboxroad, opt => opt.MapFrom(src => src.Address.road))
            .ForPath(dest => dest.comboxstatus, opt => opt.MapFrom(src => src.status))
            .ForPath(dest => dest.comboxtechname, opt => opt.MapFrom(src => src.techname))
            .ForPath(dest => dest.comrecID, opt => opt.MapFrom(src => src.comrecId))
            .ForPath(dest => dest.comboxteamname, opt => opt.MapFrom(src => src.teamname));

            CreateMap<PreBoxInfo, BoxInfo>()
            .ForPath(dest => dest.preboxID, opt => opt.MapFrom(src => src.preboxId))
            .ForPath(dest => dest.preboxcheckdate, opt => opt.MapFrom(src => src.checkdate))
            .ForPath(dest => dest.preboxcheckername, opt => opt.MapFrom(src => src.checkername))
            .ForPath(dest => dest.preboxhistory, opt => opt.MapFrom(src => src.history))
            .ForPath(dest => dest.preboxinstalldate, opt => opt.MapFrom(src => src.installdate))
            .ForPath(dest => dest.preboxisdeployed, opt => opt.MapFrom(src => src.isdeployed))
            .ForPath(dest => dest.preboxjsonid, opt => opt.MapFrom(src => src.jsonid))
            .ForPath(dest => dest.preboxlmpdnum, opt => opt.MapFrom(src => src.lmpdnum))
            .ForPath(dest => dest.preboxsimnum, opt => opt.MapFrom(src => src.simnum))
            .ForPath(dest => dest.preboxstatus, opt => opt.MapFrom(src => src.status))
            .ForPath(dest => dest.preboxlift, opt => opt.MapFrom(src => src.lift))
            .ForPath(dest => dest.preboxpostalcode, opt => opt.MapFrom(src => src.postalcode))
            .ForPath(dest => dest.preboxblock, opt => opt.MapFrom(src => src.Address.blk))
            .ForPath(dest => dest.preboxroad, opt => opt.MapFrom(src => src.Address.road))
            .ForPath(dest => dest.preboxtelco, opt => opt.MapFrom(src => src.telco));

            CreateMap<CommissionRecord, BoxInfo>()
            .ForPath(dest => dest.comrecID, opt => opt.MapFrom(src => src.comrecId))
            .ForPath(dest => dest.comreccomment, opt => opt.MapFrom(src => src.comment))
            .ForPath(dest => dest.comrechistory, opt => opt.MapFrom(src => src.history))
            .ForPath(dest => dest.comrecorddate, opt => opt.MapFrom(src => src.comrecorddate))
            .ForPath(dest => dest.comrecstatus, opt => opt.MapFrom(src => src.status))            
            .ForPath(dest => dest.comrecsupname, opt => opt.MapFrom(src => src.supname));
        }
    }
}