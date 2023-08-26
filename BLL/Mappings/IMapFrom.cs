using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


namespace BLL.Mappings
{
    public interface IMapFrom<T>
    {
        void MapFrom(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}
