//<summary>
//  Title   : public class StorageItem
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate: 2015-01-26 12:03:16 +0100 (pon., 26 sty 2015) $
//  $Rev: 11263 $
//  $LastChangedBy: mpostol $
//  $URL: http://svn.server300161.nazwa.pl/cas/VS/trunk/PR44-SharePoint/Libraries/SharePointLinqClient/Linq2SP/StorageItem.cs $
//  $Id: StorageItem.cs 11263 2015-01-26 11:03:16Z mpostol $
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.Client.Reflection;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CAS.SharePoint.Client.Linq2SP
{
  /// <summary>
  /// class Storage Item
  /// </summary>
  internal class StorageItem
  {

    #region public API
    /// <summary>
    /// Creates the storage description.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="includeReverseLookup">if set to <c>true</c> include reverse lookup].</param>
    /// <returns></returns>
    internal static StorageItemsList CreateStorageDescription(Type type, bool includeReverseLookup)
    {
      StorageItemsList storage = new StorageItemsList();
      SortedList<string, MemberInfo> _removed = new SortedList<string, MemberInfo>();
      CreateStorageDescription(type, storage, _removed, includeReverseLookup);
      return storage;
    }
    /// <summary>
    /// Creates the storage description without reverse lookup columns for all types derived from <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    internal static Dictionary<string, StorageItemsList> CreateStorageDescription(Type type)
    {
      Dictionary<string, Type> _types = type.GetDerivedTypes();
      Dictionary<string, StorageItemsList> _ret = new Dictionary<string, StorageItemsList>();
      foreach (KeyValuePair<string, Type> _typex in _types)
      {
        StorageItemsList _storage = new StorageItemsList();
        SortedList<string, MemberInfo> _removed = new SortedList<string, MemberInfo>();
        CreateStorageDescription(_typex.Value, _storage, _removed, false);
        _ret.Add(_typex.Key, _storage);
      }
      return _ret;
    }
    /// <summary>
    /// Provides enumeration of the possible destination kind
    /// </summary>
    internal enum Destination
    {
      /// <summary>
      /// The <see cref="Descriptor"/> contains value
      /// </summary>
      Value,
      /// <summary>
      /// The <see cref="Descriptor"/> contains lookup
      /// </summary>
      Lookup
    }
    /// <summary>
    /// Description of the value to be assigned to the destination object
    /// </summary>
    internal class Descriptor
    {
      /// <summary>
      /// Gets the destination kind.
      /// </summary>
      /// <value>
      /// The destination.
      /// </value>
      public Destination Destination { get; private set; }
      /// <summary>
      /// Gets the name of the property of list if lookup.
      /// </summary>
      /// <value>
      /// The name of the property of list if lookup.
      /// </value>
      public string Name { get; private set; }
      /// <summary>
      /// Gets the value or ID of the item if lookup.
      /// </summary>
      /// <value>
      /// The value returned from selected property.
      /// </value>
      public object Value { get; private set; }
      /// <summary>
      /// Initializes a new instance of the <see cref="Descriptor"/> class.
      /// </summary>
      /// <param name="destination">The destination.</param>
      /// <param name="name">The name.</param>
      /// <param name="value">The value.</param>
      internal Descriptor(Destination destination, string name, object value)
      {
        Destination = destination;
        Name = name;
        Value = value;
      }
    }
    /// <summary>
    /// Gets the value from entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="assign">A method used to assign the value to destination object</param>
    internal void GetValueFromEntity(object entity, Action<Descriptor> assign)
    {
      object _value = GetValueFromEntity(entity);
      if (IsLookup)
        assign(new Descriptor(Destination.Lookup, ((AssociationAttribute)this.m_Description).List, ((FieldLookupValue)_value).LookupId));
      else
        assign(new Descriptor(Destination.Value, PropertyName, _value));
    }
    /// <summary>
    /// Determines whether this instance is the reverse lookup.
    /// </summary>
    /// <returns><b>true</b> if it is lookup.</returns>
    internal bool IsReverseLookup()
    {
      return this.m_Association && ((AssociationAttribute)this.m_Description).MultivalueType == AssociationType.Backward;
    }
    /// <summary>
    /// Gets the name of the property.
    /// </summary>
    /// <value>
    /// The name of the property.
    /// </value>
    internal string PropertyName { get; private set; }
    /// <summary>
    /// Gets the values from entity and assign it using the <paramref name="assign" /> method and selection key provided by the <see cref="DataAttribute.Name" /> (internal SharePoint column name).
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <param name="assign">A method used to assign the value to destination object using the <see cref="DataAttribute.Name" /> .</param>
    internal void GetValueFromEntity(object entity, Action<string, object> assign)
    {
      object _value = GetValueFromEntity(entity);
      assign(this.m_Description.Name, _value);
    }
    internal TRet GetValue<TRet>(object entity)
    {
      return (TRet)m_Storage.GetValue(entity);
    }
    /// <summary>
    /// Attaches the specified entity - initialized internal fields of lookup fields <see cref="IEntityRef"/>.
    /// </summary>
    /// <param name="entity">The entity to be initialized.</param>
    /// <param name="m_DataContext">The <see cref="DataContext"/> instance.</param>
    /// <param name="listName">Name of the list.</param>
    internal void Attach(object entity, DataContext m_DataContext, string listName)
    {
      object _value = this.m_Storage.GetValue(entity);
      if (!this.m_Association)
        return;
      Debug.Assert(this.IsLookup, "Unexpected Multi-ValueType in the GetValuesFromEntity. Expected is lookup, but the filed is reverse lookup");
      ((IEntityRef)_value).SetLookup(null, m_DataContext, listName);
    }
    internal string SPFieldName { get { return m_Description.Name; } }
    internal bool IsLookup
    {
      get
      {
        if (!m_Association)
          return false;
        return ((AssociationAttribute)m_Description).MultivalueType == AssociationType.Single;
      }
    }
    internal void AssignValue2Entity(DataContext dataContext, Object entity, object value)
    {
      if (this.m_Association)
      {
        Debug.Assert(this.IsLookup, "Unexpected assignment to reverse lookup");
        IEntityRef _itemRef = (IEntityRef)this.m_Storage.GetValue(entity);
        _itemRef.SetLookup(value == null ? null : (FieldLookupValue)value, dataContext, ((AssociationAttribute)this.m_Description).List);
      }
      else
      {
        ColumnAttribute _ColumnAttribute = (ColumnAttribute)this.m_Description;
        if (_ColumnAttribute.IsLookupId)
          this.m_Storage.SetValue(entity, value == null ? new Nullable<int>() : ((FieldUserValue)value).LookupId);
        else if (_ColumnAttribute.FieldType.Equals(FieldType.Choice.ToString()))
        {
          if (value != null)
          {
            Dictionary<string, string> _enumValues = new Dictionary<string, string>();
            Type _type = this.GetEnumValues(_enumValues, false);
            string _enumFieldValue = StorageItem.NormalizeEnum((string)value);
            if (!_enumValues.ContainsKey(_enumFieldValue))
              throw new ArgumentOutOfRangeException("SP Field Value", String.Format("Cannot convert the value {0} to enum of type {1}", value, _type.Name));
            object _enumValue = Enum.Parse(_type, _enumValues[_enumFieldValue], true);
            this.m_Storage.SetValue(entity, _enumValue);
          }
        }
        else if (_ColumnAttribute.FieldType.Equals(FieldType.User.ToString()))
        {
          Debug.Assert(_ColumnAttribute.IsLookupValue, "IsLookupValue must be true for user field.");
          this.m_Storage.SetValue(entity, value == null ? string.Empty : ((FieldUserValue)value).LookupValue);
        }
        else if (_ColumnAttribute.FieldType.Equals("SPFieldUserValue"))
          this.m_Storage.SetValue(entity, value == null ? String.Empty : ((FieldUserValue)value).LookupValue);
        else
          this.m_Storage.SetValue(entity, value);
      }
    }
    internal void AssignReverseLookupValues2Entity(DataContext m_DataContext, Object _entity, int index)
    {
      IEntitySet _entitySet = (IEntitySet)this.m_Storage.GetValue(_entity);
      Debug.Assert(index > 0, "We try to get reverse lookup for new item.");
      _entitySet.LoadValues(m_DataContext, (AssociationAttribute)this.m_Description, index);
    }
    internal string TargetListName
    {
      get
      {
        Debug.Assert(IsLookup, "The value for TargetListName only exist if the field is a lookup.");
        return ((AssociationAttribute)m_Description).List;
      }
    }
    #endregion

    #region private
    //fields
    private FieldInfo m_Storage = null;
    private DataAttribute m_Description = null;
    private bool m_Association = false;
    //methods
    private static void CreateStorageDescription(Type type, StorageItemsList storage, SortedList<string, MemberInfo> removed, bool includeReverseLookup)
    {
      Dictionary<string, MemberInfo> _mmbrs = type.GetDictionaryOfMembers();
      foreach (MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value)
      {
        if (removed.ContainsKey(_ax.Name))
          continue;
        foreach (Attribute _AttributeX in _ax.GetCustomAttributes(false))
        {
          if (_AttributeX is RemovedColumnAttribute)
          {
            if (!removed.ContainsKey(_ax.Name))
              removed.Add(_ax.Name, _ax);
            break;
          }
          if (!(_AttributeX is DataAttribute))
            continue;
          DataAttribute _dataAttribute = _AttributeX as DataAttribute;
          AssociationAttribute _att = _dataAttribute as AssociationAttribute;
          if (_att != null && !includeReverseLookup && _att.MultivalueType == AssociationType.Backward)
            break;
          ColumnAttribute _columnAttribute = _dataAttribute as ColumnAttribute;
          if (_columnAttribute != null)
          {
            if (_columnAttribute.FieldType.Equals(FieldType.Lookup.ToString()) ||
                 _columnAttribute.FieldType.Equals(FieldType.WorkflowStatus.ToString()) ||
                (_columnAttribute.FieldType.Equals(FieldType.User.ToString()) && _columnAttribute.IsLookupId)
              )
              break;
          }
          //The property could be overridden in the derived class and the storage is in the base class.
          StorageItem _newStorageItem = null;
          if (!storage.TryGetValue(_ax.Name, out _newStorageItem))
          {
            _newStorageItem = new StorageItem(_ax.Name, _dataAttribute);
            storage.Add(_ax.Name, _newStorageItem);
          }
          if (!_mmbrs.Keys.Contains<string>(_dataAttribute.Storage))
            break;
          _newStorageItem.m_Storage = _mmbrs[_dataAttribute.Storage] as FieldInfo;
          if (_columnAttribute != null && _columnAttribute.FieldType.Contains(FieldType.Counter.ToString()))
            storage.IdStorage = _newStorageItem;
          break;
        }
      }
      if (type.BaseType != typeof(Object))
        CreateStorageDescription(type.BaseType, storage, removed, includeReverseLookup);
      else
        Debug.Assert(storage.IdStorage != null);
    }
    private object GetValueFromEntity(object entity)
    {
      object _value = this.m_Storage.GetValue(entity);
      if (this.m_Association)
      {
        Debug.Assert(this.IsLookup, "Unexpected MultivalueType in the GetValuesFromEntity. Expected is lookup, but the filed is reverse lookup");
        _value = ((IEntityRef)_value).GetLookup(((AssociationAttribute)m_Description).List);
      }
      else if (((ColumnAttribute)this.m_Description).FieldType.Equals(FieldType.Choice.ToString()) && _value != null)
      {
        Dictionary<string, string> _values = new Dictionary<string, string>();
        Type _type = this.GetEnumValues(_values, true);
        _value = _values[_value.ToString()];
      }
      return _value;
    }
    private StorageItem(string propertyName, DataAttribute description)
    {
      PropertyName = propertyName;
      m_Association = description is AssociationAttribute;
      m_Description = description;
    }
    private Type GetEnumValues(Dictionary<string, string> values, bool fieldNameIsKey)
    {
      Type[] _types = this.m_Storage.FieldType.GetGenericArguments();
      if (_types.Length != 1)
        throw new ApplicationException("Unexpected type in the AssignValues2Entity");
      FieldInfo[] _fields = _types[0].GetFields();
      foreach (FieldInfo _fld in _fields)
      {
        object[] _attrbts = _fld.GetCustomAttributes(false);
        foreach (Attribute _attr in _attrbts)
        {
          ChoiceAttribute _ca = _attr as ChoiceAttribute;
          if (_ca == null)
            continue;
          if (fieldNameIsKey)
            values.Add(_fld.Name, _ca.Value);
          else
            values.Add(NormalizeEnum(_ca.Value), _fld.Name);
        }
      }
      return _types[0];
    }
    /// <summary>
    /// Normalizes the enum value by removing any spaces.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>System.String.</returns>
    private static string NormalizeEnum(string value)
    {
      return value.Replace(" ", "");
    }
    #endregion

  }
}
