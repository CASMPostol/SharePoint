//<summary>
//  Title   : class Extensions
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-27 13:33:07 +0100 (wt., 27 sty 2015) $
//  $Rev: 11265 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Reflection/Extensions.cs $
//  $Id: Extensions.cs 11265 2015-01-27 12:33:07Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CAS.SharePoint.Client.Reflection
{
  /// <summary>
  ///  internal static class Extensions
  /// </summary>
  public static class Extensions
  {
    internal static Dictionary<string, MemberInfo> GetDictionaryOfMembers(this Type type)
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = (from _midx in type.GetMembers(_flgs)
                                               where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                               select _midx).ToDictionary<MemberInfo, string>(_mi => _mi.Name);
      return _mmbrs;
    }
    internal static Dictionary<string, Type> GetDerivedTypes(this Type type)
    {
      Dictionary<string, Type> _ret = new Dictionary<string, Type>();
      GetContentType(type, _ret);
      //get derived types if any
      Type[] _types = (from _ax in type.GetCustomAttributes(false)
                       where _ax is DerivedEntityClassAttribute
                       select _ax).Cast<DerivedEntityClassAttribute>().Select<DerivedEntityClassAttribute, Type>(x => x.Type).ToArray<Type>();
      foreach (Type _typex in _types)
        GetContentType(_typex, _ret);
      return _ret;
    }
    /// <summary>
    /// Gets the <see cref="ContentTypeAttribute"/>
    /// </summary>
    /// <param name="type">The type of entity.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException"></exception>
    public static ContentTypeAttribute GetContentType(this Type type)
    {
      ContentTypeAttribute _cta = type.GetCustomAttributes(false).Where<Object>(x => x is ContentTypeAttribute).Cast<ContentTypeAttribute>().FirstOrDefault<ContentTypeAttribute>();
      if (_cta == null)
        throw new ArgumentException(String.Format("The type {0} doesn't have the expected ContentTypeAttribute attribute", type.Name));
      return _cta;
    }
    internal static string GetContentTypeID(this ListItem listItem)
    {
      const int _idlistLength = 34;
      string _id = listItem.ContentType.Id.ToString();
      if (_id.Length > _idlistLength)
        _id = _id.Remove(_id.Length - _idlistLength);
      return _id;
    }
    private static void GetContentType(this Type type, Dictionary<string, Type> typesDictionary)
    {
      ContentTypeAttribute _cta = GetContentType(type);
      if (typesDictionary.ContainsKey(_cta.Id))
        typesDictionary[_cta.Id] = type;
      else
        typesDictionary.Add(_cta.Id, type);
    }

  }
}
