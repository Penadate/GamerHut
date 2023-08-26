using BLL.Mappings;
using BLL.Models.Category;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace BLL.Models.Product
{
    public class ProductModel : IMapFrom<DAL.Models.Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public string Nickname { get; set; }
        public string Contact { get; set; }
        public string CategoryName { get; set; }
        public void MapFrom(Profile profile)
        {
            profile.CreateMap<DAL.Models.Product, ProductModel>()
                .ForMember(dest => dest.CategoryName, src => src.MapFrom(otp => otp.Category!.Name));
        }
    }
}
