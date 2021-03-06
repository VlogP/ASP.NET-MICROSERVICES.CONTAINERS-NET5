using AutoMapper;
using MongoDB.Bson;
using ProductMicroservice.API.Models.Client;
using ProductMicroservice.API.Models.Product;
using ProductMicroservice.BLL.Models.Client;
using ProductMicroservice.BLL.Models.DTO.Client;
using ProductMicroservice.BLL.Models.DTO.Product;
using ProductMicroservice.BLL.Models.Product;
using ProductMicroservice.DAL.Models.SQLServer;
using System;

namespace ProductMicroservice.API.Infrasrtucture.Automapper
{
    public class AutomapperProductProfile: Profile
    {
        public AutomapperProductProfile()
        {
            CreateMap<string, Guid>()
                .ConvertUsing(s => Guid.Parse(s));

            CreateMap<ObjectId, string>()
                .ConvertUsing(s => s.ToString());

            CreateMap<DAL.Models.Mongo.Product, ProductGetDTO>()
               .ReverseMap();

            CreateMap<DAL.Models.Mongo.Product, ProductPostDTO>()
               .ReverseMap();

            CreateMap<DAL.Models.ElasticSearch.Product, ProductPost>()
               .ReverseMap();

            CreateMap<DAL.Models.Mongo.Product, ProductPost>()
              .ReverseMap();

            CreateMap<Product, ProductPost>()
              .ReverseMap();

            CreateMap<ProductPost, ProductPostAPI>()
              .ReverseMap();
        }
    }
}
