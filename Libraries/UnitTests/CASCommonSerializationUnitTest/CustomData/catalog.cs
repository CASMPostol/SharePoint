using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CAS.Common.Serialization.UnitTests.CustomData
{

  /// <summary>
  /// class catalog
  /// </summary>
  [DataContract]//[System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://tempuri.org/CreateXMLFile.xsd", IsNullable = false)]
  public partial class catalog
  {

    private CDDescription[] cdField;

    /// <summary>
    /// Gets or sets the cd.
    /// </summary>
    /// <value>
    /// The cd.
    /// </value>
    [System.Xml.Serialization.XmlElementAttribute("cd")]
    [DataMember]
    public CDDescription[] cd
    {
      get
      {
        return this.cdField;
      }
      set
      {
        this.cdField = value;
      }
    }
  }
  /// <summary>
  /// class CDDescription 
  /// </summary>
  [DataContract] //[System.SerializableAttribute()]
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  public partial class CDDescription
  {
    private string titleField;
    private string artistField;
    private string countryField;
    private string companyField;
    private decimal priceField;
    private ushort yearField;
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    /// <uwagi />
    [DataMember]
    public string title
    {
      get
      {
        return this.titleField;
      }
      set
      {
        this.titleField = value;
      }
    }
    /// <summary>
    /// Gets or sets the artist.
    /// </summary>
    /// <value>
    /// The artist.
    /// </value>
    [DataMember]
    public string artist
    {
      get
      {
        return this.artistField;
      }
      set
      {
        this.artistField = value;
      }
    }
    /// <summary>
    /// Gets or sets the country.
    /// </summary>
    /// <value>
    /// The country.
    /// </value>
    [DataMember]
    public string country
    {
      get
      {
        return this.countryField;
      }
      set
      {
        this.countryField = value;
      }
    }
    /// <summary>
    /// Gets or sets the company.
    /// </summary>
    /// <value>
    /// The company.
    /// </value>
    [DataMember]
    public string company
    {
      get
      {
        return this.companyField;
      }
      set
      {
        this.companyField = value;
      }
    }
    /// <summary>
    /// Gets or sets the price.
    /// </summary>
    /// <value>
    /// The price.
    /// </value>
    [DataMember]
    public decimal price
    {
      get
      {
        return this.priceField;
      }
      set
      {
        this.priceField = value;
      }
    }
    /// <summary>
    /// Gets or sets the year.
    /// </summary>
    /// <value>
    /// The year.
    /// </value>
    [DataMember]
    public ushort year
    {
      get
      {
        return this.yearField;
      }
      set
      {
        this.yearField = value;
      }
    }
  }

}
