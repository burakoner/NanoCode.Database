using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NanoCode.Database
{
    public static class NanoMapper
    {
        public static void RegisterModel<T>()
        {
            SqlMapper.SetTypeMap(typeof(T), new ColumnAttributeTypeMapper<T>());
        }

        public static void RegisterModels(Assembly assembly)
        {
            var mappedTypes = assembly.GetTypes().Where(
                f => f.GetProperties().Any(
                    p => p.GetCustomAttributes(false).Any(a => a.GetType().Name == nameof(NanoTableColumnAttribute))));

            var mapper = typeof(ColumnAttributeTypeMapper<>);
            foreach (var mappedType in mappedTypes)
            {
                var genericType = mapper.MakeGenericType(new[] { mappedType });
                SqlMapper.SetTypeMap(mappedType, Activator.CreateInstance(genericType) as SqlMapper.ITypeMap);
            }
        }
    }

    internal class ColumnAttributeTypeMapper<T> : FallbackTypeMapper
    {
        public ColumnAttributeTypeMapper() : base(new SqlMapper.ITypeMap[] {
                new CustomPropertyTypeMap(typeof (T), SelectProperty),
                new DefaultTypeMap(typeof (T))
            })
        { }

        public static PropertyInfo SelectProperty(Type type, string columnName)
        {
            return
                type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .FirstOrDefault(prop =>
                        prop.GetCustomAttributes(typeof(NanoTableColumnAttribute), false)
                            .Any(attr =>
                                ((NanoTableColumnAttribute)attr).ColumnName.ToLower() == columnName.ToLower()));
        }
    }

    internal class FallbackTypeMapper : SqlMapper.ITypeMap
    {
        private readonly IEnumerable<SqlMapper.ITypeMap> _mappers;

        public FallbackTypeMapper(IEnumerable<SqlMapper.ITypeMap> mappers)
        {
            _mappers = mappers;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindConstructor(names, types);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    ConstructorInfo result = mapper.FindExplicitConstructor();
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetConstructorParameter(constructor, columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            foreach (var mapper in _mappers)
            {
                try
                {
                    var result = mapper.GetMember(columnName);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (NotImplementedException)
                {
                }
            }
            return null;
        }
    }

}
