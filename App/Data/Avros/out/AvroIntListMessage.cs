//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:6.0.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SerializationDataLib.AvroMessage
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class AvroIntList : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"AvroIntList\",\"namespace\":\"SerializationDataLib.AvroMessa" +
				"ge\",\"fields\":[{\"name\":\"IntList\",\"type\":{\"type\":\"array\",\"items\":\"int\"}}]}");
		private IList<System.Int32> _IntList;
		public virtual Schema Schema
		{
			get
			{
				return AvroIntList._SCHEMA;
			}
		}
		public IList<System.Int32> IntList
		{
			get
			{
				return this._IntList;
			}
			set
			{
				this._IntList = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this.IntList;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this.IntList = (IList<System.Int32>)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}