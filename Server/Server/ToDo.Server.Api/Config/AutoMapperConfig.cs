﻿namespace ToDo.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Ninject.Mapping;
    using Models.Account;
    using Models.Note;

    public static class AutoMapperConfig
    {
        public static void RegisterMappings(params Assembly[] assemblies)
        {
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetExportedTypes());
            }

            LoadStandardMappings(types);
        }

        private static void LoadStandardMappings(IEnumerable<Type> types)
        {
            var maps = types.SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
                .Where(
                    type =>
                        type.i.IsGenericType && type.i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                        !type.t.IsAbstract
                        && !type.t.IsInterface)
                .Select(type => new { Source = type.i.GetGenericArguments()[0], Destination = type.t });

            Mapper.Initialize(a =>
            {
                foreach (var map in maps)
                {
                    var source = map.Source;
                    var dest = map.Destination;
                    a.CreateMap(source, dest);
                    a.CreateMap(dest, source);
                }

                a.CreateMap<User, FriendResponseModel>()
                .ForMember(x => x.FullName, z => z.MapFrom(user => user.ProfileDetails.FullName));
                a.CreateMap<User, IdentityResponseModel>()
                .ForMember(x => x.FullName, z => z.MapFrom(user => user.ProfileDetails.FullName));
                a.CreateMap<SharedNote, NoteResponseModel>()
                    .ForMember(users => users.Users, x => x.MapFrom(shared => shared.Users.Select(e => e.ProfileDetails.FullName)));
            });
        }
    }
}