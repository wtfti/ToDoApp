namespace ToDo.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using Infrastructure.Mapping;

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
            });
        }

        // Uncomment if you want automapper to map custom mappings
        // Create interface ICustomMappings and implement it on every model which you need
        //private static void LoadCustomMappings(IEnumerable<Type> types)
        //{
        //    var maps =
        //        types.SelectMany(t => t.GetInterfaces(), (t, i) => new { t, i })
        //            .Where(
        //                type =>
        //                    typeof(IHaveCustomMappings).IsAssignableFrom(type.t) && !type.t.IsAbstract &&
        //                    !type.t.IsInterface)
        //            .Select(type => (IHaveCustomMappings)Activator.CreateInstance(type.t));

        //    foreach (var map in maps)
        //    {
        //        map.Initialize(a => a.CreateMap(Mapper.Configuration));
        //    }
        //}
    }
}